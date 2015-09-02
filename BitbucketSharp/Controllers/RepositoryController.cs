using System.Collections.Generic;
using BitbucketSharp.Models;
using BitbucketSharp.MonoTouch.Controllers;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// Provides access to repositories owned by a user
    /// </summary>
    public class UserRepositoriesController : Controller
    {
        /// <summary>
        /// Gets the owner of the repositories
        /// </summary>
        public UserController Owner { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="owner">The owner of the repositories</param>
        public UserRepositoriesController(Client client, UserController owner) : base(client)
        {
            Owner = owner;
        }

        /// <summary>
        /// Access a specific repository via the slug
        /// </summary>
        /// <param name="slug">The repository slug</param>
        /// <returns></returns>
        public RepositoryController this[string slug]
        {
            get { return new RepositoryController(Client, Owner, slug); } 
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return "repositories"; }
        }
    }

    /// <summary>
    /// Provides access to 'global' repositories via a search method
    /// </summary>
    public class RepositoriesController : Controller
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        public RepositoriesController(Client client) : base(client)
        {
        }

        /// <summary>
        /// Search for a specific repository via the name
        /// </summary>
        /// <param name="name">The partial or full name to search for</param>
        /// <returns>A list of RepositorySimpleModel</returns>
        public RepositorySearchModel Search(string name)
        {
            return Client.Request<RepositorySearchModel>(Uri + "/?name=" + name);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return "repositories"; }
        }
    }

    /// <summary>
    /// Provides access to a repository
    /// </summary>
    public class RepositoryController : Controller
    {
        /// <summary>
        /// Gets a handle to the issue controller
        /// </summary>
        public IssuesController Issues
        {
            get { return new IssuesController(Client, this); }
        }

        /// <summary>
        /// Gets the owner of the repository
        /// </summary>
        public UserController Owner { get; private set; }

        /// <summary>
        /// Gets the slug of the repository
        /// </summary>
        public string Slug { get; private set; }

        /// <summary>
        /// Gets the wikis of this repository
        /// </summary>
        public WikisController Wikis
        {
            get { return new WikisController(Client, this); }
        }

        /// <summary>
        /// Gets the invitations to this repository
        /// </summary>
        public InvitationController Invitations
        {
            get { return new InvitationController(Client, this); }
        }

        /// <summary>
        /// Gets the changesets.
        /// </summary>
        public ChangesetsController Changesets
        {
            get { return new ChangesetsController(Client, this); }
        }

        /// <summary>
        /// Gets the branches
        /// </summary>
        public BranchesController Branches
        {
            get { return new BranchesController(Client, this); }
        }

        /// <summary>
        /// Gets the privileges for this repository
        /// </summary>
        public RepositoryPrivilegeController Privileges
        {
            get { return new RepositoryPrivilegeController(Client, new UserPrivilegesController(Client, Owner), Slug); }
        }

        /// <summary>
        /// Gets the group privileges for this repository
        /// </summary>
        public RepositoryGroupPrivilegeController GroupPrivileges
        {
            get { return new RepositoryGroupPrivilegeController(Client, this); }
        }

		/// <summary>
		/// Gets the pull requests.
		/// </summary>
		public PullRequestsController PullRequests
		{
			get { return new PullRequestsController(Client, this); }
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">The owner of this repository</param>
        /// <param name="slug">The slug of this repository</param>
        /// <param name="client">A handle to the client</param>
        public RepositoryController(Client client, UserController owner, string slug) 
            : base(client)
        {
            Owner = owner;
            Slug = slug.Replace(' ', '-').ToLower();
        }

        /// <summary>
        /// Requests the information on a specific repository
        /// </summary>
        /// <returns>A RepositoryDetailedModel</returns>
        public RepositoryDetailedModel GetInfo(bool forceCacheInvalidation = false)
        {
            return Client.Get<RepositoryDetailedModel>(Uri, forceCacheInvalidation);
        }

        /// <summary>
        /// Requests the followers of a specific repository
        /// </summary>
        /// <returns>A FollowersModel</returns>
        public FollowersModel GetFollowers(bool forceCacheInvalidation = false)
        {
            return Client.Get<FollowersModel>(Uri + "/followers", forceCacheInvalidation);
        }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        public Dictionary<string, TagModel> GetTags(bool forceCacheInvalidation = false)
        {
            return Client.Get<Dictionary<string, TagModel>>(Uri + "/tags", forceCacheInvalidation);
        }

        /// <summary>
        /// Requests the events of a repository
        /// </summary>
        /// <param name="start">The start index of returned items (default: 0)</param>
        /// <param name="limit">The limit of returned items (default: 25)</param>
        /// <param name="type">The type of event to return. If null, all event types are returned</param>
        /// <returns>A EventsModel</returns>
        public EventsModel GetEvents(int start = 0, int limit = 25, string type = null)
        {
            return Client.Request<EventsModel>(Uri + "/events/?start=" + start + "&limit=" +
                                               limit + (type == null ? "" : "&type=" + type));
        }

        /// <summary>
        /// Forks the repository.
        /// </summary>
        /// <returns>The repository model of the forked repository.</returns>
        /// <param name="name">The name of the new forked repository.</param>
        /// <param name="description">the description of the forked repository.</param>
        /// <param name="language">The language of the forked repository.</param>
        /// <param name="isPrivate">Whether or not the forked repository is private.</param>
        public RepositoryDetailedModel ForkRepository(string name, string description = null, string language = null, bool? isPrivate = null)
        {
            var data = new Dictionary<string, string>();
            data.Add("name", name);
            if (description != null)
                data.Add("description", description);
            if (language != null)
                data.Add("language", language);
            if (isPrivate != null)
                data.Add("is_private", isPrivate.Value.ToString());

            return Client.Post<RepositoryDetailedModel>(Uri + "/fork", data);
        }

        /// <summary>
        /// Toggle's the following for this repository. Don't use this...
        /// </summary>
        /// <returns><c>true</c>, if follow was toggled, <c>false</c> otherwise.</returns>
        public RepositoryFollowModel ToggleFollow()
        {
            return Client.Post<RepositoryFollowModel>(Owner + "/" + Slug + "/follow", null, "https://bitbucket.org/");
        }

        /// <summary>
        /// Gets the primary branch
        /// </summary>
        /// <returns>The primary branch.</returns>
        /// <param name="forceCacheInvalidation">If set to <c>true</c> force cache invalidation.</param>
        public PrimaryBranchModel GetPrimaryBranch(bool forceCacheInvalidation = false)
        {
            return Client.Get<PrimaryBranchModel>(Uri + "/main-branch", forceCacheInvalidation);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return "repositories/" + Owner.Username + "/" + Slug; }
        }
    }
}
