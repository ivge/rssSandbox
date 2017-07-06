using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;

namespace rssSandbox.Entities
{
    /// <summary>
    /// This class represents RSS feeds which were added selected by user 
    /// and now should be aggregated. 
    /// </summary>
    public class UserFeed
    {
        /// <summary>
        /// list of unique subscribed feeds
        /// </summary>
        public HashSet<Feed> SubscribedFeeds { get; }

        /// <summary>
        /// list of all agregated items from all RSS feeds 
        /// </summary>
        private List<FeedItem> items;
        public List<FeedItem> Items
        {
            get
            {
                CheckFeedsCache();
                return items;
            }
            set
            {
                items = value;
            }
        }

        /// <summary>
        /// Check if cache on each feed among selected RSS feeds is valid by checking how much time spent since last RSS feed get 
        /// Cache will be updated if it's not valid 
        /// </summary>
        private void CheckFeedsCache()
        {
            foreach (var feed in SubscribedFeeds)
            {
                bool updateAggregatedItems = false;
                if (DateTime.UtcNow - feed.Updated > Settings.CacheInvalidatePeriod)
                {
                    feed.UpdateItems();
                    updateAggregatedItems = true;
                }
                if (updateAggregatedItems)
                    this.Update();
            }

        }

        /// <summary>
        /// Name of the feed
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier
        /// </summary>
        public Guid ID { get; }

        public UserFeed()
        {
            SubscribedFeeds = new HashSet<Feed>();
            Items = new List<FeedItem>();
            ID = Guid.NewGuid();
        }

        public UserFeed(string name) : this()
        {
            this.Name = name;
        }


        /// <summary>
        /// Add new RSS feed to aggregation and update resulting aggregated list. 
        /// </summary>
        /// <param name="newFeed"></param>
        public void Add(Feed newFeed)
        {
            SubscribedFeeds.Add(newFeed);
            Update();
        }


        /// <summary>
        /// Remove feed from aggregation and update resulting aggregated list. 
        /// </summary>
        /// <param name="removeFeed"></param>
        public void Remove(Feed removeFeed)
        {
            SubscribedFeeds.Remove(removeFeed);
            Update();
        }

        /// <summary>
        /// Update resulting aggregated list. 
        /// </summary>
        private void Update()
        {
            Items.Clear();
            var list = new List<FeedItem>();
            foreach (var feed in SubscribedFeeds)
                foreach (var item in feed.Items)
                {
                    list.Add(item);
                }
            Items.AddRange(list.OrderByDescending(i => i.PublishDate).Take(Settings.MaxItemsInFeed));
        }

    }

    public class UserFeedNotFoundException : Exception
    {
        public UserFeedNotFoundException()
        {
        }

        public UserFeedNotFoundException(string message)
        : base(message)
        {
        }

        public UserFeedNotFoundException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    public class UserFeedAlreadyExistsException : Exception
    {
        public UserFeedAlreadyExistsException()
        {
        }

        public UserFeedAlreadyExistsException(string message)
        : base(message)
        {
        }

        public UserFeedAlreadyExistsException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}