using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace XrmEntitySerializer.Tests
{
    [ExcludeFromCodeCoverage]
    public class RelatedEntityCollectionConverterTests
    {
        [Fact]
        public void RelatedEntityCollectionCanBeSerializedAndDeserialized()
        {
            RelatedEntityCollection realtedEntityCollection = new RelatedEntityCollection();
            Relationship relationship = new Relationship("related_entity");

            realtedEntityCollection.Add(relationship, new EntityCollection());
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new RelatedEntityCollectionConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), realtedEntityCollection);
            }

            RelatedEntityCollection deserializedRelatedEntityCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedRelatedEntityCollection = (RelatedEntityCollection)serializer.Deserialize(new JsonTextReader(reader), typeof(RelatedEntityCollection));
            }
            
            Assert.Equal(realtedEntityCollection.Count, deserializedRelatedEntityCollection.Count);
            Assert.Equal(realtedEntityCollection.Keys.First(), deserializedRelatedEntityCollection.Keys.First());
            Assert.Equal(realtedEntityCollection.Values.First().Entities.Count, deserializedRelatedEntityCollection.Values.First().Entities.Count);

        }

        [Fact]
        public void RelatedEntityCollectionInObjectCanBeSerializedAndDeserialized()
        {
            RelatedEntityCollectionContainer relatedEntityCollectionContainer = new RelatedEntityCollectionContainer();
            RelatedEntityCollection relatedEntityCollection = new RelatedEntityCollection();
            relatedEntityCollectionContainer.RelatedEntityCollection = relatedEntityCollection;
            Relationship relationship = new Relationship("related_entity");

            relatedEntityCollection.Add(relationship, new EntityCollection());
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.ContractResolver = new XrmContractResolver();
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), relatedEntityCollectionContainer);
            }

            RelatedEntityCollectionContainer deserializedRelatedEntityCollectionContainer;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedRelatedEntityCollectionContainer = (RelatedEntityCollectionContainer)serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(relatedEntityCollectionContainer.GetType(), deserializedRelatedEntityCollectionContainer.GetType());
            RelatedEntityCollection deserializedRelatedEntityCollection = (RelatedEntityCollection) deserializedRelatedEntityCollectionContainer.RelatedEntityCollection;

            Assert.Equal(relatedEntityCollection.Count, deserializedRelatedEntityCollection.Count);
            Assert.Equal(relatedEntityCollection.Keys.First(), deserializedRelatedEntityCollection.Keys.First());
            Assert.Equal(relatedEntityCollection.Values.First().Entities.Count, deserializedRelatedEntityCollection.Values.First().Entities.Count);

        }

        class RelatedEntityCollectionContainer
        {
            public object RelatedEntityCollection { get; set; }
        }
    }
}
