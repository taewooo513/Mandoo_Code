using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Utillity
{
    public static int GetIndexInListToObject<T>(List<T> types, T t) // 리스트에서 T 값으로 인덱스반환
    {
        for (int i = 0; i < types.Count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(types[i], t))
            {
                return i;
            }
        }
        Debug.Log(t.GetType().ToString() + "IsNotFind In List");
        return -1;
    }

    public static T GetObjectInListToIndex<T>(List<T> types, int index)// 리스트에서 인덱스 값으로 T반환
    {
        if (types.Count >= index)
        {
            return default(T);
        }
        return types[index];
    }
}
