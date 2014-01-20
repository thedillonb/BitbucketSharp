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
        public DateTime Utctimestamp { get; set; }
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
        public DateTime UtcLastUpdated { get; set; }
        public string FilenameHash { get; set; }
        public string Filename { get; set; }
        public string Content { get; set; }
        public string ContentRendered { get; set; }
        public string UserAvatarUrl { get; set; }
        public long? LineFrom { get; set; }
        public long? LineTo { get; set; }
        public DateTime UtcCreatedOn { get; set; }
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
        public class CommitModel
        {
            public string Hash { get; set; }
            public RepositoryModel Repository { get; set; }
            public AuthorModel Author { get; set; }
            public string Message { get; set; }
            public DateTime Date { get; set; }

            public class AuthorModel
            {
                public string Raw { get; set; }
                public UserModel User { get; set; }
            }
        }

        public class RepositoryModel
        {
            public string FullName { get; set; }
            public string Name { get; set; }
            public LinksModel Links { get; set; }

            public class LinksModel
            {
                public LinkModel Avatar { get; set; }
            }
        }
    }
}
