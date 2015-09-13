using System.Collections.Generic;
using BitbucketSharp.Models;
using BitbucketSharp.Models.V2;

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
        /// <param name="branch">The starting node</param>
        /// <returns></returns>
        public Collection<CommitModel> GetCommits(string branch = null)
        {
            var url = Client.ApiUrl2 + "repositories/" + Repository.Owner.Username + "/" + Repository.Slug + "/commits/" + branch;
            return Client.Request2<Collection<CommitModel>>(url);
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
        /// Gets the comments.
        /// </summary>
        public ChangesetCommentsController Comments
        {
            get { return new ChangesetCommentsController(Client, this); }
        }

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
            Node = node;
        }

        /// <summary>
        /// Requests information about the changeset
        /// </summary>
        /// <returns></returns>
        public ChangesetModel GetInfo(bool forceCacheInvalidation = false)
        {
            return Client.Get<ChangesetModel>(Uri, forceCacheInvalidation);
        }

        /// <summary>
        /// /Gets the diffs
        /// </summary>
        /// <returns></returns>
        public List<ChangesetDiffModel> GetDiffs(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<ChangesetDiffModel>>(Uri + "/diffstat", forceCacheInvalidation);
        }

        /// <summary>
        /// Gets the likes for a changeset
        /// </summary>
        /// <returns>The likes.</returns>
        /// <param name="forceCacheInvalidation">If set to <c>true</c> force cache invalidation.</param>
        public List<ChangesetLikeModel> GetLikes(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<ChangesetLikeModel>>(Uri + "/likes", forceCacheInvalidation);
        }

        /// <summary>
        /// Gets the participants for a changeset
        /// </summary>
        /// <returns>The participants.</returns>
        /// <param name="forceCacheInvalidation">If set to <c>true</c> force cache invalidation.</param>
        public List<ChangesetParticipantsModel> GetParticipants(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<ChangesetParticipantsModel>>(Uri + "/participants", forceCacheInvalidation);
        }

		public string GetPatch(bool forceCacheInvalidation = false)
		{
			return Client.Get<string>(Uri + "/patch/" + Node, forceCacheInvalidation, Client.ApiUrl2);
		}

        /// <summary>
        /// Approve this instance.
        /// </summary>
        public void Approve()
        {
			Client.Post(Repository.Uri + "/commit/" + Node + "/approve", null, Client.ApiUrl2);
        }

        /// <summary>
        /// Unapprove this instance.
        /// </summary>
        public void Unapprove()
        {
			Client.Delete(Repository.Uri + "/commit/" + Node + "/approve", Client.ApiUrl2);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Repository.Uri + "/changesets/" + Node; }
        }
    }
}
