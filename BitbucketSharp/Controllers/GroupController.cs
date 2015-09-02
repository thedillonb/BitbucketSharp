using System.Collections.Generic;
using BitbucketSharp.Models;

namespace BitbucketSharp.Controllers
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
        /// Gets the groups.
        /// </summary>
        public List<GroupModel> GetGroups(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<GroupModel>>(Uri, forceCacheInvalidation);
        }

        /// <summary>
        /// Access a specific group with a specified groupname
        /// </summary>
        /// <param name="groupname">The name of the group</param>
        /// <returns></returns>
        public GroupController this[string groupname]
        {
            get { return new GroupController(Client, this, groupname); }
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return "groups/" + User.Username; }
        }
    }

    /// <summary>
    /// Provides access to a group object
    /// </summary>
    public class GroupController : Controller
    {
        /// <summary>
        /// The groups this group belongs to
        /// </summary>
        public GroupsController Groups { get; private set; }

        /// <summary>
        /// The group name
        /// </summary>
        public string Groupname { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="groups">The groups the group belongs to</param>
        /// <param name="groupname">The name of the group</param>
        public GroupController(Client client, GroupsController groups, string groupname) 
            : base(client)
        {
            Groups = groups;
            Groupname = groupname;
        }

        /// <summary>
        /// Gets the group info
        /// </summary>
        public GroupModel GetInfo(bool forceCacheInvalidation = false)
        {
            return Client.Get<GroupModel>(Uri, forceCacheInvalidation);
        }

        /// <summary>
        /// Add a member to this group
        /// </summary>
        public void AddMember(string member)
        {
            Client.InvalidateCacheObjects(Uri);
            Client.Put<string>(Uri + "/members/" + member);
        }

        /// <summary>
        /// Remove a member of this group
        /// </summary>
        public void RemoveMember(string member)
        {
            Client.InvalidateCacheObjects(Uri);
            Client.Delete(Uri + "/members/" + member);
        }

        /// <summary>
        /// List the members of this group
        /// </summary>
        /// <returns></returns>
        public List<UserModel> ListMembers(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<UserModel>>(Uri + "/members", forceCacheInvalidation);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Groups.Uri + "/" + Groupname; }
        }
    }
}
