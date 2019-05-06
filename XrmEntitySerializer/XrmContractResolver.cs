using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using XrmEntitySerializer.ValueProviders;

namespace XrmEntitySerializer
{
    public class XrmContractResolver : DefaultContractResolver
    {
        public static readonly XrmContractResolver Instance = new XrmContractResolver();

        public static readonly Dictionary<Type, KeyValuePair<JsonConverter, IValueProvider>> Map = new Dictionary<Type, KeyValuePair<JsonConverter, IValueProvider>>()
        {
            { typeof(AttributeCollection), new KeyValuePair<JsonConverter, IValueProvider>(new AttributeCollectionConverter(), new AttributeCollectionValueProvider())},
#if !XRM_7 && !XRM_6 && !XRM_5
            { typeof(KeyAttributeCollection), new KeyValuePair<JsonConverter, IValueProvider>(new KeyAttributeCollectionConverter(), new KeyAttributeCollectionValueProvider())},
#if !XRM_8
            { typeof(OptionSetValueCollection), new KeyValuePair<JsonConverter, IValueProvider>(new OptionSetValueCollectionConverter(), new OptionSetValueCollectionValueProvider())},
#endif
#endif
            { typeof(FormattedValueCollection), new KeyValuePair<JsonConverter, IValueProvider>(new FormattedValueCollectionConverter(), new FormattedValueCollectionValueProvider())},
            { typeof(RelatedEntityCollection), new KeyValuePair<JsonConverter, IValueProvider>(new RelatedEntityCollectionConverter(), new RelatedEntityCollectionValueProvider())},
            { typeof(OptionSetValue), new KeyValuePair<JsonConverter, IValueProvider>(new OptionSetValueConverter(), new OptionSetValueValueProvider())}
        };

        protected override JsonContract CreateContract(Type objectType)
        {
            JsonContract contract = null;
            KeyValuePair<JsonConverter, IValueProvider> converter;
            if (Map.TryGetValue(objectType, out converter))
            {
                JsonObjectContract objectContract = base.CreateObjectContract(objectType);
                contract = objectContract;
                contract.Converter = converter.Key;
                objectContract.Properties.Add(new JsonProperty()
                {
                    DefaultValue = null,
                    Writable = true,
                    Readable = true,
                    ValueProvider = converter.Value,
                    PropertyName = "$value"
                });
            }
            else
            {
                contract = base.CreateContract(objectType);
            }
            return contract;
        }
    }
}