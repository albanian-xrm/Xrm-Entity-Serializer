using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XrmEntitySerializer
{
#if !XRM_7
    public class KeyAttributeCollectionConverter : JsonConverter<KeyAttributeCollection>
    {
        public override KeyAttributeCollection ReadJson(JsonReader reader, Type objectType, KeyAttributeCollection existingKeyAttributes, bool hasExistingValue, JsonSerializer serializer)
        {
            for (int i = 0; i < 2; i++)
            {
                reader.Read();

                if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$value")
                {
                    reader.Read();
                    JArray jArray = JArray.Load(reader);
                    foreach (var item in jArray)
                    {
                        KeyValuePair<string, object> pair = item.ToObject<KeyValuePair<string, object>>(serializer);
                        existingKeyAttributes.Add(pair.Key, pair.Value);
                    }
                }
                else
                {
                    reader.Read();
                }
            }

            reader.Read();
            return existingKeyAttributes;
        }

        public override void WriteJson(JsonWriter writer, KeyAttributeCollection keyAttributes, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$type");
            writer.WriteValue(string.Format("{0}, {1}", typeof(FormattedValueCollection).FullName, typeof(FormattedValueCollection).Assembly.GetName().Name));
            writer.WritePropertyName("$value");
            writer.WriteStartArray();
            foreach (KeyValuePair<string, object> attribute in keyAttributes)
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
#endif
}
