/*
 * Code based on Microsoft MVC 3 RTM Source code.
 */

using System.Linq;

namespace System.Web.Mvc.Html
{
    public static class Html5LabelExtensions
    {
        public static MvcHtmlString LabelHelper(HtmlHelper html,
                                                ModelMetadata metadata,
                                                string htmlFieldName,
                                                string labelText = "")
        {
            string str = labelText
                         ??
                         (metadata.DisplayName ?? (metadata.PropertyName ?? htmlFieldName.Split(new[] { '.' }).Last()));
            if(string.IsNullOrEmpty(str))
            {
                return MvcHtmlString.Empty;
            }
            TagBuilder tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for",
                                      TagBuilder.CreateSanitizedId(
                                          html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tagBuilder.SetInnerText(str);

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}