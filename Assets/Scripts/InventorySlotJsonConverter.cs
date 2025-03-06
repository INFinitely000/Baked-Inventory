using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


public class InventorySlotJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(InventorySlot);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var inventorySlot = (InventorySlot)value;
        var obj = new JObject();

        obj.Add("IsUnlocked", inventorySlot.isUnlocked);

        if (inventorySlot.item.HasValue)
            obj.Add("Item", JToken.FromObject(inventorySlot.item.Value));
        
        obj.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);

        bool isUnlocked = (bool)obj["IsUnlocked"];

        JToken token;
        Item? item = null;

        if ((token = obj["Item"]) != null)
        {
            item = token.ToObject<Item>();

            UnityEngine.Debug.Log("Я умный");
        }

        return new InventorySlot(item, isUnlocked);
    }
}