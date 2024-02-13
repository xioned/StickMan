using TMPro;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public UiPowerUpItem[] inventoryItems;
    public int[] powerUpsQuanity = { 0,0,0,0};
    public UiManager uiManager;
    public Player player;
    private void Start()
    {
        UpdateInventoryUi();
    }
    public void UpdateInventoryUi()
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            inventoryItems[i].gameObject.SetActive(false);
            if (powerUpsQuanity[i] <= 0) { continue; }

            inventoryItems[i].gameObject.SetActive(true);
            inventoryItems[i].GetComponentInChildren<TextMeshProUGUI>().text = powerUpsQuanity[i].ToString();
        }
    }

    public void PowerUpItemButtonChick(int itemCode,int itemCost,bool isShopButton)
    {
        if (isShopButton)
        {
            if (GameManager.Instance.savedScore < itemCost) { return; }
            GameManager.Instance.savedScore -= itemCost;
            powerUpsQuanity[itemCode]++;
            uiManager.ShowLevelCOmplete();
            return;
        }

        powerUpsQuanity[itemCode]--;
        inventoryItems[itemCode].GetComponentInChildren<TextMeshProUGUI>().text = powerUpsQuanity[itemCode].ToString();
        if (powerUpsQuanity[itemCode] <= 0) { inventoryItems[itemCode].gameObject.SetActive(false); }
        player.ActivePlayerPowerUp(itemCode);
    }
}
