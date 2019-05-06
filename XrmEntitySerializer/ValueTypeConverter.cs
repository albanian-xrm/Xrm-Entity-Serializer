using Newtonsoft.Json;
using System;

namespace XrmEntitySerializer
{
    /// <summary>
    /// Base class to convert value types
    /// </summary>
    public abstract class ValueTypeConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else
            {
                object result = null;
                bool parsed = false;

                for (int i = 0; i < 2; i++)
                {
                    reader.Read();

                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$value")
                    {
                        reader.Read();
                        result = ReadValue(reader, objectType, existingValue, serializer);
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
        }

        protected abstract object ReadValue(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);
    }
}