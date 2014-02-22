using System.Collections.Generic;
using BitbucketSharp.Models;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// A controller dedicated to the actions for the user logged in!
    /// </summary>
    public class AccountController : UserController
    {
        /// <summary>
        /// Email for this user
        /// </summary>
        public EmailController Emails 
        {
            get { return new EmailController(Client); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public AccountController(Client client)
            : base(client, client.Username)
        {
        }

        /// <summary>
        /// Gets information about this user
        /// </summary>
        /// <returns>A UsersModel</returns>
        public new UsersModel GetInfo(bool forceCacheInvalidation = false)
        {
            return Client.Get<UsersModel>("user", forceCacheInvalidation);
        }

        /// <summary>
        /// Requests the repositories that is visible to the current user
        /// </summary>
        /// <returns>A list of repositories</returns>
        public List<RepositoryDetailedModel> GetRepositories(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<RepositoryDetailedModel>>("user/repositories", forceCacheInvalidation);
        }


        /// <summary>
        /// Requests the repositories that the current logged in user is following
        /// </summary>
        /// <returns>A list of repositories</returns>
        public List<RepositoryDetailedModel> GetRepositoriesFollowing(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<RepositoryDetailedModel>>("user/follows", forceCacheInvalidation);
        }

        /// <summary>
        /// Gets the privileges of user
        /// </summary>
        public AccountPrivileges GetPrivileges(bool forceCacheInvalidation = false)
        {
            return Client.Get<AccountPrivileges>("user/privileges", forceCacheInvalidation);
        }
    }
}
