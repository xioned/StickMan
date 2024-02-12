using System;
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
        if(GameObject.FindObjectOfType<Player>() == null) { return; }
        int randPos = UnityEngine.Random.Range(0, enemySpawnPosition.Length);
        Instantiate(enemyPrefab, enemySpawnPosition[randPos].position,Quaternion.identity);
    }
}

public static class GameEvents
{
    public static Action OnPlayerIsDeadEvent;
    public static void CallPlayerIsDeadEvent()
    {
        OnPlayerIsDeadEvent?.Invoke();
    }

    public static Action<int,Vector3> EnemyDamageUiEvent;
    public static void CallEnemyDamageUiEvent(int amount,Vector3 pos)
    {
        EnemyDamageUiEvent?.Invoke(amount,pos);
    }
}