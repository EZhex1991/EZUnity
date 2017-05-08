/*
 * Author:      熊哲
 * CreateTime:  4/18/2017 5:25:35 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using UnityEngine;

public class TypeNameOfDictionary : MonoBehaviour
{
    void Start()
    {
        Debug.Log(typeof(Dictionary<string, Vector3>));
    }
}