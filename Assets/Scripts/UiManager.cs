using DG.Tweening;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public Camera cam;
    public float uiMoveUpValue;
    public Canvas parent;
    public TextMeshProUGUI scoreTextGame;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
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

    public void UpdateScoreUi(int score,int bestScore)
    {
        scoreTextGame.text = score.ToString();
        scoreText.text = "Score: "+score.ToString();
        bestScoreText.text = "Best Score: "+bestScore.ToString();
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
