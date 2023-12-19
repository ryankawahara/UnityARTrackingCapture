//using UnityEngine;
//using UnityEditor;
//using UnityEditor.Animations;
//using System.Collections.Generic;
//using System.IO;

//public class CSVAnimationImporter : MonoBehaviour
//{
//    [Header("CSV File")]
//    public TextAsset csvFile; // Drag and drop your CSV file here in the Inspector

//    [Header("Animated Object")]
//    public Transform animatedObject; // The object you want to animate (set in the Inspector)

//    private List<Vector3> positions = new List<Vector3>();
//    private List<Quaternion> rotations = new List<Quaternion>();
//    private AnimationClip animationClip;

//    private void Awake()
//    {
//        if (csvFile && animatedObject != null)
//        {
//            ImportCSV();
//        }
//    }

//    private void ImportCSV()
//    {
//        if (csvFile == null)
//        {
//            Debug.LogError("CSV file not specified. Drag and drop a CSV file into the 'csvFile' field in the Inspector.");
//            return;
//        }

//        LoadCSVData();

//        if (positions.Count != rotations.Count || positions.Count == 0)
//        {
//            Debug.LogError("Invalid CSV data. Make sure the CSV file contains valid position and rotation values.");
//            return;
//        }

//        // Create an AnimationClip
//        animationClip = new AnimationClip();
//        animationClip.legacy = true;

//        // Add position and rotation curves to the AnimationClip
//        animationClip.SetCurve("", typeof(Transform), "localPosition.x", CreateAnimationCurve(positions, p => p.x));
//        animationClip.SetCurve("", typeof(Transform), "localPosition.y", CreateAnimationCurve(positions, p => p.y));
//        animationClip.SetCurve("", typeof(Transform), "localPosition.z", CreateAnimationCurve(positions, p => p.z));

//        animationClip.SetCurve("", typeof(Transform), "localRotation.x", CreateAnimationCurve(rotations, r => r.x));
//        animationClip.SetCurve("", typeof(Transform), "localRotation.y", CreateAnimationCurve(rotations, r => r.y));
//        animationClip.SetCurve("", typeof(Transform), "localRotation.z", CreateAnimationCurve(rotations, r => r.z));
//        animationClip.SetCurve("", typeof(Transform), "localRotation.w", CreateAnimationCurve(rotations, r => r.w));

//        // Save the AnimationClip as an asset
//        string animationClipPath = "Assets/Animations/CameraAnimation.anim";
//        AssetDatabase.CreateAsset(animationClip, animationClipPath);

//        // Create an Animator Controller and attach the AnimationClip
//        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath("Assets/Animations/CameraAnimationController.controller");
//        AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
//        AnimatorState state = stateMachine.AddState("CameraAnimationState");
//        state.motion = animationClip;

//        // Add an Animator component to the object you want to animate
//        if (animatedObject != null)
//        {
//            Animator animator = animatedObject.gameObject.AddComponent<Animator>();
//            animator.runtimeAnimatorController = controller;
//        }
//    }

//    private void LoadCSVData()
//    {
//        string[] lines = csvFile.text.Split('\n');

//        foreach (string line in lines)
//        {
//            string[] values = line.Split(',');
//            if (values.Length == 8) // Assuming CSV format: timestamp, posX, posY, posZ, rotX, rotY, rotZ, rotW
//            {
//                float posX = float.Parse(values[1]);
//                float posY = float.Parse(values[2]);
//                float posZ = float.Parse(values[3]);
//                float rotX = float.Parse(values[4]);
//                float rotY = float.Parse(values[5]);
//                float rotZ = float.Parse(values[6]);
//                float rotW = float.Parse(values[7]);

//                positions.Add(new Vector3(posX, posY, posZ));
//                rotations.Add(new Quaternion(rotX, rotY, rotZ, rotW));
//            }
//        }
//    }

//    private AnimationCurve CreateAnimationCurve<T>(List<T> values, System.Func<T, float> selector)
//    {
//        Keyframe[] keyframes = new Keyframe[values.Count];
//        for (int i = 0; i < values.Count; i++)
//        {
//            keyframes[i] = new Keyframe(i, selector(values[i]));
//        }
//        return new AnimationCurve(keyframes);
//    }
//}
