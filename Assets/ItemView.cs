using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    public string id;
    public string powerupName;
    public int price;
    public int count;

    private void Start()
    {
         count = PlayerPrefs.GetInt(id, 0);
    }

    public void BuyPowerUp()
    {
        if(GameManager.Instance.savedScore >= price)
        {
            count++;
            GameManager.Instance.savedScore -= price;
            GameManager.Instance.uiManager.UpdatePoints();
            PlayerPrefs.SetInt("savedScore", GameManager.Instance.savedScore);
            PlayerPrefs.SetInt(id, count);
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }
}
