using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTest : MonoBehaviour
{
    [SerializeField] private List<BaseEntity> player;
    [SerializeField] private List<BaseEntity> enemies;

    void Start()
    {
        if (DataManager.Instance != null)
            DataManager.Instance.Initialize();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            BattleManager.Instance.BattleStartTrigger(player, enemies);
        }
    }
}
