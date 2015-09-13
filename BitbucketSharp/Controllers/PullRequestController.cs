using System;
using BitbucketSharp.Models.V2;
using BitbucketSharp.Models;
using System.Collections.Generic;

namespace BitbucketSharp.Controllers
{
	public class PullRequestsController : Controller
	{
		/// <summary>
		/// Gets the repository.
		/// </summary>
		public RepositoryController Repository { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="client">A handle to the client</param>
		/// <param name="repo">The repository the branch belongs to</param>
		public PullRequestsController(Client client, RepositoryController repo) : base(client)
		{
			Repository = repo;
		}

		/// <summary>
		/// Access a specific branch via the slug
		/// </summary>
		/// <param name="branch">The repository branch</param>
		/// <returns></returns>
		public PullRequestController this[ulong id]
		{
			get { return new PullRequestController(Client, this, id); } 
		}

		/// <summary>
		/// Gets all the branches in this repository
		/// </summary>
		/// <returns></returns>
		public Collection<PullRequestModel> GetAll(string state = "OPEN", bool forceCacheInvalidation = false)
		{
			return Client.Get<Collection<PullRequestModel>>(Uri + "?pagelen=30&state=" + state, forceCacheInvalidation, Client.ApiUrl2);
		}

		/// <summary>
		/// The URI of this controller
		/// </summary>
		public override string Uri
		{
			get { return Repository.Uri + "/pullrequests"; }
		}
	}

	public class PullRequestController : Controller
    {
		public PullRequestsController Parent { get; private set; }

		public ulong Id { get; private set; }

		public PullRequestController(Client client, PullRequestsController parent, ulong id)
			: base(client)
        {
			Parent = parent;
			Id = id;
        }

		public PullRequestModel Get(bool forceCacheInvalidation = false)
		{
			return Client.Get<PullRequestModel>(Uri, forceCacheInvalidation, Client.ApiUrl2);
		}

		public PullRequestModel Merge()
		{
			return Client.Post<PullRequestModel>(Uri + "/merge", null, baseUri: Client.ApiUrl2);
		}

		public PullRequestModel Decline()
		{
			return Client.Post<PullRequestModel>(Uri + "/decline", null, baseUri: Client.ApiUrl2);
		}

		public Collection<PullRequestCommentModel> GetComments(bool forceCacheInvalidation = false)
		{
			return Client.Get<Collection<PullRequestCommentModel>>(Uri + "/comments", forceCacheInvalidation, Client.ApiUrl2);
		}

        public Collection<CommitModel> GetCommits(bool forceCacheInvalidation = false)
        {
            return Client.Get<Collection<CommitModel>>(Uri + "/commits", forceCacheInvalidation, Client.ApiUrl2);
        }

		public OldPullRequestCommentModel AddComment(string content)
		{
			var d = new Dictionary<string, string>() {{"content", content}};
			return Client.Post<OldPullRequestCommentModel>(Uri + "/comments", d);
		}

		public override string Uri
		{
			get { return Parent.Uri + "/" + Id; }
		}
    }
}

