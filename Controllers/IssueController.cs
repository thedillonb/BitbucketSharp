using BitBucketSharp.Models;

namespace BitBucketSharp.Controllers
{
    public class IssueController : Controller
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the clietn</param>
        public IssueController(Client client) 
            : base(client)
        {
        }

        public IssuesModel SearchIssues(string owner, string slug, string search)
        {
            return Client.Get<IssuesModel>("repositories/" + owner + "/" + slug + "/issues/?search=" + search);
        }

        public IssuesModel GetIssues(string owner, string slug, int start = 0, int limit = 15)
        {
            return Client.Get<IssuesModel>("repositories/" + owner + "/" + slug + "/issues/?start=" + start + "&limit=" + limit);
        }

        public IssueModel GetIssue(string owner, string slug, int id)
        {
            return Client.Get<IssueModel>("repositories/" + owner + "/" + slug + "/issues/" + id);
        }

        public FollowersModel GetIssueFollowers(string owner, string slug, int id)
        {
            return Client.Get<FollowersModel>("repositories/" + owner + "/" + slug + "/issues/" + id + "/followers");
        }

        public void DeleteIssue(string owner, string slug, int id)
        {
            Client.Delete("repositories/" + owner + "/" + slug + "/issues/" + id + "/");
        }
    }
}
