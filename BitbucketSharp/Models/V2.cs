using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitbucketSharp.Models.V2
{
    public class Collection<T>
    {
        public ulong Size { get; set; }
        public ulong Page { get; set; }
        public uint Pagelen { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<T> Values { get; set; }
    }

    public class Repository
    {
        public string Scm { get; set; }
        public bool HasWiki { get; set; }
        public string Description { get; set; }
        public LinksModel Links { get; set; }
        public string ForkPolicy { get; set; }
        public string Language { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string FullName { get; set; }
        public string Website { get; set; }
        public bool HasIssues { get; set; }
        public User Owner { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public long Size { get; set; }
        public bool IsPrivate { get; set; }
        public string Name { get; set; }
        public ForkParent Parent { get; set; }

        public class LinksModel
        {
            public LinkModel Avatar { get; set; }
        }

        public class ForkParent
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Fullname { get; set; }
        }
    }

    public class Component
    {
        public string Name { get; set; }
    }

    public class Team
    {
        public string Username { get; set; }
        public string Website { get; set; }
        public string DisplayName { get; set; }
        public LinksModel Links { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }

        public class LinksModel
        {
            public LinkModel Avatar { get; set; }
        }
    }

    public class TeamMember
    {
        public string Username { get; set; }
        public string Website { get; set; }
        public string DisplayName { get; set; }
        public LinksModel Links { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }

        public class LinksModel
        {
            public LinkModel Avatar { get; set; }
        }
    }

    public class Download
    {
        public DateTimeOffset CreatedOn { get; set; }
        public int Downloads { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public User User { get; set; }
    }

    public class User
    {
        public string Username { get; set; }
        public string Kind { get; set; }
        public string Website { get; set; }
        public string DisplayName { get; set; }
        public string Location { get; set; }
        public DateTime CreatedOn { get; set; }
        public LinksModel Links { get; set; }

        public class LinksModel
        {
            public LinkModel Avatar { get; set; }
        }
    }

    public class LinkModel
    {
        public string Href { get; set; }
    }

    public class PullRequestComment
    {
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public int Id { get; set; }
        public ContentModel Content { get; set; }
        public InlineModel Inline { get; set; }
        public User User { get; set; }

        public class ContentModel
        {
            public string Raw { get; set; }
            public string Markup { get; set; }
            public string Html { get; set; }
        }

        public class InlineModel
        {
            public int? To { get; set; }
            public int? From { get; set; }
            public string Path { get; set; }
        }
    }
}

