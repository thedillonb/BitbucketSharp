using System.Collections.Generic;
using BitBucketSharp.Models;

namespace BitBucketSharp.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handel to the client</param>
        public UserController(Client client)
            : base(client)
        {
        }

        /// <summary>
        /// Gets information about this user
        /// </summary>
        /// <param name="username">The username to get information on</param>
        /// <returns>A UsersModel</returns>
        public UsersModel GetInfo(string username)
        {
            return Client.Get<UsersModel>("users/" + username);
        }

        /// <summary>
        /// Gets the events for a specific user
        /// </summary>
        /// <param name="username">The username to query events for</param>
        /// <param name="start">The start index for returned items(default: 0)</param>
        /// <param name="limit">The limit index for returned items (default: 25)</param>
        /// <returns>A EventsModel</returns>
        public EventsModel GetEvents(string username, int start = 0, int limit = 25)
        {
            return Client.Get<EventsModel>("users/" + username + "/events/?start=" + start + "&limit=" + limit);
        }
    }
}
