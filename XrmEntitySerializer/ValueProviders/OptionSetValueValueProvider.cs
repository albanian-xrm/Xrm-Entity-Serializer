using Microsoft.Xrm.Sdk;
using Newtonsoft.Json.Serialization;

namespace XrmEntitySerializer.ValueProviders
{
    internal class OptionSetValueValueProvider : IValueProvider
    {
        public object GetValue(object target)
        {
            OptionSetValue value = (OptionSetValue)target;
            return value;
        }

        public void SetValue(object target, object value)
        {
            ((OptionSetValue)target).Value = int.Parse(value.ToString());
        }
    }
}