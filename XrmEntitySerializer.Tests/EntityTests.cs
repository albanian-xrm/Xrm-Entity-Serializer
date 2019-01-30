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
#if !XRM_8
            OptionSetValueCollection optionSetValues = new OptionSetValueCollection();
            optionSetValues.Add(new OptionSetValue(1));
            optionSetValues.Add(new OptionSetValue(2));
            entity.Attributes.Add("optionSetValues", optionSetValues);
#endif
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

#if !XRM_7 && !XRM_6 && !XRM_5
            Assert.Equal(entity.KeyAttributes.Count, deserializedEntity.KeyAttributes.Count);
#if !XRM_8
            OptionSetValueCollection deserializedOptionSetValues = entity.GetAttributeValue<OptionSetValueCollection>("optionSetValues");
            Assert.NotNull(deserializedOptionSetValues);
            Assert.Equal(optionSetValues.Count, deserializedOptionSetValues.Count);
#endif
#endif
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
