using UnityEngine;


public class InventoryUI : MonoBehaviour
{
    [field: SerializeField] public Transform Content { get; private set; }
    [field: SerializeField] public InventorySlotUI ItemUIPrefab { get; private set; }


    private InventorySlotUI[] m_itemUIs;




    private void Awake()
    {
        m_itemUIs = new InventorySlotUI[ Game.Inventory.Size ];

        // Я люблю губку боба и запеченые пиксели
        foreach(var itemUI in m_itemUIs)
            if (itemUI)
                Destroy(itemUI.gameObject);

        for (int index = 0; index < Game.Inventory.Size; index++)
        {
            var createdItemUI = Instantiate( ItemUIPrefab, Content );

            createdItemUI.Refresh(Game.Inventory.Data.slots[index], Game.Inventory, index);                

            m_itemUIs[index] = createdItemUI;
        }

        Game.Inventory.Changed += Refresh;
    }

    private void OnDestroy()
    {
        if (Game.Inventory != null)
            Game.Inventory.Changed -= Refresh;
    }



    private void Refresh(InventoryActionData data) => Refresh();

    [ContextMenu("Refresh All")]
    public void Refresh()
    {
        for (int index = 0; index < Game.Inventory.Size; index++)
        {
            m_itemUIs[index].Refresh(Game.Inventory.Data.slots[index], Game.Inventory, index);
        }
    }
}
