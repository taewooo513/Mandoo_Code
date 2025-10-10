using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class AudioManager : Singleton<AudioManager>
{
    Dictionary<string, AudioClip> sounds;
    AudioSource audioSource;
    protected override void Awake()
    {
        base.Awake();
        sounds = new Dictionary<string, AudioClip>();
        audioSource = transform.AddComponent<AudioSource>();
    }

    private void Update()
    {

    }

    public AudioClip LoadSound(string key)
    {
        var handle = Resources.Load<AudioClip>(key);
        if (sounds.TryGetValue(key, out AudioClip clip))
        {
            Debug.Log($"{key} is duplicate in sounds");
            return handle;
        }
        sounds.Add(key, handle);
        return handle;
    }

    public void PlaySound(string key)
    {
        audioSource.PlayOneShot(LoadSound(key));
    }


}
