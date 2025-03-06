using UnityEngine;


public struct InventoryActionData
{
    public int slotIndex;
    public int difference;
    public Inventory inventory;
    public InventoryActionType type;

    public InventoryActionData(int slotIndex, int difference, Inventory inventory, InventoryActionType type)
    {
        this.slotIndex = slotIndex;
        this.difference = difference;
        this.inventory = inventory;
        this.type = type;
    }
}
