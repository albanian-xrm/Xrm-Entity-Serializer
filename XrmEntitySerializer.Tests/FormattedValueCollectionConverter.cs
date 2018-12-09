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
            FormattedValueCollectionContainer formattedValueCollection = new FormattedValueCollectionContainer();
            formattedValueCollection.FormattedValueCollection = new FormattedValueCollection();
            formattedValueCollection.FormattedValueCollection.Add("Test", "test");
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new FormattedValueCollectionConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), formattedValueCollection);
            }

            FormattedValueCollectionContainer deserializedFormattedValueCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedFormattedValueCollection = (FormattedValueCollectionContainer)serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(formattedValueCollection.GetType(), deserializedFormattedValueCollection.GetType());
            Assert.Equal(formattedValueCollection.FormattedValueCollection.Count, deserializedFormattedValueCollection.FormattedValueCollection.Count);
            Assert.Equal(formattedValueCollection.FormattedValueCollection.Keys.First(), deserializedFormattedValueCollection.FormattedValueCollection.Keys.First());
            Assert.Equal(formattedValueCollection.FormattedValueCollection.Values.First(), deserializedFormattedValueCollection.FormattedValueCollection.Values.First());

        }

        class FormattedValueCollectionContainer
        {
            public FormattedValueCollection FormattedValueCollection { get; set; }
        }
    }
}
