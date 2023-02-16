using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatParse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string inputString = "3.14";
        string outputstring = "Ãâ·Â°ª: ''";
        outputstring += (float.Parse(inputString) * 2.0f).ToString();
        outputstring += "''";
        Debug.Log(outputstring);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
