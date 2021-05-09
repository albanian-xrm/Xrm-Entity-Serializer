using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace XrmEntitySerializer.Tests
{
    [ExcludeFromCodeCoverage]
    public class RemoteExecutionContextTests
    {       
        [Fact]
        public void RemoteExecutionContextCanBeSerializedAndDeserialized()
        {
            RemoteExecutionContext executionContext = new RemoteExecutionContext();
            executionContext.InputParameters.Add("Target", new Entity("account") { Id = Guid.NewGuid() });


            EntitySerializer serializer = new EntitySerializer();
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), executionContext);
            }

            RemoteExecutionContext deserializedExecutionContext;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedExecutionContext = (RemoteExecutionContext)serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(((Entity)executionContext.InputParameters["Target"]).LogicalName, ((Entity)deserializedExecutionContext.InputParameters["Target"]).LogicalName);
        }      

        [Fact]
        public void RemoteExecutionContextCanBeDeserializedFromAzureServiceBusMessage()
        {
            EntitySerializer serializer = new EntitySerializer();
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);
            using(StreamReader reader = new StreamReader("SampleData\\RemoteExecutionContext.json"))
            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write(reader.ReadToEnd());
            }

            RemoteExecutionContext deserializedExecutionContext;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedExecutionContext = (RemoteExecutionContext)serializer.Deserialize(new JsonTextReader(reader), typeof(RemoteExecutionContext)); 
            }

            Assert.Equal("account", ((Entity)deserializedExecutionContext.InputParameters["Target"]).LogicalName);
        }
    }
}
