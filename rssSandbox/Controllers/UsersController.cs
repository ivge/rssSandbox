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
    [RoutePrefix("api/users")]
    public partial class UsersController : ApiController
    {
        /// <summary>
        /// Returns all users.
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
    }
}
