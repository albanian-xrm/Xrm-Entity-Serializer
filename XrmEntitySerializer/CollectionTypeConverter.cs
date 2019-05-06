using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace XrmEntitySerializer
{
    /// <summary>
    /// Base class to convert collection types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CollectionTypeConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            T result = default(T);
            bool parsed = false;
            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray jArray = JArray.Load(reader);
                result = ReadCollection(reader, objectType, (T)existingValue, serializer, jArray);
            }

            else
            {
                for (int i = 0; i < 2; i++)
                {
                    reader.Read();
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$value")
                    {
                        reader.Read();
                        JArray jArray = JArray.Load(reader);
                        result = ReadCollection(reader, objectType, (T)existingValue, serializer, jArray);
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
            }
            return result;
        }

        protected abstract T ReadCollection(JsonReader reader, Type objectType, T existingValue, JsonSerializer serializer, JArray jArray);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$type");
            writer.WriteValue(string.Format("{0}, {1}", typeof(T).FullName, typeof(T).Assembly.GetName().Name));
            writer.WritePropertyName("$value");
            writer.WriteStartArray();
            WriteCollection(writer, (T)value, serializer);
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        protected abstract void WriteCollection(JsonWriter writer, T value, JsonSerializer serializer);
    }
}
