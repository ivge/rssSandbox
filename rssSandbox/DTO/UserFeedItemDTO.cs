using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rssSandbox.DTO
{
    public class UserFeedItemDTO
    {
        public string Source;
        public string Title;
        public string Content;
        public Uri URL;
        public DateTime FetchDate;
        public DateTime PublishDate;

    }
}