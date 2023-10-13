using System.ServiceModel.Syndication;

namespace Our.Community.RssView.Models
{
    public class FeedResult
    {
        public SyndicationFeed SyndicationFeed { get; set; }
        public string FeedUrl { get; set; }
        public string StatusMessage { get; set; }
        public bool HasFeedResults { get; set; }
  
    }
}
