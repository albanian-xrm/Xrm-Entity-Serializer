using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XrmEntitySerializer
{
    public class RelatedEntityCollectionConverter : JsonConverter<RelatedEntityCollection>
    {
        public override RelatedEntityCollection ReadJson(JsonReader reader, Type objectType, RelatedEntityCollection existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray jArray = JArray.Load(reader);
            foreach (var item in jArray)
            {
                KeyValuePair<Relationship, EntityCollection> pair = item.ToObject<KeyValuePair<Relationship, EntityCollection>>(serializer);
                existingValue.Add(pair.Key, pair.Value);
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, RelatedEntityCollection value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
