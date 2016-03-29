using System;
using System.Threading.Tasks;
using BitbucketSharp.Models.V2;
using BitbucketSharp.Models;
using System.Collections.Generic;

namespace BitbucketSharp.Controllers
{
    public class UsersController
    {
        private readonly Client _client;

        public UsersController(Client client)
        {
            _client = client;
        }

        public Task<User> GetUser()
        {
            var uri = Client.ApiUrl2.With("user");
            return _client.Get<User>(uri);
        }

        public Task<User> GetUser(string username)
        {
            var uri = Client.ApiUrl2.With("users").WithQuery(username);
            return _client.Get<User>(uri);
        }

        public Task<Collection<User>> GetFollowers(string username)
        {
            var uri = Client.ApiUrl2.With("users").WithQuery(username).With("followers");
            return _client.Get<Collection<User>>(uri);
        }

        public Task<Collection<User>> GetFollowing(string username)
        {
            var uri = Client.ApiUrl2.With("users").WithQuery(username).With("following");
            return _client.Get<Collection<User>>(uri);
        }

        public Task<Collection<Repository>> GetRepositories(string username)
        {
            var uri = Client.ApiUrl2.With("repositories").WithQuery(username);
            return _client.Get<Collection<Repository>>(uri);
        }

        public Task<EventsModel> GetEvents(string username, int start = 0, int limit = 30)
        {
            var uri = Client.ApiUrl.With("users").WithQuery(username).With("events?start=" + start + "&limit=" + limit);
            return _client.Get<EventsModel>(uri);
        }

        public Task<List<PrivilegeModel>> GetPrivileges(string username)
        {
            var uri = Client.ApiUrl.With("users").WithQuery(username).With("privileges");
            return _client.Get<List<PrivilegeModel>>(uri);
        }

        public Task<AccountPrivileges> GetPrivileges()
        {
            var uri = Client.ApiUrl.With("user/privileges");
            return _client.Get<AccountPrivileges>(uri);
        }
    }
}
