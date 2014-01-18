using System;
using System.Collections.Generic;
using System.Net;
using BitbucketSharp.Controllers;
using RestSharp;
using RestSharp.Deserializers;
using BitbucketSharp.Models;
using RestSharp.Contrib;

namespace BitbucketSharp
{
    public class Client
    {
        public const string ApiUrl = "https://bitbucket.org/api/1.0/";
        public const string ApiUrl2 = "https://bitbucket.org/api/2.0/";
        public static string Url = "https://bitbucket.org";

        private readonly RestClient _client;

        /// <summary>
        /// The username we are logging as as.
        /// Instead of passing in the account's username everywhere we'll just set it once here.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user account
        /// </summary>
        public AccountController Account
        {
            get { return new AccountController(this); }
        }

        /// <summary>
        /// The users on Bitbucket
        /// </summary>
        public UsersController Users
        {
            get { return new UsersController(this); }
        }

        /// <summary>
        /// The repositories on Bitbucket
        /// </summary>
        public RepositoriesController Repositories
        {
            get { return new RepositoriesController(this); }
        }

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
        private Client()
        {
            Retries = 3;
            _client = new RestClient() { FollowRedirects = true };
        }

        /// <summary>
        /// Constructs the client using oauth identifiers
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="oauth_token"></param>
        /// <param name="oauth_token_secret"></param>
        public Client(string consumerKey, string consumerSecret, string oauth_token, string oauth_token_secret) 
            : this()
        {
            _client.Authenticator = RestSharp.Authenticators.OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, oauth_token, oauth_token_secret);
        }

        /// <summary>
        /// Constructs the client using username / password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Client(string username, string password)
            : this()
        {
            _client.Authenticator = new HttpBasicAuthenticator(username, password);
            Username = username;
        }

        /// <summary>
        /// Create a Client object and request the user's info to validate credentials
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns></returns>
        public static Client BasicLogin(string username, string password, out UsersModel userInfo)
        {
            var client = new Client(username, password);
            userInfo = client.Account.GetInfo();
            client.Username = userInfo.User.Username;
            return client;
        }

        /// <summary>
        /// Create a Client object with OAuth crednetials and a request the user's info to validate credentials
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="oauth_token"></param>
        /// <param name="oauth_token_secret"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static Client OAuthLogin(string consumerKey, string consumerSecret, string oauth_token, string oauth_token_secret, out UsersModel userInfo)
        {
            var client = new Client(consumerKey, consumerSecret, oauth_token, oauth_token_secret);
            userInfo = client.Account.GetInfo();
            client.Username = userInfo.User.Username;
            return client;
        }

        /// <summary>
        /// Run through the OAuth login process to obtain an auth token and secret. Then login using that information and request the user's
        /// info to validate credentials
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="redirectAction"></param>
        /// <param name="oauth_token"></param>
        /// <param name="oauth_token_secret"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static Client OAuthLogin(string consumerKey, string consumerSecret, Func<Uri, string> redirectAction, out string oauth_token, out string oauth_token_secret, out UsersModel userInfo)
        {
            var client = new RestClient(ApiUrl);
            client.Authenticator = RestSharp.Authenticators.OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);
            var request = new RestRequest("oauth/request_token", Method.POST);
            var response = client.Execute(request);

            var qs = HttpUtility.ParseQueryString(response.Content);
            oauth_token = qs["oauth_token"];
            oauth_token_secret = qs["oauth_token_secret"];

            request = new RestRequest("oauth/authenticate");
            request.AddParameter("oauth_token", oauth_token);
            var uri = new Uri(client.BuildUri(request).ToString());
            var verifier = redirectAction(uri);

            request = new RestRequest("oauth/access_token", Method.POST);
            client.Authenticator = RestSharp.Authenticators.OAuth1Authenticator.ForAccessToken(
                consumerKey, consumerSecret, oauth_token, oauth_token_secret, verifier
            );
            response = client.Execute(request);

            qs = HttpUtility.ParseQueryString(response.Content);
            oauth_token = qs["oauth_token"];
            oauth_token_secret = qs["oauth_token_secret"];

            return OAuthLogin(consumerKey, consumerSecret, oauth_token, oauth_token_secret, out userInfo);
        }

        /// <summary>
        /// Invalidates a cache object starting with a specific URI
        /// </summary>
        /// <param name="startsWithUri">The starting URI to be invalidated</param>
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
			var response = ExecuteRequest(new Uri(new Uri(baseUri), uri), method, data);
            var d = new JsonDeserializer();
            return d.Deserialize<T>(response);
        }

		/// <summary>
		/// Dummy thing.. for now
		/// </summary>
		/// <param name="uri">URI.</param>
		/// <param name="method">Method.</param>
		/// <param name="data">Data.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Request2<T>(string uri, Method method = Method.GET, Dictionary<string, string> data = null)
		{
			var response = ExecuteRequest(new Uri(uri), method, data);
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
			ExecuteRequest(new Uri(new Uri(baseUri), uri), method, data);
        }

        /// <summary>
        /// Executes a request to the server
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
		internal IRestResponse ExecuteRequest(Uri uri, Method method, Dictionary<string, string> data)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            var request = new RestRequest(method);
			request.Resource = uri.AbsoluteUri;
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

		/// <summary>
		/// Executes a request to the server
		/// </summary>
		internal IRestResponse ExecuteRequest(IRestRequest request)
		{
			var response = _client.Execute(request);
			if (response.ErrorException != null)
				throw response.ErrorException;
			return response;
		}

		public string DownloadRawResource(string rawUrl, System.IO.Stream downloadSream)
		{
			var request = new RestRequest(rawUrl, Method.GET);
			request.ResponseWriter = (s) => s.CopyTo(downloadSream);
			var response = ExecuteRequest(request);
			return response.ContentType;
		}
    }
}
