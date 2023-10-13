using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Our.Community.RssView.Models;
using Umbraco.Cms.Web.Common.Controllers;

namespace Our.Community.RssView.Controllers
{

    public class FeedController : UmbracoApiController
    {
        private readonly ILogger _logger;
        public FeedController(ILogger<FeedController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public FeedResult GetRssFeed(string feedUrl)
        {
            var feedResult = new FeedResult() { HasFeedResults = false, FeedUrl = feedUrl };
            var feedContent = default(SyndicationFeed);
            if (!String.IsNullOrEmpty(feedUrl) && feedUrl.ToLower().StartsWith("http"))
            {
                try
                {
                    using (var httpClient = new HttpClient(new HttpClientHandler
                           {
                               AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                           }))
                    {

                        httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
                        httpClient.DefaultRequestHeaders.Add("User-Agent","MediaWiz.RssView");
                        var response = httpClient.GetAsync(feedUrl).Result;
                        //var hotel = response.Content.ReadAsStream()<FeedResult>();
                        using (XmlReader reader = XmlReader.Create(response.Content.ReadAsStream()))
                        {
                            feedContent = SyndicationFeed.Load(reader);
                        }

                    }
                }
                catch (Exception ex)
                {
                    //think about maybe logging the error
                    _logger.LogError("Error loading feed: " + feedUrl, ex);
                    feedResult.StatusMessage = "A bad error has occurred: " + ex.Message;
                }
            }
            else
            {
                feedResult.StatusMessage = "Provide a Feed Url beginning with http...";
            }
            if (feedContent != null)
            {
                feedResult.HasFeedResults = true;
                feedResult.SyndicationFeed = feedContent;
            }
            return feedResult;
        }
    }
}
