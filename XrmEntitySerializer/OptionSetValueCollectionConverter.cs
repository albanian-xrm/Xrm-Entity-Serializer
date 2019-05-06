#if !XRM_8 && !XRM_7 && !XRM_6 && !XRM_5
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace XrmEntitySerializer
{
    public class OptionSetValueCollectionConverter : CollectionTypeConverter<OptionSetValueCollection>
    {
        protected override OptionSetValueCollection ReadCollection(JsonReader reader, Type objectType, OptionSetValueCollection existingOptionSetValues, JsonSerializer serializer, JArray jArray)
        {
            if (existingOptionSetValues == null)
            {
                existingOptionSetValues = new OptionSetValueCollection();
            }
            foreach (JToken item in jArray)
            {
                OptionSetValue option = item.ToObject<OptionSetValue>(serializer);
                existingOptionSetValues.Add(option);
            }
            return existingOptionSetValues;
        }

        protected override void WriteCollection(JsonWriter writer, OptionSetValueCollection optionSetValues, JsonSerializer serializer)
        {
            foreach (OptionSetValue attribute in optionSetValues)
            {
                serializer.Serialize(writer, attribute);
            }
        }
    }
}
#endif