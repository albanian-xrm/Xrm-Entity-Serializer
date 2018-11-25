using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XrmEntitySerializer
{
    public class FormattedValueCollectionConverter : CollectionTypeConverter<FormattedValueCollection>
    {


        protected override FormattedValueCollection ReadCollection(JsonReader reader, Type objectType, FormattedValueCollection existingFormattedValues, JsonSerializer serializer, JArray jArray)
        {
            foreach (JToken item in jArray)
            {
                KeyValuePair<string, string> pair = item.ToObject<KeyValuePair<string, string>>(serializer);
                existingFormattedValues.Add(pair.Key, pair.Value);
            }
            return existingFormattedValues;
        }

        protected override void WriteCollection(JsonWriter writer, FormattedValueCollection formattedValues, JsonSerializer serializer)
        {
            foreach (KeyValuePair<string, string> attribute in formattedValues)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Key");
                writer.WriteValue(attribute.Key);
                writer.WritePropertyName("Value");
                writer.WriteValue(attribute.Value);
                writer.WriteEndObject();
            }
        }
    }
}
