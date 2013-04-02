using System.Collections.Generic;

namespace BitbucketSharp.Models
{
    public class UsersModel
    {
        public UserModel User { get; set; }
        public List<RepositoryDetailedModel> Repositories { get; set; }
    }

    public class UserModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsTeam { get; set; }
        public string Avatar { get; set; }
        public string ResourceUrl { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is UserModel)
                return this.Username.Equals(((UserModel)obj).Username);
            return false;
        }

        public override int GetHashCode()
        {
            return this.Username.GetHashCode();
        }
    }
}
