using NUnit.Framework;
using BitbucketSharp;
using System;

namespace BitBucketSharp.UnitTests
{
    [TestFixture]
    public class ClientTests
    {
        private static Client CreateClient()
        {
            return new Client("thedillonb", "djames");
        }

        [Test]
        [ExpectedException(typeof(BitbucketException))]
        public async void TestInvalidResource()
        {
            await CreateClient().Get<string>(new Uri(Client.ApiUrl2, "moo"));
        }

        [Test]
        public async void TestGetRepositories()
        {
            var repos = await CreateClient().Repositories.GetRepositories("thedillonb");
            Assert.True(repos.Size > 0);
            Assert.True(repos.Values.Count > 0);
        }

        [Test]
        public async void TestGetDownloads()
        {
            var ret = await CreateClient().Teams.GetTeams();
            Assert.True(ret.Size > 0);
            Assert.True(ret.Values.Count > 0);
        }
    }
}

