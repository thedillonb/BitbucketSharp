using System.Collections.Generic;
using BitbucketSharp.Models;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// Accesses comments for an issue
    /// </summary>
    public class CommentsController : Controller
    {
        /// <summary>
        /// The issue these comments belong to
        /// </summary>
        public IssueController Issue { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="issue"></param>
        public CommentsController(Client client, IssueController issue) : base(client)
        {
            Issue = issue;
        }

        /// <summary>
        /// Access a specific comment
        /// </summary>
        /// <param name="id">The id of the comment to access</param>
        /// <returns></returns>
        public CommentController this[string id]
        {
            get { return new CommentController(Client, this, id);}
        }

        /// <summary>
        /// Gets all the comments
        /// </summary>
        /// <returns></returns>
        public List<CommentModel> GetComments()
        {
            return Client.Get<List<CommentModel>>(Uri);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Issue.Uri + "/comments"; }
        }
    }

    /// <summary>
    /// Accesses a specific comment
    /// </summary>
    public class CommentController : Controller
    {
        /// <summary>
        /// The issue this comment belongs
        /// </summary>
        public CommentsController Comments { get; private set; }

        /// <summary>
        /// The id of the comment
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The client handle</param>
        /// <param name="comments">The comments this comment belongs to</param>
        /// <param name="id"></param>
        public CommentController(Client client, CommentsController comments, string id)
            : base(client)
        {
            Comments = comments;
            Id = id;
        }

        /// <summary>
        /// Gets the comment
        /// </summary>
        /// <returns></returns>
        public CommentModel GetInfo()
        {
            return Client.Get<CommentModel>(Uri);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Comments.Uri + "/" + Id; }
        }
    }
}
