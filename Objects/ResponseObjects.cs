using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBucketSharp.Objects
{
    public class Users
    {
        public User User { get; set; }
        public List<RepositoryDetailed> Repositories { get; set; } 
    }

    public class User
    {
        public String Username { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public bool IsTeam { get; set; }
        public String Avatar { get; set; }
        public String ResourceUrl { get; set; }
    }

    public class RepositoryDetailed : RepositorySimple
    {
        public String Description { get; set; }
        public int FollowsCount { get; set; }
        public String ResourceUri { get; set; }
        public String Website { get; set; }
        public bool HasWiki { get; set; }
        public String LastUpdated { get; set; }
        public String CreatedOn { get; set; }
        public String Logo { get; set; }
        public int Size { get; set; }
        public bool ReadOnly { get; set; }
        public String Language { get; set; }
        public int FollowerCount { get; set; }
        public String State { get; set; }
        public bool HasIssues { get; set; }
        public bool IsFork { get; set; }
        public bool EmailWriters { get; set; }
        public bool NoPublicForks { get; set; }
    }

    public class RepositorySimple
    {
        public bool IsPrivate { get; set; }
        public String Slug { get; set; }
        public String Name { get; set; }
        public String Owner { get; set; }
        public String Scm { get; set; }
    }

    public class Emails : List<EmailObject> { }

    public class EmailObject
    {
        public bool Active { get; set; }
        public String Email { get; set; }
        public bool Primary { get; set; }
    }

    public class Source
    {
        public List<String> Directories { get; set; }
        public List<File> Files { get; set; }
    }

    public class File
    {
        public String Path { get; set; }
        public String Revision { get; set; }
        public int Size { get; set; }
        public String Timestamp { get; set; }
    }

    public class Follower : User { }

    public class FollowersObject
    {
        public int Count { get; set; }
        public List<Follower> Followers { get; set; } 
    }
}
