#define DEBUG_MODE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugString
{
    public static void Log(Object msg)
    {
#if DEBUG_MODE
        Debug.Log(msg);
#endif
    }
}
