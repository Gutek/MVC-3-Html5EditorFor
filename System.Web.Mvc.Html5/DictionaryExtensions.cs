using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.Web.Mvc
{
    public static class DictionaryExtensions
    {
        public static IDictionary<string, object> GetDataAttributes(this IDictionary<string, object> @this)
        {
            return @this.Where(x => x.Key.ToLowerInvariant().StartsWith("data-")).ToDictionary(x => x.Key, x => x.Value);
        }

        public static IDictionary<string, object> GetCustomAttributes(this IDictionary<string, object> @this)
        {
            return
                @this.Where(x => x.Key.ToLowerInvariant().StartsWith("custom-attr-")).ToDictionary(
                    x => x.Key.Replace("custom-attr-", string.Empty), x => x.Value);
        }

        public static IDictionary<string, object> MergeAttributes(this IDictionary<string, object> @this,
                                                                  IDictionary<string, object> toMerge,
                                                                  bool replace = false)
        {
            foreach(var pair in toMerge)
            {
                string key = Convert.ToString(pair.Key, CultureInfo.InvariantCulture);
                string val = Convert.ToString(pair.Value, CultureInfo.InvariantCulture);

                if(string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("Key cannot be null or empty", "key");
                }

                if(replace || !@this.ContainsKey(key))
                {
                    @this[key] = val;
                }
                else if(@this.ContainsKey(key))
                {
                    // combine - works great for css classes, but can cause issues with data-* and others (like double readonly)
                    @this[key] = @this[key] + " " + val;
                }
            }

            return @this;
        }
    }
}