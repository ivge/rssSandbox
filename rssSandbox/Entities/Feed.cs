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

        public string Name { get; set; }

        public List<object> Items { get; internal set; }
        public DateTime Updated { get; internal set; }

        public abstract void UpdateItems();
    }
}