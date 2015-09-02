using System.Collections.Generic;
using BitbucketSharp.Models;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// Provides access to emails
    /// </summary>
    public class EmailController : Controller
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public EmailController(Client client) 
            : base(client)
        {
        }

        /// <summary>
        /// Gets the list of emails
        /// </summary>
        /// <returns></returns>
        public IList<EmailModel> GetEmails(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<EmailModel>>(Uri, forceCacheInvalidation);
        }

        /// <summary>
        /// Search through all the emails for a particular address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="forceCacheInvalidation">Forces the invalidation of hte cahce for this object</param>
        /// <returns></returns>
        public EmailModel SearchEmails(string emailAddress, bool forceCacheInvalidation = false)
        {
            return Client.Get<EmailModel>(Uri + "/" + emailAddress, forceCacheInvalidation);
        }

        /// <summary>
        /// Adds an email address only if the email is being added to the logged in user's account
        /// </summary>
        /// <param name="emailAddress">The email address to add</param>
        /// <returns></returns>
        public EmailModel AddEmail(string emailAddress)
        {
            Client.InvalidateCacheObjects(Uri);
            return Client.Put<EmailModel>(Uri + "/" + emailAddress);
        }

        /// <summary>
        /// Sets the primary email address only if the email address is that of the logged in user's account
        /// </summary>
        /// <param name="emailAddress">The email address to designate as primary</param>
        /// <returns></returns>
        public EmailModel SetPrimaryEmail(string emailAddress)
        {
            Client.InvalidateCacheObjects(Uri);
            return Client.Post<EmailModel>(Uri + "/" + emailAddress, new Dictionary<string, string> { { "primary", "true" } });
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return "emails"; }
        }
    }
}
