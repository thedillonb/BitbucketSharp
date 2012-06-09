using System;
using BitbucketSharp.Controllers;


namespace BitbucketSharp.MonoTouch
{
    /// <summary>
    /// Provides access to 'global' branches on a repository
    /// </summary>
    public class BranchesController : Controller
    {
        /// <summary>
        /// Gets the repository.
        /// </summary>
        public RepositoryController Repository { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        public BranchesController(Client client, RepositoryController repo) : base(client)
        {
            Repository = repo;
        }

        /// <summary>
        /// Access a specific branch via the slug
        /// </summary>
        /// <param name="slug">The repository branch</param>
        /// <returns></returns>
        public BranchController this[string branch]
        {
            get { return new BranchController(Client, Repository, branch); } 
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        protected override string Uri
        {
            get { return "repositories"; }
        }
    }

    public class BranchController : Controller
    {
        public string Branch { get; private set; }

        public RepositoryController Repository { get; set; }

        public BranchController(Client client, RepositoryController repo, string branch) : base(client)
        {
            Repository = repo;
            Branch = branch;
        }

        protected override string Uri 
        {
            get { return "repositories/" + Repository.Owner.Username + "/" + Repository.Slug; }
        }
    }
}

