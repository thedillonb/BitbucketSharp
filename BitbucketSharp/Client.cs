using System;
using System.Collections.Generic;
using System.Net;
using BitbucketSharp.Controllers;
using BitbucketSharp.Utils;
using RestSharp;
using RestSharp.Deserializers;

namespace BitbucketSharp
{
    public class Client
    {
        public const string ApiUrl = "https://bitbucket.org/api/1.0/";
        public const string ApiUrl2 = "https://bitbucket.org/api/2.0/";
        public static string Url = "https://bitbucket.org";

        private readonly RestClient _client = new RestClient();

        /// <summary>
        /// Gets the username for this clietn
        /// </summary>
        public String Username { get; private set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public String Password { get; private set; }

        /// <summary>
        /// The user account
        /// </summary>
        public AccountController Account { get; private set; }

        /// <summary>
        /// The users on Bitbucket
        /// </summary>
        public UsersController Users { get; private set; }

        /// <summary>
        /// The repositories on Bitbucket
        /// </summary>
        public RepositoriesController Repositories { get; private set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        public int Timeout 
        {
            get { return _client.Timeout; }
            set { _client.Timeout = value; }
        }

        /// <summary>
        /// Gets or sets the retries for No Content errors
        /// </summary>
        public uint Retries { get; set; }

        /// <summary>
        /// Gets or sets the cache provider.
        /// </summary>
        public ICacheProvider CacheProvider { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Client(String username, String password)
        {
            Retries = 3;
            Username = username;
            Password = password;
            Account = new AccountController(this);
            Users = new UsersController(this);
            Repositories = new RepositoriesController(this);
            _client.Authenticator = new HttpBasicAuthenticator(username, password);
            _client.FollowRedirects = true;
        }

        public void InvalidateCacheObjects(string startsWithUri)
        {
            if (CacheProvider != null)
                CacheProvider.DeleteWhereStartingWith(startsWithUri);
        }

        /// <summary>
        /// Makes a 'GET' request to the server using a URI
        /// </summary>
        /// <typeparam name="T">The type of object the response should be deserialized ot</typeparam>
        /// <param name="uri">The URI to request information from</param>
        /// <param name="forceCacheInvalidation"></param>
        /// <returns>An object with response data</returns>
        public T Get<T>(String uri, bool forceCacheInvalidation = false, string baseUri = ApiUrl) where T : class
        {
            T obj = null;

            //If there's a cache provider, check it.
            if (CacheProvider != null && !forceCacheInvalidation)
                obj = CacheProvider.Get<T>(uri);

            if (obj == null)
            {
                obj = Request<T>(uri, baseUri: baseUri);

                //If there's a cache provider, save it!
                if (CacheProvider != null)
                    CacheProvider.Set(obj, uri);
            }

            return obj;
        }

        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Put<T>(string uri, Dictionary<string, string> data = null, string baseUri = ApiUrl)
        {
            return Request<T>(uri, Method.PUT, data, baseUri);
        }

        /// <summary>
        /// Makes a 'PUT' request to the server
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        public void Put(string uri, Dictionary<string, string> data = null, string baseUri = ApiUrl)
        {
            Request(uri, Method.PUT, data, baseUri);
        }

        /// <summary>
        /// Makes a 'POST' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Post<T>(string uri, Dictionary<string, string> data, string baseUri = ApiUrl)
        {
            return Request<T>(uri, Method.POST, data, baseUri);
        }

        /// <summary>
        /// Makes a 'POST' request to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Post<T>(string uri, T data, string baseUri = ApiUrl)
        {
            return Post<T>(uri, ObjectToDictionaryConverter.Convert(data), baseUri);
        }

        /// <summary>
        /// Post the specified uri and data.
        /// </summary>
        public T Post<T, TD>(string uri, TD data, string baseUri = ApiUrl)
        {
            return Post<T>(uri, ObjectToDictionaryConverter.Convert(data), baseUri);
        }

        /// <summary>
        /// Makes a 'POST' request to the server without a response
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public void Post(string uri, Dictionary<string, string> data, string baseUri = ApiUrl)
        {
            Request(uri, Method.POST, data, baseUri);
        }

        /// <summary>
        /// Makes a 'DELETE' request to the server
        /// </summary>
        /// <param name="uri"></param>
        public void Delete(string uri, string baseUri = ApiUrl)
        {
            Request(uri, Method.DELETE, baseUri: baseUri);
        }

        /// <summary>
        /// Makes a request to the server expecting a response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public T Request<T>(string uri, Method method = Method.GET, Dictionary<string, string> data = null, string baseUri = ApiUrl)
        {
            var response = ExecuteRequest(uri, method, data, baseUri);
            var d = new JsonDeserializer();
            return d.Deserialize<T>(response);
        }

        /// <summary>
        /// Makes a request to the server but does not expect a response.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        public void Request(string uri, Method method = Method.GET, Dictionary<string, string> data = null, string baseUri = ApiUrl)
        {
            ExecuteRequest(uri, method, data, baseUri);
        }

        /// <summary>
        /// Executes a request to the server
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal IRestResponse ExecuteRequest(string uri, Method method, Dictionary<string, string> data, string baseUri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            var requestUri = new Uri(new Uri(baseUri), uri);
            var request = new RestRequest(method);
            request.Resource = requestUri.AbsoluteUri;
            if (data != null)
                foreach (var hd in data)
                    request.AddParameter(hd.Key, hd.Value);
            
            //Puts without any data must be marked as having no content!
            if (method == Method.PUT && data == null)
                request.AddHeader("Content-Length", "0");

            for (var i = 0; i < Retries + 1; i++)
            {
                IRestResponse response = _client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    //A special case for deletes
                    if (request.Method == Method.DELETE && response.StatusCode == HttpStatusCode.NoContent)
                    {
                        //Do nothing. This is a special case...
                    }
                    else if (response.StatusCode == 0)
                    {
                        continue;
                    }
                    else
                    {
                        throw StatusCodeException.FactoryCreate(response.StatusCode);
                    }
                }

                //Return the response
                return response;
            }

            throw new InvalidOperationException("Unable to execute request. No connection available!");
        }
    }


}
