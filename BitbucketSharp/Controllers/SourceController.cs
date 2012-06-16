using BitbucketSharp.Models;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// Provides access to all the sources in a branch
    /// </summary>
    public class SourcesController : Controller
    {
        /// <summary>
        /// Gets the branch the sources belong to
        /// </summary>
        public BranchController Branch { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="branch">The branch the sources belong to</param>
        public SourcesController(Client client, BranchController branch) 
            : base(client)
        {
            Branch = branch;
        }

        /// <summary>
        /// Accesses a specific path in the source tree
        /// </summary>
        /// <param name="path">The path to accesses</param>
        /// <returns></returns>
        public SourceController this[string path]
        {
            get { return new SourceController(Client, this, path); }
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Branch.Branches.Repository.Uri + "/src/" + Branch.Name; }
        }
    }

    /// <summary>
    /// Accesses a source file
    /// </summary>
    public class SourceController : Controller
    {
        /// <summary>
        /// Gets the sources this controller belongs to
        /// </summary>
        public SourcesController Sources { get; private set; }

        /// <summary>
        /// Gets the path to the source
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="sources">The sources controller</param>
        /// <param name="path">The path to the source</param>
        public SourceController(Client client, SourcesController sources, string path)
            : base(client)
        {
            Sources = sources;

            //Remove the "/" if it starts with it
            Path = path.StartsWith("/") ? path.Substring(1) : path;
        }

        /// <summary>
        /// Gets the source model for ths controller
        /// </summary>
        /// <returns></returns>
        public SourceModel GetInfo()
        {
            return Client.Get<SourceModel>(Uri);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Sources.Uri + "/" + Path; }
        }
    }
}
