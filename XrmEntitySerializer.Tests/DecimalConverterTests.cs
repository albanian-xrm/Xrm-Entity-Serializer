using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace XrmEntitySerializer.Tests
{
    [ExcludeFromCodeCoverage]
    public class DecimalConverterTests
    {
        [Fact]
        public void DecimalCanBeSerializedAndDeserialized()
        {
            object value = 2m;
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new DecimalConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), value);
            }

            object deserializedValue;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedValue = serializer.Deserialize(new JsonTextReader(reader), typeof(decimal));
            }

            Assert.Equal(value, deserializedValue);
            Assert.Equal(value.GetType(), deserializedValue.GetType());
        }

        [Fact]
        public void NullDecimalCanBeSerializedAndDeserialized()
        {
            decimal? value = null;
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new DecimalConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write("{\"$type\": \"System.Decimal, mscorlib\", \"$value\": null}");
            }

            decimal? deserializedValue;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedValue = (decimal?)serializer.Deserialize(new JsonTextReader(reader), typeof(decimal?));
            }

            Assert.Null(deserializedValue);
            Assert.Equal(value, deserializedValue);
        }

        [Fact]
        public void DeserializationOfNullDecimalThrows()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new DecimalConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write("{\"$type\": \"System.Decimal, mscorlib\", \"$value\": null}");
            }

            Assert.Throws<JsonSerializationException>(() =>
            {
                object deserializedEntity;
                memoryStream = new MemoryStream(memoryStream.ToArray());
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    deserializedEntity = serializer.Deserialize(new JsonTextReader(reader), typeof(decimal));
                }
            });
        }

        [Theory]
        [InlineData("{\"$type\": \"System.Decimal, mscorlib\", \"$value\": \"Not a Decimal\"}")]
        [InlineData("{\"$type\": \"System.Decimal, mscorlib\"}")]
        public void DeserializationOfWronglyFormattedDecimalThrows(string serialized)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new DecimalConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write(serialized);
            }

            Assert.Throws<JsonSerializationException>(() =>
            {
                object deserializedEntity;
                memoryStream = new MemoryStream(memoryStream.ToArray());
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    deserializedEntity = serializer.Deserialize(new JsonTextReader(reader), typeof(decimal));
                }
            });
        }
    }
}
