/*
 * Code based on Microsoft MVC 3 RTM Source code.
 */

using System.Linq;

namespace System.Web.Mvc.Html
{
    public static class Html5TypeHelpers
    {
        public static bool IsNullableValueType(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static Type ExtractGenericInterface(Type queryType, Type interfaceType)
        {
            Func<Type, bool> predicate = t => t.IsGenericType && (t.GetGenericTypeDefinition() == interfaceType);
            if(!predicate(queryType))
            {
                return queryType.GetInterfaces().FirstOrDefault(predicate);
            }
            return queryType;
        }
    }
}