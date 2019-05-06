using Microsoft.Xrm.Sdk;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace XrmEntitySerializer.ValueProviders
{
    internal class RelatedEntityCollectionValueProvider : IValueProvider
    {
        public object GetValue(object target)
        {
            RelatedEntityCollection value = (RelatedEntityCollection)target;
            return value;
        }

        public void SetValue(object target, object value)
        {
            RelatedEntityCollection attributeCollection = (RelatedEntityCollection)target;
            IEnumerable<KeyValuePair<Relationship, EntityCollection>> values = (IEnumerable<KeyValuePair<Relationship, EntityCollection>>)value;
            foreach (var item in values)
            {
                attributeCollection.Add(item);
            }
        }
    }
}