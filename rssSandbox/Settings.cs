using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rssSandbox
{
    /// <summary>
    /// Global settings 
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Maximum count of items in users feed
        /// </summary>
        public static int MaxItemsInFeed = 10;

        /// <summary>
        /// Cache invalidation timeout. Used to determine when RSS feed should be updated 
        /// </summary>
        public static TimeSpan CacheInvalidatePeriod = TimeSpan.FromSeconds(30);
    }
}