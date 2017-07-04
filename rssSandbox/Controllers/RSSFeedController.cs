using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using rssSandbox.Entities;
using rssSandbox.Models;
using rssSandbox;
using System.Threading.Tasks;

namespace rssSandbox.Controllers
{
    [RoutePrefix("api/feeds")]
    public class RSSFeedController : ApiController
    {
        [Route("Add")]
        [HttpPost]
        public HttpResponseMessage Add([FromBody]RSSFeed newRSSFeed)
        {
            var response = new HttpResponseMessage();
            if (!ModelState.IsValid)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            DataModel.RSSFeeds.Add(newRSSFeed);
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent("New RSS feed succesfully added.");
            return response;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<RSSFeed> GetAll()
        {
            return DataModel.RSSFeeds;
        }

    }
}
