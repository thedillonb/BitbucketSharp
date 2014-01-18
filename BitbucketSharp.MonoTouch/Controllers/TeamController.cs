using System;
using System.Collections.Generic;

namespace BitbucketSharp.Controllers
{
	public class TeamsController : Controller
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="client">A handle to the client</param>
		public TeamsController(Client client)
			: base(client)
		{
		}

		/// <summary>
		/// Provides access to a specific user via a username
		/// </summary>
		/// <param name="username">The username of the user</param>
		/// <returns></returns>
		public TeamController this[string username]
		{
			get { return new TeamController(this, username); }
		}

		/// <summary>
		/// The URI of this controller
		/// </summary>
		public override string Uri
		{
			get { return "teams"; }
		}
	}

	public class TeamController : Controller
    {
		/// <summary>
		/// The username
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The parent
		/// </summary>
		public TeamsController Parent { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public TeamController(TeamsController parent, string name)
			: base(parent.Client)
		{
			Parent = parent;
			Name = name;
		}

		/// <summary>
		/// Gets information about this user
		/// </summary>
		/// <returns>A UsersModel</returns>
		public BitbucketSharp.Models.V2.Collection<BitbucketSharp.Models.V2.UserModel> GetMembers(bool forceCacheInvalidation = false)
		{
			return Client.Get<BitbucketSharp.Models.V2.Collection<BitbucketSharp.Models.V2.UserModel>>(Uri, forceCacheInvalidation, Client.ApiUrl2);
		}

		/// <summary>
		/// The URI of this controller
		/// </summary>
		public override string Uri
		{
			get { return Parent.Uri + "/" + Name + "/members"; }
		}
    }
}

