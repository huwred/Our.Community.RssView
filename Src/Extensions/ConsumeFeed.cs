using System;
using System.Net.Http;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;
using Our.Community.RssView.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace Our.Community.RssView.Extensions
{
    public static class ConsumeFeed
    {
        public static FeedResult FeedResult(object source)
        {
            var feedUrl = source.ToString();
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
                        httpClient.DefaultRequestHeaders.Add("User-Agent", "MediaWiz.RssView");
                        var response = httpClient.GetAsync(feedUrl).Result;
                        var responsString = response.Content.ReadAsStringAsync().Result;
                        //strip off cloudfare script
                        responsString = Regex.Replace(responsString, "<script.*script>", "");
                        TextReader tr = new StringReader(responsString);

                        using (XmlReader reader = XmlReader.Create(tr))
                        {
                            feedContent = SyndicationFeed.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
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
