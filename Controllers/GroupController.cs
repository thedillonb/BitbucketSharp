using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitBucketSharp.Models;

namespace BitBucketSharp.Controllers
{
    public class GroupController : Controller
    {
        public string Username { get; private set; }

        public GroupController(Client client, string username) : base(client)
        {
            Username = username;
        }

        public List<GroupModel> GetGroups()
        {
            return Client.Get<List<GroupModel>>("groups/" + Username);
        }

        public void AddMember(string username)
        {
            
        }

        public void RemoveMember(string username)
        {
            
        }

        public List<UserModel> ListMembers(string groupName)
        {
            return Client.Get<List<UserModel>>("groups/" + Username + "/" + groupName + "/members");
        }
    }
}
