using System;
using System.Collections.Generic;
using System.Net;
using BitBucketSharp.Controllers;
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
        /// The user account
        /// </summary>
        public AccountController Account { get; private set; }

        /// <summary>
        /// The users on BitBucket
        /// </summary>
        public UsersController Users { get; private set; }

        /// <summary>
        /// The repositories on BitBucket
        /// </summary>
        public RepositoriesController Repositories { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Client(String username, String password)
        {
            Username = username;
            Account = new AccountController(this);
            Users = new UsersController(this);
            Repositories = new RepositoriesController(this);
            _client.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        /// <summary>
        /// Makes a 'GET' request to the server using a URI
        /// </summary>
        /// <typeparam name="T">The type of object the response should be deserialized ot</typeparam>
        /// <param name="uri">The URI to request information from</param>
        /// <returns>An object with response data</returns>
        public T Get<T>(String uri)
        {
            return Request<T>(uri);
        }

        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public T Put<T>(string uri)
        {
            return Request<T>(uri, Method.PUT, null, new Dictionary<string, string> {{"Content-Length", "0"}});
        }

        /// <summary>
        /// Makes a 'POST' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Post<T>(string uri, Dictionary<string, string> data)
        {
            return Request<T>(uri, Method.POST, data);
        }

        /// <summary>
        /// Makes a 'DELETE' request to the server
        /// </summary>
        /// <param name="uri"></param>
        public void Delete(string uri)
        {
            Request<string>(uri, Method.DELETE);
        }

        /// <summary>
        /// Makes a request to the server using a URI with optional POST data and optional header modifications
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="header"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public T Request<T>(string uri, Method method = Method.GET, Dictionary<string, string> data = null , Dictionary<string, string> header = null)
        {
            var response = ExecuteRequest(uri, method, data, header);
            var d = new JsonDeserializer();
            return d.Deserialize<T>(response);
        }

        private IRestResponse ExecuteRequest(string uri, Method method, Dictionary<string, string> data, Dictionary<string, string> header)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            var request = new RestRequest(uri, method);
            if (data != null)
                foreach (var hd in data)
                    request.AddParameter(hd.Key, hd.Value);
            if (header != null)
                foreach (var hd in header)
                    request.AddHeader(hd.Key, hd.Value);

            var response = _client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException("Request returned status code: " + response.StatusCode);

            return response;
        }
    }
}
