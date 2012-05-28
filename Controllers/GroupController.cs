using System.Collections.Generic;
using BitBucketSharp.Models;

namespace BitBucketSharp.Controllers
{
    public class GroupController : Controller
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public GroupController(Client client) 
            : base(client)
        {
        }

        /// <summary>
        /// Add a member to this group
        /// </summary>
        public void AddMember(string username, string groupname, string member)
        {
            Client.Put<string>("groups/" + username + "/" + groupname + "/members/" + member);
        }

        /// <summary>
        /// Remove a member of this group
        /// </summary>
        public void RemoveMember(string username, string groupname, string member)
        {
            Client.Delete("groups/" + username + "/" + groupname + "/members/" + member);
        }

        /// <summary>
        /// List the members of this group
        /// </summary>
        /// <returns></returns>
        public List<UserModel> ListMembers(string username, string groupname)
        {
            return Client.Get<List<UserModel>>("groups/" + username + "/" + groupname + "/members");
        }
    }
}
