namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// Provides a base class for all controllers
    /// </summary>
    public abstract class Controller
    {
        /// <summary>
        /// The client this controller belongs to
        /// </summary>
		internal Client Client { get; private set; }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public abstract string Uri { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        protected Controller(Client client)
        {
            Client = client;
        }
    }
}
