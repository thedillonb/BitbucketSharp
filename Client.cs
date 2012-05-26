using System;
using System.Collections.Generic;
using BitBucketSharp.Models;
using RestSharp;
using RestSharp.Deserializers;

namespace BitBucketSharp
{
    public class Client
    {
        private readonly RestClient _client = new RestClient("https://api.bitbucket.org/1.0");

        /// <summary>
        /// Gets the username for this clietn
        /// </summary>
        public String Username { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Client(String username, String password)
        {
            Username = username;
            _client.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        public EmailsModel GetMyEmails()
        {
            return Get<EmailsModel>("emails");
        }

        public List<RepositoryDetailedModel> GetRepositoriesIFollow()
        {
            return Get<List<RepositoryDetailedModel>>("user/follows");
        }

        public UsersModel GetUser(String username)
        {
            return Get<UsersModel>("users/" + Username);
        }

        public FollowersModel GetUserFollowers(String username)
        {
            return Get<FollowersModel>("users/" + username + "/followers");
        }

        public FollowersModel GetRepositoryFollowers(String repoOwner, String repoSlug)
        {
            return Get<FollowersModel>("repositories/" + repoOwner + "/" + repoSlug + "/followers");
        }

        public FollowersModel GetIssueFollowers(String repoOwner, String repoSlug, int issueId)
        {
            return Get<FollowersModel>("repositories/" + repoOwner + "/" + repoSlug + "/issues/" + issueId + "/followers");
        }

        /// <summary>
        /// Makes a 'GET' request to the server using an URI
        /// </summary>
        /// <typeparam name="T">The type of object the response should be deserialized ot</typeparam>
        /// <param name="uri">The URI to request information from</param>
        /// <returns>An object with response data</returns>
        private T Get<T>(String uri)
        {
            var request = new RestRequest(uri);
            var response = _client.Execute(request);
            var d = new JsonDeserializer();
            return d.Deserialize<T>(response);
        }
    }
}
