using System;
using System.Collections.Generic;

namespace BitbucketSharp.Models
{
    public class TeamModel
    {
        public string Username { get; set; }
        public string Privilege { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
    }

    public class TeamWrapperModel
    {
        public List<TeamModel> Teams { get; set; }
    }
}

