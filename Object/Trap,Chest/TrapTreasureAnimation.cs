using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTreasureAnimation : MonoBehaviour
{
    private Trap _trap;
    private TreasureChest _treasureChest;

    public void Awake()
    {
        _trap = GetComponentInParent<Trap>();
        _treasureChest = GetComponentInParent<TreasureChest>();
    }

    public void TrapEventEnd()
    {
        _trap.EndTrapActive();
    }

    public void TreasureEventEnd()
    {
        _treasureChest.OpenChestEventEnd();
    }
}
