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
    partial class Program
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



    }
}
