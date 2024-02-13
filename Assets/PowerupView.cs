using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerupView : MonoBehaviour
{
    public string id;
    public int count;
    public TextMeshProUGUI countText;
    public GameObject[] powerUpPrefab;

    private void Start()
    {
        ShowHide();
    }

    public void ClaimPowerUp()
    {
        for (int i = 0; i < powerUpPrefab.Length; i++)
        {
            powerUpPrefab[i].SetActive(true);
        }
        count--;
        PlayerPrefs.SetInt(id, count);
        ShowHide();

        switch (id)
        {
            case "hat":
                Global.isHitActive = true;
                break;
                case "body":    
                    Global.isBodyActive = true; 
                break;
                case "speed":
                    Global.isSpeedActive = true;    
                break;
            default:
                break;
        }
    }

    public void ShowHide()
    {
        count = PlayerPrefs.GetInt(id, 0);
        countText.text = "x" + count.ToString();

        if (count <= 0)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
