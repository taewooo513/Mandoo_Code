using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAppear : MonoBehaviour
{
    public GameObject[] enemyPrefabs;      // Inspector에서 프리팹 배열
    public Transform[] spawnPoints;        // Inspector에서 위치 배열 (4개)

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public void SpawnEnemies()
    {
        int length = Mathf.Min(enemyPrefabs.Length, spawnPoints.Length);

        for (int i = 0; i < length; i++)
        {
            // 해당 위치에 이미 적이 있으면, 다음 스폰 위치를 찾음
            int spawnIndex = i;
            while (spawnIndex < spawnPoints.Length)
            {
                bool alreadySpawned = spawnedEnemies.Exists(e =>
                    e != null && Vector3.Distance(e.transform.position, spawnPoints[spawnIndex].position) < 0.1f);

                if (!alreadySpawned)
                    break;

                spawnIndex++;
            }

            // 모든 스폰 위치에 적이 있으면 소환하지 않음
            if (spawnIndex >= spawnPoints.Length)
            {
                //Debug.Log($"모든 스폰 위치에 적이 있습니다. {enemyPrefabs[i].name} 소환 불가.");
                continue;
            }

            //if (enemyPrefabs[i] == null)
            //{
            //    //Debug.LogError($"프리팹이 연결되지 않았습니다: {i}");
            //    continue;
            //}

            GameObject obj = Instantiate(enemyPrefabs[i], spawnPoints[spawnIndex].position, Quaternion.identity);
            spawnedEnemies.Add(obj);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnEnemies();
        }
    }
}