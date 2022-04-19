﻿using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace XrmEntitySerializer
{
    public class AttributeCollectionConverter : CollectionTypeConverter<AttributeCollection>
    {
        protected override AttributeCollection ReadCollection(JsonReader reader, Type objectType, AttributeCollection existingAttributes, JsonSerializer serializer, JArray jArray)
        {
            bool checkExisting = true;
            if (existingAttributes == null)
            {
                checkExisting = false;
                existingAttributes = new AttributeCollection();
            }
            foreach (JToken item in jArray)
            {
                KeyValuePair<string, object> pair = item.ToObject<KeyValuePair<string, object>>(serializer);
                if (checkExisting && existingAttributes.ContainsKey(pair.Key))
                {
                    existingAttributes[pair.Key] = pair.Value;
                }
                else
                {
                    existingAttributes.Add(pair.Key, pair.Value);
                }
            }
            return existingAttributes;
        }

        protected override void WriteCollection(JsonWriter writer, AttributeCollection attributes, JsonSerializer serializer)
        {
            foreach (KeyValuePair<string, object> attribute in attributes)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Key");
                writer.WriteValue(attribute.Key);
                writer.WritePropertyName("Value");
                serializer.Serialize(writer, attribute.Value);
                writer.WriteEndObject();
            }
        }
    }
}