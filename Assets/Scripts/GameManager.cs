using System;
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
    int enemyKilledCount;
    int difficultyLevel = 1;

    Transform[] emptySpawnSlot;
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
        if (enemyKilledCount > 5)
        {
            difficultyLevel = 2;
        }

        int randPos = UnityEngine.Random.Range(0, enemySpawnPosition.Length);
        Instantiate(enemyPrefab, enemySpawnPosition[randPos].position,Quaternion.identity);
    }
    void LevelDifficulty()
    {
        switch(enemyKilledCount)
        {
            case 5:
                difficultyLevel++; break;
            case 10:
                difficultyLevel++; break;
            case 15:
                difficultyLevel++; break;
            case 20:
                difficultyLevel++; break;
            case 25:
                difficultyLevel++; break;
            case 30:
                difficultyLevel++; break;
            case 35:
                difficultyLevel++; break;
            case 40:
                difficultyLevel++; break;
            case 45:
                difficultyLevel++; break;
            case 50:
                difficultyLevel++; break;
                default: break;

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