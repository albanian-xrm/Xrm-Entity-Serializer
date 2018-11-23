﻿using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XrmEntitySerializer
{
    public class KeyAttributeCollectionConverter : JsonConverter<KeyAttributeCollection>
    {
        public override KeyAttributeCollection ReadJson(JsonReader reader, Type objectType, KeyAttributeCollection existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray jArray = JArray.Load(reader);
            foreach (var item in jArray)
            {
                KeyValuePair<string, object> pair = item.ToObject<KeyValuePair<string, object>>(serializer);
                existingValue.Add(pair.Key, pair.Value);
            }
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, KeyAttributeCollection value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
