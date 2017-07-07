using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using rssSandbox.DTO;
using rssSandboxClient.DTO;
using rssSandbox.Entities;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace rssSandboxClient
{
    class Program
    {
        /*private async static void showfeeds(FeedClient client)
        {

        }*/
        static FeedClient client = null;
        static void Main()
        {
            client = new FeedClient(new Uri(@"http://localhost:59313"));
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Select action:");
                Console.WriteLine("1. Show user list & login");
                Console.WriteLine("2. Add new user");
                Console.WriteLine("3. View list of available feeds");
                if (client.user != null)
                {
                    Console.WriteLine("4. View custom feeds");
                    Console.WriteLine("5. Create new custom feed");
                }
                var key = Console.ReadKey();
                //Console.WriteLine(k.Key.ToString());
                Console.WriteLine();
                switch (key.Key.ToString().Replace("Numpad", string.Empty).Replace("D", string.Empty))
                {
                    case "1":
                        DisplaUserList();
                        break;
                    case "2":
                        AddNewUser();
                        break;
                    case "3":
                        DisplayAvailableFeeds();
                        break;
                    case "4":
                        if (client.user != null)
                        {
                            var selectedUserFeed = DisplayUserFeeds();
                            if (selectedUserFeed != null)
                            {
                                DisplayUserFeedMenu(selectedUserFeed);
                            }
                        }
                        break;
                    case "5":
                        if (client.user != null)
                        {
                            CreateUserFeed();
                        }
                        break;
                }
            }
        }

        private static void CreateUserFeed()
        {
            Console.WriteLine("Add new name for new user feed:");
            var newUserFeed = Console.ReadLine();
            client.CreateUserFeed(newUserFeed);
        }

        private static void DisplayUserFeedMenu(UserFeedsDTO selectedUserFeed)
        {
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("Select action:");
                Console.WriteLine("1. Add new feed to selected user feed");
                Console.WriteLine("2. Delete feed from selected user feed");
                Console.WriteLine("3. Display selected user feed");
                Console.WriteLine("4. Display formatted selected user feed");
                Console.WriteLine("5. Go to previous menu");
                var _key = Console.ReadKey();
                Console.WriteLine();
                //Console.WriteLine(k.Key.ToString());
                switch (_key.Key.ToString().Replace("Numpad", string.Empty).Replace("D", string.Empty))
                {
                    case "1":
                        AddFeedToUserFeed(selectedUserFeed);
                        selectedUserFeed = client.GetUserFeeds().Result.Where(userfeed => userfeed.ID == selectedUserFeed.ID).FirstOrDefault();
                        break;
                    case "2":
                        DeleteFeedFromUserFeed(selectedUserFeed);
                        selectedUserFeed = client.GetUserFeeds().Result.Where(userfeed => userfeed.ID == selectedUserFeed.ID).FirstOrDefault();
                        break;
                    case "3":
                        DisplayFeedItemsFromUserFeed(selectedUserFeed);
                        break;
                    case "4":
                        DisplayFeedItemsFromUserFeedFormatted(selectedUserFeed);
                        break;
                    case "5":
                        flag = false;
                        break;
                }
            }
        }

        public static void DisplaUserList()
        {
            var users = client.GetUsers().Result;
            Console.WriteLine("==Users==");
            foreach (var user in users)
            {
                Console.WriteLine("{0}. {1}", users.IndexOf(user), user.Login);
            }
            Console.WriteLine("Please type an index and press enter, -1 to exit");
            int userIndex;
            while (!int.TryParse(Console.ReadLine(), out userIndex)) ;
            if (userIndex == -1)
                return;
            var _user = users[userIndex];
            client.Login(_user.Login).Wait();
            Console.Title = client.user.Login;
        }

        public static void AddNewUser()
        {
            Console.Write("Enter new user login:");
            var login = Console.ReadLine();
            Console.Write("Enter new user password:");
            var password = Console.ReadLine();
            client.AddUser(login, password);
        }

        public static void DisplayAvailableFeeds()
        {
            var feeds = client.GetFeed().Result;
            Console.WriteLine("==Available feeds==");
            foreach (var feed in feeds)
            {
                Console.WriteLine("{0}. {1} {2}", feeds.IndexOf(feed), feed.Name, feed.Url);
            }

            Console.WriteLine("Press ");
            Console.WriteLine("1. To add new available feed");
            Console.WriteLine("2. To exit ");
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                switch (key.Key.ToString().Replace("Numpad", string.Empty).Replace("D", string.Empty))
                {
                    case "1":
                        AddNewFeed();
                        break;
                    case "2":
                        return;
                };
            }
        }

        private static void AddNewFeed()
        {
            Console.WriteLine("New feed name:");
            var feedName = Console.ReadLine();
            Console.WriteLine("URL:");
            var url = Console.ReadLine();
            var newFeed = new CreateFeedDTO()
            {
                Name = feedName,
                Url = new Uri(url)
            };
            client.AddFeed(newFeed);
        }

        public static UserFeedsDTO DisplayUserFeeds()
        {
            UserFeedsDTO selectedUserFeed = null;
            var userfeeds = client.GetUserFeeds().Result;
            Console.WriteLine("==UserFeeds==");
            foreach (var userfeed in userfeeds)
            {
                Console.WriteLine("{0}. {1}", userfeeds.IndexOf(userfeed), userfeed.Name);
                List<FeedDTO> feeds = new List<FeedDTO>();
                feeds.AddRange(userfeed.SubscribedFeeds);
                foreach (var feed in feeds)
                {
                    Console.WriteLine("\t{0}. {1}", feeds.IndexOf(feed), feed.Name);
                };
            }
            Console.WriteLine("Please type an index and press enter, -1 to exit");
            int userfeedIndex;
            while (!int.TryParse(Console.ReadLine(), out userfeedIndex)) ;
            if (userfeedIndex == -1) return null;
            selectedUserFeed = userfeeds[userfeedIndex];
            return selectedUserFeed;
        }

        public static void AddFeedToUserFeed(UserFeedsDTO selectedUserFeed)
        {
            Console.WriteLine("Select available feeds");
            var feeds = client.GetFeed().Result;
            foreach (var _feed in feeds)
            {
                Console.WriteLine("{0}. {1}", feeds.IndexOf(_feed), _feed.Name);
            }
            Console.WriteLine("Please type an index and press enter, -1 to exit");
            int feedIndex;
            while (!int.TryParse(Console.ReadLine(), out feedIndex)) ;
            if (feedIndex == -1) return;
            var feed = feeds[feedIndex];
            client.AddFeedToUserFeed(selectedUserFeed.Name, feed.ID).Wait();
        }

        public static void DeleteFeedFromUserFeed(UserFeedsDTO selectedUserFeed)
        {
            Console.WriteLine("Select feed to delete from this userfeed");
            List<FeedDTO> feeds = new List<FeedDTO>();
            feeds.AddRange(selectedUserFeed.SubscribedFeeds);
            foreach (var feed in feeds)
            {
                Console.WriteLine("{0}. {1}", feeds.IndexOf(feed), feed.Name);
            };
            Console.WriteLine("Please type an index and press enter, -1 to exit");
            int feedIndex;
            while (!int.TryParse(Console.ReadLine(), out feedIndex)) ;
            if (feedIndex == -1) return;
            var _feed = feeds[feedIndex];
            client.DeleteFeedFromUserFeed(selectedUserFeed.Name, _feed.ID).Wait();
        }

        public static void DisplayFeedItemsFromUserFeed(UserFeedsDTO selectedUserFeed)
        {
            Console.WriteLine("=={0}==", selectedUserFeed.Name);

            var items = client.GetUserFeedItems(selectedUserFeed.Name).Result;
            foreach (var item in items)
            {
                Console.WriteLine(item.Source);
                Console.WriteLine(item.Title);
                Console.WriteLine(item.Content);
                Console.WriteLine(item.URL);
                Console.WriteLine(item.PublishDate);
                Console.WriteLine(item.FetchDate);
            }
        }

        public static void DisplayFeedItemsFromUserFeedFormatted(UserFeedsDTO selectedUserFeed)
        {
            Console.WriteLine("==FORMATTED=={0}==", selectedUserFeed.Name);
            var items = client.GetUserFeedItemsFormatted(selectedUserFeed.Name).Result;
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }
    }
}
