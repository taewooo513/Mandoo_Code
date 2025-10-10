using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteManager : Singleton<SpriteManager>
{
    private Dictionary<string, Sprite> sprites;
    protected override void Awake()
    {
        sprites = new Dictionary<string, Sprite>();
    }

    public Sprite AddSprite(string path)
    {
        Sprite sprite;
        if (sprites.TryGetValue(path, out sprite))
        {
            Debug.Log($"{path} is 중복 in SpriteManager");
            return sprite;
        }
        sprite = Resources.Load<Sprite>(path);
        if (sprite == null)
        {
            Debug.Log($"Not find {path}");
        }
        return sprite;
    }

    public Sprite FindSprite(string path)
    {
        Sprite sprite;
        if (sprites.TryGetValue(path, out sprite))
        {
            return sprite;
        }
        sprite = AddSprite(path);
        return sprite;
    }
}
