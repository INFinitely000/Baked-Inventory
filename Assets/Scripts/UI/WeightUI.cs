using System.Linq;
using TMPro;
using UnityEngine;


public class WeightUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_weightBar;


    private void Awake()
    {
        Game.Inventory.Changed += OnChanged;
    }

    private void OnDestroy()
    {
        if (Game.Inventory != null)
            Game.Inventory.Changed -= OnChanged;
    }

    private void OnChanged(InventoryActionData data) => OnChanged();

    private void OnChanged()
    {
        m_weightBar.text = Game.Inventory.Data.slots.Sum( s => s.item.HasValue ? s.item.Value.data.Weigth : 0).ToString() + " kg";
    }
}
