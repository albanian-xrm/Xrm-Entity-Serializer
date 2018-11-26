using Newtonsoft.Json;
using System;
using System.Globalization;

namespace XrmEntitySerializer
{
    public class GuidConverter : ValueTypeConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Guid) || objectType == typeof(Nullable<Guid>);
        }

        protected override object ReadValue(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Guid result;
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType != typeof(Guid?))
                {
                    throw new JsonSerializationException(String.Format(CultureInfo.InvariantCulture, "Cannot convert null value to {0}.", objectType));
                }
                return default(Guid?);
            }
            else if (!Guid.TryParse(reader.Value.ToString(), out result))
            {
                throw new JsonSerializationException(string.Format(CultureInfo.InvariantCulture, "Unexpected token or value when parsing Guid. Token: {0}, Value: {1}", reader.TokenType, reader.Value));
            }
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
