using UnityEngine;


public class TestInventory : MonoBehaviour, IInventoryOwner
{
    public Inventory Inventory { get; private set; }


    private void OnEnable()
    {
        Inventory = new PurchasableInventory(GameData.Instance.InventorySize);

        for (int index = 0; index < GameData.Instance.InventoryUnlockedSlots; index++)
        {
            Inventory.UnlockSlot(index);
        }
    }
}
