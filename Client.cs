using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BitBucketSharp.Controllers;
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

        public MeController GetUser()
        {
            return new MeController(this);
        }

        public UserController GetUser(string username)
        {
            return new UserController(this, username);
        }

        public RepositoryController GetRepository(string repoOwner, string repoSlug)
        {
            return new RepositoryController(this, repoOwner, repoSlug);
        }



        public FollowersModel GetIssueFollowers(String repoOwner, String repoSlug, int issueId)
        {
            return Get<FollowersModel>("repositories/" + repoOwner + "/" + repoSlug + "/issues/" + issueId + "/followers");
        }

        /// <summary>
        /// Makes a 'GET' request to the server using a URI
        /// </summary>
        /// <typeparam name="T">The type of object the response should be deserialized ot</typeparam>
        /// <param name="uri">The URI to request information from</param>
        /// <returns>An object with response data</returns>
        public T Get<T>(String uri)
        {
            var request = new RestRequest(uri);
            var response = _client.Execute(request);
            var d = new JsonDeserializer();
            return d.Deserialize<T>(response);
        }

        /// <summary>
        /// Makes a 'POST' request to the server using a URI with optional POST data and optional header modifications
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public T Post<T>(string uri, object data = null, object header = null)
        {
            var request = new RestRequest(uri, Method.POST);
            if (data != null)
                foreach (var prop in data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    request.AddParameter(prop.Name, prop.GetValue(data, null));
            if (header != null)
                foreach (var prop in header.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(prop => prop.GetValue(header, null) is string))
                    request.AddHeader(prop.Name, prop.GetValue(header, null) as string);

            var response = _client.Execute(request);
            var d = new JsonDeserializer();
            return d.Deserialize<T>(response);
        }
    }
}
