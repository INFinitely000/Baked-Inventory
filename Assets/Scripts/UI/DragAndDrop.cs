using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class DragAndDrop : MonoBehaviour, IInventoryOwner
{
    [SerializeField] private GraphicRaycaster m_raycaster;
    [SerializeField] private InventorySlotUI m_slotUI;

    public Inventory Inventory { get; private set; }



    private void OnEnable()
    {
        Inventory = new Inventory(1);

        Inventory.UnlockSlot(0);

        Refresh(default);

        Inventory.Changed += Refresh;
    }

    private void OnDisable()
    {
        Inventory.Changed -= Refresh;
    }



    private void Update()
    {
        if (m_slotUI.isActiveAndEnabled)
            m_slotUI.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);


        if (Input.GetMouseButtonDown(1))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            pointerData.position = Input.mousePosition;
            m_raycaster.Raycast(pointerData, results);

            if (results.Count > 0 && results[0].gameObject.TryGetComponent<InventorySlotUI>(out var slot))
            {
                if (slot.Inventory.IsUnlocked(slot.Index) == false)
                {
                    if (slot.Inventory is PurchasableInventory purchasable)
                        purchasable.PurchaseSlot(slot.Index);
                    else
                        slot.Inventory.UnlockSlot(slot.Index);
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            pointerData.position = Input.mousePosition;
            m_raycaster.Raycast(pointerData, results);

            if (results.Count > 0 && results[0].gameObject.TryGetComponent<InventorySlotUI>(out var slot) && slot.Inventory.IsUnlocked(slot.Index))
            {
                if (Inventory.Has(0))
                {
                    if (slot.Inventory.TryTake(slot.Index, out var inventoryItem))
                    {
                        Inventory.TryTake(0, out var dragItem);

                        if (inventoryItem.data == dragItem.data)
                        {
                            var difference = Mathf.Min(dragItem.data.MaxAmount - dragItem.Amount, inventoryItem.Amount);

                            dragItem.Amount += difference;
                            inventoryItem.Amount -= difference;
                        }

                        slot.Inventory.TryAdd( dragItem, slot.Index );

                        if (inventoryItem.Amount > 0)
                            Inventory.TryAdd( inventoryItem, 0 );
                    }
                    else
                    {
                        Inventory.TryTake(0, out var dragItem);

                        slot.Inventory.TryAdd(dragItem, slot.Index);
                    }
                }
                else
                {
                    if (slot.Inventory.TryTake(slot.Index, out var inventoryItem))
                    {
                        Inventory.TryAdd(inventoryItem, 0);
                    }
                }
            }

            m_slotUI.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }


    private void Refresh(InventoryActionData data)
    {
        if (Inventory.Has(0))
        {
            m_slotUI.gameObject.SetActive(true);
            m_slotUI.Refresh(Inventory.Data.slots[0], Inventory, 0);
        }
        else
        {
            m_slotUI.gameObject.SetActive(false);
        }
    }
}
