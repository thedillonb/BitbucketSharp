namespace BitbucketSharp.Models
{
    public class WikiModel
    {
        public string Data { get; set; }
        public string Rev { get; set; }
        public string Markup { get; set; }
    }

    public class OldPullRequestComment
    {
        public int CommentId { get; set; }
        public int PullRequestId { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string ContentRendered { get; set; }
    }
}
