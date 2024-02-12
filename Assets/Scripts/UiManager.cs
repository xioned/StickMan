using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public Camera cam;
    public float uiMoveUpValue;
    public Canvas parent;
    private void Start()
    {
        cam = Camera.main;
    }
    private void OnEnable()
    {
        GameEvents.EnemyDamageUiEvent += DamageUipop;
    }

    private void DamageUipop(int amount, Vector3 pos)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(pos);
        GameObject a = Instantiate(damageTextPrefab, screenPos,Quaternion.identity,parent.transform);
        a.GetComponent<TextMeshProUGUI>().text = amount.ToString();

        Tween t = a.transform.DOMoveY(screenPos.y+uiMoveUpValue, 1, false);

        t.OnComplete(() => { Destroy(a); });
    }
}
