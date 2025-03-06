using System.Linq;
using UnityEngine;


[CreateAssetMenu( fileName = "GameData", menuName = "GameData" )]
public class GameData : ScriptableObject
{
    [field: Header("Wallet")]
    [field: SerializeField] public int StartCoins { get; private set; } = 100;

    [field: Header("Inventory")]
    [field: SerializeField] public int InventorySlotCost { get; private set; } = 10;
    [field: SerializeField, Min(1)] public int InventorySize { get; private set; } = 30;
    [field: SerializeField, Min(0)] public int InventoryUnlockedSlots { get; private set; } = 15;




    private void OnValidate()
    {
        InventoryUnlockedSlots = Mathf.Min(InventorySize, InventoryUnlockedSlots);
    }


    #region Singleton

    public static GameData Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = Resources.LoadAll<GameData>("").FirstOrDefault();

            return m_instance;
        }
    }

    private static GameData m_instance;

    #endregion
}
