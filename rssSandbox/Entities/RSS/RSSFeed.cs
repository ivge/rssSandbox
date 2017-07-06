using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;

namespace rssSandbox.Entities
{
    public class RSSFeed : Feed
    {
        public RSSFeed(): base()
        {
            this.ID = Guid.NewGuid();
        }

        public RSSFeed(string name, Uri url) : this()
        {
            this.Name = name;
            this.URL = url;
        }

        public override void UpdateItems()
        {
            this.Items.Clear();
            using (XmlReader reader = XmlReader.Create(this.URL.AbsoluteUri))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                foreach (var item in feed.Items)
                {
                    Items.Add(new RSSFeedItem(feed.Title.Text, item));
                }

            }
            this.Updated = DateTime.UtcNow;
        }

        public bool Equals(RSSFeed feed)
        {
            return this.URL == feed.URL && this.Name == feed.Name;
        }

        public override int GetHashCode()
        {
            return (this.Name + this.URL).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is RSSFeed && Equals((RSSFeed)obj);
        }
    }

    public class RSSFeedNotFoundException : Exception
    {
        public RSSFeedNotFoundException()
        {
        }

        public RSSFeedNotFoundException(string message)
        : base(message)
        {
        }

        public RSSFeedNotFoundException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
