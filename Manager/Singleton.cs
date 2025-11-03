using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }
    public void Init()
    {
    }
    protected virtual void Awake()
    {
        if (instance == null)
        {
            if (transform.parent == null)
                DontDestroyOnLoad(gameObject);
            instance = GetComponent<T>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public virtual void Release()
    {

    }
}
