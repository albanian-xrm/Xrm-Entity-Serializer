using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XrmEntitySerializer
{
    public class EntitySerializer : JsonSerializer
    {
        public EntitySerializer() : base()
        {
            TypeNameHandling = TypeNameHandling.Objects;
            Converters.Add(new GuidConverter());
            Converters.Add(new AttributeCollectionConverter());
            Converters.Add(new FormattedValueCollectionConverter());
            Converters.Add(new RelatedEntityCollectionConverter());
#if !XRM_7
            Converters.Add(new KeyAttributeCollectionConverter());
#endif
        }
    }
}
