using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject enemyPrefab;

    public Transform[] enemySpawnPosition;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SpawnNewEnemy()
    {
        int randPos = Random.Range(0, enemySpawnPosition.Length);
        Instantiate(enemyPrefab, enemySpawnPosition[randPos].position,Quaternion.identity);
    }

}
