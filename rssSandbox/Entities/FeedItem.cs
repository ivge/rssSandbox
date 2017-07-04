using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;

namespace rssSandbox.Entities
{
    public class FeedItem : SyndicationItem
    {
        public string FormattedDescription
        {
            get
            {
                return
                    base.Title.Text + Environment.NewLine +
                    base.Summary.Text + Environment.NewLine +
                    base.BaseUri;
            }
        }


    }
}
