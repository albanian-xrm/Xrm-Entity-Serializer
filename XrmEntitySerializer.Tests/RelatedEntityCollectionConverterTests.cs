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
            RelatedEntityCollectionContainer realtedEntityCollection = new RelatedEntityCollectionContainer();
            realtedEntityCollection.RelatedEntityCollection = new RelatedEntityCollection();
            Relationship relationship = new Relationship("related_entity");

            realtedEntityCollection.RelatedEntityCollection.Add(relationship, new EntityCollection());
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new RelatedEntityCollectionConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), realtedEntityCollection);
            }

            RelatedEntityCollectionContainer deserializedRelatedEntityCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedRelatedEntityCollection = (RelatedEntityCollectionContainer)serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(realtedEntityCollection.GetType(), deserializedRelatedEntityCollection.GetType());
            Assert.Equal(realtedEntityCollection.RelatedEntityCollection.Count, deserializedRelatedEntityCollection.RelatedEntityCollection.Count);
            Assert.Equal(realtedEntityCollection.RelatedEntityCollection.Keys.First(), deserializedRelatedEntityCollection.RelatedEntityCollection.Keys.First());
            Assert.Equal(realtedEntityCollection.RelatedEntityCollection.Values.First().Entities.Count, deserializedRelatedEntityCollection.RelatedEntityCollection.Values.First().Entities.Count);

        }

        class RelatedEntityCollectionContainer
        {
            public RelatedEntityCollection RelatedEntityCollection { get; set; }
        }
    }
}
