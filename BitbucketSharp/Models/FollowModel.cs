using System.Collections.Generic;

namespace BitbucketSharp.Models
{
    public class FollowersModel
    {
        public int Count { get; set; }
        public List<UserModel> Followers { get; set; }
    }
}
