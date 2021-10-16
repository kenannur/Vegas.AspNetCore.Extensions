using Microsoft.Extensions.Configuration;

namespace Vegas.AspNetCore.Configuration.Extensions
{
    public static class ConfigurationBuilderExtensions
	{
		public static IConfigurationBuilder AddJsonString(this IConfigurationBuilder configurationBuilder, string json)
		{
			return configurationBuilder.Add(new JsonStringConfigurationSource(json));
		}
	}

	public class JsonStringConfigurationSource : IConfigurationSource
	{
		private readonly string _json;
		public JsonStringConfigurationSource(string json) => _json = json;

		public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
			return new JsonStringConfigurationProvider(_json);
		}
	}

	public class JsonStringConfigurationProvider : ConfigurationProvider
	{
		private readonly string _json;
		public JsonStringConfigurationProvider(string json) => _json = json;

		public override void Load()
        {
			//Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(_json);
		}
	}
}
