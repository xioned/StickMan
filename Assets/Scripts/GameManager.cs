using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UiManager uiManager;
    public GameObject enemyPrefab;
    public int score;
    public int bestScore;
    public Transform[] enemySpawnPosition;

    int level =1;
    int enemyLeftCount=1;
    List<Transform> emptySpawnSlot;
    private void OnEnable()
    {
        GameEvents.EnemyDamageUiEvent += AddScore;
    }

    private void OnDisable()
    {
        GameEvents.EnemyDamageUiEvent -= AddScore;
    }


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
        ScoreUpdate();
    }

    private void AddScore(int arg1, Vector3 vector)
    {
        score += arg1;
        if(score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", score);
        }
        uiManager.UpdateScoreUi(score, bestScore);
    }

    public void ScoreUpdate()
    {
        if (!PlayerPrefs.HasKey("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", 0);
        }
        bestScore = PlayerPrefs.GetInt("BestScore");
    }

    

    public void SpawnNewEnemy()
    {
        if(GameObject.FindObjectOfType<Player>() == null) { return; }
        enemyLeftCount--;
        if(enemyLeftCount > 0) { return; }
        level++;

        
        int enemySpawnAmount = 1;
        if(level >3)
        {
            enemySpawnAmount = 2;
        }
        if(level > 8)
        {
            enemySpawnAmount = 3;
        }

        enemyLeftCount = enemySpawnAmount;

        emptySpawnSlot = enemySpawnPosition.ToList<Transform>();
        for (int i = 0; i < enemySpawnAmount; i++)
        {
            int randPos = UnityEngine.Random.Range(0, emptySpawnSlot.Count);
            Instantiate(enemyPrefab, enemySpawnPosition[randPos].position, Quaternion.identity);
            emptySpawnSlot.RemoveAt(randPos);
        }
    }
    
    public void ExitGame() => Application.Quit();
    public void NewGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    
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