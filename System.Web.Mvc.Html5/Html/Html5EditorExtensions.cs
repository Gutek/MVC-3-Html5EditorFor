/*
 * Code based on Microsoft MVC 3 RTM Source code.
 */

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Web.UI.WebControls;

namespace System.Web.Mvc.Html
{
    public static class Html5EditorExtensions
    {
        public static MvcHtmlString Html5Editor(this HtmlHelper html, string expression)
        {
            return Html5TemplateHelpers.Template(html,
                                                 expression,
                                                 null /* templateName */,
                                                 null /* htmlFieldName */,
                                                 DataBoundControlMode.Edit,
                                                 null /* additionalViewData */);
        }

        public static MvcHtmlString Html5Editor(this HtmlHelper html, string expression, object additionalViewData)
        {
            return Html5TemplateHelpers.Template(html,
                                                 expression,
                                                 null /* templateName */,
                                                 null /* htmlFieldName */,
                                                 DataBoundControlMode.Edit,
                                                 additionalViewData);
        }

        public static MvcHtmlString Html5Editor(this HtmlHelper html, string expression, string templateName)
        {
            return Html5TemplateHelpers.Template(html,
                                                 expression,
                                                 templateName,
                                                 null /* htmlFieldName */,
                                                 DataBoundControlMode.Edit,
                                                 null /* additionalViewData */);
        }

        public static MvcHtmlString Html5Editor(this HtmlHelper html,
                                                string expression,
                                                string templateName,
                                                object additionalViewData)
        {
            return Html5TemplateHelpers.Template(html,
                                                 expression,
                                                 templateName,
                                                 null /* htmlFieldName */,
                                                 DataBoundControlMode.Edit,
                                                 additionalViewData);
        }

        public static MvcHtmlString Html5Editor(this HtmlHelper html,
                                                string expression,
                                                string templateName,
                                                string htmlFieldName)
        {
            return Html5TemplateHelpers.Template(html,
                                                 expression,
                                                 templateName,
                                                 htmlFieldName,
                                                 DataBoundControlMode.Edit,
                                                 null /* additionalViewData */);
        }

        public static MvcHtmlString Html5Editor(this HtmlHelper html,
                                                string expression,
                                                string templateName,
                                                string htmlFieldName,
                                                object additionalViewData)
        {
            return Html5TemplateHelpers.Template(html,
                                                 expression,
                                                 templateName,
                                                 htmlFieldName,
                                                 DataBoundControlMode.Edit,
                                                 additionalViewData);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString Html5EditorFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                   Expression<Func<TModel, TValue>> expression)
        {
            return html.TemplateFor(expression,
                                    null /* templateName */,
                                    null /* htmlFieldName */,
                                    DataBoundControlMode.Edit,
                                    null /* additionalViewData */);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString Html5EditorFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                   Expression<Func<TModel, TValue>> expression,
                                                                   object additionalViewData,
                                                                   object attributes)
        {
            return html.TemplateFor(expression,
                                    null /* templateName */,
                                    null /* htmlFieldName */,
                                    DataBoundControlMode.Edit,
                                    additionalViewData,
                                    attributes);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString Html5EditorFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                   Expression<Func<TModel, TValue>> expression,
                                                                   object additionalViewData)
        {
            return html.TemplateFor(expression,
                                    null /* templateName */,
                                    null /* htmlFieldName */,
                                    DataBoundControlMode.Edit,
                                    additionalViewData);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString Html5EditorFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                   Expression<Func<TModel, TValue>> expression,
                                                                   string templateName)
        {
            return html.TemplateFor(expression, templateName, null /* htmlFieldName */, DataBoundControlMode.Edit, null
                /* additionalViewData */);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString Html5EditorFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                   Expression<Func<TModel, TValue>> expression,
                                                                   string templateName,
                                                                   object additionalViewData)
        {
            return html.TemplateFor(expression,
                                    templateName,
                                    null /* htmlFieldName */,
                                    DataBoundControlMode.Edit,
                                    additionalViewData);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString Html5EditorFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                   Expression<Func<TModel, TValue>> expression,
                                                                   string templateName,
                                                                   string htmlFieldName)
        {
            return html.TemplateFor(expression, templateName, htmlFieldName, DataBoundControlMode.Edit, null
                /* additionalViewData */);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString Html5EditorFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                   Expression<Func<TModel, TValue>> expression,
                                                                   string templateName,
                                                                   string htmlFieldName,
                                                                   object additionalViewData)
        {
            return html.TemplateFor(expression,
                                    templateName,
                                    htmlFieldName,
                                    DataBoundControlMode.Edit,
                                    additionalViewData);
        }

        public static MvcHtmlString Html5EditorForModel(this HtmlHelper html)
        {
            return
                MvcHtmlString.Create(Html5TemplateHelpers.TemplateHelper(html,
                                                                         html.ViewData.ModelMetadata,
                                                                         String.Empty,
                                                                         null /* templateName */,
                                                                         DataBoundControlMode.Edit,
                                                                         null /* additionalViewData */,
                                                                         null /* attributes */));
        }

        public static MvcHtmlString Html5EditorForModel(this HtmlHelper html, object additionalViewData)
        {
            return
                MvcHtmlString.Create(Html5TemplateHelpers.TemplateHelper(html,
                                                                         html.ViewData.ModelMetadata,
                                                                         String.Empty,
                                                                         null /* templateName */,
                                                                         DataBoundControlMode.Edit,
                                                                         additionalViewData,
                                                                         null /* attributes */));
        }

        public static MvcHtmlString Html5EditorForModel(this HtmlHelper html, string templateName)
        {
            return
                MvcHtmlString.Create(Html5TemplateHelpers.TemplateHelper(html,
                                                                         html.ViewData.ModelMetadata,
                                                                         String.Empty,
                                                                         templateName,
                                                                         DataBoundControlMode.Edit,
                                                                         null /* additionalViewData */,
                                                                         null /* attributes */));
        }

        public static MvcHtmlString Html5EditorForModel(this HtmlHelper html,
                                                        string templateName,
                                                        object additionalViewData)
        {
            return
                MvcHtmlString.Create(Html5TemplateHelpers.TemplateHelper(html,
                                                                         html.ViewData.ModelMetadata,
                                                                         String.Empty,
                                                                         templateName,
                                                                         DataBoundControlMode.Edit,
                                                                         additionalViewData,
                                                                         null /* attributes */));
        }

        public static MvcHtmlString Html5EditorForModel(this HtmlHelper html, string templateName, string htmlFieldName)
        {
            return
                MvcHtmlString.Create(Html5TemplateHelpers.TemplateHelper(html,
                                                                         html.ViewData.ModelMetadata,
                                                                         htmlFieldName,
                                                                         templateName,
                                                                         DataBoundControlMode.Edit,
                                                                         null /* additionalViewData */,
                                                                         null /* attributes */));
        }

        public static MvcHtmlString Html5EditorForModel(this HtmlHelper html,
                                                        string templateName,
                                                        string htmlFieldName,
                                                        object additionalViewData)
        {
            return
                MvcHtmlString.Create(Html5TemplateHelpers.TemplateHelper(html,
                                                                         html.ViewData.ModelMetadata,
                                                                         htmlFieldName,
                                                                         templateName,
                                                                         DataBoundControlMode.Edit,
                                                                         additionalViewData,
                                                                         null /* attributes */));
        }
    }
}