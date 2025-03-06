using UnityEngine;


[System.Serializable]
public struct SaveData
{
    public InventoryData inventoryData;
    public int coins;

    public SaveData(InventoryData inventoryData, int coins)
    {
        this.inventoryData = inventoryData;
        this.coins = coins;
    }
}
