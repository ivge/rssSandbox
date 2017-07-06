using System;
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
    public partial class UsersController : ApiController
    {
        /// <summary>
        /// Creates new userfeed. 
        /// </summary>
        /// <param name="userID">Users GUID</param>
        /// <param name="feedname">name of a new user feed, should be unique within user feeds list</param>
        /// <returns></returns>
        [Route("{userid:guid}/CreateFeed/{feedname}")]
        [HttpPost]
        public IHttpActionResult CreateFeed(Guid userID, string feedname)
        {
            var response = new HttpResponseMessage();
            var user = DataModel.Users.Where(_user => _user.ID == userID).FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException("User not found!");
            var userfeedID = user.CreateNewUserFeed(feedname);
            return Ok("New user feed succesfully created, ID: " + userfeedID);
        }

        /// <summary>
        /// Add feed to a user feed via post request with userID, UserfeedID and new feed ID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("AddFeed")]
        [HttpPost]
        public IHttpActionResult AddFeed([FromBody]PostRSSFeedtoUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = DataModel.Users.Where(_user => _user.ID == request.userID).FirstOrDefault();
            if (user == null)
                return BadRequest("User not found! Please check users GUID!");

            var userFeed = user.Feeds.Where(_userfeed => _userfeed.ID == request.userFeedID).FirstOrDefault();
            if (userFeed == null)
                return BadRequest("Users feed not found! Please check users feed GUID!");

            var feed = DataModel.Feeds.Where(_rssfeed => _rssfeed.ID == request.rssFeedID).FirstOrDefault();
            if (feed == null)
                return BadRequest("New feed not found! Please check new feeds GUID!");
            else userFeed.Add(feed);

            return new PostRSSFeedtoUserResponse();
        }


        /// <summary>
        /// Adds new feed to user feed 
        /// </summary>
        /// <param name="userid">Users ID</param>
        /// <param name="userfeedname">User feeds ID</param>
        /// <param name="feedid">Id of a feed you want to add</param>
        /// <returns>OK() if succesfully added, Badrequest if user, user feed or adding feed is not found</returns>
        [Route("{userid:guid}/Feeds/{userfeedname}/AddFeed/{feedid:guid}")]
        [HttpPost]
        public IHttpActionResult AddFeed(Guid userid, string  userfeedname, Guid feedid)
        {
            var user = DataModel.Users.Where(_user => _user.ID == userid).FirstOrDefault();
            if (user == null)
                return BadRequest("User not found! Please check users GUID!");

            var userFeed = user.Feeds.Where(_userfeed => _userfeed.Name.Equals(userfeedname, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (userFeed == null)
                return BadRequest("Users feed not found! Please check users feed GUID!");

            var feed = DataModel.Feeds.Where(_rssfeed => _rssfeed.ID == feedid).FirstOrDefault();
            if (feed == null)
                return BadRequest("New feed not found! Please check new feeds GUID!");
            else userFeed.Add(feed);

            return Ok("Feed added!");
        }


        /// <summary>
        /// remove feed from userfeed
        /// </summary>
        /// <param name="userid">Users GUID</param>
        /// <param name="userfeedname"> User feeds name</param>
        /// <param name="feedid">GUID of a feed you want to delete </param>
        /// <returns>OK() if succesfully deleted, 
        /// Badrequest if user, user feed or deleting feed is not found</returns>
        [Route("{userid:guid}/Feeds/{userfeedname}/DeleteFeed/{feedid:guid}")]
        [HttpDelete]
        public IHttpActionResult DeleteFeed(Guid userid, string userfeedname, Guid feedid)
        {
            var user = DataModel.Users.Where(_user => _user.ID == userid).FirstOrDefault();
            if (user == null)
                return BadRequest("User not found! Please check users GUID!");

            var userFeed = user.Feeds.Where(_userfeed => _userfeed.Name.Equals(userfeedname, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (userFeed == null)
                return BadRequest("Users feed not found! Please check users feed GUID!");

            var feed = DataModel.Feeds.Where(_rssfeed => _rssfeed.ID == feedid).FirstOrDefault();
            if (feed == null)
                return BadRequest("Feed not found! Please check GUID of a feed you trying to delete!");
            else userFeed.Remove(feed);

            return Ok("Feed deleted!");
        }

        /// <summary>
        /// Returns all user feeds 
        /// </summary>
        /// <param name="userID">customer ID(GUID)</param>
        /// <returns>List of all user feed </returns>
        [Route("{userid:guid}/Feeds")]
        [HttpGet]
        public IEnumerable<UserFeedsDTO> GetFeeds(Guid userID)
        {
            var user = DataModel.Users.Where(_user => _user.ID == userID).FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException("User not found!");

            var feeds = from uf in user.Feeds
                        select new UserFeedsDTO
                        {
                            ID = uf.ID,
                            Name = uf.Name,
                            SubscribedFeeds = from f in uf.SubscribedFeeds
                                              select new FeedDTO
                                              {
                                                  ID = f.ID,
                                                  Name = f.Name,
                                                  Url = f.URL
                                              }
                        };

            return feeds;
        }

        [Route("{userid:guid}/Feeds/{userfeedname}")]
        [HttpGet]
        public IEnumerable<UserFeedItemDTO> GetFeedItems(Guid userID, string userfeedname)
        {
            var user = DataModel.Users.Where(_user => _user.ID == userID).FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException("User not found!");
            var userfeed = user.Feeds.Where(_userfeed => _userfeed.Name.Equals(userfeedname, StringComparison.InvariantCulture)).FirstOrDefault();
            if (userfeed == null)
                throw new UserFeedNotFoundException("Users feed not found!");
            var items = from uf in userfeed.Items
                        select new UserFeedItemDTO
                        {
                            Title = uf.Title,
                            Content = uf.Content,
                            FetchDate = uf.FetchDate,
                            URL = uf.URL,
                            PublishDate = uf.PublishDate,
                            Source = uf.Source
                        };
            return items;
        }

        [Route("{userid:guid}/Feeds/{userfeedname}/Formatted")]
        [HttpGet]
        public IEnumerable<string> GetFormattedFeedItems(Guid userID, string userfeedname)
        {
            var user = DataModel.Users.Where(_user => _user.ID == userID).FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException("User not found!");
            var userfeed = user.Feeds.Where(_userfeed => _userfeed.Name.Equals(userfeedname, StringComparison.InvariantCulture)).FirstOrDefault();
            if (userfeed == null)
                throw new UserFeedNotFoundException("Users feed not found!");
            foreach (var i in userfeed.Items)
            {
                yield return i.FormattedContent;
            }
        }
    }
}