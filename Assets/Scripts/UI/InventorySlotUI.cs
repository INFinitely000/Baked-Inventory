using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image m_icon;
    [SerializeField] private Image m_frame;
    [SerializeField] private TextMeshProUGUI m_amountBar;

    public InventorySlot Slot { get; private set; }
    public Inventory Inventory { get; private set; }
    public int Index { get; private set; }
    

    public void Refresh(InventorySlot slot, Inventory inventory, int index)
    {
        if (slot.item.HasValue)
        {
            m_icon.sprite = slot.item.Value.data.Icon;
            m_icon.enabled = true;
            m_amountBar.text = slot.item.Value.Amount > 1 ? slot.item.Value.Amount.ToString() : string.Empty;
        }
        else
        {
            m_icon.sprite = null;
            m_icon.enabled = false;
            m_amountBar.text = string.Empty;
        }

        Index = index;
        Inventory = inventory;

        m_icon.color = slot.isUnlocked ? Color.white : Color.grey;
        m_frame.color = slot.isUnlocked ? Color.white : Color.grey;

        Slot = slot;
    }
}
