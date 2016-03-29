using System;
using BitbucketSharp.Models.V2;
using System.Threading.Tasks;

namespace BitbucketSharp.Controllers
{
    public class TeamsController
    {
        private readonly Client _client;

        public TeamsController(Client client)
        {
            _client = client;
        }

        public Task<Collection<Team>> GetTeams(TeamRole role = TeamRole.Admin)
        {
            var uri = Client.ApiUrl2.With("teams?role=" + role.ToString().ToLower());
            return _client.Get<Collection<Team>>(uri);
        }

        public Task<Team> GetTeam(string team)
        {
            var uri = Client.ApiUrl2.With("teams").WithQuery(team);
            return _client.Get<Team>(uri);
        }

        public Task<Collection<TeamMember>> GetMembers(string team)
        {
            var uri = Client.ApiUrl2.With("teams").WithQuery(team).With("members");
            return _client.Get<Collection<TeamMember>>(uri);
        }

        public Task<Collection<User>> GetFollowers(string team)
        {
            var uri = Client.ApiUrl2.With("teams").WithQuery(team).With("followers");
            return _client.Get<Collection<User>>(uri);
        }

        public Task<Collection<User>> GetFollowing(string team)
        {
            var uri = Client.ApiUrl2.With("teams").WithQuery(team).With("followers");
            return _client.Get<Collection<User>>(uri);
        }

        public Task<Collection<Repository>> GetRepositories(string team)
        {
            var uri = Client.ApiUrl2.With("teams").WithQuery(team).With("repositories");
            return _client.Get<Collection<Repository>>(uri);
        }
    }

    public enum TeamRole
    {
        Admin,
        Contributor,
        Member
    }
}

