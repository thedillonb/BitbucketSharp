using BitbucketSharp.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace BitbucketSharp.Controllers
{
    public class IssuesController
    {
        private readonly Client _client;

        public IssuesController(Client client)
        {
            _client = client;
        }

        public Task<IssueModel> GetIssue(string owner, string repository, int id)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("issues").WithQuery(id.ToString());
            return _client.Get<IssueModel>(uri);
        }

        public Task<IssuesModel> GetIssues(string owner, string repository, int start = 0, int limit = 30)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("issues?start=" + start + "&limit=" + limit);
            return _client.Get<IssuesModel>(uri);
        }

        public Task<List<IssueComponent>> GetComponents(string owner, string repository)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("issues/components");
            return _client.Get<List<IssueComponent>>(uri);
        }

        public Task<List<IssueVersion>> GetVersions(string owner, string repository)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("issues/versions");
            return _client.Get<List<IssueVersion>>(uri);
        }

        public Task<List<IssueMilestone>> GetMilestones(string owner, string repository)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("issues/milestones");
            return _client.Get<List<IssueMilestone>>(uri);
        }

        public Task<IssueModel> UpdateMilestone(string owner, string repository, int id, string milestone)
        {
            var d = new Dictionary<string, string>();
            d.Add("milestone", milestone);
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("issues").WithQuery(id.ToString());
            return _client.Put<IssueModel>(uri, d);
        }

        public Task<IssueModel> UpdateVersion(string owner, string repository, int id, string version)
        {
            var d = new Dictionary<string, string>();
            d.Add("version", version);
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("issues").WithQuery(id.ToString());
            return _client.Put<IssueModel>(uri, d);
        }

        public Task<IssueModel> UpdateComponent(string owner, string repository, int id, string component)
        {
            var d = new Dictionary<string, string>();
            d.Add("component", component);
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("issues").WithQuery(id.ToString());
            return _client.Put<IssueModel>(uri, d);
        }
    }
}
