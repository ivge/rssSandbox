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
        }

        static void Seed()
        {
            Feeds.Add(new RSSFeed("Meduza", new Uri(@"https://meduza.io/rss/all")));
            Feeds.Add(new RSSFeed("ЛІГА.Новости", new Uri(@"http://news.liga.net/all/rss.xml")));
            Feeds.Add(new RSSFeed("ЛІГА.Бизнес", new Uri(@"http://biz.liga.net/all/rss.xml")));
            Users.Add(new User("John Doe", "se7en"));
            Users.Add(new User("Marry Sue", "qwerty"));
        }


    }
}