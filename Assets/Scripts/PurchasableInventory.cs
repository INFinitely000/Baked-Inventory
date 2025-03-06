using UnityEngine;



public class PurchasableInventory : Inventory
{
    public void PurchaseSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Data.slots.Length) throw new System.ArgumentOutOfRangeException(nameof(slotIndex));

        if (Wallet.TryTake(GameData.Instance.InventorySlotCost))
            base.UnlockSlot(slotIndex);
        else
            Debug.LogWarning("Not enough coins to purchase a inventory slot");
    }


    public PurchasableInventory(int size) : base(size) { }

    public PurchasableInventory(InventoryData data) : base(data) { }
}
