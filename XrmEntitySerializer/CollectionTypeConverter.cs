using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XrmEntitySerializer
{
    public abstract class CollectionTypeConverter<T> : JsonConverter<T>
    {
        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            T result = default(T);
            bool parsed = false;
            for (int i = 0; i < 2; i++)
            {
                reader.Read();
                if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$value")
                {
                    reader.Read();
                    JArray jArray = JArray.Load(reader);
                    result = ReadCollection(reader, objectType, existingValue, serializer, jArray);
                    parsed = true;
                }
                else
                {
                    reader.Read();
                }
            }

            reader.Read();
            if (!parsed)
            {
                throw new JsonSerializationException("Could not find a property $value");
            }
            return result;
        }

        protected abstract T ReadCollection(JsonReader reader, Type objectType, T existingValue, JsonSerializer serializer, JArray jArray);

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$type");
            writer.WriteValue(string.Format("{0}, {1}", typeof(T).FullName, typeof(T).Assembly.GetName().Name));
            writer.WritePropertyName("$value");
            writer.WriteStartArray();
            WriteCollection(writer, value, serializer);
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        protected abstract void WriteCollection(JsonWriter writer, T value, JsonSerializer serializer);
    }
}
