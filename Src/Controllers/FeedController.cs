using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUglify;
using Our.Community.RssView.Extensions;
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
            return ConsumeFeed.FeedResult(feedUrl);
        }
    }
}
