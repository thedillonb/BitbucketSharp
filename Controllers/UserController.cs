using System.Collections.Generic;
using BitBucketSharp.Models;

namespace BitBucketSharp.Controllers
{
    public class UserController : Controller
    {
        public string Username { get; private set; }

        public UserController(Client client, string username)
            : base(client)
        {
            Username = username;
        }

        public UsersModel GetInfo()
        {
            return Client.Get<UsersModel>("users/" + Username);
        }

        public FollowersModel GetFollowers()
        {
            return Client.Get<FollowersModel>("users/" + Username + "/followers");
        }

        public IList<EmailModel> GetEmails()
        {
            return Client.Get<List<EmailModel>>("users/" + Username + "/emails");
        }

        public EmailModel GetEmail(string emailAddress)
        {
            return Client.Get<EmailModel>("users/" + Username + "/emails/" + emailAddress);
        }
    }

    /// <summary>
    /// A controller dedicated to the actions for the user logged in!
    /// </summary>
    public class MeController : UserController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public MeController(Client client) 
            : base(client, client.Username)
        {
        }

        /// <summary>
        /// Requests the repositories that the current logged in user is following
        /// </summary>
        /// <returns>A list of repositories</returns>
        public List<RepositoryDetailedModel> GetRepositoriesFollowing()
        {
            return Client.Get<List<RepositoryDetailedModel>>("user/follows");
        }

    }
}
