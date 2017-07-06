using System;
using System.Collections.Generic;

namespace rssSandbox.Entities
{
    abstract public class Feed
    {
        public Guid ID;

        private Uri url;

        public Uri URL
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                UpdateItems();
            }
        }

        public Feed()
        {
            Items = new List<FeedItem>();
        }

        public string Name { get; set; }

        private List<FeedItem> items;

        public List<FeedItem> Items { get; set; }
        public DateTime Updated { get; internal set; }

        public abstract void UpdateItems();
    }
}