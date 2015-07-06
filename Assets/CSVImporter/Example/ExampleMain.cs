using UnityEngine;
using System.Collections.Generic;
using gametheory.Utilities;

public class ExampleMain : MonoBehaviour 
{
    public string FilePath;

    void Start()
    {
        List<SampleClass> list = CSVImporter.GenerateList<SampleClass>(FilePath);

        for (int index = 0; index < list.Count; index++)
            Debug.Log(list[index].Name);
    }
}
