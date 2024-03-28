using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UiManager uiManager;
    public GameObject[] enemyPrefabs;
    public int score;
    public int savedScore;
    public int tmpPoint;
    public Transform[] enemySpawnPosition;
    public int enemyKilledCount;
    public int difficultyLevel;
    public bool gameOver;
    public bool levelCelared;
    public PowerupView[] powerupViews;
    List<Vector3> emptyEnemySpawnPos;
    int levelEnemyCount = 0;
    private void OnEnable() => GameEvents.EnemyDamageUiEvent += AddScore;
    private void OnDisable()=> GameEvents.EnemyDamageUiEvent -= AddScore;


    private void Awake() => Instance = this;

    private void Start()
    {
        difficultyLevel = 1;
        ScoreUpdate();
        SpawnLevelEnemy();
    }

    public void NextLevel()
    {
        levelCelared = false;
        enemyKilledCount = 0;
        tmpPoint = savedScore;
        Global.isSpeedActive = false;

        SpawnLevelEnemy();

        for (int i = 0; i < powerupViews.Length; i++)
        {
            powerupViews[i].ShowHide();

            for (int j = 0; j < powerupViews[i].powerUpPrefab.Length; j++)
            {
                powerupViews[i].powerUpPrefab[j].SetActive(false);
            }
        }

        //UI
        uiManager.UpdateScoreUi(score, savedScore);
        uiManager.levelCleared.SetActive(false);
        uiManager.powerupPanel.SetActive(true);
    }

    private void AddScore(int arg1, Vector3 vector)
    {
        score += arg1;
        savedScore += arg1;
        PlayerPrefs.SetInt("savedScore", savedScore);
        uiManager.UpdateScoreUi(score, savedScore);
    }

    public void ScoreUpdate()
    {
        if (!PlayerPrefs.HasKey("savedScore"))
        {
            PlayerPrefs.SetInt("savedScore", 0);
        }
        savedScore = PlayerPrefs.GetInt("savedScore");
        tmpPoint = savedScore;
    }

    
    public void IncreaseKillCount()
    {
        levelEnemyCount--;
        enemyKilledCount++;
        if (enemyKilledCount >= 3 * difficultyLevel)
        {
            levelCelared = true;
            difficultyLevel ++;

            uiManager.ShowLevelComplete(); 
        }
    }

    public void SpawnLevelEnemy()
    {
        if (gameOver || levelCelared) return;
        
        if (levelEnemyCount > 0) return;
        emptyEnemySpawnPos = new();
        levelEnemyCount = 0;
        for (int i = 0; i < enemySpawnPosition.Length; i++)
        {
            emptyEnemySpawnPos.Add(enemySpawnPosition[i].position);
        }

        for (int i = 0; i < difficultyLevel; i++)
        {
            levelEnemyCount++;
            int randPos = UnityEngine.Random.Range(0, emptyEnemySpawnPos.Count);
            Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], emptyEnemySpawnPos[randPos], Quaternion.identity);
            emptyEnemySpawnPos.RemoveAt(randPos);
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