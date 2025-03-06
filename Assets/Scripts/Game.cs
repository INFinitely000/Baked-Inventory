using Newtonsoft.Json;
using System.IO;
using UnityEngine;



public class Game : MonoBehaviour
{
    public static Game Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindFirstObjectByType<Game>();

            return m_instance;
        }
    }

    private static Game m_instance;

    [SerializeField, InterfaceType(typeof(IInventoryOwner))] private Object m_inventoryOwner;

    public IInventoryOwner InventoryOwner => m_inventoryOwner as IInventoryOwner;
    public static Inventory Inventory => Instance?.InventoryOwner?.Inventory;




    private void Start()
    {
        Wallet.Add(GameData.Instance.StartCoins);
    }


    [ContextMenu("Save")]
    public void Save()
    {
        var saveData = new SaveData(Inventory.Data, Wallet.Coins);

        var json = JsonConvert.SerializeObject(saveData);

        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        var json = File.ReadAllText(Application.persistentDataPath + "/save.json");

        var saveData = JsonConvert.DeserializeObject<SaveData>(json);

        Inventory.Load(saveData.inventoryData);

        Wallet.Clear();
        Wallet.Add(saveData.coins);
    }
}