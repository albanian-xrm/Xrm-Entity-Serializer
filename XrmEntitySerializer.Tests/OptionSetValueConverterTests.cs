using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Xunit;

namespace XrmEntitySerializer.Tests
{
    [ExcludeFromCodeCoverage]
    public class OptionSetValueConverterTests
    {
        [Fact]
        public void OptionSetValueCanBeSerializedAndDeserialized()
        {
            object optionSetValue = new OptionSetValue(1);
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new OptionSetValueConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), optionSetValue);
            }

            object deserializedOptionSetValue;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedOptionSetValue = serializer.Deserialize(new JsonTextReader(reader), typeof(OptionSetValue));
            }

            Assert.Equal(optionSetValue, deserializedOptionSetValue);
        }

        [Fact]
        public void OptionSetAndOtherValuesCanBeSerializedAndDeserialized()
        {
            Entity entity = new Entity("Test");
            entity["optionsetvalue"] = new OptionSetValue(9);
            entity["decimal"] = 10m;
            entity["number"] = 2384;
#if !XRM_8 && !XRM_7 && !XRM_6 && !XRM_5
            OptionSetValueCollection optionSetValue = new OptionSetValueCollection();
            optionSetValue.Add(new OptionSetValue(10));
            optionSetValue.Add(new OptionSetValue(20));
            entity["multioptionset"] = optionSetValue;
#endif

            JsonSerializer serializer = new EntitySerializer();
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), entity);
            }

            Entity deserializedEntity;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedEntity = (Entity)serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(entity.Attributes.Count, deserializedEntity.Attributes.Count);
        }

        [Fact]
        public void DeserializationOfNullValue()
        {
            JsonSerializer serializer = new EntitySerializer();
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(writer, null);
            }

            object deserializedOptionSetValue;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedOptionSetValue = serializer.Deserialize(new JsonTextReader(reader), typeof(OptionSetValue));
            }

            Assert.Equal(default(OptionSetValue), deserializedOptionSetValue);
        }

        [Fact]
        public void DeserializationOfNullOptionSetValue()
        {
            JsonSerializer serializer = new EntitySerializer();
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write("{\"$type\": \"Microsoft.Xrm.Sdk.OptionSetValue, Microsoft.Xrm.Sdk\", \"$value\": null}");
            }

            object deserializedOptionSetValue;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedOptionSetValue = serializer.Deserialize(new JsonTextReader(reader), typeof(OptionSetValue));
            }

            Assert.Equal(default(OptionSetValue), deserializedOptionSetValue);
        }

        [Theory]
        [InlineData("{\"$type\": \"Microsoft.Xrm.Sdk.OptionSetValue, Microsoft.Xrm.Sdk\", \"$value\": \"Not an OptionSetValue\"}")]
        [InlineData("{\"$type\": \"Microsoft.Xrm.Sdk.OptionSetValue, Microsoft.Xrm.Sdk\"}")]
        public void DeserializationOfWronglyFormattedOptionSetValueThrows(string serialized)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new OptionSetValueConverter());

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
                    deserializedEntity = serializer.Deserialize(new JsonTextReader(reader), typeof(OptionSetValue));
                }
            });
        }

#if !XRM_8 && !XRM_7 && !XRM_6 && !XRM_5
        [Fact]
        public void OptionSetValueCollectionCanBeSerializedAndDeserialized()
        {
            OptionSetValueCollection optionSetValue = new OptionSetValueCollection();
            optionSetValue.Add(new OptionSetValue(1));
            optionSetValue.Add(new OptionSetValue(2));
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Converters.Add(new OptionSetValueCollectionConverter());
            serializer.Converters.Add(new OptionSetValueConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), optionSetValue);
            }

            object deserializedOptionSetValueCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedOptionSetValueCollection = serializer.Deserialize(new JsonTextReader(reader), typeof(OptionSetValueCollection));
            }

            Assert.Equal(optionSetValue.GetType(), deserializedOptionSetValueCollection.GetType());
            Assert.Equal(optionSetValue.Count, ((OptionSetValueCollection)deserializedOptionSetValueCollection).Count);
        }
#endif
    }
}
