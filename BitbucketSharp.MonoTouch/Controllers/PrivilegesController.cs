using System.Collections.Generic;
using BitbucketSharp.Models;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// Provides access to repositories owned by a user
    /// </summary>
    public class UserPrivilegesController : Controller
    {
        /// <summary>
        /// Gets the user
        /// </summary>
        public UserController Owner { get; private set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="owner">The owner</param>
        public UserPrivilegesController(Client client, UserController owner) : base(client)
        {
            Owner = owner;
        }

        /// <summary>
        /// Gets the privileges for this user
        /// </summary>
        public List<PrivilegeModel> GetPrivileges()
        {
            return Client.Get<List<PrivilegeModel>>(Uri);
        }
        
        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return "privileges/" + Owner.Username; }
        }
    }

    /// <summary>
    /// Provides access to a repository's privileges
    /// </summary>
    public class RepositoryPrivilegeController : Controller
    {
        /// <summary>
        /// Gets the user's privileges
        /// </summary>
        public UserPrivilegesController UserPrivileges { get; private set; }

        //The repository slug
        public string Slug { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">The owner of this repository</param>
        /// <param name="slug">The slug of this repository</param>
        /// <param name="client">A handle to the client</param>
        public RepositoryPrivilegeController(Client client, UserPrivilegesController userPrivileges, string slug) 
            : base(client)
        {
            UserPrivileges = userPrivileges;
            Slug = slug.ToLower();
        }
        
        /// <summary>
        /// Requests the information on a specific repository's privileges
        /// </summary>
        /// <returns>A list of PrivilegeModels</returns>
        public List<PrivilegeModel> GetPrivileges()
        {
            return Client.Get<List<PrivilegeModel>>(Uri);
        }
  
        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return UserPrivileges.Uri + "/" + Slug; }
        }
    }
}