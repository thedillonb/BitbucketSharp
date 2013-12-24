using BitbucketSharp.Models;
using BitbucketSharp.MonoTouch.Controllers;
using BitbucketSharp.Models.V2;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// Provides access to a list of users
    /// </summary>
    public class UsersController : Controller
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        public UsersController(Client client)
            : base(client)
        {
        }

        /// <summary>
        /// Provides access to a specific user via a username
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns></returns>
        public UserController this[string username]
        {
            get { return new UserController(Client, username); }
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return "users/"; }
        }
    }

    /// <summary>
    /// Provides access to a user
    /// </summary>
    public class UserController : Controller
    {
        /// <summary>
        /// The username
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Groups that belong to this user
        /// </summary>
        public GroupsController Groups
        {
            get { return new GroupsController(Client, this); }
        }

        /// <summary>
        /// Repositories that belong to this user
        /// </summary>
        public UserRepositoriesController Repositories
        {
            get { return new UserRepositoriesController(Client, this); }
        }

        /// <summary>
        /// Gets the privileges for this user
        /// </summary>
        public UserPrivilegesController Privileges
        {
            get { return new UserPrivilegesController(Client, this); }
        }

        /// <summary>
        /// Gets the SSH keys.
        /// </summary>
        public SSHKeyController SSHKeys
        {
            get { return new SSHKeyController(Client, this); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserController(Client client, string username)
            : base(client)
        {
            Username = username;
        }

        /// <summary>
        /// Gets information about this user
        /// </summary>
        /// <returns>A UsersModel</returns>
        public UsersModel GetInfo(bool forceCacheInvalidation = false)
        {
            return Client.Get<UsersModel>(Uri, forceCacheInvalidation);
        }

        /// <summary>
        /// Gets the events for a specific user
        /// </summary>
        /// <param name="start">The start index for returned items(default: 0)</param>
        /// <param name="limit">The limit index for returned items (default: 25)</param>
        /// <returns>A EventsModel</returns>
        public EventsModel GetEvents(int start = 0, int limit = 25)
        {
            return Client.Request<EventsModel>(Uri + "/events/?start=" + start + "&limit=" + limit);
        }
		
		/// <summary>
		/// Gets the followers.
		/// </summary>
		/// <returns>The followers./returns>
        public FollowersModel GetFollowers(bool forceCacheInvalidation = false)
		{
            return Client.Get<FollowersModel>(Uri + "/followers", forceCacheInvalidation);
		}

		public Collection<BitbucketSharp.Models.V2.UserModel> GetFollowing(bool forceCacheInvalidation = false, int limit = 100)
		{
			return Client.Get<Collection<BitbucketSharp.Models.V2.UserModel>>(Uri + "/following?pagelen=" + limit, forceCacheInvalidation, Client.ApiUrl2);
		}

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return "users/" + Username; }
        }
    }
}
