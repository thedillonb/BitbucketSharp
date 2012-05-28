== BitBucketSharp ==

A C# implementation of BitBucket's REST request framework.

== Example Usage ==

{{{
//Instantiate a client with your username and password
var client = BitBucketSharp.Client("username", "password");

//Request information about a user
var userInfo = client.Users["thedillonb"].GetInfo();

//Request information about a specific repository
var repoInfo = client.Users["thedillonb"].Repositories["bitbucketsharp"].GetInfo();
}}}
