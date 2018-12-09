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
            AttributeCollectionContainer attributeCollection = new AttributeCollectionContainer();
            attributeCollection.AttributeCollection = new AttributeCollection();
            attributeCollection.AttributeCollection.Add("Test", "test");
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new AttributeCollectionConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), attributeCollection);
            }

            AttributeCollectionContainer deserializedAttributeCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedAttributeCollection = (AttributeCollectionContainer)serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(attributeCollection.GetType(), deserializedAttributeCollection.GetType());
            Assert.Equal(attributeCollection.AttributeCollection.Count, deserializedAttributeCollection.AttributeCollection.Count);
            Assert.Equal(attributeCollection.AttributeCollection.Keys.First(), deserializedAttributeCollection.AttributeCollection.Keys.First());
            Assert.Equal(attributeCollection.AttributeCollection.Values.First(), deserializedAttributeCollection.AttributeCollection.Values.First());

        }

        class AttributeCollectionContainer
        {
            public AttributeCollection AttributeCollection { get; set; }
        }
    }
}
