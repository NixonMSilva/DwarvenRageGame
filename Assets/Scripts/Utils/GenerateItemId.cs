using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using UnityEngine;

public static class GenerateItemId
{
    public static int Generate ()
    {
        int number = Mathf.FloorToInt(UnityEngine.Random.Range(-65535, 65535));
        return number;
    }

    public static bool IsDuplicate (int id, List<int> context)
    {
        return context.Exists(x => x == id);
    }
}