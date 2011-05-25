using System.Globalization;
using System.Reflection;

namespace System.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class Html5DataAttribute : Attribute, IMetadataAware
    {
        private object _value;
        private Func<object> _resourceAccessor;
        private string _resourceName;
        private Type _resourceType;

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            string smallKey = DataKey.ToLowerInvariant();
            string key = smallKey.Contains("data-")
                             ? DataKey
                             : string.Format(CultureInfo.InvariantCulture, "data-{0}", smallKey);

            metadata.AdditionalValues[key] = Value;
        }

        protected Html5DataAttribute(string dataKey, object value)
        {
            if(string.IsNullOrWhiteSpace(dataKey))
            {
                throw new ArgumentNullException("dataKey", "Data key cannot be null.");
            }

            DataKey = string.Format("data-{0}", dataKey);
            if(value != null)
            {
                Value = value;
            }
        }

        private void SetResourceAccessorByPropertyLookup()
        {
            if((_resourceType == null) || string.IsNullOrEmpty(_resourceName))
            {
                throw new InvalidOperationException(
                    "Both ResourceType and ResourceName need to be set on this attribute.");
            }

            PropertyInfo property = _resourceType.GetProperty(_resourceName,
                                                              BindingFlags.NonPublic | BindingFlags.Public
                                                              | BindingFlags.Static);
            if(property != null)
            {
                MethodInfo getMethod = property.GetGetMethod(true);

                if((getMethod == null) || (!getMethod.IsAssembly && !getMethod.IsPublic))
                {
                    property = null;
                }
            }

            if(property == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                                                                  "The resource type '{0}' does not have an accessible static property named '{1}'",
                                                                  _resourceType.FullName,
                                                                  _resourceName));
            }

            if(property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                                                                  "The property '{0}' on resource type '{1}' is not a string type.",
                                                                  property.Name,
                                                                  _resourceType.FullName));
            }

            _resourceAccessor = () => (string)property.GetValue(null, null);
        }

        private void SetupResourceAccessor()
        {
            if(_resourceAccessor != null)
            {
                return;
            }

            object localValue = _value;

            bool resourceNameIsNotNull = !string.IsNullOrEmpty(_resourceName);
            bool localValueIsNotNull = localValue != null;
            bool resourceTypeIsNotNull = _resourceType != null;

            if(resourceNameIsNotNull == localValueIsNotNull)
            {
                throw new InvalidOperationException("Either Value or ResourceName must be set, but not both.");
            }

            if(resourceTypeIsNotNull != resourceNameIsNotNull)
            {
                throw new InvalidOperationException(
                    "Both ResourceType and ResourceName need to be set on this attribute.");
            }

            if(resourceNameIsNotNull)
            {
                SetResourceAccessorByPropertyLookup();
            }
            else
            {
                Func<object> func = () => localValue;
                _resourceAccessor = func;
            }
        }

        public string DataKey { get; set; }

        public object Value
        {
            get
            {
                SetupResourceAccessor();
                return _resourceAccessor();
            }
            set
            {
                _value = value;
                _resourceAccessor = null;
            }
        }

        public string ResourceName
        {
            get { return _resourceName; }
            set
            {
                _resourceName = value;
                _resourceAccessor = null;
            }
        }

        public Type ResourceType
        {
            get { return _resourceType; }
            set
            {
                _resourceType = value;
                _resourceAccessor = null;
            }
        }
    }
}