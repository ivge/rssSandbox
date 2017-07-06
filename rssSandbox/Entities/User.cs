using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rssSandbox.Entities
{
    public class User
    {
        /// <summary>
        /// Users login
        /// </summary>
        public string Login { get; }

        /// <summary>
        /// Users password
        /// </summary>
        private string Password { get; }

        /// <summary>
        /// Unique identifier
        /// </summary>
        public Guid ID; 


        /// <summary>
        /// List of users feeds 
        /// </summary>
        public List<UserFeed> Feeds { get; }

        public User()
        {
            Feeds = new List<UserFeed>();
            ID = Guid.NewGuid();
        }

        public User(string login, string password):this()
        {
            this.Login = login;
            this.Password = password;
        }

        /// <summary>
        /// Create new users feed
        /// </summary>
        /// <param name="name">Name for new user feed</param>
        /// <returns>User feed ID</returns>
        public Guid CreateNewUserFeed(string name)
        {
            if (this.Feeds.Any(uf => uf.Name.Equals(name,StringComparison.InvariantCultureIgnoreCase)))
                throw new UserFeedAlreadyExistsException();
            var userFeed = new UserFeed(name);
            this.Feeds.Add(userFeed);
            return userFeed.ID;
        }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message)
        : base(message)
        {
        }

        public UserNotFoundException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    
}
