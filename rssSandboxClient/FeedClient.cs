using rssSandbox.DTO;
using rssSandboxClient.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;

namespace rssSandboxClient
{
    class FeedClient
    {
        private HttpClient client;

        private Uri serverURL;

        public UserDTO user;

        public FeedClient()
        {
            client = new HttpClient();
        }

        public FeedClient(Uri serverURL) : this()
        {
            this.serverURL = serverURL;
            client.BaseAddress = serverURL;
        }

        public async Task Login(string username)
        {
            var users = await this.GetUsers();
            var user = users.Where(u => u.Login.Equals(username, StringComparison.Ordinal)).FirstOrDefault();
            if (user != null)
                this.user = user;
            else
                this.user = null;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            List<UserDTO> users = null;
            HttpResponseMessage response = await client.GetAsync(@"api/users/");
            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<UserDTO>>();
            }
            return users;
        }

        public async Task<Guid> AddUser(string login, string password)
        {
            Guid id;
            var url = Url.Combine(this.serverURL.ToString(), @"api/users/add", Uri.EscapeDataString(login), "password", Uri.EscapeDataString(password));
            HttpResponseMessage response = await client.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            if (Guid.TryParse(result.Replace("\"", string.Empty), out id))
                return id;
            else
                throw new InvalidCastException();
        }

        public async Task<List<FeedDTO>> GetFeed()
        {
            List<FeedDTO> users = null;
            HttpResponseMessage response = await client.GetAsync(@"api/feeds");
            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<FeedDTO>>();
            }
            return users;
        }

        public async Task<Guid> AddFeed(CreateFeedDTO rssFeed)
        {
            Guid guid;
            var response = await client.PostAsJsonAsync("api/feeds/AddRSS", rssFeed);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            if (Guid.TryParse(result.Replace("\"", string.Empty), out guid))
                return guid;
            else
                throw new InvalidCastException();
        }

        public async Task DeleteFeed(Guid id)
        {
            Uri deleteFeedURL = new Uri(this.serverURL, @"api/feeds/Delete/");
            deleteFeedURL = new Uri(deleteFeedURL, id.ToString());
            var response = await client.DeleteAsync(deleteFeedURL);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Guid> CreateUserFeed(string feedName)
        {
            Guid guid;
            var url = Url.Combine(this.serverURL.ToString(), @"api/users", user.ID.ToString(), @"/CreateFeed/", Uri.EscapeDataString(feedName));
            var response = await client.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            if (Guid.TryParse(result.Replace("\"", string.Empty), out guid))
                return guid;
            else
                throw new InvalidCastException();
        }

        public async Task<List<UserFeedsDTO>> GetUserFeeds()
        {
            List<UserFeedsDTO> userFeeds = null;
            var url = Url.Combine(this.serverURL.ToString(), @"api/users", user.ID.ToString(), "feeds");
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                userFeeds = await response.Content.ReadAsAsync<List<UserFeedsDTO>>();
            }
            return userFeeds;
        }

        public async Task AddFeedToUserFeed(string userFeedName, Guid feedID)
        {
            var url = Url.Combine(this.serverURL.ToString(), @"api/users", user.ID.ToString(), "Feeds", Uri.EscapeDataString(userFeedName), "AddFeed", feedID.ToString());
            HttpResponseMessage response = await client.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteFeedFromUserFeed(string userFeedName, Guid feedID)
        {
            var url = Url.Combine(this.serverURL.ToString(), @"api/users", user.ID.ToString(), "Feeds", Uri.EscapeDataString(userFeedName), "DeleteFeed", feedID.ToString());
            HttpResponseMessage response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<UserFeedItemDTO>> GetUserFeedItems(string userFeedName)
        {
            List<UserFeedItemDTO> userFeedItems = null;
            var url = Url.Combine(this.serverURL.ToString(), @"api/users", user.ID.ToString(), "Feeds", Uri.EscapeDataString(userFeedName));
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                userFeedItems = await response.Content.ReadAsAsync<List<UserFeedItemDTO>>();
            }
            return userFeedItems;
        }

        public async Task<List<string>> GetUserFeedItemsFormatted(string userFeedName)
        {
            List<string> userFeedItemsFormatted = null;
            var url = Url.Combine(this.serverURL.ToString(), @"api/users", user.ID.ToString(), "Feeds", Uri.EscapeDataString(userFeedName), "Formatted");
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                userFeedItemsFormatted = await response.Content.ReadAsAsync<List<string>>();
            }
            return userFeedItemsFormatted;
        }
    }
}
