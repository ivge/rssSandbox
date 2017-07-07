using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using rssSandbox.Entities;
using rssSandbox.Models;
using rssSandbox.DTO;
using System.Security;

namespace rssSandbox.Controllers
{
    [RoutePrefix("api/users")]
    public partial class UsersController : ApiController
    {
        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns>List of existing users</returns>
        [Route("")]
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

        [Route("Add/{login}/password/{password}")]
        [HttpPost]
        public IHttpActionResult CreateUser(string login, string password)
        {
            var user = new User(login, password);
            DataModel.Users.Add(user);
            return Ok(user.ID);
        }

    }
}
