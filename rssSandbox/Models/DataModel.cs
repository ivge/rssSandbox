using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using rssSandbox.Entities;

namespace rssSandbox.Models
{
    public static class DataModel
    {
        public static HashSet<RSSFeed> RSSFeeds = new HashSet<RSSFeed>()
        {
            new RSSFeed("Meduza", new Uri(@"https://meduza.io/rss/all"))/*,
            new RSSFeed("ЛІГА.Новости", new Uri(@"http://news.liga.net/all/rss.xml")),
            new RSSFeed("ЛІГА.Бизнес", new Uri(@"http://biz.liga.net/all/rss.xml"))*/
        };

        public static  List<User> Users = new List<User>()
        {
            new User("John Doe", "se7en"),
            new User("Marry Sue", "qwerty"),
        };



    }
}