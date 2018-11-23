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
        public override FormattedValueCollection ReadJson(JsonReader reader, Type objectType, FormattedValueCollection existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray jArray = JArray.Load(reader);
            foreach (var item in jArray)
            {
                KeyValuePair<string, string> pair = item.ToObject<KeyValuePair<string, string>>(serializer);
                existingValue.Add(pair.Key, pair.Value);
            }
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, FormattedValueCollection value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
