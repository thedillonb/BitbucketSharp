using System;
using System.Collections.Generic;
using BitbucketSharp.Models;
using RestSharp;
using RestSharp.Deserializers;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// A controller dedicated to the team for the logged in user
    /// </summary>
    public class TeamController : Controller
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public TeamController(Client client)
            : base(client)
        {
        }

        /// <summary>
        /// EXPERIMENTAL
        /// Requests the teams that the current user is a part of. 
        /// This API is not part of the REST interface and will most likely not be supported in the future.
        /// </summary>
        /// <returns>A list of teams</returns>
        public List<TeamModel> GetTeams()
        {
            var client = new RestClient(Client.Url);
            client.Authenticator = Client.RestClient.Authenticator;
            client.Timeout = Client.Timeout;
            var response = client.Execute(new RestRequest(Uri, Method.GET));
            return (new JsonDeserializer()).Deserialize<TeamWrapperModel>(response).Teams;
        }


        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri {
            get { return "account/xhr/header_information/"; }
        }
    }
}

