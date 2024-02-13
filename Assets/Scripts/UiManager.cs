using DG.Tweening;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public Camera cam;
    public float uiMoveUpValue;
    public Canvas parent;

    public GameObject levelCleared;
    public GameObject powerupPanel;
    public TextMeshProUGUI savedPointText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI totalPoint;

    public TextMeshProUGUI scoreTextGame;
    public TextMeshProUGUI abilityText;

    private void Start()
    {
        cam = Camera.main;
    }
    private void OnEnable()
    {
        GameEvents.EnemyDamageUiEvent += DamageUipop;
    }
    private void OnDisable()
    {
        GameEvents.EnemyDamageUiEvent -= DamageUipop;
    }

    public void ShowLevelCOmplete()
    {
        levelCleared.SetActive(true);
        powerupPanel.SetActive(false);
        UpdatePoints();
    }

    public void UpdatePoints()
    {
        savedPointText.text = "Inventory Points: " + GameManager.Instance.tmpPoint.ToString("00");
        pointsText.text = "Points Earned: " + GameManager.Instance.score.ToString("00");
        totalPoint.text = "Total Points: " +  GameManager.Instance.savedScore.ToString("00");
        abilityText.text = "Buy Abilities - " +  GameManager.Instance.savedScore.ToString("00");
    }

    public void UpdateScoreUi(int score,int bestScore)
    {
        scoreTextGame.text = score.ToString();
        //scoreText.text = "Score: "+score.ToString();
        //bestScoreText.text = "Best Score: "+bestScore.ToString();
    }
    private void DamageUipop(int amount, Vector3 pos)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(pos);
        GameObject a = Instantiate(damageTextPrefab, screenPos,Quaternion.identity,parent.transform);
        a.GetComponent<TextMeshProUGUI>().text = "+"+amount.ToString();

        Tween t = a.transform.DOMoveY(screenPos.y+uiMoveUpValue, 1, false);

        t.OnComplete(() => { Destroy(a); });
    }
}
