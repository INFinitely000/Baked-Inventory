using Newtonsoft.Json;
using System;
using System.Linq;
using UnityEngine;


[JsonConverter(typeof(ItemJsonConverter))]
[JsonObject]
public struct Item
{
    public ItemData data;


    public int Amount
    {
        get
        {
            return m_amount;
        }
        set
        {
            if (m_amount > 0)
            {
                m_amount = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(m_amount));
            }
        }
    }
    
    private int m_amount;



    public Item(ItemData data, int amount)
    {
        this.data = data;
        
        m_amount = Mathf.Max(1, amount);
    }
}
