using System.Collections.Generic;
using System;

namespace BitbucketSharp.Models
{
    public class ChangesetsModel
    {
        public int Count { get; set; }
        public string Start { get; set; }
        public int Limit { get; set; }
        public List<ChangesetModel> Changesets { get; set; }
    }

    public class ChangesetModel
    {
        public string Node { get; set; }
        public string Author { get; set; }
        public string Timestamp { get; set; }
        public DateTimeOffset Utctimestamp { get; set; }
        public string Branch { get; set; }
        public string Message { get; set; }
        public int Revision { get; set; }
        public long Size { get; set; }
        public List<FileModel> Files { get; set; } 
        public string RawNode { get; set; }
        public List<string> Parents { get; set; }

        public class FileModel
        {
            public string Type { get; set; }
            public string File { get; set; }
        }
    }

    public class ChangesetCommentModel
    {
        public string Username { get; set; }
        public string Node { get; set; }
        public long CommentId { get; set; }
        public string DisplayName { get; set; }
        public long? ParentId { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset UtcLastUpdated { get; set; }
        public string FilenameHash { get; set; }
        public string Filename { get; set; }
        public string Content { get; set; }
        public string ContentRendered { get; set; }
        public string UserAvatarUrl { get; set; }
        public long? LineFrom { get; set; }
        public long? LineTo { get; set; }
        public DateTimeOffset UtcCreatedOn { get; set; }
        public bool IsSpam { get; set; }
    }

    public class CreateChangesetCommentModel
    {
        public string Content { get; set; }
        public long? LineFrom { get; set; }
        public long? LineTo { get; set; }
        public long? ParentId { get; set; }
        public string Filename { get; set; }
    }

    public class ChangesetLikeModel
    {
        public DateTime UtcLikedOn { get; set; }
        public UserModel User { get; set; }
    }

    public class ChangesetParticipantsModel
    {
        public bool Approved { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
    }

    public class ChangesetDiffModel
    {
        public string Type { get; set; }
        public string File { get; set; }
        public DiffModel Diffstat { get; set; } 

        public class DiffModel
        {
            public int Removed { get; set; }
            public int Added { get; set; }
        }
    }

    namespace V2
    {
        public class Commit
        {
            public string Hash { get; set; }
            public RepositorySource Repository { get; set; }
            public AuthorModel Author { get; set; }
            public string Message { get; set; }
            public DateTimeOffset Date { get; set; }
            public List<Participant> Participants { get; set; }
            public List<CommitParent> Parents { get; set; }

            public class AuthorModel
            {
                public string Raw { get; set; }
                public User User { get; set; }
            }
        }

        public class CommitComment
        {
            public User User { get; set; }
            public CommitCommentContent Content { get; set; }
            public DateTimeOffset CreatedOn { get; set; }
            public DateTimeOffset UpdatedOn { get; set; }
        }

        public class CommitCommentContent
        {
            public string Raw { get; set; }
            public string Markup { get; set; }
            public string Html { get; set; }
        }

        public class CommitParent
        {
            public string Hash { get; set; }
        }

        public class Participant
        {
            public string Role { get; set; }
            public bool Approved { get; set; }
            public User User { get; set; }
        }


        public class PullRequest
        {
            public string Description { get; set; }
            public User Author { get; set; }
            public bool ClosedSourceBranch { get; set; }
            public string Title { get; set; }
            public string Reason { get; set; }
            public User ClosedBy { get; set; }
            public string State { get; set; }
            public DateTimeOffset CreatedOn { get; set; }
            public DateTimeOffset UpdatedOn { get; set; }
            public string MergeCommit { get; set; }
            public int Id { get; set; }
            public int? CommentCount { get; set; }
            public int? TaskCount { get; set; }
            public List<User> Reviewers { get; set; }
            public List<Participant> Participants { get; set; }
            public PullRequestSource Source { get; set; }
            public PullRequestSource Destination { get; set; }

            public class LinksModel
            {
                public LinkModel Avatar { get; set; }
            }
        }

        public class PullRequestSource
        {
            public RepositorySource Repository { get; set; }
            public CommitSource Commit { get; set; }

            public class CommitSource
            {
                public string Hash { get; set; }
            }
       
        }

        public class RepositorySource
        {
            public string Name { get; set; }
            public string FullName { get; set; }
            public LinksModel Links { get; set; }

            public class LinksModel
            {
                public LinkModel Avatar { get; set; }
            }
        }

        public enum PullRequestState
        {
            Open,
            Merged,
            Declined
        }
    }
}
