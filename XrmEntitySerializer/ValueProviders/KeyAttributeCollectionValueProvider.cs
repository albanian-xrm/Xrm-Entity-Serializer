#if !XRM_5 && !XRM_6 && !XRM_7
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace XrmEntitySerializer.ValueProviders
{
    internal class KeyAttributeCollectionValueProvider : IValueProvider
    {
        public object GetValue(object target)
        {
            KeyAttributeCollection value = (KeyAttributeCollection)target;
            return value;
        }

        public void SetValue(object target, object value)
        {
            KeyAttributeCollection keyAttributeCollection = (KeyAttributeCollection)target;
            IEnumerable<KeyValuePair<string, object>> values = (IEnumerable<KeyValuePair<string, object>>)value;
            foreach (var item in values)
            {
                keyAttributeCollection.Add(item);
            }
        }
    }
}
#endif