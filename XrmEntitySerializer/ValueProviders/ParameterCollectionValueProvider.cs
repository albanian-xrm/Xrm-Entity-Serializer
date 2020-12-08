using Microsoft.Xrm.Sdk;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace XrmEntitySerializer.ValueProviders
{
    internal class ParameterCollectionValueProvider : IValueProvider
    {
        public object GetValue(object target)
        {
            ParameterCollection value = (ParameterCollection)target;
            return value;
        }

        public void SetValue(object target, object value)
        {
            ParameterCollection attributeCollection = (ParameterCollection)target;
            IEnumerable<KeyValuePair<string, object>> values = (IEnumerable<KeyValuePair<string, object>>)value;
            foreach (var item in values)
            {
                attributeCollection.Add(item);
            }
        }
    }
}