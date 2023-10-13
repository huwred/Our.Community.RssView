# Our.Community.RssView
Property editor for adding an RSS feed viewer to you Umbraco web site.

## Displaying in a View

```csharp
@inherits Umbraco.Cms.Web.Common.Views.UmbracoViewPage<ContentModels.HomePage>
@using ContentModels = Umbraco.Cms.Web.Common.PublishedModels;
@using Our.Community.RssView.Models
@using static Our.Community.RssView.Extensions.DateTimeOffsetExtensions
@{
	Layout = null;

    var rssFeed =Model.Value<FeedResult>("rssFeedUrl");
}

@if (rssFeed != null && rssFeed.HasFeedResults)
{
    <h2>@rssFeed.SyndicationFeed.Title.Text</h2>
    <h3 class="muted">(@rssFeed.FeedUrl)</h3>
    <div class="container">
        <div class="row">
            @foreach (var feedItem in rssFeed.SyndicationFeed.Items.Take(5))
            {
                <div class="col-4"><h4>@feedItem.Title.Text</h4><h6 class="muted">@feedItem.PublishDate.ToTimeAgo()<br />@feedItem.PublishDate.ToString("hh:mmtt dddd, dd MMM yyyy")</h6><div>@Html.Raw(feedItem.Summary.Text)</div><a class="btn btn-primary" title="@feedItem.Title.Text" href="@feedItem.Links[0].Uri">Read more...</a></div>
            }
        </div>
    </div>
}
```
