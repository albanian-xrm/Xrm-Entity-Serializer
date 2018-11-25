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
            return ValueType.GetT(reader, objectType, existingValue, serializer, (_reader, _objectType, _existingValue, _serializer) =>
            {
                Guid result;
                if (_reader.TokenType == JsonToken.Null)
                {
                    if (_objectType != typeof(Guid?))
                    {
                        throw new JsonSerializationException(String.Format(CultureInfo.InvariantCulture, "Cannot convert null value to {0}.", _objectType));
                    }
                    return default(Guid?);
                }
                else if (!Guid.TryParse(_reader.Value.ToString(), out result))
                {
                    throw new JsonSerializationException(string.Format(CultureInfo.InvariantCulture, "Unexpected token or value when parsing Guid. Token: {0}, Value: {1}", _reader.TokenType, _reader.Value));
                }
                return result;
            });
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
