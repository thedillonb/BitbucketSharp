using System;
using BitbucketSharp.Models;
using System.Collections.Generic;

namespace BitbucketSharp.Controllers
{
    public class SSHKeyController : Controller
    {
        public UserController User { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public SSHKeyController(Client client, UserController user)
            : base(client)
        {
            User = user;
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <returns>
        /// The keys.
        /// </returns>
        public List<SSHKeyModel> GetKeys(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<SSHKeyModel>>(Uri, forceCacheInvalidation);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri 
        {
            get { return User.Uri + "/ssh-keys"; }
        }
    }
}

