using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor; 
#if UNITY_EDITOR && !UNITY_EDITOR_PLAYMODE


public class CSVToAnimation : MonoBehaviour
{
    [Header("CSV Data")]
    public TextAsset csvFile;

    [Header("Output Name")]
    public string outputName = "camera_anim_data.anim";

    [Header("Output Folder")]
    public string outputFolder = "Assets";

    public int smoothingThreshold = 100;

    [Header("Animation Curves")]
    public AnimationCurve positionXCurve;
    public AnimationCurve positionYCurve;
    public AnimationCurve positionZCurve;
    public AnimationCurve rotationXCurve;
    public AnimationCurve rotationYCurve;
    public AnimationCurve rotationZCurve;

   

    private AnimationClip animationClip;
    private float animationLength = 0f;
    private bool firstRun = true;

    private void createTag(string tagName)
    {
        // sourced from https://discussions.unity.com/t/is-it-possible-to-create-a-tag-programmatically/9882/4

        // Open tag manager
        // Didn't end up needing tags, used for experimenting early on
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        SerializedProperty layersProp = tagManager.FindProperty("layers");

        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(tagName)) { found = true; break; }
        }

        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = tagName;
        }

        tagManager.ApplyModifiedProperties();
    }


    [ContextMenu("Convert CSV to Animation")]
    private void Start()
    {
        if (csvFile != null)
        {
            LoadCSV(csvFile.text);
            CreateAnimationClip();
        }
        else
        {
            Debug.LogError("No CSV file selected.");
        }
    }

    private void LoadCSV(string csvData)
    {
        string[] lines = csvData.Split('\n');
        Vector3 previousRotation = Vector3.zero;

        foreach (string line in lines)
        {
            string[] values = line.Split(',');

            if (values.Length >= 7)
            {

                if (firstRun == true)
                {
                    createTag("TrackedCamera");
                    firstRun = false;
                }

                float time = float.Parse(values[0]);
                Vector3 position = new Vector3(
                    float.Parse(values[1]),
                    float.Parse(values[2]),
                    float.Parse(values[3])
                );
                Vector3 rotation = new Vector3(
                    float.Parse(values[4]),
                    float.Parse(values[5]),
                    float.Parse(values[6])
                );
                Vector3 rotationDifference = rotation - previousRotation;

               
                if (Mathf.Abs(rotationDifference.x) > smoothingThreshold ||
                    Mathf.Abs(rotationDifference.y) > smoothingThreshold ||
                    Mathf.Abs(rotationDifference.z) > smoothingThreshold)
                {
                    // If the rotation difference is above the threshold
                    // discard it
                    rotation = previousRotation;
                }

                // sourced from
                // https://docs.unity3d.com/ScriptReference/AnimationCurve.AddKey.html


                positionXCurve.AddKey(time, position.x);
                positionYCurve.AddKey(time, position.y);
                positionZCurve.AddKey(time, position.z);
                rotationXCurve.AddKey(time, rotation.x);
                rotationYCurve.AddKey(time, rotation.y);
                rotationZCurve.AddKey(time, rotation.z);

                animationLength = Mathf.Max(animationLength, time);

                if (values.Length >= 9)
                {
                    float xScale = float.Parse(values[7]);
                    float yScale = float.Parse(values[8]);

                    transform.localScale = new Vector3(xScale, yScale, yScale);
                }
            }
        }
    }

    private void CreateAnimationClip()
    {
        animationClip = new AnimationClip();
        // adapted from https://docs.unity3d.com/ScriptReference/AnimationClip.SetCurve.html

        animationClip.SetCurve("", typeof(Transform), "localPosition.x", positionXCurve);
        animationClip.SetCurve("", typeof(Transform), "localPosition.y", positionYCurve);
        animationClip.SetCurve("", typeof(Transform), "localPosition.z", positionZCurve);
        animationClip.SetCurve("", typeof(Transform), "localEulerAngles.x", rotationXCurve);
        animationClip.SetCurve("", typeof(Transform), "localEulerAngles.y", rotationYCurve);
        animationClip.SetCurve("", typeof(Transform), "localEulerAngles.z", rotationZCurve);


        animationClip.wrapMode = WrapMode.Once;

        string animationClipPath = outputFolder + "/" + outputName;
        AssetDatabase.CreateAsset(animationClip, animationClipPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }
}
#endif