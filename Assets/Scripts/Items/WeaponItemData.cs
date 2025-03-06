using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon")]
public class WeaponItemData : ItemData
{
    [field: SerializeField] public AmmoItemData Ammo { get; private set; }
    [field: SerializeField, Min(0)] public float Damage { get; private set; }

    public override ItemType Type => ItemType.Weapon;
}