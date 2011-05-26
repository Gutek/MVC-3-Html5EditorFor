/*
 * Code based on Microsoft MVC 3 RTM Source code.
 */

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace System.Web.Mvc.Html
{
    internal static class Html5DefaultEditorTemplates
    {
        internal static string BooleanTemplate(HtmlHelper html, object attributes = null)
        {
            bool? value = null;
            if(html.ViewContext.ViewData.Model != null)
            {
                value = Convert.ToBoolean(html.ViewContext.ViewData.Model, CultureInfo.InvariantCulture);
            }

            return html.ViewContext.ViewData.ModelMetadata.IsNullableValueType
                       ? BooleanTemplateDropDownList(html, value, attributes)
                       : BooleanTemplateCheckbox(html, value ?? false, attributes);
        }

        private static string BooleanTemplateCheckbox(HtmlHelper html, bool value, object attributes = null)
        {
            return
                html.CheckBox(String.Empty,
                              value,
                              CreateHtmlAttributes("check-box",
                                                   html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                   attributes)).ToHtmlString();
        }

        private static string BooleanTemplateDropDownList(HtmlHelper html, bool? value, object attributes = null)
        {
            return
                html.DropDownList(String.Empty,
                                  TriStateValues(value),
                                  CreateHtmlAttributes("list-box tri-state",
                                                       html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                       attributes)).ToHtmlString();
        }

        internal static string CollectionTemplate(HtmlHelper html, object attributes = null)
        {
            return CollectionTemplate(html, Html5TemplateHelpers.TemplateHelper, attributes);
        }

        internal static string CollectionTemplate(HtmlHelper html,
                                                  Html5TemplateHelpers.TemplateHelperDelegate templateHelper,
                                                  object attributes = null)
        {
            object model = html.ViewContext.ViewData.ModelMetadata.Model;
            if(model == null)
            {
                return String.Empty;
            }

            IEnumerable collection = model as IEnumerable;
            if(collection == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "The Collection template was used with an object of type '{0}', which does not implement System.IEnumerable.",
                        model.GetType().FullName
                        )
                    );
            }

            Type typeInCollection = typeof(string);
            Type genericEnumerableType = Html5TypeHelpers.ExtractGenericInterface(collection.GetType(),
                                                                                  typeof(IEnumerable<>));
            if(genericEnumerableType != null)
            {
                typeInCollection = genericEnumerableType.GetGenericArguments()[0];
            }
            bool typeInCollectionIsNullableValueType = Html5TypeHelpers.IsNullableValueType(typeInCollection);

            string oldPrefix = html.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix;

            try
            {
                html.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = String.Empty;

                string fieldNameBase = oldPrefix;
                StringBuilder result = new StringBuilder();
                int index = 0;

                foreach(object item in collection)
                {
                    Type itemType = typeInCollection;
                    if(item != null && !typeInCollectionIsNullableValueType)
                    {
                        itemType = item.GetType();
                    }
                    ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForType(() => item, itemType);
                    string fieldName = String.Format(CultureInfo.InvariantCulture, "{0}[{1}]", fieldNameBase, index++);
                    string output = templateHelper(html,
                                                   metadata,
                                                   fieldName,
                                                   null /* templateName */,
                                                   DataBoundControlMode.Edit,
                                                   null /* additionalViewData */,
                                                   attributes);
                    result.Append(output);
                }

                return result.ToString();
            }
            finally
            {
                html.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = oldPrefix;
            }
        }

        internal static string DecimalTemplate(HtmlHelper html, object attributes = null)
        {
            if(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue
               == html.ViewContext.ViewData.ModelMetadata.Model)
            {
                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue = String.Format(CultureInfo.CurrentCulture,
                                                                                           "{0:0.00}",
                                                                                           html.ViewContext.ViewData.
                                                                                               ModelMetadata.Model);
            }

            html.SetNumeric(decimal.MaxValue, decimal.MinValue);

            return html.TextBox(String.Empty,
                                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue,
                                CreateHtmlAttributes("text-box single-line numeric-float",
                                                     html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                     attributes)).ToHtmlString();
        }

        internal static string FloatTemplate(HtmlHelper html, object attributes = null)
        {
            if(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue
               == html.ViewContext.ViewData.ModelMetadata.Model)
            {
                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue = String.Format(CultureInfo.CurrentCulture,
                                                                                           "{0:0.00}",
                                                                                           html.ViewContext.ViewData.
                                                                                               ModelMetadata.Model);
            }

            html.SetNumeric(float.MaxValue, float.MinValue);

            return html.TextBox(String.Empty,
                                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue,
                                CreateHtmlAttributes("text-box single-line numeric-float",
                                                     html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                     attributes)).ToHtmlString();
        }

        internal static string DoubleTemplate(HtmlHelper html, object attributes = null)
        {
            if(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue
               == html.ViewContext.ViewData.ModelMetadata.Model)
            {
                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue = String.Format(CultureInfo.CurrentCulture,
                                                                                           "{0:0.00}",
                                                                                           html.ViewContext.ViewData.
                                                                                               ModelMetadata.Model);
            }

            html.SetNumeric(double.MaxValue, double.MinValue);

            return html.TextBox(String.Empty,
                                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue,
                                CreateHtmlAttributes("text-box single-line numeric-float",
                                                     html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                     attributes)).ToHtmlString();
        }

        internal static string IntegerTemplate(HtmlHelper html, object attributes = null)
        {
            if(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue
               == html.ViewContext.ViewData.ModelMetadata.Model)
            {
                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue = String.Format(CultureInfo.CurrentCulture,
                                                                                           "{0:0}",
                                                                                           html.ViewContext.ViewData.
                                                                                               ModelMetadata.Model);
            }

            html.SetNumeric(int.MaxValue, int.MinValue);

            return html.TextBox(String.Empty,
                                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue,
                                CreateHtmlAttributes("text-box single-line numeric-int",
                                                     html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                     attributes)).ToHtmlString();
        }

        internal static string ByteTemplate(HtmlHelper html, object attributes = null)
        {
            if(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue
               == html.ViewContext.ViewData.ModelMetadata.Model)
            {
                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue = String.Format(CultureInfo.CurrentCulture,
                                                                                           "{0:0}",
                                                                                           html.ViewContext.ViewData.
                                                                                               ModelMetadata.Model);
            }

            html.SetNumeric(byte.MaxValue, byte.MinValue);
            return html.TextBox(String.Empty,
                                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue,
                                CreateHtmlAttributes("text-box single-line numeric-int",
                                                     html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                     attributes)).ToHtmlString();
        }

        internal static string ShortTemplate(HtmlHelper html, object attributes = null)
        {
            if(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue
               == html.ViewContext.ViewData.ModelMetadata.Model)
            {
                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue = String.Format(CultureInfo.CurrentCulture,
                                                                                           "{0:0}",
                                                                                           html.ViewContext.ViewData.
                                                                                               ModelMetadata.Model);
            }

            html.SetNumeric(short.MaxValue, short.MinValue);

            return html.TextBox(String.Empty,
                                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue,
                                CreateHtmlAttributes("text-box single-line numeric-int",
                                                     html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                     attributes)).ToHtmlString();
        }

        internal static string LongTemplate(HtmlHelper html, object attributes = null)
        {
            if(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue
               == html.ViewContext.ViewData.ModelMetadata.Model)
            {
                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue = String.Format(CultureInfo.CurrentCulture,
                                                                                           "{0:0}",
                                                                                           html.ViewContext.ViewData.
                                                                                               ModelMetadata.Model);
            }

            html.SetNumeric(long.MaxValue, long.MinValue);

            return html.TextBox(String.Empty,
                                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue,
                                CreateHtmlAttributes("text-box single-line numeric-int",
                                                     html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                     attributes)).ToHtmlString();
        }

        internal static void SetNumeric<T>(this HtmlHelper html, T max, T min) where T : struct
        {
            if(!html.ViewContext.ViewData.ModelMetadata.AdditionalValues.ContainsKey("custom-attr-type"))
                html.ViewContext.ViewData.ModelMetadata.AdditionalValues["custom-attr-type"] = "number";


            html.ViewContext.ViewData.ModelMetadata.AdditionalValues.MergeAttributes(
                html.GetUnobtrusiveValidationAttributes(string.Empty, html.ViewContext.ViewData.ModelMetadata));

            if(html.ViewContext.ViewData.ModelMetadata.AdditionalValues.ContainsKey("custom-attr-use-min-max"))
            {
                html.ViewContext.ViewData.ModelMetadata.AdditionalValues["custom-attr-min"] = min;
                html.ViewContext.ViewData.ModelMetadata.AdditionalValues["custom-attr-max"] = max;
                html.ViewContext.ViewData.ModelMetadata.AdditionalValues.Remove("custom-attr-use-min-max");
            } 
            else if(html.ViewContext.ViewData.ModelMetadata.AdditionalValues.ContainsKey("data-val-range"))
            {
                html.ViewContext.ViewData.ModelMetadata.AdditionalValues["custom-attr-min"] = html.ViewContext.ViewData.ModelMetadata.AdditionalValues["data-val-range-min"];
                html.ViewContext.ViewData.ModelMetadata.AdditionalValues["custom-attr-max"] = html.ViewContext.ViewData.ModelMetadata.AdditionalValues["data-val-range-max"];
            }

            var type = typeof(T);
            if(type == typeof(double) 
                || type == typeof(float)
                || type == typeof(decimal))
            {
                if(!html.ViewContext.ViewData.ModelMetadata.AdditionalValues.ContainsKey("custom-attr-step"))
                    html.ViewContext.ViewData.ModelMetadata.AdditionalValues["custom-attr-step"] = 0.1;
            }
        }

        internal static string HiddenInputTemplate(HtmlHelper html, object attributes = null)
        {
            string result;

            if(html.ViewContext.ViewData.ModelMetadata.HideSurroundingHtml)
            {
                result = String.Empty;
            }
            else
            {
                result = Html5DefaultDisplayTemplates.StringTemplate(html);
            }

            object model = html.ViewContext.ViewData.Model;

            Binary modelAsBinary = model as Binary;
            if(modelAsBinary != null)
            {
                model = Convert.ToBase64String(modelAsBinary.ToArray());
            }
            else
            {
                byte[] modelAsByteArray = model as byte[];
                if(modelAsByteArray != null)
                {
                    model = Convert.ToBase64String(modelAsByteArray);
                }
            }

            result += html.Hidden(String.Empty, model).ToHtmlString();
            return result;
        }

        internal static string MultilineTextTemplate(HtmlHelper html, object attributes = null)
        {
            return html.TextArea(String.Empty,
                                 html.ViewContext.ViewData.TemplateInfo.FormattedModelValue.ToString(),
                                 0 /* rows */,
                                 0 /* columns */,
                                 CreateHtmlAttributes("text-box multi-line editor-markdown",
                                                      html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                      attributes)).ToHtmlString();
        }

        private static IDictionary<string, object> CreateHtmlAttributes(string className,
                                                                        IDictionary<string, object> additionalData,
                                                                        object attributes = null)
        {
            var dataAttributes = additionalData.GetDataAttributes();
            var customAttributes = additionalData.GetCustomAttributes();

            if(attributes != null)
            {
                var attr = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
                dataAttributes.MergeAttributes(attr);
            }

            dataAttributes.MergeAttributes(customAttributes);

            if(dataAttributes.ContainsKey("class"))
            {
                dataAttributes["class"] = dataAttributes["class"] + " " + className;
            }
            else
            {
                dataAttributes["class"] = className;
            }

            return dataAttributes;
        }

        internal static string ObjectTemplate(HtmlHelper html, object attributes = null)
        {
            return ObjectTemplate(html, Html5TemplateHelpers.TemplateHelper, attributes);
        }

        internal static string ObjectTemplate(HtmlHelper html,
                                              Html5TemplateHelpers.TemplateHelperDelegate templateHelper,
                                              object attributes = null)
        {
            ViewDataDictionary viewData = html.ViewContext.ViewData;
            TemplateInfo templateInfo = viewData.TemplateInfo;
            ModelMetadata modelMetadata = viewData.ModelMetadata;
            StringBuilder builder = new StringBuilder();

            if(templateInfo.TemplateDepth > 1)
            {
                // DDB #224751
                return modelMetadata.Model == null ? modelMetadata.NullDisplayText : modelMetadata.SimpleDisplayText;
            }

            foreach(ModelMetadata propertyMetadata in modelMetadata.Properties.Where(pm => ShouldShow(pm, templateInfo))
                )
            {
                if(!propertyMetadata.HideSurroundingHtml)
                {
                    string label =
                        Html5LabelExtensions.LabelHelper(html, propertyMetadata, propertyMetadata.PropertyName).
                            ToHtmlString();
                    if(!String.IsNullOrEmpty(label))
                    {
                        builder.AppendFormat(CultureInfo.InvariantCulture,
                                             "<div class=\"editor-label\">{0}</div>\r\n",
                                             label);
                    }

                    builder.Append("<div class=\"editor-field\">");
                }

                builder.Append(templateHelper(html,
                                              propertyMetadata,
                                              propertyMetadata.PropertyName,
                                              null /* templateName */,
                                              DataBoundControlMode.Edit,
                                              null /* additionalViewData */,
                                              attributes));

                if(!propertyMetadata.HideSurroundingHtml)
                {
                    builder.Append(" ");
                    builder.Append(html.ValidationMessage(propertyMetadata.PropertyName));
                    builder.Append("</div>\r\n");
                }
            }

            return builder.ToString();
        }

        internal static string PasswordTemplate(HtmlHelper html, object attributes = null)
        {
            html.ViewContext.ViewData.ModelMetadata.AdditionalValues["type"] = "password";
            return html.Password(String.Empty,
                                 html.ViewContext.ViewData.TemplateInfo.FormattedModelValue,
                                 CreateHtmlAttributes("text-box single-line password",
                                                      html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                      attributes)).ToHtmlString();
        }

        private static bool ShouldShow(ModelMetadata metadata, TemplateInfo templateInfo)
        {
            return
                metadata.ShowForEdit
                && metadata.ModelType != typeof(EntityState)
                && !metadata.IsComplexType
                && !templateInfo.Visited(metadata);
        }

        internal static string StringTemplate(HtmlHelper html, object attributes = null)
        {
            html.SetInputType();

            return html.TextBox(String.Empty,
                                html.ViewContext.ViewData.TemplateInfo.FormattedModelValue,
                                CreateHtmlAttributes("text-box single-line",
                                                     html.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                     attributes)).ToHtmlString();
        }

        internal static void SetInputType(this HtmlHelper html)
        {
            DataType dataType;
            if(!Enum.TryParse(html.ViewData.ModelMetadata.DataTypeName, true, out dataType))
                return;

            if(html.ViewContext.ViewData.ModelMetadata.AdditionalValues.ContainsKey("custom-attr-type"))
                return;

            Action<string> set = t => html.ViewContext.ViewData.ModelMetadata.AdditionalValues["custom-attr-type"] = t;

            switch(dataType)
            {
                case DataType.Password:
                    set("password");
                    break;
                case DataType.Currency:
                    set("number");
                    break;
                case DataType.Date:
                    set("date");
                    break;
                case DataType.DateTime:
                    set("datetime-local");
                    break;
                case DataType.EmailAddress:
                    set("email");
                    break;
                case DataType.ImageUrl:
                    set("url");
                    break;
                case DataType.Time:
                    set("time");
                    break;
                case DataType.PhoneNumber:
                    set("tel");
                    break;
                case DataType.Url:
                    set("url");
                    break;
            }
        }

        internal static List<SelectListItem> TriStateValues(bool? value)
        {
            return new List<SelectListItem>
                   {
                       new SelectListItem { Text = "Not Set", Value = String.Empty, Selected = !value.HasValue },
                       new SelectListItem { Text = "True", Value = "true", Selected = value.HasValue && value.Value },
                       new SelectListItem { Text = "False", Value = "false", Selected = value.HasValue && !value.Value },
                   };
        }
    }
}