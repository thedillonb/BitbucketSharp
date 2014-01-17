using System;

namespace BitbucketSharp
{
	public class SimpleJsonSerializer : IJsonSerializer
    {
		private readonly RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();
		private readonly RestSharp.Serializers.JsonSerializer _serializer = new RestSharp.Serializers.JsonSerializer();

		public string Serialize(object item)
		{
			return _serializer.Serialize(item);
		}

		public T Deserialize<T>(string json)
		{
			return _deserializer.Deserialize<T>(new RestSharp.RestResponse() { Content = json });
		}
    }
}

