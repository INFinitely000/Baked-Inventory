using UnityEngine;

[CreateAssetMenu(fileName = "Helmet", menuName = "Inventory/Helmet")]
public class HelmetItemData : ItemData
{
    [field: SerializeField] public float Protection { get; private set; }

    public override ItemType Type => ItemType.Helmet;
}