using System;
using System.Data.Common;
using UnityEngine;



public class Inventory
{
    public InventoryData Data => m_data;
    public int Size => Data.slots.Length;

    private InventoryData m_data;

    public event Action<InventoryActionData> Changed;




    #region Add

    public virtual bool TryAdd(Item item, out int rest) => TryAdd(item.data, item.Amount, out rest);

    public virtual bool TryAdd(Item item) => TryAdd(item.data, item.Amount, out int rest);

    public virtual bool TryAdd(ItemData data, int amount) => TryAdd(data, amount, out int rest);

    public virtual bool TryAdd(ItemData data, int amount, out int rest)
    {
        rest = 0;

        if (amount < 1) return false;
        if (data == null) throw new System.ArgumentNullException(nameof(data));

        int remain = amount;

        // Заполняю уже существующие слоты с таким типом предмета
        for (int index = 0; index < m_data.slots.Length; index++)
        {
            if (remain < 1) break;

            if (m_data.slots[index].item.HasValue && 
                m_data.slots[index].item.Value.data == data && 
                m_data.slots[index].item.Value.Amount < m_data.slots[index].item.Value.data.MaxAmount)
            {
                TryAdd(data, amount, index, out var slotRest);

                remain -= amount - slotRest;
            }
        }

        // Заполняю пустые слоты оставшимися единицами предмета
        while (remain > 0 && m_data.firstEmptySlotIndex != -1)
        {
            var difference = Mathf.Min( remain, data.MaxAmount );

            m_data.slots[m_data.firstEmptySlotIndex].item = new Item(data, difference);

            remain -= difference;

            Changed?.Invoke(new InventoryActionData(m_data.firstEmptySlotIndex, amount - rest, this, InventoryActionType.Add));

            FindNextEmptySlot();
        }

        rest = remain;

        return true;
    }

    public virtual bool TryAdd(Item item, int slotIndex) => TryAdd(item.data, item.Amount, slotIndex, out int rest);

    public virtual bool TryAdd(ItemData data, int amount, int slotIndex, out int rest)
    {
        rest = 0;

        if (data == null) throw new ArgumentNullException(nameof(data));

        if (slotIndex < 0 || slotIndex >= Data.slots.Length) throw new ArgumentOutOfRangeException(nameof(slotIndex));

        if (Data.slots[slotIndex].isUnlocked == false) return false;

        var remain = amount;

        if (m_data.slots[slotIndex].item.HasValue)
        {
            if (m_data.slots[slotIndex].item.Value.data == data)
            {
                if (m_data.slots[slotIndex].item.Value.Amount < data.MaxAmount)
                {
                    remain += m_data.slots[slotIndex].item.Value.Amount;
                }
            }
            else
            {
                return false;
            }
        }

        rest = Mathf.Max(remain - data.MaxAmount, 0);
        remain = Mathf.Min(remain, data.MaxAmount);

        m_data.slots[slotIndex].item = new Item(data, remain);

        FindNextEmptySlot();

        Changed?.Invoke(new InventoryActionData(slotIndex, amount - rest, this, InventoryActionType.Add));

        return true;
    }

    #endregion


    #region Take

    public virtual bool TryTake(ItemData data, int amount, out int rest)
    {
        if (amount < 1) throw new ArgumentOutOfRangeException(nameof(amount));
        if (data == null) throw new System.ArgumentNullException(nameof(data));

        int remain = amount;

        for (int index = 0; index < Data.slots.Length; index++)
        {
            if (m_data.slots[index].item.HasValue && m_data.slots[index].item.Value.data == data)
            {
                TryTake(index, remain, out Item item, out int slotRest);

                remain -= amount - slotRest;

                if (remain == 0) break;
            }
        }

        rest = remain;

        return rest != amount;
    }

    public virtual bool TryTake(int slotIndex, out Item item) => TryTake(slotIndex, int.MaxValue, out item, out int rest);

    public virtual bool TryTake(int slotIndex, int amount, out Item item, out int rest)
    {
        rest = 0;
        item = default;

        if (slotIndex < 0 || slotIndex >= Data.slots.Length) throw new ArgumentOutOfRangeException(nameof(slotIndex));

        if (Data.slots[slotIndex].isUnlocked == false || Data.slots[slotIndex].item.HasValue == false) return false;

        var slotItem = m_data.slots[slotIndex].item.Value;

        var remain = Mathf.Min(amount, slotItem.Amount);

        slotItem.Amount -= remain;

        if (slotItem.Amount > 0)
        {
            m_data.slots[slotIndex].item = slotItem;
        }
        else
        {
            m_data.slots[slotIndex].item = null;

            if (m_data.firstEmptySlotIndex == -1)
                m_data.firstEmptySlotIndex = slotIndex;
            else
                m_data.firstEmptySlotIndex = Mathf.Min(m_data.firstEmptySlotIndex, slotIndex);
        }

        rest = amount - remain;
        item = new Item(slotItem.data, remain);

        Changed?.Invoke(new InventoryActionData(slotIndex, amount - rest, this, InventoryActionType.Take));

        return rest != amount;
    }

    #endregion


    public virtual bool Has(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Data.slots.Length) throw new ArgumentOutOfRangeException(nameof(slotIndex));

        return m_data.slots[slotIndex].item.HasValue;
    }

    public virtual bool IsUnlocked(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Data.slots.Length) throw new ArgumentOutOfRangeException(nameof(slotIndex));

        return m_data.slots[slotIndex].isUnlocked;
    }


    public virtual void UnlockSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Data.slots.Length) throw new ArgumentOutOfRangeException(nameof(slotIndex));

        Data.slots[slotIndex].isUnlocked = true;

        if (m_data.firstEmptySlotIndex == -1)
            m_data.firstEmptySlotIndex = slotIndex;
        else
            m_data.firstEmptySlotIndex = Mathf.Min(m_data.firstEmptySlotIndex, slotIndex);

        Changed?.Invoke( new InventoryActionData( slotIndex, 0, this, InventoryActionType.Unlock ) );
    }


    private void FindNextEmptySlot()
    {
        if (m_data.slots[Data.firstEmptySlotIndex].item.HasValue == false && m_data.slots[Data.firstEmptySlotIndex].isUnlocked == true) return;

        for (int index = m_data.firstEmptySlotIndex; index < m_data.slots.Length; index++)
        {
            if (m_data.slots[index].item.HasValue == false && m_data.slots[index].isUnlocked == true)
            {
                m_data.firstEmptySlotIndex = index;

                return;
            }
        }

        m_data.firstEmptySlotIndex = -1;
    }



    public Inventory(int size)
    {
        m_data = new InventoryData(size, this);

        FindNextEmptySlot();
    }


    public Inventory(InventoryData data)
    {
        m_data = data;

        m_data.inventory = this;

        FindNextEmptySlot();
    }


    public void Load(InventoryData data)
    {
        m_data = data;

        m_data.inventory = this;

        FindNextEmptySlot();

        Changed?.Invoke(new InventoryActionData(0, 0, this, InventoryActionType.Reload));
    }

    protected Inventory() { }
}
