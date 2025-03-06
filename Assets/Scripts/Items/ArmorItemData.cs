using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Inventory/Armor")]
public class ArmorItemData : ItemData
{
    [field: SerializeField, Min(0)] public float Protection { get; private set; }

    public override ItemType Type => ItemType.Armor;
}