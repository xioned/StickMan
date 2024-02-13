using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UiManager uiManager;
    public GameObject enemyPrefab;
    public int score;
    public int savedScore;
    public Transform[] enemySpawnPosition;
    public int enemyKilledCount;
    public int difficultyLevel = 1;
    public bool gameOver;
    public bool levelCelared;

    Transform[] emptySpawnSlot;
    private void OnEnable()
    {
        GameEvents.EnemyDamageUiEvent += AddScore;
    }

    private void OnDisable()
    {
        GameEvents.EnemyDamageUiEvent -= AddScore;
    }


    private void Awake()
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

    private void Start()
    {
        ScoreUpdate();

    }

    public void NextLevel()
    {
        uiManager.levelCleared.SetActive(false);
        levelCelared = false;
        SpawnNewEnemy();
        enemyKilledCount = 0;
    }

    private void AddScore(int arg1, Vector3 vector)
    {
        score += arg1;
        savedScore += score;
        PlayerPrefs.SetInt("savedScore", savedScore);
        uiManager.UpdateScoreUi(score, savedScore);

        Debug.Log("SCORING!");
    }

    public void ScoreUpdate()
    {
        if (!PlayerPrefs.HasKey("savedScore"))
        {
            PlayerPrefs.SetInt("savedScore", 0);
        }
        savedScore = PlayerPrefs.GetInt("savedScore");
    }

    
    public void IncreaseKillCount()
    {
        enemyKilledCount++;
        if (enemyKilledCount >= 3 * difficultyLevel)
        {
            uiManager.ShowLevelCOmplete(); 
            levelCelared = true;
            difficultyLevel = 2;
        }
    }


    public void SpawnNewEnemy()
    {
        if(gameOver || levelCelared) 
        { 
            return; 
        }
    
        int randPos = UnityEngine.Random.Range(0, enemySpawnPosition.Length);
        Instantiate(enemyPrefab, enemySpawnPosition[randPos].position,Quaternion.identity);

        Debug.Log("Spwaning new enemy");
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