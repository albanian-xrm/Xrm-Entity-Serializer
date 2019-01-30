using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Xunit;

namespace XrmEntitySerializer.Tests
{
#if !XRM_5 && !XRM_6 && !XRM_7
    [ExcludeFromCodeCoverage]
    public class KeyAttributeCollectionConverterTests
    {
        [Fact]
        public void Test()
        {
            KeyAttributeCollection keyAttributeCollection = new KeyAttributeCollection();
            keyAttributeCollection.Add("Test", "test");
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Converters.Add(new KeyAttributeCollectionConverter());
            serializer.ContractResolver = new XrmContractResolver();


            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), keyAttributeCollection);
            }
            KeyAttributeCollection deserializedKeyAttributeCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedKeyAttributeCollection = (KeyAttributeCollection)serializer.Deserialize(new JsonTextReader(reader), typeof(KeyAttributeCollection));
            }

            Assert.Equal(keyAttributeCollection.Count, deserializedKeyAttributeCollection.Count);
            Assert.Equal(keyAttributeCollection.Keys.First(), deserializedKeyAttributeCollection.Keys.First());
            Assert.Equal(keyAttributeCollection.Values.First(), deserializedKeyAttributeCollection.Values.First());
        }

        [Fact]
        public void KeyAttributeCollectionCanBeSerializedAndDeserialized()
        {
            KeyAttributeCollection keyAttributeCollection = new KeyAttributeCollection();
            keyAttributeCollection.Add("Test", "test");
            JsonSerializer serializer = new JsonSerializer();
         var num =    serializer.Converters.Where(x => x.CanConvert(typeof(object)));
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Converters.Add(new KeyAttributeCollectionConverter());
            serializer.ContractResolver = new XrmContractResolver();
            

            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), keyAttributeCollection);
            }
            KeyAttributeCollection deserializedKeyAttributeCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedKeyAttributeCollection = (KeyAttributeCollection)serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(keyAttributeCollection.Count, deserializedKeyAttributeCollection.Count);
            Assert.Equal(keyAttributeCollection.Keys.First(), deserializedKeyAttributeCollection.Keys.First());
            Assert.Equal(keyAttributeCollection.Values.First(), deserializedKeyAttributeCollection.Values.First());
        }
        
    }
#endif
}
