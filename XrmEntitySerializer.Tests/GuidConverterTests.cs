using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace XrmEntitySerializer.Tests
{
    [ExcludeFromCodeCoverage]
    public class GuidConverterTests
    {
        [Fact]
        public void GuidCanBeSerializedAndDeserialized()
        {
            object guid = Guid.NewGuid();
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new GuidConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), guid);
            }

            object deserializedGuid;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedGuid = serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(guid, deserializedGuid);
        }

        [Fact]
        public void DeserializationOfNullableGuid()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new GuidConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write("{\"$type\": \"System.Guid, mscorlib\", \"$value\": null}");
            }

            object deserializedEntity;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedEntity = serializer.Deserialize(new JsonTextReader(reader), typeof(Guid?));
            }

            Assert.Equal(default(Guid?), deserializedEntity);
        }

        [Fact]
        public void DeserializationOfNullGuidThrows()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new GuidConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write("{\"$type\": \"System.Guid, mscorlib\", \"$value\": null}");
            }

            Assert.Throws<JsonSerializationException>(() =>
            {
                object deserializedEntity;
                memoryStream = new MemoryStream(memoryStream.ToArray());
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    deserializedEntity = serializer.Deserialize(new JsonTextReader(reader), typeof(Guid));
                }
            });
        }

        [Fact]
        public void DeserializationOfWronglyFormattedGuidThrows()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new GuidConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write("{\"$type\": \"System.Guid, mscorlib\", \"$value\": \"Not a Guid\"}");
            }

            Assert.Throws<JsonSerializationException>(() =>
            {
                object deserializedEntity;
                memoryStream = new MemoryStream(memoryStream.ToArray());
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    deserializedEntity = serializer.Deserialize(new JsonTextReader(reader), typeof(Guid));
                }
            });
        }

        [Fact]
        public void DeserializationOfGuidWithMissingValuePropertyThrows()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new GuidConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write("{\"$type\": \"System.Guid, mscorlib\"}");
            }

            Assert.Throws<JsonSerializationException>(() =>
            {
                object deserializedEntity;
                memoryStream = new MemoryStream(memoryStream.ToArray());
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    deserializedEntity = serializer.Deserialize(new JsonTextReader(reader), typeof(Guid));
                }
            });
        }
    }
}
