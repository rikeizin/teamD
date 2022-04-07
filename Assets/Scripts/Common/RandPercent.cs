using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandPercent
{
    public Dictionary<string, int> regist = new Dictionary<string, int>();
    public List<string> setReigstList = new List<string>();
    public string CallResult()
    {
        foreach (var key in regist)
        {
            for (int i = 0; i < key.Value; i++)
            {
                setReigstList.Add(key.Key);
            }
        }

        int result = Random.Range(0, setReigstList.Count);
        return setReigstList[result];
    }
}
