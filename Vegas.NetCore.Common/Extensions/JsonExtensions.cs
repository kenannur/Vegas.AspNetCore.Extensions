using System.Text.Json;

namespace Vegas.NetCore.Common.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T obj) where T : class
        {
            try
            {
                return JsonSerializer.Serialize(obj, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch
            {
                return null;
            }
        }

        public static T ToObject<T>(this string json) where T : class
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                return null;
            }
        }
    }
}
