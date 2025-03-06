using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TestInventoryUI : MonoBehaviour
{
    public void RemoveRandomItem()
    {
        var filledIndexes = new List<int>();

        for (int index = 0; index < Game.Inventory.Size; index++)
        {
            if (Game.Inventory.Data.slots[index].item.HasValue)
                filledIndexes.Add(index);
        }

        if (filledIndexes.Count > 0)
            Game.Inventory.TryTake(filledIndexes.Random(), out var item);
        else
            throw new System.Exception("Can't remove random item: Inventory is empty");
    }



    public void TryShoot()
    {
        var weapons = Game.Inventory.Data.slots.Where(i => i.item.HasValue && i.item.Value.data.Type == ItemType.Weapon).ToArray();

        if (weapons.Length == 0) return;

        weapons = weapons.OrderBy(w => Random.Range(0, weapons.Length)).ToArray();

        foreach(var weapon in weapons)
        {
            if (Game.Inventory.TryTake((weapon.item.Value.data as WeaponItemData).Ammo, 1, out int rest))
            {
                Debug.LogFormat("Damaged for {0} points", (weapon.item.Value.data as WeaponItemData).Damage);

                return;
            }
        }
        
        Debug.LogWarning("Can't shoot: Inventory does have any ammo or weapons");

    }


    public void AddAmmos()
    {
        var ammoDatas = ItemData.All.Values.Where(i => i.Type == ItemType.Ammo).ToArray();

        foreach(var ammoData in ammoDatas)
            Game.Inventory.TryAdd(ammoData, ammoData.MaxAmount, out var rest);
    }


    public void AddAllTypesItems()
    {
        AddItemOfType(ItemType.Weapon);
        AddItemOfType(ItemType.Armor);
        AddItemOfType(ItemType.Helmet);
    }

    private void AddItemOfType(ItemType type)
    {
        var itemData = ItemData.All.Values.Where(i => i.Type == type).ToArray().Random();
        
        if (itemData)
        {
            Game.Inventory.TryAdd(itemData, 1, out var rest);
        }
        else
        {
            Debug.LogWarningFormat("Item with type: {0} is not exist", type);
        }
    }
}
