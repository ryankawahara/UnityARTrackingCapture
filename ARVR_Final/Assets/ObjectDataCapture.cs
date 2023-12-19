using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.IO;
using UnityEditor;


public class ObjectDataCapture : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;
    private string csvName = "object_pos_data.csv";
    private string outputPath;
    private float timer = 0f;
    public static float StartTime;
    public float printDuration = 15f;
    public static bool ready = false;
    private bool first = true;


    void Start()
    {
        outputPath = Path.Combine(Application.persistentDataPath, csvName);
        File.WriteAllText(outputPath, string.Empty);

        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        if (trackedImageManager == null)
        {
            Debug.LogError("No ARTrackedImageManager found in the scene.");
        }

    }

    void Update()
    {
        // code adapted from
        // https://discussions.unity.com/t/how-to-efficiently-save-prefab-transform-position-and-rotation-to-csv/256369
        float elapsedTime = Time.time - StartTime;

        if (trackedImageManager != null)
        {
            // only start recording information when image is being tracked
            Debug.Log(trackedImageManager.trackables.count);
            foreach (var trackedImage in trackedImageManager.trackables)
            {

                if (timer < printDuration)
                {
                    if (trackedImage.trackingState == TrackingState.Tracking)
                    {
                        ready = true;
                        Vector3 imagePosition = trackedImage.transform.position;
                        Quaternion imageRotation = trackedImage.transform.rotation;
                        Vector3 eulerRotation = imageRotation.eulerAngles;
                        Vector3 objScale = trackedImage.transform.localScale;
                        Debug.Log("Image size: " + trackedImage.transform.localScale.x);
                        Debug.Log("Tracked Image Position: " + imagePosition.ToString("F2"));
                        Debug.Log("Tracked Image Rotation: " + imageRotation.eulerAngles.ToString("F2"));

                        string data = $"{Time.time}, {imagePosition.x}, {imagePosition.y}, {imagePosition.z}, {eulerRotation.x}, {eulerRotation.y}, {eulerRotation.z}, {objScale.x}, {objScale.y} \n";

                        Debug.Log(data);
                        File.AppendAllText(outputPath, data);

                    }


                    timer += Time.deltaTime;
                }

            }
        }
    }
}
