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
    public class FormattedValueCollectionConverterTests
    {
        [Fact]
        public void FormattedValueCollectionCanBeSerializedAndDeserialized()
        {
            FormattedValueCollection formattedValueCollection = new FormattedValueCollection();
            formattedValueCollection.Add("Test", "test");

            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new FormattedValueCollectionConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), formattedValueCollection);
            }

            FormattedValueCollection deserializedFormattedValueCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedFormattedValueCollection = (FormattedValueCollection)serializer.Deserialize(new JsonTextReader(reader), typeof(FormattedValueCollection));
            }

            Assert.Equal(formattedValueCollection.GetType(), deserializedFormattedValueCollection.GetType());
            Assert.Equal(formattedValueCollection.Count, deserializedFormattedValueCollection.Count);
            Assert.Equal(formattedValueCollection.Keys.First(), deserializedFormattedValueCollection.Keys.First());
            Assert.Equal(formattedValueCollection.Values.First(), deserializedFormattedValueCollection.Values.First());
        }

        [Fact]
        public void FormattedValueCollectionCanBeSerializedAndDeserializedWithoutSpecifyingType()
        {
            FormattedValueCollection formattedValueCollection = new FormattedValueCollection();
            formattedValueCollection.Add("Test", "test");

            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.ContractResolver = new XrmContractResolver();
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), formattedValueCollection);
            }

            FormattedValueCollection deserializedFormattedValueCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedFormattedValueCollection = (FormattedValueCollection)serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(formattedValueCollection.GetType(), deserializedFormattedValueCollection.GetType());
            Assert.Equal(formattedValueCollection.Count, deserializedFormattedValueCollection.Count);
            Assert.Equal(formattedValueCollection.Keys.First(), deserializedFormattedValueCollection.Keys.First());
            Assert.Equal(formattedValueCollection.Values.First(), deserializedFormattedValueCollection.Values.First());
        }

        [Fact]
        public void FormattedValueCollectionInObjectCanBeSerializedAndDeserialized()
        {
            FormattedValueCollectionContainer formattedValueCollectionContainer = new FormattedValueCollectionContainer();
            FormattedValueCollection formattedValueCollection = new FormattedValueCollection();
            formattedValueCollectionContainer.FormattedValueCollection = formattedValueCollection;
            formattedValueCollection.Add("Test", "test");
            JsonSerializer serializer = new EntitySerializer();
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), formattedValueCollectionContainer);
            }

            FormattedValueCollectionContainer deserializedFormattedValueCollectionContainer;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedFormattedValueCollectionContainer = (FormattedValueCollectionContainer)serializer.Deserialize(new JsonTextReader(reader));
            }
            FormattedValueCollection deserializedFormattedValueCollection = (FormattedValueCollection)deserializedFormattedValueCollectionContainer.FormattedValueCollection;


            Assert.Equal(formattedValueCollectionContainer.GetType(), deserializedFormattedValueCollectionContainer.GetType());
            Assert.Equal(formattedValueCollection.Count, deserializedFormattedValueCollection.Count);
            Assert.Equal(formattedValueCollection.Keys.First(), deserializedFormattedValueCollection.Keys.First());
            Assert.Equal(formattedValueCollection.Values.First(), deserializedFormattedValueCollection.Values.First());

        }

        class FormattedValueCollectionContainer
        {
            public object FormattedValueCollection { get; set; }
        }
    }
}
