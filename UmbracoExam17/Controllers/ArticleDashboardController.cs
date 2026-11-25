using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Services;

namespace UmbracoExam17.Controllers;

[VersionedApiBackOfficeRoute("article-dashboard")]
[ApiExplorerSettings(GroupName = "Article Dashboard")]
public class ArticleDashboardController : ManagementApiControllerBase
{
    private readonly IContentService _contentService;

    private readonly IPublishedContentQuery _publishedContentQuery;


    public ArticleDashboardController(IContentService contentService, IPublishedContentQuery publishedContentQuery)
    {
        _contentService = contentService;
        _publishedContentQuery = publishedContentQuery;
    }

    [HttpGet("statistics")]
    public IActionResult GetStatistics()
    {
        var rootNodes = _publishedContentQuery.ContentAtRoot();

        var articles1 = rootNodes
            .SelectMany(x => x.DescendantsOrSelf())
            .Where(x => x.ContentType.Alias == "article")
            .ToList();

        var b = new
        {
            totalArticles = articles1.Count,
            publishedArticles = articles1.Count(x => x.IsPublished())
        };

        var articles = _contentService
            .GetRootContent()
            .SelectMany(root => GetArticles(root))
            .ToList();

        return Ok(new
        {
            totalArticles = articles.Count,
            publishedArticles = articles.Count(x => x.Published)
        });
    }

    private IEnumerable<Umbraco.Cms.Core.Models.IContent> GetArticles(Umbraco.Cms.Core.Models.IContent parent)
    {
        foreach (var child in _contentService.GetPagedChildren(parent.Id, 0, 1000, out _))
        {
            if (child.ContentType.Alias == "article")
            {
                yield return child;
            }

            foreach (var descendant in GetArticles(child))
            {
                yield return descendant;
            }
        }
    }
}

