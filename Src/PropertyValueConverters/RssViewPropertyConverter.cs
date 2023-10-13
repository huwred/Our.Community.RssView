using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Xml;
using Our.Community.RssView.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace Our.Community.RssView.PropertyValueConverters
{

    public class RssViewPropertyConverter : IPropertyValueConverter
    {
        //private readonly ILogger _logger;

        public RssViewPropertyConverter()
        {
            //_logger = logger;
        }
 
        public bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias.Equals("RssView");
        }

        public bool? IsValue(object value, PropertyValueLevel level)
        {
            return true;
        }

        public Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            return typeof(FeedResult);
        }

        public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            return PropertyCacheLevel.Element;
        }

        public object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source,
            bool preview)
        {
            if (source == null)
            {
                return null;
            }
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
                    //think about maybe logging the error MethodBase.GetCurrentMethod().DeclaringType,
                    //_logger.LogError( "Error loading feed: " + feedUrl, ex);
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
            //what if requested dynamically...
            return feedResult;
        }

        public object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType,
            PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            //let's keep the string of the url a string
            return inter;
        }

        public object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType,
            PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            throw new NotImplementedException();
        }



    }
}
