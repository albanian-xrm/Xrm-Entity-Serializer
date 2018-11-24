using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace XrmEntitySerializer
{
    public class GuidConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Guid) || objectType == typeof(Nullable<Guid>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType != typeof(Guid?))
                {
                    throw new JsonSerializationException(String.Format(CultureInfo.InvariantCulture, "Cannot convert null value to {0}.", objectType));
                }
                return default(Guid?);
            }

            Guid result = default(Guid);

            for (int i = 0; i < 2; i++)
            {
                reader.Read();

                if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$value")
                {
                    reader.Read(); //reader.Value == System.Guid
                    if (!Guid.TryParse(reader.Value.ToString(), out result))
                    {
                        throw new JsonSerializationException(string.Format(CultureInfo.InvariantCulture, "Unexpected token or value when parsing Guid. Token: {0}, Value: {1}", reader.TokenType, reader.Value));
                    }
                }
                else
                {
                    reader.Read(); //reader.Value == System.Guid, mscorlib             
                }
            }

            reader.Read();
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$type");
            writer.WriteValue(string.Format("{0}, {1}", typeof(System.Guid).FullName, typeof(System.Guid).Assembly.GetName().Name));
            writer.WritePropertyName("$value");
            writer.WriteValue(value);
            writer.WriteEndObject();
        }
    }
}
