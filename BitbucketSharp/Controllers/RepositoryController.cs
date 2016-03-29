using BitbucketSharp.Models;
using System.Threading.Tasks;
using System;
using System.IO;
using BitbucketSharp.Models.V2;
using System.Collections.Generic;

namespace BitbucketSharp.Controllers
{
    public class RepositoriesController
    {
        private readonly Client _client;

        public RepositoriesController(Client client)
        {
            _client = client;
        }

        public Task<RepositorySearchModel> Search(string name)
        {
            var uri = new Uri(Client.ApiUrl, "/repositories?name=" + Uri.EscapeDataString(name));
            return _client.Get<RepositorySearchModel>(uri);
        }

        public Task<Collection<Repository>> GetRepositories(string username)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(username);
            return _client.Get<Collection<Repository>>(uri);
        }

        public Task<Repository> GetRepository(string owner, string repository)
        {
            return _client.Get<Repository>(
                Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository));
        }

        public Task<Collection<User>> GetWatchers(string owner, string repository)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("watchers");
            return _client.Get<Collection<User>>(uri);
        }

        public Task<Collection<Repository>> GetForks(string owner, string repository)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("forks");
            return _client.Get<Collection<Repository>>(uri);
        }

        public Task<Collection<Download>> GetDownloads(string owner, string repository)
        {
            return _client.Get<Collection<Download>>(
                Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("downloads"));
        }

        public Task<Collection<Component>> GetComponents(string owner, string repository)
        {
            return _client.Get<Collection<Component>>(
                Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("components"));
        }

        public Task<Stream> GetDiff(string owner, string repository, string spec)
        {
            return _client.GetRaw(Client.ApiUrl2.With("repositories").WithQuery(owner)
                .WithQuery(repository).With("diff").WithQuery(spec));
        }

        public Task<Stream> GetPatch(string owner, string repository, string spec)
        {
            return _client.GetRaw(Client.ApiUrl2.With("repositories").WithQuery(owner)
                .WithQuery(repository).With("patch").WithQuery(spec));
        }

        public Task<Collection<PullRequest>> GetPullRequests(string owner, string repository, PullRequestState state = PullRequestState.Open)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("pullrequests?state=" + state);
            return _client.Get<Collection<PullRequest>>(uri);
        }

        public Task<PullRequest> GetPullRequest(string owner, string repository, int id)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("pullrequests").WithQuery(id.ToString());
            return _client.Get<PullRequest>(uri);
        }
   
        public Task<Collection<Commit>> GetCommits(string owner, string repository, string branch = null)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("commits").WithQuery(branch);
            return _client.Get<Collection<Commit>>(uri);
        }

        public Task<IDictionary<string, BranchModel>> GetBranches(string owner, string repository)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("branches");
            return _client.Get<IDictionary<string, BranchModel>>(uri);
        }

        public Task<MainBranch> GetMainBranch(string owner, string repository)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("main-branch");
            return _client.Get<MainBranch>(uri);
        }

        public Task<IDictionary<string, BranchModel>> GetTags(string owner, string repository)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("tags");
            return _client.Get<IDictionary<string, BranchModel>>(uri);
        }

        public Task<FollowersModel> GetFollowers(string owner, string repository)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("followers");
            return _client.Get<FollowersModel>(uri);
        }

        public Task<Collection<PullRequestComment>> GetPullRequestComments(string owner, string repository, int id)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("pullrequests").WithQuery(id.ToString()).With("comments");
            return _client.Get<Collection<PullRequestComment>>(uri);
        }

        public Task<PullRequestComment> GetPullRequestComment(string owner, string repository, int id, int commentId)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("pullrequests").WithQuery(id.ToString()).With("comments").WithQuery(commentId.ToString());
            return _client.Get<PullRequestComment>(uri);
        }

        public Task<PullRequest> MergePullRequest(string owner, string repository, int id, string message = null)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("pullrequests").WithQuery(id.ToString()).With("merge");
            return _client.Post<PullRequest>(uri, new { message });
        }

        public Task<Participant> ApprovePullRequest(string owner, string repository, int id)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("pullrequests").WithQuery(id.ToString()).With("approve");
            return _client.Post<Participant>(uri);
        }

        public Task DeclinePullRequest(string owner, string repository, int id)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("pullrequests").WithQuery(id.ToString()).With("approve");
            return _client.Delete(uri);
        }

        public Task<OldPullRequestComment> CommentPullRequest(string owner, string repository, int id, string comment)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("pullrequests").WithQuery(id.ToString()).With("comments");
            return _client.Post<OldPullRequestComment>(uri, new { content = comment });
        }

        public Task<Collection<Commit>> GetPullRequestCommits(string owner, string repository, int id)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("pullrequests").WithQuery(id.ToString()).With("commits");
            return _client.Get<Collection<Commit>>(uri);
        }

        public Task<Commit> GetCommit(string owner, string repository, string revision)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("commit").WithQuery(revision);
            return _client.Get<Commit>(uri);
        }

        public Task<EventsModel> GetEvents(string owner, string repository, int start = 0, int limit = 30)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("events?start=" + start + "&limit=" + limit);
            return _client.Get<EventsModel>(uri);
        }

        public Task<Participant> ApproveCommit(string owner, string repository, string revision)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("commit").WithQuery(revision).With("approve");
            return _client.Post<Participant>(uri);
        }

        public Task UnapproveCommit(string owner, string repository, string revision)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(owner).WithQuery(repository).With("commit").WithQuery(revision).With("approve");
            return _client.Delete(uri);
        }

        public Task<WikiModel> GetWiki(string owner, string repository, string page)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("wiki").WithQuery(page);
            return _client.Get<WikiModel>(uri);
        }

        public Task<RepositoryDetailedModel> Fork(string owner, string repository)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("fork");
            return _client.Post<RepositoryDetailedModel>(uri, new { name = repository });
        }

        public Task<FileModel> GetFile(string owner, string repository, string revision, string file)
        {
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("src").WithQuery(revision).With(file.TrimStart('/'));
            return _client.Get<FileModel>(uri);
        } 

        public Task<SourceModel> GetSource(string owner, string repository, string revision, string source = null)
        {
            source = source ?? "/";
            var uri = Client.ApiUrl.With("repositories").WithQuery(owner).WithQuery(repository).With("src").WithQuery(revision).With(source.TrimStart('/'));
            return _client.Get<SourceModel>(uri);
        } 
    }
}
