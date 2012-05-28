using System.Collections.Generic;
using BitBucketSharp.Models;

namespace BitBucketSharp.Controllers
{
    public class RepositoryController : Controller
    {
        /// <summary>
        /// Gets a handle to the issue controller
        /// </summary>
        public IssueController Issues { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        public RepositoryController(Client client) 
            : base(client)
        {
            Issues = new IssueController(client);
        }

        /// <summary>
        /// Search for a specific repository via the name
        /// </summary>
        /// <param name="name">The partial or full name to search for</param>
        /// <returns>A list of RepositorySimpleModel</returns>
        public IList<RepositorySimpleModel> Search(string name)
        {
            return Client.Get<List<RepositorySimpleModel>>("repositories/?name=" + name);
        }

        /// <summary>
        /// Requests the information on a specific repository
        /// </summary>
        /// <param name="owner">The owner username of the repository</param>
        /// <param name="slug">The slug of the repository</param>
        /// <returns>A RepositoryDetailedModel</returns>
        public RepositoryDetailedModel GetInfo(string owner, string slug)
        {
            return Client.Get<RepositoryDetailedModel>("repositories/" + owner + "/" + slug);
        }

        /// <summary>
        /// Requests the followers of a specific repository
        /// </summary>
        /// <param name="owner">The owner username of the repository</param>
        /// <param name="slug">The slug of the repository</param>
        /// <returns>A FollowersModel</returns>
        public FollowersModel GetFollowers(string owner, string slug)
        {
            return Client.Get<FollowersModel>("repositories/" + owner + "/" + slug + "/followers");
        }

        /// <summary>
        /// Requests the events of a repository
        /// </summary>
        /// <param name="owner">The owner username of the repository</param>
        /// <param name="slug">The slug of the repository</param>
        /// <param name="start">The start index of returned items (default: 0)</param>
        /// <param name="limit">The limit of returned items (default: 25)</param>
        /// <param name="type">The type of event to return. If null, all event types are returned</param>
        /// <returns>A EventsModel</returns>
        public EventsModel GetEvents(string owner, string slug, int start = 0, int limit = 25, string type = null)
        {
            return Client.Get<EventsModel>("repositories/" + owner + "/" + slug + "/events/?start=" + start + "&limit=" +
                                           limit + (type == null ? "" : "&type=" + type));
        }
    }
}
