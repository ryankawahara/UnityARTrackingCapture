using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CameraDataCapture : MonoBehaviour
{
    // Captures for 15 seconds
    public float printDuration = 15f;
    public static float StartTime;
    private float timer = 0f;

    public string outputFileName = "camera_info.csv";

    private string outputPath;

    void Start()
    {
        outputPath = Path.Combine(Application.persistentDataPath, outputFileName);
        File.WriteAllText(outputPath, string.Empty);

    }

    void Update()
    {
        // code adapted from
        // https://discussions.unity.com/t/how-to-efficiently-save-prefab-transform-position-and-rotation-to-csv/256369

        if (timer < printDuration)
        {

            // Captures the position and orientation of the camera at each frame
            Vector3 cameraLocalPosition = transform.localPosition;

            Vector3 cameraPosition = transform.position;

            Debug.Log($"Camera Local Position: {cameraLocalPosition}");
            Debug.Log($"Camera World Position: {cameraPosition}");

            Quaternion cameraRotation = transform.rotation;

            Vector3 eulerRotation = cameraRotation.eulerAngles;

            // Stores and writes the position and orientation data to a row in the .csv
            string data = $"{Time.time}, {cameraPosition.x}, {cameraPosition.y}, {cameraPosition.z}, {eulerRotation.x}, {eulerRotation.y}, {eulerRotation.z}\n";
            File.AppendAllText(outputPath, data);

            if (ObjectDataCapture.ready == true)
                // only counts towards time limit if the object is tracking

            {
                timer += Time.deltaTime;
            }


        }

    }
}
