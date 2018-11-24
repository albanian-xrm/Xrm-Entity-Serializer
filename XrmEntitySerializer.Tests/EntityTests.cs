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
    public class EntityTests
    {
        [Fact]
        public void DeserializingAnEntityWithoutConvertersThrows()
        {
            Entity entity = new Entity("entity");
            entity.Id = Guid.NewGuid();
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), entity);
            }

            Assert.Throws<JsonSerializationException>(() =>
            {
                object deserializedEntity;
                memoryStream = new MemoryStream(memoryStream.ToArray());
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    deserializedEntity = serializer.Deserialize(new JsonTextReader(reader));
                }
            });
        }

        [Fact]
        public void EntityCanBeSerializedAndDeserialized()
        {
            Entity entity = new Entity("entity");
            entity.Id = Guid.NewGuid();
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new GuidConverter());
            serializer.Converters.Add(new AttributeCollectionConverter());
            serializer.Converters.Add(new FormattedValueCollectionConverter());
            serializer.Converters.Add(new RelatedEntityCollectionConverter());
#if !XRM_7
            serializer.Converters.Add(new KeyAttributeCollectionConverter());
#endif
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), entity);
            }

            object deserializedEntity;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedEntity = serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(entity.LogicalName, (deserializedEntity as Entity).LogicalName);
            Assert.Equal(entity.Id, (deserializedEntity as Entity).Id);
        }
    }
}
