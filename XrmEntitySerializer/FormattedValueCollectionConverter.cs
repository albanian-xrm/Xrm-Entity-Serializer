using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XrmEntitySerializer
{
    public class FormattedValueCollectionConverter : JsonConverter<FormattedValueCollection>
    {
        public override FormattedValueCollection ReadJson(JsonReader reader, Type objectType, FormattedValueCollection existingFormattedValues, bool hasExistingValue, JsonSerializer serializer)
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
                        KeyValuePair<string, string> pair = item.ToObject<KeyValuePair<string, string>>(serializer);
                        existingFormattedValues.Add(pair.Key, pair.Value);
                    }
                }
                else
                {
                    reader.Read();
                }
            }

            reader.Read();
            return existingFormattedValues;
        }

        public override void WriteJson(JsonWriter writer, FormattedValueCollection formattedValues, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$type");
            writer.WriteValue(string.Format("{0}, {1}", typeof(FormattedValueCollection).FullName, typeof(FormattedValueCollection).Assembly.GetName().Name));
            writer.WritePropertyName("$value");
            writer.WriteStartArray();
            foreach (KeyValuePair<string, string> attribute in formattedValues)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Key");
                writer.WriteValue(attribute.Key);
                writer.WritePropertyName("Value");
                writer.WriteValue(attribute.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
