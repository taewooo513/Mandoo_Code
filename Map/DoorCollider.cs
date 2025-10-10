using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        MapManager.Instance.CurrentCorridor.ExitCorridor();
    }
}
