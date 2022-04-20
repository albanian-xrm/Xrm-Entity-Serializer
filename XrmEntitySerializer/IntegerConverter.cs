using System;
using System.Globalization;
using Newtonsoft.Json;

namespace XrmEntitySerializer
{
    public class IntegerConverter : ValueTypeConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(int) == objectType || typeof(int?) == objectType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$type");
            writer.WriteValue(string.Format("{0}, {1}", value.GetType().FullName, value.GetType().Assembly.GetName().Name));
            writer.WritePropertyName("$value");
            writer.WriteValue(value.ToString());
            writer.WriteEndObject();
        }

        protected override object ReadValue(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int result;
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType != typeof(int?))
                {
                    throw new JsonSerializationException(String.Format(CultureInfo.InvariantCulture, "Cannot convert null value to {0}.", objectType));
                }
                return default(int?);
            }
            else if (!int.TryParse(reader.Value.ToString(), out result))
            {
                throw new JsonSerializationException(string.Format(CultureInfo.InvariantCulture, "Unexpected token or value when parsing Integer. Token: {0}, Value: {1}", reader.TokenType, reader.Value));
            }
            return result;
        }
    }
}