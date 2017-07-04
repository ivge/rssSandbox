using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace rssSandbox.Entities
{
    /// <summary>
    /// this class used to add RSS feed to users feed
    /// </summary>
    public class PostRSSFeedtoUserRequest
    {
        public Guid userID { get; set; }
        public Guid userFeedID { get; set; }
        public Guid rssFeedID { get; set; }
    }

    /// <summary>
    /// This class represents result of adding new RSS feed to users feed
    /// </summary>
    public class PostRSSFeedtoUserResponse : IHttpActionResult
    {
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage();
            return Task.FromResult(response);
        }
    }
}