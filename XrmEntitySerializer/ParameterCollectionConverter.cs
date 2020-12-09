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
                var value = item.SelectToken("value");
                KeyValuePair<string, object> pair;
                if (value?.SelectToken("__type")?.ToString()?.StartsWith("Entity:") == true)
                {
                    var deserialized = item.ToObject<KeyValuePair<string, Entity>>(serializer);
                    pair = new KeyValuePair<string, object>(deserialized.Key, deserialized.Value);
                }
                else if (value?.SelectToken("__type")?.ToString()?.StartsWith("EntityReference:") == true)
                {
                    var deserialized = item.ToObject<KeyValuePair<string, EntityReference>>(serializer);
                    pair = new KeyValuePair<string, object>(deserialized.Key, deserialized.Value);
                }
                else
                {
                    pair = item.ToObject<KeyValuePair<string, object>>(serializer);
                }

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