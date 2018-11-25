using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace XrmEntitySerializer
{
    internal class ValueType
    {
        internal static object GetT(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer, Func<JsonReader, Type, object, JsonSerializer, object> parser)
        {
            object result = null;
            bool parsed = false;
            for (int i = 0; i < 2; i++)
            {
                reader.Read();

                if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$value")
                {
                    reader.Read(); 
                    result = parser(reader, objectType, existingValue, serializer);
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
}
