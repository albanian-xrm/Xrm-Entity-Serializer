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
#if !XRM_5 && !XRM_6 && !XRM_7
    [ExcludeFromCodeCoverage]
    public class KeyAttributeCollectionConverterTests
    {
        [Fact]
        public void KeyAttributeCollectionCanBeSerializedAndDeserialized()
        {
            KeyAttributeCollectionContainer keyAttributeCollection = new KeyAttributeCollectionContainer();
            keyAttributeCollection.KeyAttributeCollection = new KeyAttributeCollection();
            keyAttributeCollection.KeyAttributeCollection.Add("Test", "test");
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new KeyAttributeCollectionConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), keyAttributeCollection);
            }

            KeyAttributeCollectionContainer deserializedKeyAttributeCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedKeyAttributeCollection = (KeyAttributeCollectionContainer)serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(keyAttributeCollection.GetType(), deserializedKeyAttributeCollection.GetType());
            Assert.Equal(keyAttributeCollection.KeyAttributeCollection.Count, deserializedKeyAttributeCollection.KeyAttributeCollection.Count);
            Assert.Equal(keyAttributeCollection.KeyAttributeCollection.Keys.First(), deserializedKeyAttributeCollection.KeyAttributeCollection.Keys.First());
            Assert.Equal(keyAttributeCollection.KeyAttributeCollection.Values.First(), deserializedKeyAttributeCollection.KeyAttributeCollection.Values.First());

        }

        class KeyAttributeCollectionContainer
        {
            public KeyAttributeCollection KeyAttributeCollection { get; set; }
        }
    }
#endif
}
