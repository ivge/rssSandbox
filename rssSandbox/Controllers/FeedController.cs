using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using rssSandbox.Entities;
using rssSandbox.Models;
using rssSandbox.DTO;

namespace rssSandbox.Controllers
{
    [RoutePrefix("api/feeds")]
    public class FeedController : ApiController
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
            DataModel.Feeds.Add(newRSSFeed);
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent("New RSS feed succesfully added.");
            return response;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<FeedDTO> GetAll()
        {
            var feeds = from f in DataModel.Feeds
                        select new FeedDTO
                        {
                            ID = f.ID,
                            Name = f.Name,
                            Url = f.URL
                        };
            return feeds;
        }

    }
}
