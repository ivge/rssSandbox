using rssSandbox.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rssSandbox.DTO
{
    /// <summary>
    /// This class used to display users feed 
    /// </summary>
    public class UserFeedDTO
    {
        public string Name { get; set; }
        public Guid ID { get; set; }
        public HashSet<RSSFeed> SubscribedFeeds { get; set; }
    }
}