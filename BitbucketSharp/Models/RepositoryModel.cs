using System.Collections.Generic;

namespace BitbucketSharp.Models
{
    public class RepositoryDetailedModel : RepositorySimpleModel
    {
        public string Description { get; set; }
        public int ForkCount { get; set; }
        public string ResourceUri { get; set; }
        public string Website { get; set; }
        public bool HasWiki { get; set; }
        public string LastUpdated { get; set; }
        public string CreatedOn { get; set; }
        public string Logo { get; set; }
        public int Size { get; set; }
        public bool ReadOnly { get; set; }
        public string Language { get; set; }
        public int FollowersCount { get; set; }
        public string State { get; set; }
        public bool HasIssues { get; set; }
        public bool IsFork { get; set; }
        public bool EmailWriters { get; set; }
        public bool NoPublicForks { get; set; }
    }

    public class RepositorySimpleModel
    {
        public bool IsPrivate { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Scm { get; set; }
    }

    //Cant use any of the above :(
    public class RepositorySearchModel
    {
        public int Count { get; set; }
        public string Query { get; set; }
        public List<RepositoryDetailedModel> Repositories { get; set; }
    }

    public class TagModel
    {
        public string Node { get; set; }
        public List<FileModel> Files { get; set; }
        public string RawAuthor { get; set; }
        public string Utctimestamp { get; set; }
        public string Timestamp { get; set; }
        public string Author { get; set; }
        public string RawNode { get; set; }
        public List<string> Parents { get; set; }
        public string Branch { get; set; }
        public string Message { get; set; }
        public string Revision { get; set; }
        public int Size { get; set; }


        public class FileModel
        {
            public string Type { get; set; }
            public string File { get; set; }
        }
    }
}
