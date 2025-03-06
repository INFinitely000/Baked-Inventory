using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class ItemData : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField, Min(1)] public int MaxAmount { get; private set; }
    [field: SerializeField, Min(0)] public float Weigth { get; private set; }
    
    public abstract ItemType Type { get; }


    public string GUID { get; private set; }



    private void Awake()
    {
        GUID = System.Guid.NewGuid().ToString();
    }



    #region Static

    public static Dictionary<string, ItemData> All { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void LoadItems()
    {
        All = Resources.LoadAll<ItemData>("").ToDictionary(i => i.GUID);
    }

    #endregion
}
