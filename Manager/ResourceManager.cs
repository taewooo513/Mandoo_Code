using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class LableKey
{
    const string test = "test";
}


public class ResourceManager : Singleton<ResourceManager>
{
    public AsyncOperationHandle<IList<T>> LoadResource<T>(string label, Action<T> callback) where T : UnityEngine.Object
    {
        return Addressables.LoadAssetsAsync<T>(label, callback);
    }
}