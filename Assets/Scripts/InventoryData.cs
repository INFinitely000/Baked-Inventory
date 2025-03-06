using UnityEngine;
using Newtonsoft.Json;


public struct InventoryData
{
    public InventorySlot[] slots;

    [JsonIgnore]
    public int firstEmptySlotIndex;

    [JsonIgnore]
    public Inventory inventory;


    public InventoryData(int size, Inventory inventory)
    {
        if (size < 0)
            throw new System.ArgumentOutOfRangeException(nameof(size));

        slots = new InventorySlot[size];

        for (int index = 0; index < size; index++)
            slots[index] = new InventorySlot(null, false);

        this.inventory = inventory;

        firstEmptySlotIndex = 0;
    }
}
