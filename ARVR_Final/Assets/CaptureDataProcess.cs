using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
#if UNITY_EDITOR && !UNITY_EDITOR_PLAYMODE


public class CaptureDataProcess : MonoBehaviour
{

    private AnimationClip animationClip;
    private float animationLength = 0f;



    [Header("Camera CSV Data")]
    public TextAsset camCsvFile;

    [Header("Camera Output Name")]
    public string camOutputName = "camera_anim_data.anim";

    string outputName = "objectAnims";
    [Header("Camera Animation Curves")]
    public AnimationCurve camPositionXCurve;
    public AnimationCurve camPositionYCurve;
    public AnimationCurve camPositionZCurve;
    public AnimationCurve camRotationXCurve;
    public AnimationCurve camRotationYCurve;
    public AnimationCurve camRotationZCurve;

    [Header("Object CSV Data")]
    public TextAsset objCsvFile;


    [Header("Object Output Name")]
    public string objOutputName = "camera_anim_data.anim";

    [Header("Object Animation Curves")]
    public AnimationCurve objPositionXCurve;
    public AnimationCurve objPositionYCurve;
    public AnimationCurve objPositionZCurve;
    public AnimationCurve objRotationXCurve;
    public AnimationCurve objRotationYCurve;
    public AnimationCurve objRotationZCurve;

    // Start is called before the first frame update
    [ContextMenu("Convert CSV to Animation")]

    void Start()
    {

        GameObject trackingObject = transform.Find("TrackingObject").gameObject;

        Debug.Log(trackingObject.transform);

        if (objCsvFile != null)
        {
            LoadCSV(objCsvFile.text);
            CreateAnimationClip();
        }
        else
        {
            Debug.LogError("No CSV file selected.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadCSV(string csvData)
    {
        string[] lines = csvData.Split('\n');

        foreach (string line in lines)
        {
            string[] values = line.Split(',');

            if (values.Length >= 7)
            {


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

                objPositionXCurve.AddKey(time, position.x);
                objPositionYCurve.AddKey(time, position.y);
                objPositionZCurve.AddKey(time, position.z);
                objRotationXCurve.AddKey(time, rotation.x);
                objRotationYCurve.AddKey(time, rotation.y);
                objRotationZCurve.AddKey(time, rotation.z);

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
        //animationClip.legacy = true;

        animationClip.SetCurve("TrackingObject", typeof(Transform), "localPosition.x", objPositionXCurve);
        animationClip.SetCurve("TrackingObject", typeof(Transform), "localPosition.y", objPositionYCurve);
        animationClip.SetCurve("TrackingObject", typeof(Transform), "localPosition.z", objPositionZCurve);
        animationClip.SetCurve("TrackingObject", typeof(Transform), "localEulerAngles.x", objRotationXCurve);
        animationClip.SetCurve("TrackingObject", typeof(Transform), "localEulerAngles.y", objRotationYCurve);
        animationClip.SetCurve("TrackingObject", typeof(Transform), "localEulerAngles.z", objRotationZCurve);

        animationClip.wrapMode = WrapMode.Once;

        string animationClipPath = "Assets/" + objOutputName;
        AssetDatabase.CreateAsset(animationClip, animationClipPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        //Debug.Log(gameObject.name);
        //Animation animation = gameObject.AddComponent<Animation>();

        //animation.AddClip(animationClip, "CSVAnimation");

    }
}


#endif