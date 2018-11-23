using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace XrmEntitySerializer.Tests
{
    public class GuidConverterTests
    {
        [Fact]
        public void GuidCanBeSerializedAndDeserialized()
        {
            object guid = Guid.NewGuid();
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Converters.Add(new GuidConverter());
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);

            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                serializer.Serialize(new JsonTextWriter(writer), guid);
            }

            object deserializedGuid;
            memoryStream = new MemoryStream(memoryStream.ToArray());
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                deserializedGuid = serializer.Deserialize(new JsonTextReader(reader));
            }

            Assert.Equal(guid, deserializedGuid);
        }
    }
}
