namespace BitBucketSharp.Controllers
{
    public abstract class Controller
    {
        /// <summary>
        /// The client this controller belongs to
        /// </summary>
        protected Client Client { get; private set; }

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
