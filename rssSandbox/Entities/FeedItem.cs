using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;

namespace rssSandbox.Entities
{
    abstract public class FeedItem 
    {
        public string Source { get; set; }
        public string Title{ get; set; }
        public string Content { get; set; }
        public Uri URL { get; set; }
        public DateTime FetchDate { get; set; }
        public DateTime PublishDate { get; set; }

        public FeedItem()
        {
            FetchDate = DateTime.UtcNow;
        }

        public FeedItem(string source, string title, string content, Uri url) : this()
        {
            this.Source = source;
            this.Title = title;
            this.Content = content;
            this.URL = url;
        }

    }
}