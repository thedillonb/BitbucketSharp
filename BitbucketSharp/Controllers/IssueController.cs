using System.Collections.Generic;
using BitbucketSharp.Models;
using BitbucketSharp.Utils;
using System;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// Provides access to issues owned by a repository
    /// </summary>
    public class IssuesController : Controller
    {
        /// <summary>
        /// Gets the repository the issues belong to
        /// </summary>
        public RepositoryController Repository { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="repository">The repository the issues belong to</param>
        public IssuesController(Client client, RepositoryController repository) : base(client)
        {
            Repository = repository;
        }

        /// <summary>
        /// Access specific issues via the id
        /// </summary>
        /// <param name="id">The id of the issue</param>
        /// <returns></returns>
        public IssueController this[int id]
        {
            get { return new IssueController(Client, Repository, id); }
        }

        /// <summary>
        /// Search through the issues for a specific match
        /// </summary>
        /// <param name="search">The match to search for</param>
        /// <returns></returns>
        public IssuesModel Search(string search)
        {
            return Client.Request<IssuesModel>(Uri + "/?search=" + search);
        }

        /// <summary>
        /// Gets all the issues for this repository
        /// </summary>
        /// <param name="start">The start index of the returned set (default: 0)</param>
        /// <param name="limit">The limit of items of the returned set (default: 15)</param>
        /// <returns></returns>
        public IssuesModel GetIssues(int start = 0, int limit = 15, IEnumerable<Tuple<string, string>> search = null)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(Uri).Append("/?start=").Append(start).Append("&limit=").Append(limit);
            if (search != null)
                foreach (var a in search)
                    sb.Append("&").Append(a.Item1).Append("=").Append(a.Item2);

            return Client.Request<IssuesModel>(sb.ToString());
        }

        public List<ComponentModel> GetComponents(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<ComponentModel>>(Uri + "/components", forceCacheInvalidation);
        }

        public List<VersionModel> GetVersions(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<VersionModel>>(Uri + "/versions", forceCacheInvalidation);
        }

        public List<MilestoneModel> GetMilestones(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<MilestoneModel>>(Uri + "/milestones", forceCacheInvalidation);
        }

        /// <summary>
        /// Create a new issue for this repository
        /// </summary>
        /// <param name="issue">The issue model to create</param>
        /// <returns></returns>
        public IssueModel Create(CreateIssueModel issue)
        {
            return Client.Post<IssueModel, CreateIssueModel>(Uri, issue);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Repository.Uri + "/issues"; }
        }
    }

    /// <summary>
    /// Provides access to an issue
    /// </summary>
    public class IssueController : Controller
    {
        /// <summary>
        /// Gets the id of the issue
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the repository the issue belongs to
        /// </summary>
        public RepositoryController Repository { get; private set; }

        /// <summary>
        /// Gets the comments this issue has
        /// </summary>
        public CommentsController Comments { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="repository">The repository this issue belongs to</param>
        /// <param name="id">The id of this issue</param>
        public IssueController(Client client, RepositoryController repository, int id) 
            : base(client)
        {
            Id = id;
            Repository = repository;
            Comments = new CommentsController(Client, this);
        }

        /// <summary>
        /// Requests the issue information
        /// </summary>
        /// <returns></returns>
        public IssueModel GetIssue(bool forceCacheInvalidation= false)
        {
            return Client.Get<IssueModel>(Uri, forceCacheInvalidation);
        }

        /// <summary>
        /// Requests the follows of this issue
        /// </summary>
        /// <returns></returns>
        public FollowersModel GetIssueFollowers(bool forceCacheInvalidation = false)
        {
            return Client.Get<FollowersModel>(Uri + "/followers", forceCacheInvalidation);
        }

        /// <summary>
        /// Deletes this issue
        /// </summary>
        public void Delete()
        {
            Client.InvalidateCacheObjects(Uri);
            Client.Delete(Uri);
        }

        /// <summary>
        /// Updates an issue
        /// </summary>
        /// <param name="issue">The issue model</param>
        /// <returns></returns>
        public IssueModel Update(CreateIssueModel issue)
        {
            return Update(ObjectToDictionaryConverter.Convert(issue));
        }

        /// <summary>
        /// Updates an issue
        /// </summary>
        /// <param name="data">The update data</param>
        /// <returns></returns>
        public IssueModel Update(Dictionary<string,string> data)
        {
            Client.InvalidateCacheObjects(Uri);
            return Client.Put<IssueModel>(Uri, data);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Repository.Uri + "/issues/" + Id; }
        }
    }
}
