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

            EntityReference entityReference = new EntityReference("entityReference", Guid.NewGuid());
            entityReference.Name = "EntityReference";
            entity.Attributes.Add("entityReference", entityReference);
            entity.FormattedValues.Add("entityReference", entityReference.Name);
#if !XRM_7 && !XRM_6 && !XRM_5
            entity.KeyAttributes.Add("hello", "world");
#endif
            Relationship relationship = new Relationship("relationship");
            Entity relatedEntity = new Entity("entity");
            relatedEntity.Id = Guid.NewGuid();
            entity.RelatedEntities.Add(relationship, new EntityCollection(new List<Entity> { relatedEntity }));
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

            Assert.Equal(entity.LogicalName, deserializedEntity.LogicalName);
            Assert.Equal(entity.Id, deserializedEntity.Id);
            Assert.Equal(entity.Attributes.Count, deserializedEntity.Attributes.Count);
        }

        [Fact]
        public void DeserializationOfWronglyFormattedAttributeCollectionThrows()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new AttributeCollectionConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write("{\"$type\": \"Microsoft.Xrm.Sdk.FormattedValueCollection, Microsoft.Xrm.Sdk\"}");
            }

            Assert.Throws<JsonSerializationException>(() =>
            {
                object deserializedAttributeCollection;
                memoryStream = new MemoryStream(memoryStream.ToArray());
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    deserializedAttributeCollection = serializer.Deserialize(new JsonTextReader(reader), typeof(AttributeCollection));
                }
            });
        }

        [Fact]
        public void EntitySerializerConvertersCanBeOverriden()
        {
            Entity entity = new Entity("entity");
            entity.Id = Guid.NewGuid();
            List<JsonConverter> converters = new List<JsonConverter>();
            converters.Add(new GuidConverter());
            JsonSerializer serializer = new EntitySerializer(converters);

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

            Assert.Equal(entity.LogicalName, deserializedEntity.LogicalName);
        }

        [Fact]
        public void EntitySerializerConvertersCanBeEmpty()
        {
            Entity entity = new Entity("entity");
            entity.Id = Guid.NewGuid();
            List<JsonConverter> converters = new List<JsonConverter>();
            JsonSerializer serializer = new EntitySerializer(converters);

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

            Assert.Equal(entity.LogicalName, deserializedEntity.LogicalName);
        }

        [Fact]
        public void CreatingAnEntitySerializerWithNullConvertersThrows()
        {
            List<JsonConverter> serializers = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                JsonSerializer serializer = new EntitySerializer(serializers);
            });
        }
    }
}
