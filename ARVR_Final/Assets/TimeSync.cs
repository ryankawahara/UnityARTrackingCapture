using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSync : MonoBehaviour
{
    void Start()
    {
        // Initialize the start time for both ObjectDataCapture and CameraDataCapture scripts
        ObjectDataCapture.StartTime = Time.time;
        CameraDataCapture.StartTime = Time.time;
    }
}
