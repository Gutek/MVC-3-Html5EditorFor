/*
 * Code based on Microsoft MVC 3 RTM Source code.
 */

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace System.Web.Mvc.Html
{
    public static class Html5TemplateHelpers
    {
        private static readonly Dictionary<DataBoundControlMode, string> ModeViewPaths =
            new Dictionary<DataBoundControlMode, string>
            {
                { DataBoundControlMode.ReadOnly, "DisplayTemplates" },
                { DataBoundControlMode.Edit, "EditorTemplates" }
            };

        private static readonly Dictionary<string, Func<HtmlHelper, object, string>> DefaultDisplayActions =
            new Dictionary<string, Func<HtmlHelper, object, string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "EmailAddress", Html5DefaultDisplayTemplates.EmailAddressTemplate },
                { "HiddenInput", Html5DefaultDisplayTemplates.HiddenInputTemplate },
                { "Html", Html5DefaultDisplayTemplates.HtmlTemplate },
                { "Text", Html5DefaultDisplayTemplates.StringTemplate },
                { "Url", Html5DefaultDisplayTemplates.UrlTemplate },
                { "Collection", Html5DefaultDisplayTemplates.CollectionTemplate },
                { typeof(bool).Name, Html5DefaultDisplayTemplates.BooleanTemplate },
                { typeof(int).Name, Html5DefaultDisplayTemplates.IntegerTemplate },
                { typeof(byte).Name, Html5DefaultDisplayTemplates.IntegerTemplate },
                { typeof(short).Name, Html5DefaultDisplayTemplates.IntegerTemplate },
                { typeof(long).Name, Html5DefaultDisplayTemplates.IntegerTemplate },
                { typeof(decimal).Name, Html5DefaultDisplayTemplates.DecimalTemplate },
                { typeof(float).Name, Html5DefaultDisplayTemplates.DecimalTemplate },
                { typeof(double).Name, Html5DefaultDisplayTemplates.DecimalTemplate },
                { typeof(string).Name, Html5DefaultDisplayTemplates.StringTemplate },
                { typeof(object).Name, Html5DefaultDisplayTemplates.ObjectTemplate },
            };

        private static readonly Dictionary<string, Func<HtmlHelper, object, string>> DefaultEditorActions =
            new Dictionary<string, Func<HtmlHelper, object, string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "HiddenInput", Html5DefaultEditorTemplates.HiddenInputTemplate },
                { "MultilineText", Html5DefaultEditorTemplates.MultilineTextTemplate },
                { "Password", Html5DefaultEditorTemplates.PasswordTemplate },
                { "Text", Html5DefaultEditorTemplates.StringTemplate },
                { "Collection", Html5DefaultEditorTemplates.CollectionTemplate },
                { typeof(bool).Name, Html5DefaultEditorTemplates.BooleanTemplate },
                { typeof(int).Name, Html5DefaultEditorTemplates.IntegerTemplate },
                { typeof(byte).Name, Html5DefaultEditorTemplates.ByteTemplate },
                { typeof(short).Name, Html5DefaultEditorTemplates.ShortTemplate},
                { typeof(long).Name, Html5DefaultEditorTemplates.LongTemplate },
                { typeof(double).Name, Html5DefaultEditorTemplates.DoubleTemplate },
                { typeof(float).Name, Html5DefaultEditorTemplates.FloatTemplate },
                { typeof(decimal).Name, Html5DefaultEditorTemplates.DecimalTemplate },
                { typeof(string).Name, Html5DefaultEditorTemplates.StringTemplate },
                { typeof(object).Name, Html5DefaultEditorTemplates.ObjectTemplate },
            };

        internal static string CacheItemId = Guid.NewGuid().ToString();

        internal delegate string ExecuteTemplateDelegate(
            HtmlHelper html,
            ViewDataDictionary viewData,
            string templateName,
            DataBoundControlMode mode,
            GetViewNamesDelegate getViewNames,
            GetDefaultActionsDelegate getDefaultActions,
            object attributes);

        internal static string ExecuteTemplate(HtmlHelper html,
                                               ViewDataDictionary viewData,
                                               string templateName,
                                               DataBoundControlMode mode,
                                               GetViewNamesDelegate getViewNames,
                                               GetDefaultActionsDelegate getDefaultActions,
                                               object attributes = null)
        {
            Dictionary<string, ActionCacheItem> actionCache = GetActionCache(html);
            Dictionary<string, Func<HtmlHelper, object, string>> defaultActions = getDefaultActions(mode);
            string modeViewPath = ModeViewPaths[mode];

            foreach(
                string viewName in
                    getViewNames(viewData.ModelMetadata,
                                 templateName,
                                 viewData.ModelMetadata.TemplateHint,
                                 viewData.ModelMetadata.DataTypeName))
            {
                string fullViewName = modeViewPath + "/" + viewName;
                ActionCacheItem cacheItem;

                if(actionCache.TryGetValue(fullViewName, out cacheItem))
                {
                    if(cacheItem != null)
                    {
                        return cacheItem.Execute(html, viewData, attributes);
                    }
                }
                else
                {
                    ViewEngineResult viewEngineResult = ViewEngines.Engines.FindPartialView(html.ViewContext,
                                                                                            fullViewName);
                    if(viewEngineResult.View != null)
                    {
                        actionCache[fullViewName] = new ActionCacheViewItem { ViewName = fullViewName };

                        using(StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
                        {
                            viewEngineResult.View.Render(
                                new ViewContext(html.ViewContext,
                                                viewEngineResult.View,
                                                viewData,
                                                html.ViewContext.TempData,
                                                writer),
                                writer);
                            return writer.ToString();
                        }
                    }

                    Func<HtmlHelper, object, string> defaultAction;
                    if(defaultActions.TryGetValue(viewName, out defaultAction))
                    {
                        actionCache[fullViewName] = new ActionCacheCodeItem { Action = defaultAction };
                        return defaultAction(MakeHtmlHelper(html, viewData), attributes);
                    }

                    actionCache[fullViewName] = null;
                }
            }

            throw new InvalidOperationException(
                String.Format(
                    CultureInfo.CurrentCulture,
                    "Unable to locate an appropriate template for type {0}.",
                    viewData.ModelMetadata.GetPropertyValue<Type>("RealModelType").FullName
                    )
                );
        }

        internal static Dictionary<string, ActionCacheItem> GetActionCache(HtmlHelper html)
        {
            HttpContextBase context = html.ViewContext.HttpContext;
            Dictionary<string, ActionCacheItem> result;

            if(!context.Items.Contains(CacheItemId))
            {
                result = new Dictionary<string, ActionCacheItem>();
                context.Items[CacheItemId] = result;
            }
            else
            {
                result = (Dictionary<string, ActionCacheItem>)context.Items[CacheItemId];
            }

            return result;
        }

        internal delegate Dictionary<string, Func<HtmlHelper, object, string>> GetDefaultActionsDelegate(
            DataBoundControlMode mode);

        internal static Dictionary<string, Func<HtmlHelper, object, string>> GetDefaultActions(DataBoundControlMode mode)
        {
            return mode == DataBoundControlMode.ReadOnly ? DefaultDisplayActions : DefaultEditorActions;
        }

        internal delegate IEnumerable<string> GetViewNamesDelegate(ModelMetadata metadata, params string[] templateHints
            );

        internal static IEnumerable<string> GetViewNames(ModelMetadata metadata, params string[] templateHints)
        {
            foreach(string templateHint in templateHints.Where(s => !String.IsNullOrEmpty(s)))
            {
                yield return templateHint;
            }

            var tt = metadata.GetPropertyValue<Type>("RealModelType");
            // We don't want to search for Nullable<T>, we want to search for T (which should handle both T and Nullable<T>)
            Type fieldType = Nullable.GetUnderlyingType(tt) ?? tt;

            // TODO: Make better string names for generic types
            yield return fieldType.Name;

            if(!metadata.IsComplexType)
            {
                yield return "String";
            }
            else if(fieldType.IsInterface)
            {
                if(typeof(IEnumerable).IsAssignableFrom(fieldType))
                {
                    yield return "Collection";
                }

                yield return "Object";
            }
            else
            {
                bool isEnumerable = typeof(IEnumerable).IsAssignableFrom(fieldType);

                while(true)
                {
                    fieldType = fieldType.BaseType;
                    if(fieldType == null)
                    {
                        break;
                    }

                    if(isEnumerable && fieldType == typeof(object))
                    {
                        yield return "Collection";
                    }

                    yield return fieldType.Name;
                }
            }
        }

        internal static MvcHtmlString Template(HtmlHelper html,
                                               string expression,
                                               string templateName,
                                               string htmlFieldName,
                                               DataBoundControlMode mode,
                                               object additionalViewData,
                                               object attributes = null)
        {
            return
                MvcHtmlString.Create(Template(html,
                                              expression,
                                              templateName,
                                              htmlFieldName,
                                              mode,
                                              additionalViewData,
                                              TemplateHelper,
                                              attributes));
        }

        // Unit testing version
        internal static string Template(HtmlHelper html,
                                        string expression,
                                        string templateName,
                                        string htmlFieldName,
                                        DataBoundControlMode mode,
                                        object additionalViewData,
                                        TemplateHelperDelegate templateHelper,
                                        object attributes = null)
        {
            return templateHelper(html,
                                  ModelMetadata.FromStringExpression(expression, html.ViewData),
                                  htmlFieldName ?? ExpressionHelper.GetExpressionText(expression),
                                  templateName,
                                  mode,
                                  additionalViewData,
                                  attributes);
        }

        internal static MvcHtmlString TemplateFor<TContainer, TValue>(this HtmlHelper<TContainer> html,
                                                                      Expression<Func<TContainer, TValue>> expression,
                                                                      string templateName,
                                                                      string htmlFieldName,
                                                                      DataBoundControlMode mode,
                                                                      object additionalViewData,
                                                                      object attributes = null)
        {
            return
                MvcHtmlString.Create(TemplateFor(html,
                                                 expression,
                                                 templateName,
                                                 htmlFieldName,
                                                 mode,
                                                 additionalViewData,
                                                 TemplateHelper,
                                                 attributes));
        }

        // Unit testing version
        internal static string TemplateFor<TContainer, TValue>(this HtmlHelper<TContainer> html,
                                                               Expression<Func<TContainer, TValue>> expression,
                                                               string templateName,
                                                               string htmlFieldName,
                                                               DataBoundControlMode mode,
                                                               object additionalViewData,
                                                               TemplateHelperDelegate templateHelper,
                                                               object attributes = null)
        {
            return templateHelper(html,
                                  ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                                  htmlFieldName ?? ExpressionHelper.GetExpressionText(expression),
                                  templateName,
                                  mode,
                                  additionalViewData,
                                  attributes);
        }

        internal delegate string TemplateHelperDelegate(
            HtmlHelper html,
            ModelMetadata metadata,
            string htmlFieldName,
            string templateName,
            DataBoundControlMode mode,
            object additionalViewData,
            object attributes);

        internal static string TemplateHelper(HtmlHelper html,
                                              ModelMetadata metadata,
                                              string htmlFieldName,
                                              string templateName,
                                              DataBoundControlMode mode,
                                              object additionalViewData,
                                              object attributes)
        {
            return TemplateHelper(html,
                                  metadata,
                                  htmlFieldName,
                                  templateName,
                                  mode,
                                  additionalViewData,
                                  ExecuteTemplate,
                                  attributes);
        }

        internal static string TemplateHelper(HtmlHelper html,
                                              ModelMetadata metadata,
                                              string htmlFieldName,
                                              string templateName,
                                              DataBoundControlMode mode,
                                              object additionalViewData,
                                              ExecuteTemplateDelegate executeTemplate,
                                              object attributes)
        {
            // TODO: Convert Editor into Display if model.IsReadOnly is true? Need to be careful about this because
            // the Model property on the ViewPage/ViewUserControl is get-only, so the type descriptor automatically
            // decorates it with a [ReadOnly] attribute...

            if(metadata.ConvertEmptyStringToNull && String.Empty.Equals(metadata.Model))
            {
                metadata.Model = null;
            }

            object formattedModelValue = metadata.Model;
            if(metadata.Model == null && mode == DataBoundControlMode.ReadOnly)
            {
                formattedModelValue = metadata.NullDisplayText;
            }

            string formatString = mode == DataBoundControlMode.ReadOnly
                                      ? metadata.DisplayFormatString
                                      : metadata.EditFormatString;
            if(metadata.Model != null && !String.IsNullOrEmpty(formatString))
            {
                formattedModelValue = String.Format(CultureInfo.CurrentCulture, formatString, metadata.Model);
            }

            // Normally this shouldn't happen, unless someone writes their own custom Object templates which
            // don't check to make sure that the object hasn't already been displayed
            object visitedObjectsKey = metadata.Model ?? metadata.GetPropertyValue<Type>("RealModelType");
            var hash = html.ViewContext.ViewData.TemplateInfo.GetPropertyValue<HashSet<object>>("VisitedObjects");
            if(hash.Contains(visitedObjectsKey))
            {
                // DDB #224750
                return String.Empty;
            }

            var templateInfo = new TemplateInfo
                               {
                                   FormattedModelValue = formattedModelValue,
                                   HtmlFieldPrefix =
                                       html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName),
                                   //VisitedObjects = new HashSet<object>(html.ViewContext.ViewData.TemplateInfo.GetPropertyValue<HashSet<object>>("VisitedObjects")),    // DDB #224750
                               };

            templateInfo.SetPropertyValue("VisitedObjects",
                                          new HashSet<object>(
                                              html.ViewContext.ViewData.TemplateInfo.GetPropertyValue<HashSet<object>>(
                                                  "VisitedObjects")));

            ViewDataDictionary viewData = new ViewDataDictionary(html.ViewDataContainer.ViewData)
                                          {
                                              Model = metadata.Model,
                                              ModelMetadata = metadata,
                                              TemplateInfo = templateInfo
                                          };

            if(additionalViewData != null)
            {
                foreach(KeyValuePair<string, object> kvp in new RouteValueDictionary(additionalViewData))
                {
                    viewData[kvp.Key] = kvp.Value;
                }
            }

            viewData.TemplateInfo.AddItem("VisitedObjects", visitedObjectsKey); // DDB #224750

            return executeTemplate(html, viewData, templateName, mode, GetViewNames, GetDefaultActions, attributes);
        }

        // Helpers

        private static HtmlHelper MakeHtmlHelper(HtmlHelper html, ViewDataDictionary viewData)
        {
            return new HtmlHelper(
                new ViewContext(html.ViewContext,
                                html.ViewContext.View,
                                viewData,
                                html.ViewContext.TempData,
                                html.ViewContext.Writer),
                new ViewDataContainer(viewData)
                );
        }

        private static void SetPropertyValue<T>(this object obj, string propName, T val)
        {
            Type t = obj.GetType();
            if(t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
            {
                throw new ArgumentOutOfRangeException("propName",
                                                      string.Format("Property {0} was not found in Type {1}",
                                                                    propName,
                                                                    obj.GetType().FullName));
            }

            t.InvokeMember(propName,
                           BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty
                           | BindingFlags.Instance,
                           null,
                           obj,
                           new object[] { val });
        }

        private static T GetPropertyValue<T>(this object obj, string propName)
        {
            if(obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            PropertyInfo pi = obj.GetType().GetProperty(propName,
                                                        BindingFlags.Public | BindingFlags.NonPublic
                                                        | BindingFlags.Instance);
            if(pi == null)
            {
                throw new ArgumentOutOfRangeException("propName",
                                                      string.Format("Property {0} was not found in Type {1}",
                                                                    propName,
                                                                    obj.GetType().FullName));
            }

            return (T)pi.GetValue(obj, null);
        }

        private static void AddItem(this object obj, string propName, object toAdd)
        {
            if(obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            PropertyInfo pi = obj.GetType().GetProperty(propName,
                                                        BindingFlags.Public | BindingFlags.NonPublic
                                                        | BindingFlags.Instance);
            if(pi == null)
            {
                throw new ArgumentOutOfRangeException("propName",
                                                      string.Format("Property {0} was not found in Type {1}",
                                                                    propName,
                                                                    obj.GetType().FullName));
            }

            var col = pi.GetValue(obj, null);

            pi.PropertyType.GetMethod("Add").Invoke(col, new[] { toAdd });
        }

        internal abstract class ActionCacheItem
        {
            public abstract string Execute(HtmlHelper html, ViewDataDictionary viewData, object attributes);
        }

        internal class ActionCacheCodeItem : ActionCacheItem
        {
            public Func<HtmlHelper, object, string> Action { get; set; }

            public override string Execute(HtmlHelper html, ViewDataDictionary viewData, object attributes)
            {
                return Action(MakeHtmlHelper(html, viewData), attributes);
            }
        }

        internal class ActionCacheViewItem : ActionCacheItem
        {
            public string ViewName { get; set; }

            public override string Execute(HtmlHelper html, ViewDataDictionary viewData, object attributes)
            {
                ViewEngineResult viewEngineResult = ViewEngines.Engines.FindPartialView(html.ViewContext, ViewName);
                using(StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
                {
                    viewEngineResult.View.Render(
                        new ViewContext(html.ViewContext,
                                        viewEngineResult.View,
                                        viewData,
                                        html.ViewContext.TempData,
                                        writer),
                        writer);
                    return writer.ToString();
                }
            }
        }

        private class ViewDataContainer : IViewDataContainer
        {
            public ViewDataContainer(ViewDataDictionary viewData)
            {
                ViewData = viewData;
            }

            public ViewDataDictionary ViewData { get; set; }
        }
    }
}