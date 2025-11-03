using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollider : MonoBehaviour
{
    [SerializeField] private bool isExitToDestinationRoom;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isExitToDestinationRoom) MapManager.Instance.CurrentCorridor.ExitCorridor();
        else MapManager.Instance.CurrentCorridor.ExitCorridorToCurrentRoom();
    }
}
