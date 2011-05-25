/*
 * Code based on Microsoft MVC 3 RTM Source code.
 */

using System.Collections.Generic;

namespace System.Web.Mvc.Html
{
    public static class Html5InputExtensions
    {
        public static MvcHtmlString Html5RadioButton(this HtmlHelper htmlHelper, string name, object value)
        {
            // var attributes = CreateHtmlAttributes(htmlHelper.ViewContext.ViewData.ModelMetadata.AdditionalValues, null);
            return Html5RadioButton(htmlHelper, name, value, (object)null);
        }

        public static MvcHtmlString Html5RadioButton(this HtmlHelper htmlHelper,
                                                     string name,
                                                     object value,
                                                     object htmlAttributes)
        {
            //var attributes = CreateHtmlAttributes(htmlHelper.ViewContext.ViewData.ModelMetadata.AdditionalValues, htmlAttributes);
            return htmlHelper.Html5RadioButton(name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Html5RadioButton(this HtmlHelper htmlHelper,
                                                     string name,
                                                     object value,
                                                     IDictionary<string, object> htmlAttributes)
        {
            var attributes = CreateHtmlAttributes(htmlHelper.ViewContext.ViewData.ModelMetadata.AdditionalValues);
            htmlAttributes.MergeAttributes(attributes);

            var fullFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            var rendered = htmlHelper.ViewContext.FormContext.RenderedField(fullFieldName);

            if(rendered && htmlAttributes.ContainsKey("data-val"))
            {
                htmlAttributes.Remove("data-val");
            }

            return htmlHelper.RadioButton(name, value, htmlAttributes);
        }

        public static MvcHtmlString Html5RadioButton(this HtmlHelper htmlHelper,
                                                     string name,
                                                     object value,
                                                     bool isChecked)
        {
            //var attributes = CreateHtmlAttributes(htmlHelper.ViewContext.ViewData.ModelMetadata.AdditionalValues, null);
            return htmlHelper.Html5RadioButton(name, value, isChecked, null);
        }

        public static MvcHtmlString Html5RadioButton(this HtmlHelper htmlHelper,
                                                     string name,
                                                     object value,
                                                     bool isChecked,
                                                     object htmlAttributes)
        {
            var attributes = CreateHtmlAttributes(htmlHelper.ViewContext.ViewData.ModelMetadata.AdditionalValues,
                                                  htmlAttributes);

            var fullFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            var rendered = htmlHelper.ViewContext.FormContext.RenderedField(fullFieldName);

            if(rendered && attributes.ContainsKey("data-val"))
            {
                attributes.Remove("data-val");
            }

            return htmlHelper.RadioButton(name, value, isChecked, attributes);
        }

        private static IDictionary<string, object> CreateHtmlAttributes(IDictionary<string, object> additionalData,
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

            return dataAttributes;
        }
    }
}