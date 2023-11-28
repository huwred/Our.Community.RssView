using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Xml;
using Our.Community.RssView.Extensions;
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

            //what if requested dynamically...
            return ConsumeFeed.FeedResult(source);
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
