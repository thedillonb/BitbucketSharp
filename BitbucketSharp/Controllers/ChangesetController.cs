using System.Collections.Generic;
using BitbucketSharp.Models;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// Provides access to changesets belonging to a repository
    /// </summary>
    public class ChangesetsController : Controller
    {
        /// <summary>
        /// Gets the repository these changesets belongs to
        /// </summary>
        public RepositoryController Repository { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="repository">The repository these changesets belongs to</param>
        public ChangesetsController(Client client, RepositoryController repository)
            : base(client)
        {
            Repository = repository;
        }

        /// <summary>
        /// Access a specific changeset that references the node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public ChangesetController this[string node]
        {
            get { return new ChangesetController(Client, Repository, node);}
        }

        /// <summary>
        /// Requests all the changesets
        /// </summary>
        /// <param name="start">The start index of returned items (default: 0)</param>
        /// <param name="limit">The limit of returned items (default: 15)</param>
        /// <returns></returns>
        public ChangesetsModel GetChangesets(int limit = 15, string startNode = null)
        {
            var url = Uri + "?limit=" + limit;
            if (startNode != null)
                url = url + "&start=" + startNode;

            return Client.Request<ChangesetsModel>(url);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Repository.Uri + "/changesets"; }
        }
    }

    /// <summary>
    /// Provides access to a specific changeset
    /// </summary>
    public class ChangesetController : Controller
    {
        /// <summary>
        /// Gets the repository this changeset belongs to
        /// </summary>
        public RepositoryController Repository { get; private set; }

        /// <summary>
        /// Gets the node this changeset refers to
        /// </summary>
        public string Node { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="repository"></param>
        /// <param name="node"></param>
        public ChangesetController(Client client, RepositoryController repository, string node) 
            : base(client)
        {
            Repository = repository;
            Node = node.ToLower();
        }

        /// <summary>
        /// Requests information about the changeset
        /// </summary>
        /// <returns></returns>
        public ChangesetModel GetInfo(bool forceCacheInvalidation = false)
        {
            return Client.Get<ChangesetModel>(Uri + "/" + Node, forceCacheInvalidation);
        }

        /// <summary>
        /// /Gets the diffs
        /// </summary>
        /// <returns></returns>
        public IList<ChangesetDiffModel> GetDiffs(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<ChangesetDiffModel>>(Uri + "/" + Node + "/diffstat", forceCacheInvalidation);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Repository.Uri + "/changesets"; }
        }
    }
}
