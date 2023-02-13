using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Calculate", 1, 1);
        
    }
    
    void Calculate()
    {
        int
        value = 100 / 15;
        print("value "+value);
        float value2 = Mathf.RoundToInt((float)100/15);
        print("value2 "+value2);
    }
    static int add(int value)
    { return value + 1; }

    // Update is called once per frame
    void Update()
    {
        
    }
}
