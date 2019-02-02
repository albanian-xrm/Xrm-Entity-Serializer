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
    public class AttributeCollectionConverterTests
    {
        [Fact]
        public void AttributeCollectionCanBeSerializedAndDeserialized()
        {
            AttributeCollection attributeCollection = new AttributeCollection();
            attributeCollection.Add("Test", "test");

            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new AttributeCollectionConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), attributeCollection);
            }

            AttributeCollection deserializedAttributeCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedAttributeCollection = (AttributeCollection)serializer.Deserialize(new JsonTextReader(reader), typeof(AttributeCollection));
            }

            Assert.Equal(attributeCollection.GetType(), deserializedAttributeCollection.GetType());
            Assert.Equal(attributeCollection.Count, deserializedAttributeCollection.Count);
            Assert.Equal(attributeCollection.Keys.First(), deserializedAttributeCollection.Keys.First());
            Assert.Equal(attributeCollection.Values.First(), deserializedAttributeCollection.Values.First());
        }

        [Fact]
        public void AttributeCollectionInObjectCanBeSerializedAndDeserialized()
        {
            AttributeCollectionContainer attributeCollectionContainer = new AttributeCollectionContainer();
            AttributeCollection attributeCollection = new AttributeCollection();
            attributeCollection.Add("Test", "test");
            attributeCollectionContainer.AttributeCollection = attributeCollection;

            JsonSerializer serializer = new JsonSerializer();
            serializer.ContractResolver = new XrmContractResolver();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new AttributeCollectionConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), attributeCollectionContainer);
            }

            AttributeCollectionContainer deserializedAttributeCollectionContainer;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedAttributeCollectionContainer = (AttributeCollectionContainer)serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(attributeCollectionContainer.GetType(), deserializedAttributeCollectionContainer.GetType());

            AttributeCollection deserializedAttributeCollection = (AttributeCollection)deserializedAttributeCollectionContainer.AttributeCollection;

            Assert.Equal(attributeCollectionContainer.AttributeCollection.GetType(), deserializedAttributeCollectionContainer.AttributeCollection.GetType());
            Assert.Equal(attributeCollection.Count, deserializedAttributeCollection.Count);
            Assert.Equal(attributeCollection.Keys.First(), deserializedAttributeCollection.Keys.First());
            Assert.Equal(attributeCollection.Values.First(), deserializedAttributeCollection.Values.First());
        }

        class AttributeCollectionContainer
        {
            public object AttributeCollection { get; set; }
        }
    }
}
