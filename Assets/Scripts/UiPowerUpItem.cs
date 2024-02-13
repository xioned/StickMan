using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiPowerUpItem : MonoBehaviour,IPointerClickHandler
{
    public PowerUpManager powerUpManager;
    public int itemCode;
    public int itemCost;
    public bool isShopButton;
    public void OnPointerClick(PointerEventData eventData)
    {
        powerUpManager.PowerUpItemButtonChick(itemCode, itemCost,isShopButton);
    }
}
