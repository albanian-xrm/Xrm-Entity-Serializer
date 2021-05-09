using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace XrmEntitySerializer.Tests
{
    public class ParameterCollectionTests
    {
        [Fact]
        public void ParameterCollectionCanBeSerializedAndDeserialized()
        {
            ParameterCollection parameterCollection = new ParameterCollection();
            parameterCollection.Add("Entity", new Entity("account"));
            parameterCollection.Add("EntityReference", new EntityReference("contact", Guid.NewGuid()));

            EntitySerializer serializer = new EntitySerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), parameterCollection);
            }

            ParameterCollection deserializedParameterCollection;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedParameterCollection = (ParameterCollection)serializer.Deserialize(new JsonTextReader(reader), typeof(ParameterCollection));
            }

            Assert.Equal(parameterCollection.GetType(), deserializedParameterCollection.GetType());
            Assert.Equal(parameterCollection.Count, deserializedParameterCollection.Count);
            Assert.Equal(parameterCollection.Keys.First(), deserializedParameterCollection.Keys.First());
        }
    }
}
