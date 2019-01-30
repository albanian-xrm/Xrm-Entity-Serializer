#if !XRM_8 && !XRM_7 && !XRM_6 && !XRM_5
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace XrmEntitySerializer.ValueProviders
{
    internal class OptionSetValueCollectionValueProvider : IValueProvider
    {
        public object GetValue(object target)
        {
            OptionSetValueCollection value = (OptionSetValueCollection)target;
            return value;
        }

        public void SetValue(object target, object value)
        {
            OptionSetValueCollection attributeCollection = (OptionSetValueCollection)target;
            IEnumerable<OptionSetValue> values = (IEnumerable<OptionSetValue>)value;
            foreach (var item in values)
            {
                attributeCollection.Add(item);
            }
        }
    }
}
#endif