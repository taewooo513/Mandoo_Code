using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{

    void Start()
    {
        //UIManager.Instance.OpenUI<RestartGame>();
        MapManager.Instance.Initialize();
    }
}
