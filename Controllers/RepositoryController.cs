using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitBucketSharp.Models;

namespace BitBucketSharp.Controllers
{
    public class RepositoryController : Controller
    {
        public string Owner { get; private set; }

        public string Slug { get; private set; }

        public RepositoryController(Client client, string repoOwner, string repoSlug) 
            : base(client)
        {
            Owner = repoOwner;
            Slug = repoSlug;
        }

        public RepositoryDetailedModel GetInfo()
        {
            return Client.Get<RepositoryDetailedModel>("repositories/" + Owner + "/" + Slug);
        }

        public FollowersModel GetFollowers()
        {
            return Client.Get<FollowersModel>("repositories/" + Owner + "/" + Slug + "/followers");
        }

        public IssuesModel GetIssues()
        {
            return Client.Get<IssuesModel>("repositories/" + Owner + "/" + Slug + "/issues");
        }

        public IssueModel GetIssue(int id)
        {
            return Client.Get<IssueModel>("repositories/" + Owner + "/" + Slug + "/issues/" + id);
        }

    }
}
