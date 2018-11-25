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
        public override RelatedEntityCollection ReadJson(JsonReader reader, Type objectType, RelatedEntityCollection existingRelatedEntities, bool hasExistingValue, JsonSerializer serializer)
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
                        KeyValuePair<Relationship, EntityCollection> pair = item.ToObject<KeyValuePair<Relationship, EntityCollection>>(serializer);
                        existingRelatedEntities.Add(pair.Key, pair.Value);
                    }
                }
                else
                {
                    reader.Read();
                }
            }

            reader.Read();
            return existingRelatedEntities;
        }

        public override void WriteJson(JsonWriter writer, RelatedEntityCollection relatedEntities, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$type");
            writer.WriteValue(string.Format("{0}, {1}", typeof(FormattedValueCollection).FullName, typeof(FormattedValueCollection).Assembly.GetName().Name));
            writer.WritePropertyName("$value");
            writer.WriteStartArray();
            foreach (KeyValuePair<Relationship, EntityCollection> attribute in relatedEntities)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Key");
                serializer.Serialize(writer, attribute.Key);
                writer.WritePropertyName("Value");
                serializer.Serialize(writer, attribute.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
