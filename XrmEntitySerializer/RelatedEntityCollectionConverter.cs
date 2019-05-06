using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace XrmEntitySerializer
{
    public class RelatedEntityCollectionConverter : CollectionTypeConverter<RelatedEntityCollection>
    {
        protected override RelatedEntityCollection ReadCollection(JsonReader reader, Type objectType, RelatedEntityCollection existingRelatedEntities, JsonSerializer serializer, JArray jArray)
        {
            if (existingRelatedEntities == null)
            {
                existingRelatedEntities = new RelatedEntityCollection();
            }
            foreach (JToken item in jArray)
            {
                KeyValuePair<Relationship, EntityCollection> pair = item.ToObject<KeyValuePair<Relationship, EntityCollection>>(serializer);
                existingRelatedEntities.Add(pair.Key, pair.Value);
            }
            return existingRelatedEntities;
        }

        protected override void WriteCollection(JsonWriter writer, RelatedEntityCollection relatedEntities, JsonSerializer serializer)
        {
            foreach (KeyValuePair<Relationship, EntityCollection> attribute in relatedEntities)
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