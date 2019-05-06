using Microsoft.Xrm.Sdk;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace XrmEntitySerializer.ValueProviders
{
    internal class AttributeCollectionValueProvider : IValueProvider
    {
        public object GetValue(object target)
        {
            AttributeCollection value = (AttributeCollection)target;
            return value;
        }

        public void SetValue(object target, object value)
        {
            AttributeCollection attributeCollection = (AttributeCollection)target;
            IEnumerable<KeyValuePair<string, object>> values = (IEnumerable<KeyValuePair<string, object>>)value;
            foreach (var item in values)
            {
                attributeCollection.Add(item);
            }
        }
    }
}