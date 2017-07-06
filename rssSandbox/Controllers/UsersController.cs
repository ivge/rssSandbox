using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel.Syndication;
using rssSandbox.Entities;
using rssSandbox.Models;
using rssSandbox.DTO;


namespace rssSandbox.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        /// <summary>
        /// Returns all users.
        /// Using DTO to hide password and lists of aggregated feeds.
        /// </summary>
        /// <returns>List of existing users</returns>
        [Route("getall")]
        [HttpGet]
        public IEnumerable<UserDTO> GetUsers()
        {
            var users = from u in DataModel.Users
                        select new UserDTO
                        {
                            ID = u.ID,
                            Login = u.Login
                        };
            return users;
        }
        /// <summary>
        /// Returns all user feeds 
        /// </summary>
        /// <param name="userID">customer ID(GUID)</param>
        /// <returns>List of all user feed </returns>
        [Route("{userid:guid}/getfeeds")]
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

        [Route("{userid:guid}/getfeed/{userfeedname}")]
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

        [Route("{userid:guid}/getformattedfeed/{userfeedname}")]
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
                yield return i.Title;
            }


        }

        [Route("{userid:guid}/createfeed/{feedname}")]
        [HttpPost]
        public HttpResponseMessage CreateFeed(Guid userID, string feedname)
        {
            var response = new HttpResponseMessage();
            var user = DataModel.Users.Where(_user => _user.ID == userID).FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException("User not found!");
            user.Feeds.Add(new UserFeed() { Name = feedname });
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        [Route("AddRSSFeed")]
        [HttpPost]
        public IHttpActionResult AddRSSFeed([FromBody]PostRSSFeedtoUserRequest request)
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

            var rssFeed = DataModel.Feeds.Where(_rssfeed => _rssfeed.ID == request.rssFeedID).FirstOrDefault();
            if (rssFeed == null)
                return BadRequest("New feed not found! Please check new feeds GUID!");
            else userFeed.Add(rssFeed);

            return new PostRSSFeedtoUserResponse();
        }

    }
}
