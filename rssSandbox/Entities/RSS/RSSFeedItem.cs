using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Text;

namespace rssSandbox.Entities
{
    public class RSSFeedItem : FeedItem
    {
        public RSSFeedItem(string source, string title, string content, Uri url) : base(source, title, content, url) { }

        public RSSFeedItem(string source, 
                           TextSyndicationContent title, 
                           TextSyndicationContent content, 
                           ICollection<SyndicationLink> links, 
                           DateTimeOffset publishDate)
        {
            this.Source = source;
            this.Title = title.Text.Replace("\n", String.Empty).Trim();
            this.Content = content.Text.Replace("\n", String.Empty).Trim();
            foreach (var _link in links)
            {
                if (_link.Uri.IsAbsoluteUri)
                {
                    this.URL = _link.Uri;
                    break;
                }
            }
            this.PublishDate = publishDate.DateTime;
        }

        public RSSFeedItem(string source, SyndicationItem syndicationItem) 
            : this(source, syndicationItem.Title, syndicationItem.Summary, syndicationItem.Links, syndicationItem.PublishDate) { }

        override internal void Format(object formatOptions)
        {
            var s = new StringBuilder();
            s.AppendLine(this.Title);
            s.AppendLine(this.Content);
            s.AppendFormat("More information: {0}", this.URL);
            this.FormattedContent = s.ToString();
        }
    }
}
