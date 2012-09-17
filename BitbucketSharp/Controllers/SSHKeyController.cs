using System;
using BitbucketSharp.Models;
using System.Collections.Generic;

namespace BitbucketSharp.Controllers
{
    public class SSHKeyController : Controller
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public SSHKeyController(Client client)
            : base(client)
        {
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
            get { return "ssh-keys"; }
        }
    }
}

