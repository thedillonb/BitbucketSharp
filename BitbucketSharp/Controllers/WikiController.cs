using System.Collections.Generic;
using BitbucketSharp.Models;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// Provides access to wikis owned by a repository
    /// </summary>
    public class WikisController : Controller
    {
        /// <summary>
        /// Gets the repository this wiki belongs to
        /// </summary>
        public RepositoryController Repository { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="repository"></param>
        public WikisController(Client client, RepositoryController repository)
            : base(client)
        {
            Repository = repository;
        }

        /// <summary>
        /// Access the wiki for a specific page
        /// </summary>
        /// <param name="page">The page of the wiki</param>
        /// <returns></returns>
        public WikiController this[string page]
        {
            get { return new WikiController(Client, this, page); }
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Repository.Uri + "/wiki/"; }
        }
    }

    /// <summary>
    /// Provides access to a wiki page
    /// </summary>
    public class WikiController : Controller
    {
        /// <summary>
        /// Gets the wikis this wiki belongs to
        /// </summary>
        public WikisController Wikis { get; private set; }

        /// <summary>
        /// Gets the page of the wiki
        /// </summary>
        public string Page { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="wikis">The wikis this wiki belongs to</param>
        /// <param name="page">The page of this wiki</param>
        public WikiController(Client client, WikisController wikis, string page)
            : base(client)
        {
            Wikis = wikis;
            Page = page;
        }

        /// <summary>
        /// Requests the Wiki page
        /// </summary>
        /// <returns></returns>
        public WikiModel GetInfo(bool forceCacheInvalidation = false)
        {
            return Client.Get<WikiModel>(Uri, forceCacheInvalidation);
        }

        /// <summary>
        /// Updates a wiki page
        /// </summary>
        /// <param name="data">The data to put on the wiki</param>
        public void Update(string data, string path)
        {
            Client.InvalidateCacheObjects(Uri);
            Client.Put(Uri, new Dictionary<string, string> {{"data", data}, {"path", path}});
        }

        /// <summary>
        /// Creates a wiki page
        /// </summary>
        /// <param name="data"></param>
        public void Create(string data)
        {
            Client.InvalidateCacheObjects(Uri);
            Client.Post(Uri, new Dictionary<string, string> { { "data", data } });
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Wikis.Uri + Page; }
        }
    }
}
