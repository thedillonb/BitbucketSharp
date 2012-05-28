using System.Collections.Generic;
using BitBucketSharp.Models;

namespace BitBucketSharp.Controllers
{
    /// <summary>
    /// Provides access to a list of groups for a user
    /// </summary>
    public class GroupsController : Controller
    {
        /// <summary>
        /// The user to search through for groups
        /// </summary>
        public UserController User { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="user"></param>
        public GroupsController(Client client, UserController user) : base(client)
        {
            User = user;
        }

        /// <summary>
        /// Access a specific group with a specified groupname
        /// </summary>
        /// <param name="groupname">The name of the group</param>
        /// <returns></returns>
        public GroupController this[string groupname]
        {
            get { return new GroupController(Client, User, groupname); }
        }

    }

    /// <summary>
    /// Provides access to a group object
    /// </summary>
    public class GroupController : Controller
    {
        /// <summary>
        /// The username the group belongs to
        /// </summary>
        public UserController User { get; private set; }

        /// <summary>
        /// The group name
        /// </summary>
        public string Groupname { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="user">The user the group belongs to</param>
        /// <param name="groupname">The name of the group</param>
        public GroupController(Client client, UserController user, string groupname) 
            : base(client)
        {
            User = user;
            Groupname = groupname;
        }

        /// <summary>
        /// Add a member to this group
        /// </summary>
        public void AddMember(string member)
        {
            Client.Put<string>("groups/" + User.Username + "/" + Groupname + "/members/" + member);
        }

        /// <summary>
        /// Remove a member of this group
        /// </summary>
        public void RemoveMember(string member)
        {
            Client.Delete("groups/" + User.Username + "/" + Groupname + "/members/" + member);
        }

        /// <summary>
        /// List the members of this group
        /// </summary>
        /// <returns></returns>
        public List<UserModel> ListMembers()
        {
            return Client.Get<List<UserModel>>("groups/" + User.Username + "/" + Groupname + "/members");
        }
    }
}
