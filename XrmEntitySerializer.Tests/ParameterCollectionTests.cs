using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace XrmEntitySerializer.Tests
{
    public class ParameterCollectionTests
    {
        [Fact]
        public void ParameterCollectionCanBeSerializedAndDeserialized()
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
    }
}
