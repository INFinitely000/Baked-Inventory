using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class ItemJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Item);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var item = (Item)value;
        var obj = new JObject();
        
        obj.Add("Data", item.data.GUID.ToString());
        obj.Add("Amount", item.Amount);
        obj.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);         
        
        string GUID = (string)obj["Data"];
        int amount = (int)obj["Amount"];
        
        if (ItemData.All.TryGetValue(GUID, out var data))
        {
            return new Item(data, amount);
        }
        else
        {
            throw new Exception("Can't deserialize item: data GUID is incorrect"); 
        }
    }
}