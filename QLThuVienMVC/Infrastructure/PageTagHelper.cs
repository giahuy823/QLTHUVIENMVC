using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using QLThuVienMVC.ViewModels;

namespace QLThuVienMVC.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            _urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        public Paging? PageModel { get; set; }
        public string? PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; } = string.Empty;
        public string PageClassNormal { get; set; } = string.Empty;
        public string PageClassSelected { get; set; } = string.Empty;
        public string PageClassDisabled { get; set; } = "disabled";

        [HtmlAttributeName("page-url-tag")]
        public string? CurrentTag { get; set; }

        [HtmlAttributeName("page-url-name")]
        public string? CurrentName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext != null && PageModel != null)
            {
                IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
                TagBuilder result = new TagBuilder("div");

                // === PREVIOUS BUTTON ===
                TagBuilder prevTag = CreateNavigationTag(urlHelper, "« Previous",
                    PageModel.CurrentPage - 1, isDisabled: PageModel.CurrentPage <= 1);
                result.InnerHtml.AppendHtml(prevTag);

                // === PAGE NUMBERS ===
                int totalPages = PageModel.TotalPages;
                int currentPage = Math.Min(PageModel.CurrentPage, totalPages);
                int range = 2;
                int minPage = Math.Max(1, currentPage - range);
                int maxPage = Math.Min(totalPages, currentPage + range);

                if (minPage > 1)
                {
                    result.InnerHtml.AppendHtml(CreatePageTag(urlHelper, 1));
                    if (minPage > 2)
                    {
                        result.InnerHtml.AppendHtml(CreateEllipsisTag());
                    }
                }

                for (int i = minPage; i <= maxPage; i++)
                {
                    result.InnerHtml.AppendHtml(CreatePageTag(urlHelper, i));
                }

                if (maxPage < totalPages)
                {
                    if (maxPage < totalPages - 1)
                    {
                        result.InnerHtml.AppendHtml(CreateEllipsisTag());
                    }
                    result.InnerHtml.AppendHtml(CreatePageTag(urlHelper, totalPages));
                }

                // === NEXT BUTTON ===
                TagBuilder nextTag = CreateNavigationTag(urlHelper, "Next »",
                    PageModel.CurrentPage + 1, isDisabled: PageModel.CurrentPage >= totalPages);
                result.InnerHtml.AppendHtml(nextTag);

                // === PAGE INFO ===
                TagBuilder pageInfo = new TagBuilder("span");
                pageInfo.AddCssClass("page-info ms-2");
                pageInfo.InnerHtml.Append($"Page {currentPage} of {totalPages}");
                result.InnerHtml.AppendHtml(pageInfo);

                output.Content.AppendHtml(result.InnerHtml);
            }
        }

        private TagBuilder CreatePageTag(IUrlHelper urlHelper, int pageNumber)
        {
            var tag = new TagBuilder("a");

            // Merge all route values
            var routeValues = new Dictionary<string, object>(PageUrlValues)
            {
                ["page"] = pageNumber
            };

            // Add filter parameters if they exist
            if (!string.IsNullOrEmpty(CurrentTag))
                routeValues["tag"] = CurrentTag;

            if (!string.IsNullOrEmpty(CurrentName))
                routeValues["name"] = CurrentName;

            tag.Attributes["href"] = urlHelper.Action(PageAction, routeValues);
            tag.InnerHtml.Append(pageNumber.ToString());

            if (PageClassesEnabled)
            {
                tag.AddCssClass(PageClass);
                tag.AddCssClass(pageNumber == PageModel?.CurrentPage ? PageClassSelected : PageClassNormal);
            }

            return tag;
        }

        private TagBuilder CreateNavigationTag(IUrlHelper urlHelper, string text, int targetPage, bool isDisabled)
        {
            var tag = new TagBuilder("a");

            if (!isDisabled)
            {
                var routeValues = new Dictionary<string, object>(PageUrlValues)
                {
                    ["page"] = targetPage
                };

                if (!string.IsNullOrEmpty(CurrentTag))
                    routeValues["tag"] = CurrentTag;

                if (!string.IsNullOrEmpty(CurrentName))
                    routeValues["name"] = CurrentName;

                tag.Attributes["href"] = urlHelper.Action(PageAction, routeValues);
            }

            tag.InnerHtml.Append(text);
            tag.AddCssClass(isDisabled ? "btn disabled" : "btn btn-outline-primary");
            tag.AddCssClass(text.Contains("Previous") ? "me-1" : "ms-1");

            return tag;
        }

        private TagBuilder CreateEllipsisTag()
        {
            TagBuilder span = new TagBuilder("span");
            span.InnerHtml.Append("...");
            span.AddCssClass("btn btn-light disabled");
            return span;
        }
    }
}