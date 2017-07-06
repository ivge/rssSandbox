using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using rssSandbox.Entities;

namespace rssSandbox.Models
{
    public static class DataModel
    {
        public static HashSet<Feed> Feeds;

        public static List<User> Users;

        static DataModel()
        {
            Feeds = new HashSet<Feed>();
            Users = new List<User>();
            
            SeedFeeds();
            SeedUsers();
        }

        static void SeedFeeds()
        {
            try
            {

                Feeds.Add(new RSSFeed("Meduza", new Uri(@"https://meduza.io/rss/all")));
                Feeds.Add(new RSSFeed("ЛІГА.Новости", new Uri(@"http://news.liga.net/all/rss.xml")));
                Feeds.Add(new RSSFeed("ЛІГА.Бизнес", new Uri(@"http://biz.liga.net/all/rss.xml")));
            }
            catch (Exception e) { }
        }

        static void SeedUsers()
        {
 
            Users.Add(new User("John Doe", "se7en"));
            Users.Add(new User("Marry Sue", "qwerty"));
            try
            {
                var feed = new RSSFeed("Факты и комментарии.Житейские истории", new Uri(@"http://fakty.ua/rss_feed/life-stories"));
                var user = new User("John Smith", "1234");
                var userfeed = new UserFeed("testFeed");
                userfeed.Add(feed);
                userfeed.Add(Feeds.ToArray()[0]);
                user.Feeds.Add(userfeed);
                Users.Add(user);
                Feeds.Add(feed);
            }
            catch (Exception e) { }
        }
    }
}