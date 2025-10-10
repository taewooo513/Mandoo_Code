using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjectManager : Singleton<ObjectManager>
{
    Dictionary<string, GameObject> objects;
    AsyncOperationHandle objectsHandle;
    protected override void Awake()
    {
        base.Awake();
        objects = new Dictionary<string, GameObject>();
    }

    public AsyncOperationHandle LoadGameObject(string label)
    {
        var handle = ResourceManager.Instance.LoadResource<GameObject>(label, obj =>
        {
            InsertObject(obj.name, obj);
        });
        handle.Completed += OnLoadCompleteObject;
        objectsHandle = handle;
        handle.WaitForCompletion();
        
        return objectsHandle;
    }

    public override void Release()
    {
        objects.Clear();
        Addressables.Release(objectsHandle);
    }

    public void InsertObject(string key, GameObject obj)
    {
        if (objects.TryGetValue(key, out GameObject res))
        {
            Debug.Log($"{key} is duplicate in obj");
            return;
        }

        objects.Add(key, obj);
    }

    public GameObject AddObject(string key, Vector3 position, Quaternion quaternion, Transform parent = null)
    {
        if (objects.TryGetValue(key, out GameObject res))
        {
            if (parent == null)
            {
                return Instantiate(res, position, quaternion);
            }
            else
            {
                return Instantiate(res, position, quaternion, parent);
            }
        }
        Debug.Log($"Not Find {key} in Objects");

        return null;
    }

    private void OnLoadCompleteObject<T>(AsyncOperationHandle<IList<T>> handle) where T : UnityEngine.Object
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Load Prefabs Succeeded");
        }
        else if (handle.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError("Load Prefabs Failed");
        }
    }
}
