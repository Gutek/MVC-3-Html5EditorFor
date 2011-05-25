using System.Globalization;

namespace System.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class HtmlAttribute : Attribute, IMetadataAware
    {
        public string Name { get; set; }
        public object Value { get; set; }

        protected string Prefix { get { return "custom-attr-{0}"; } }

        protected HtmlAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public virtual void OnMetadataCreated(ModelMetadata metadata)
        {
            string smallKey = Name.ToLowerInvariant();
            string key = string.Format(CultureInfo.InvariantCulture, Prefix, smallKey);

            metadata.AdditionalValues[key] = Value;
        }
    }
}