using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Globalization;

namespace XrmEntitySerializer
{
    public class OptionSetValueConverter : ValueTypeConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OptionSetValue);
        }

        protected override object ReadValue(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int value;
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else if (!int.TryParse(reader.Value.ToString(), out value))
            {
                throw new JsonSerializationException(string.Format(CultureInfo.InvariantCulture, "Unexpected token or value when parsing Guid. Token: {0}, Value: {1}", reader.TokenType, reader.Value));
            }

            return new OptionSetValue(value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$type");
            writer.WriteValue(string.Format("{0}, {1}", value.GetType().FullName, value.GetType().Assembly.GetName().Name));
            writer.WritePropertyName("$value");
            writer.WriteValue(((OptionSetValue)value).Value);
            writer.WriteEndObject();
        }
    }
}