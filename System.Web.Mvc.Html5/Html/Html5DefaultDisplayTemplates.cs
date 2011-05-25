/*
 * Code based on Microsoft MVC 3 RTM Source code.
 */ 

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace System.Web.Mvc.Html
{
    public static class Html5DefaultDisplayTemplates
    {
        internal static string BooleanTemplate(HtmlHelper html, object attributes = null)
        {
            bool? value = null;
            if(html.ViewContext.ViewData.Model != null)
            {
                value = Convert.ToBoolean(html.ViewContext.ViewData.Model, CultureInfo.InvariantCulture);
            }

            return html.ViewContext.ViewData.ModelMetadata.IsNullableValueType
                       ? BooleanTemplateDropDownList(value, html.ViewContext.ViewData.ModelMetadata.AdditionalValues)
                       : BooleanTemplateCheckbox(value ?? false);
        }

        private static string BooleanTemplateCheckbox(bool value, object attributes = null)
        {
            var htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);

            TagBuilder inputTag = new TagBuilder("input");
            inputTag.AddCssClass("check-box");
            inputTag.Attributes["disabled"] = "disabled";
            inputTag.Attributes["type"] = "checkbox";
            inputTag.MergeAttributes(htmlAttributes);

            if(value)
            {
                inputTag.Attributes["checked"] = "checked";
            }

            return inputTag.ToString(TagRenderMode.SelfClosing);
        }

        private static string BooleanTemplateDropDownList(bool? value,
                                                          IDictionary<string, object> additionalValues,
                                                          object attributes = null)
        {
            StringBuilder builder = new StringBuilder();

            var htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            var dataAttributes = additionalValues.GetDataAttributes();
            var customAttributes = additionalValues.GetCustomAttributes();
            dataAttributes.MergeAttributes(customAttributes);

            TagBuilder selectTag = new TagBuilder("select");
            selectTag.AddCssClass("list-box");
            selectTag.AddCssClass("tri-state");
            selectTag.Attributes["disabled"] = "disabled";
            selectTag.MergeAttributes(dataAttributes);
            selectTag.MergeAttributes(htmlAttributes);

            builder.Append(selectTag.ToString(TagRenderMode.StartTag));

            foreach(SelectListItem item in Html5DefaultEditorTemplates.TriStateValues(value))
            {
                builder.Append(Html5SelectExtensions.ListItemToOption(item));
            }

            builder.Append(selectTag.ToString(TagRenderMode.EndTag));
            return builder.ToString();
        }

        internal static string CollectionTemplate(HtmlHelper html, object attributes = null)
        {
            return CollectionTemplate(html, Html5TemplateHelpers.TemplateHelper);
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
                                                   DataBoundControlMode.ReadOnly,
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

            return StringTemplate(html);
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

            return StringTemplate(html);
        }

        internal static string EmailAddressTemplate(HtmlHelper html, object attributes = null)
        {
            var dataAttributes = html.ViewContext.ViewData.ModelMetadata.AdditionalValues.GetDataAttributes();
            var customAttributes = html.ViewContext.ViewData.ModelMetadata.AdditionalValues.GetCustomAttributes();
            dataAttributes.MergeAttributes(customAttributes);

            TagBuilder mailTo = new TagBuilder("a");
            mailTo.MergeAttribute("href",
                                  string.Format(CultureInfo.InvariantCulture,
                                                "mailto:{0}",
                                                html.AttributeEncode(html.ViewContext.ViewData.Model)));
            mailTo.MergeAttributes(dataAttributes);
            mailTo.InnerHtml = html.Encode(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue);

            return mailTo.ToString(TagRenderMode.Normal);
        }

        internal static string HiddenInputTemplate(HtmlHelper html, object attributes = null)
        {
            if(html.ViewContext.ViewData.ModelMetadata.HideSurroundingHtml)
            {
                return String.Empty;
            }
            return StringTemplate(html);
        }

        internal static string HtmlTemplate(HtmlHelper html, object attributes = null)
        {
            return html.ViewContext.ViewData.TemplateInfo.FormattedModelValue.ToString();
        }

        internal static string ObjectTemplate(HtmlHelper html, object attributes = null)
        {
            return ObjectTemplate(html, Html5TemplateHelpers.TemplateHelper);
        }

        internal static string ObjectTemplate(HtmlHelper html,
                                              Html5TemplateHelpers.TemplateHelperDelegate templateHelper,
                                              object attributes = null)
        {
            ViewDataDictionary viewData = html.ViewContext.ViewData;
            TemplateInfo templateInfo = viewData.TemplateInfo;
            ModelMetadata modelMetadata = viewData.ModelMetadata;
            StringBuilder builder = new StringBuilder();

            if(modelMetadata.Model == null)
            {
                // DDB #225237
                return modelMetadata.NullDisplayText;
            }

            if(templateInfo.TemplateDepth > 1)
            {
                // DDB #224751
                return modelMetadata.SimpleDisplayText;
            }

            foreach(ModelMetadata propertyMetadata in modelMetadata.Properties.Where(pm => ShouldShow(pm, templateInfo))
                )
            {
                if(!propertyMetadata.HideSurroundingHtml)
                {
                    string label = propertyMetadata.GetDisplayName();
                    if(!String.IsNullOrEmpty(label))
                    {
                        builder.AppendFormat(CultureInfo.InvariantCulture,
                                             "<div class=\"display-label\">{0}</div>",
                                             label);
                        builder.AppendLine();
                    }

                    builder.Append("<div class=\"display-field\">");
                }

                builder.Append(templateHelper(html,
                                              propertyMetadata,
                                              propertyMetadata.PropertyName,
                                              null /* templateName */,
                                              DataBoundControlMode.ReadOnly,
                                              null /* additionalViewData */,
                                              attributes));

                if(!propertyMetadata.HideSurroundingHtml)
                {
                    builder.AppendLine("</div>");
                }
            }

            return builder.ToString();
        }

        private static bool ShouldShow(ModelMetadata metadata, TemplateInfo templateInfo)
        {
            return
                metadata.ShowForDisplay
                && metadata.ModelType != typeof(EntityState)
                && !metadata.IsComplexType
                && !templateInfo.Visited(metadata);
        }

        internal static string StringTemplate(HtmlHelper html, object attributes = null)
        {
            return html.Encode(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue);
        }

        internal static string UrlTemplate(HtmlHelper html, object attributes = null)
        {
            var dataAttributes = html.ViewContext.ViewData.ModelMetadata.AdditionalValues.GetDataAttributes();
            var customAttributes = html.ViewContext.ViewData.ModelMetadata.AdditionalValues.GetCustomAttributes();
            dataAttributes.MergeAttributes(customAttributes);

            TagBuilder mailTo = new TagBuilder("a");
            mailTo.MergeAttribute("href", html.AttributeEncode(html.ViewContext.ViewData.Model));
            mailTo.MergeAttributes(dataAttributes);
            mailTo.InnerHtml = html.Encode(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue);

            return mailTo.ToString(TagRenderMode.Normal);
        }
    }
}