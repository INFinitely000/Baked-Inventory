using UnityEngine;

[CreateAssetMenu( fileName = "Ammo", menuName = "Inventory/Ammo")]
public class AmmoItemData : ItemData
{
    public override ItemType Type => ItemType.Ammo;
}
