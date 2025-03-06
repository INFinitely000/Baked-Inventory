using Newtonsoft.Json;
using UnityEngine;


[JsonConverter(typeof(InventorySlotJsonConverter))]
public struct InventorySlot
{
    public Item? item;
    public bool isUnlocked;

    public InventorySlot(Item? item, bool isUnlocked)
    {
        this.item = item;
        this.isUnlocked = isUnlocked;
    }
}
