using System.Collections.Generic;
using BitbucketSharp.Models;
using System.Threading.Tasks;

namespace BitbucketSharp.Controllers
{
    public class GroupsController
    {
        private readonly Client _client;

        public GroupsController(Client client)
        {
            _client = client;
        }

        public Task<List<GroupModel>> GetGroups(string accountName)
        {
            var uri = Client.ApiUrl.With("groups").WithQuery(accountName);
            return _client.Get<List<GroupModel>>(uri);
        }

        public Task<List<UserModel>> GetMembers(string accountName, string group)
        {
            var uri = Client.ApiUrl.With("groups").WithQuery(accountName).WithQuery(group).With("members");
            return _client.Get<List<UserModel>>(uri);
        }
    }
}
