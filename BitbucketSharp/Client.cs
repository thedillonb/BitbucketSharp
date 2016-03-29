using System;
using BitbucketSharp.Controllers;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace BitbucketSharp
{
    public class Client
    {
        private readonly AuthenticationHeaderValue _authorizationHeader;
        public static Uri ApiUrl = new Uri("https://bitbucket.org/api/1.0/");
        public static Uri ApiUrl2 = new Uri("https://api.bitbucket.org/2.0/");
        public static Uri Url = new Uri("https://bitbucket.org");

        private static readonly Lazy<JsonSerializer> _serializer = new Lazy<JsonSerializer>(() => 
            {
                var serializer = new JsonSerializer();
                serializer.ContractResolver = new ContractResolver();
                return serializer;
            });


        public static Func<HttpClient> Factory = new Func<HttpClient>(() => new HttpClient());

        public UsersController Users
        {
            get { return new UsersController(this); }
        }

        public RepositoriesController Repositories
        {
            get { return new RepositoriesController(this); }
        }

        public IssuesController Issues
        {
            get { return new IssuesController(this); }
        }

        public TeamsController Teams
        {
            get { return new TeamsController(this); }
        }

        public GroupsController Groups
        {
            get { return new GroupsController(this); }
        }

        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

        private Client(AuthenticationHeaderValue authorizationHeader)
        {
            _authorizationHeader = authorizationHeader;
        }

        public static Client WithBasicAuthentication(string username, string password)
        {
            var byteArray = Encoding.UTF8.GetBytes(username + ":" + password);
            return new Client(new AuthenticationHeaderValue("basic", Convert.ToBase64String(byteArray)));
        }

//        /// <summary>
//        /// Constructs the client using oauth identifiers
//        /// </summary>
//        /// <param name="consumerKey"></param>
//        /// <param name="consumerSecret"></param>
//        /// <param name="oauth_token"></param>
//        /// <param name="oauth_token_secret"></param>
//        public Client(string consumerKey, string consumerSecret, string oauth_token, string oauth_token_secret) 
//            : this()
//        {
//            _client.Authenticator = RestSharp.Authenticators.OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, oauth_token, oauth_token_secret);
//        }
//
        /// <summary>
        /// Create a Client object and request the user's info to validate credentials
        /// </summary>
        public static Client WithBearerAuthentication(string token)
        {
            return new Client(new AuthenticationHeaderValue("Bearer", token));
        }
//
//        /// <summary>
//        /// Create a Client object with OAuth crednetials and a request the user's info to validate credentials
//        /// </summary>
//        /// <param name="consumerKey"></param>
//        /// <param name="consumerSecret"></param>
//        /// <param name="oauth_token"></param>
//        /// <param name="oauth_token_secret"></param>
//        /// <param name="userInfo"></param>
//        /// <returns></returns>
//        public static Client OAuthLogin(string consumerKey, string consumerSecret, string oauth_token, string oauth_token_secret, out UsersModel userInfo)
//        {
//            var client = new Client(consumerKey, consumerSecret, oauth_token, oauth_token_secret);
//            userInfo = client.Account.GetInfo();
//            client.Username = userInfo.User.Username;
//            return client;
//        }
//
//        /// <summary>
//        /// Run through the OAuth login process to obtain an auth token and secret. Then login using that information and request the user's
//        /// info to validate credentials
//        /// </summary>
//        /// <param name="consumerKey"></param>
//        /// <param name="consumerSecret"></param>
//        /// <param name="redirectAction"></param>
//        /// <param name="oauth_token"></param>
//        /// <param name="oauth_token_secret"></param>
//        /// <param name="userInfo"></param>
//        /// <returns></returns>
//        public static Client OAuthLogin(string consumerKey, string consumerSecret, Func<Uri, string> redirectAction, out string oauth_token, out string oauth_token_secret, out UsersModel userInfo)
//        {
//            var client = new RestClient(ApiUrl);
//            client.Authenticator = RestSharp.Authenticators.OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);
//            var request = new RestRequest("oauth/request_token", Method.POST);
//            var response = client.Execute(request);
//
//            var qs = HttpUtility.ParseQueryString(response.Content);
//            oauth_token = qs["oauth_token"];
//            oauth_token_secret = qs["oauth_token_secret"];
//
//            request = new RestRequest("oauth/authenticate");
//            request.AddParameter("oauth_token", oauth_token);
//            var uri = new Uri(client.BuildUri(request).ToString());
//            var verifier = redirectAction(uri);
//
//            request = new RestRequest("oauth/access_token", Method.POST);
//            client.Authenticator = RestSharp.Authenticators.OAuth1Authenticator.ForAccessToken(
//                consumerKey, consumerSecret, oauth_token, oauth_token_secret, verifier
//            );
//            response = client.Execute(request);
//
//            qs = HttpUtility.ParseQueryString(response.Content);
//            oauth_token = qs["oauth_token"];
//            oauth_token_secret = qs["oauth_token_secret"];
//
//            return OAuthLogin(consumerKey, consumerSecret, oauth_token, oauth_token_secret, out userInfo);
//        }
//
        public static async Task<OAuthResponse> GetRefreshToken(string clientId, string clientSecret, string refreshToken)
        {
            var client = Client.WithBasicAuthentication(clientId, clientSecret);
            var uri = new Uri("https://bitbucket.org/site/oauth2/access_token");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            var data = new Dictionary<string, string>();
            data.Add("grant_type", "refresh_token");
            data.Add("refresh_token", refreshToken);
            request.Content = new FormUrlEncodedContent(data);
            request.Headers.Add("Accept", "application/json");
            var resp = await client.ExecuteRequest(request).ConfigureAwait(false);
            return await ParseBody<OAuthResponse>(resp).ConfigureAwait(false);
        }

        public static async Task<OAuthResponse> GetAuthorizationCode(string clientId, string clientSecret, string code)
        {
            var client = Client.WithBasicAuthentication(clientId, clientSecret);
            var uri = new Uri("https://bitbucket.org/site/oauth2/access_token");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            var data = new Dictionary<string, string>();
            data.Add("grant_type", "authorization_code");
            data.Add("code", code);
            request.Content = new FormUrlEncodedContent(data);
            request.Headers.Add("Accept", "application/json");
            var resp = await client.ExecuteRequest(request).ConfigureAwait(false);
            return await ParseBody<OAuthResponse>(resp).ConfigureAwait(false);
        }

        private static async Task<T> ParseBody<T>(HttpResponseMessage message)
        {
            var body = await message.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using (var reader = new StreamReader(body))
            using (var textReader = new JsonTextReader(reader))
                return _serializer.Value.Deserialize<T>(textReader);
        }

        public async Task<T> Get<T>(Uri uri) where T : class
		{
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Accept", "application/json");
            var resp = await ExecuteRequest(request).ConfigureAwait(false);
            return await ParseBody<T>(resp).ConfigureAwait(false);
		}

        public async Task<Stream> GetRaw(Uri uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            var resp = await ExecuteRequest(request).ConfigureAwait(false);
            return await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        public async Task<T> Put<T>(Uri uri, object data)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            var json = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Add("Accept", "application/json");
            if (json.Length == 0) request.Headers.Add("Content-Length", "0");
            var resp = await ExecuteRequest(request).ConfigureAwait(false);
            return await ParseBody<T>(resp).ConfigureAwait(false);
        }

        public Task Put(Uri uri, object data)
        {
            return Put<string>(uri, data);
        }

        public async Task<T> Post<T>(Uri uri, object data = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            var json = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Add("Accept", "application/json");
            var resp = await ExecuteRequest(request).ConfigureAwait(false);
            return await ParseBody<T>(resp).ConfigureAwait(false);
        }

        public Task Post(Uri uri, object data)
        {
            return Post<string>(uri, data);
        }

        public Task Delete(Uri uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            return ExecuteRequest(request);
        }

		/// <summary>
		/// Executes a request to the server
		/// </summary>
        private async Task<HttpResponseMessage> ExecuteRequest(HttpRequestMessage request)
		{
            var client = Factory();
            client.DefaultRequestHeaders.Authorization = _authorizationHeader;
            client.Timeout = Timeout;
            var resp = await client.SendAsync(request).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                var error = JsonConvert.DeserializeObject<ErrorResponse>(body);
                if (string.IsNullOrEmpty(error?.Error?.Message))
                    throw new BitbucketException("Error", "Server returned an invalid status code: " + resp.StatusCode);
                throw new BitbucketException(error.Error.Message, error.Error.Detail);
            }

            return resp;
		}
    }

    public class OAuthResponse
    {
        public string AccessToken { get; set; }
        public string Scopes { get; set; }
        public string RefreshToken { get; set; }
    }

    public class ErrorResponse
    {
        public ErrorDetails Error { get; set ;}
    }

    public class ErrorDetails
    {
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}
