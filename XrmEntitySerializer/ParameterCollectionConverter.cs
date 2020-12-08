using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace XrmEntitySerializer
{
    public class ParameterCollectionConverter : CollectionTypeConverter<ParameterCollection>
    {
        protected override ParameterCollection ReadCollection(JsonReader reader, Type objectType, ParameterCollection existingParameters, JsonSerializer serializer, JArray jArray)
        {
            if (existingParameters == null)
            {
                existingParameters = new ParameterCollection();
            }
            foreach (JToken item in jArray)
            {
                KeyValuePair<string, object> pair = item.ToObject<KeyValuePair<string, object>>(serializer);
                existingParameters.Add(pair.Key, pair.Value);
            }
            return existingParameters;
        }

        protected override void WriteCollection(JsonWriter writer, ParameterCollection existingParameters, JsonSerializer serializer)
        {
            foreach (KeyValuePair<string, object> attribute in existingParameters)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Key");
                serializer.Serialize(writer, attribute.Key);
                writer.WritePropertyName("Value");
                serializer.Serialize(writer, attribute.Value);
                writer.WriteEndObject();
            }
        }
    }
}