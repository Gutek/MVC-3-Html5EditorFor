/*
 * Code based on Microsoft MVC 3 RTM Source code.
 */

namespace System.Web.Mvc.Html
{
    public static class Html5SelectExtensions
    {
        public static string ListItemToOption(SelectListItem item)
        {
            TagBuilder builder = new TagBuilder("option")
                                 {
                                     InnerHtml = HttpUtility.HtmlEncode(item.Text)
                                 };
            if(item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }
            if(item.Selected)
            {
                builder.Attributes["selected"] = "selected";
            }
            return builder.ToString(TagRenderMode.Normal);
        }
    }
}