using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XrmEntitySerializer
{
    public class AttributeCollectionConverter : JsonConverter<AttributeCollection>
    {
        public override AttributeCollection ReadJson(JsonReader reader, Type objectType, AttributeCollection existingAttributes, bool hasExistingValue, JsonSerializer serializer)
        {
            for (int i = 0; i < 2; i++)
            {
                reader.Read();

                if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$value")
                {
                    reader.Read(); 
                    JArray jArray = JArray.Load(reader);
                    foreach (JToken item in jArray)
                    {
                        KeyValuePair<string, object> pair = item.ToObject<KeyValuePair<string, object>>(serializer);
                        existingAttributes.Add(pair.Key, pair.Value);
                    }
                }
                else
                {
                    reader.Read();          
                }
            }

            reader.Read();
           
            return existingAttributes;
        }

        public override void WriteJson(JsonWriter writer, AttributeCollection attributes, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$type");
            writer.WriteValue(string.Format("{0}, {1}", typeof(AttributeCollection).FullName, typeof(AttributeCollection).Assembly.GetName().Name));
            writer.WritePropertyName("$value");
            writer.WriteStartArray();
            foreach (KeyValuePair<string, object> attribute in attributes)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Key");
                writer.WriteValue(attribute.Key);
                writer.WritePropertyName("Value");
                serializer.Serialize(writer, attribute.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
