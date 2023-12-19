using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVToObjectPlace : MonoBehaviour
{
    [Header("CSV Data")]
    public TextAsset csvFile;

    [ContextMenu("Convert CSV to Placement")]
    void Start()
    {
        if (csvFile != null)
        {
            LoadCSV(csvFile.text);
        }
        else
        {
            Debug.Log("No CSV found");
        }

    }

    private void LoadCSV(string csvData)
    {
        string[] lines = csvData.Split('\n');

        foreach (string line in lines)
        {
            string[] values = line.Split(',');

            if (values.Length >= 7)
            {
                //float xTranslation = float.Parse(values[0]);
                //float yTranslation = float.Parse(values[1]);
                //float zTranslation = float.Parse(values[2]);

                //float xRotation = float.Parse(values[3]);
                //float yRotation = float.Parse(values[4]);
                //float zRotation = float.Parse(values[5]);

                //float xScale = float.Parse(values[6]);
                //float yScale = float.Parse(values[7]);

                //transform.localPosition = new Vector3(xTranslation, yTranslation, zTranslation);
                //transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);
                //transform.localScale = new Vector3(xScale, yScale, yScale);
            }
        }
    }
}
