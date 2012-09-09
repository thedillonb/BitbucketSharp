using System;

namespace BitbucketSharp.Models
{
    public class PrivilegeModel
    {
        public string Repo { get; set; }
        public string Privilege { get; set; }
        public UserModel User { get; set; }
    }
}

