using Microsoft.Xrm.Sdk;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace XrmEntitySerializer.ValueProviders
{
    internal class FormattedValueCollectionValueProvider : IValueProvider
    {
        public object GetValue(object target)
        {
            FormattedValueCollection value = (FormattedValueCollection)target;
            return value;
        }

        public void SetValue(object target, object value)
        {
            FormattedValueCollection formattedValueCollection = (FormattedValueCollection)target;
            IEnumerable<KeyValuePair<string, string>> values = (IEnumerable<KeyValuePair<string, string>>)value;
            foreach (var item in values)
            {
                formattedValueCollection.Add(item);
            }
        }
    }
}