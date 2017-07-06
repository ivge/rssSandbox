using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using rssSandbox.Entities;
using rssSandbox.Models;
using rssSandbox.DTO;
using System;

namespace rssSandbox.Controllers
{
    [RoutePrefix("api/feeds")]
    public class FeedsController : ApiController
    {
        /// <summary>
        /// Add new RSS feed to globally available list of feeds 
        /// </summary>
        /// <param name="newRSSFeed">Name and URL is expected</param>
        /// <returns></returns>
        [Route("AddRSS")]
        [HttpPost]
        public IHttpActionResult Add([FromBody]RSSFeed newRSSFeed)
        {
            if (!ModelState.IsValid)
                return BadRequest("Sent data is invalid!");
            if (DataModel.Feeds.Add(newRSSFeed))
                return Ok(newRSSFeed.ID);
            else
                return BadRequest("New RSS feed wasn't added, probably already exists!");
        }

        /// <summary>
        /// Remove Feed from globally available list of feeds. 
        /// </summary>
        /// <param name="id">Feed ID</param>
        /// <returns></returns>
        [Route("Delete/{feedid:guid}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid feedid)
        {
            var feed = DataModel.Feeds.Where(f => f.ID == feedid).FirstOrDefault();
            if (feed == null)
                return BadRequest("Feed not found!");
            else
            {
                DataModel.Feeds.Remove(feed);
                return Ok();
            }
        }

        /// <summary>
        /// Returns globally available list of feeds. 
        /// </summary>
        /// <returns>list of Feeds</returns>
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var feeds = from f in DataModel.Feeds
                        select new FeedDTO
                        {
                            ID = f.ID,
                            Name = f.Name,
                            Url = f.URL
                        };
            return Ok(feeds);
        }

    }
}
