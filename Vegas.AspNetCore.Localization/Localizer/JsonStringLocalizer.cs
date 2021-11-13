using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Vegas.AspNetCore.Localization.Options;

namespace Vegas.AspNetCore.Localization.Localizer
{
    public interface IJsonStringLocalizer
    {
        string GetString(string key, string cultureName = default);
    }

    public class JsonStringLocalizer : IJsonStringLocalizer
    {
        private readonly Dictionary<string, Dictionary<string, string>> _localizerCache;

        public JsonStringLocalizer(IOptions<JsonStringLocalizerOptions> jsonStringLocalizerOptions, IOptions<RequestLocalizationOptions> requestLocalizationOptions)
        {
            if (jsonStringLocalizerOptions == null)
            {
                throw new ArgumentNullException(nameof(jsonStringLocalizerOptions));
            }
            if (requestLocalizationOptions == null)
            {
                throw new ArgumentNullException(nameof(requestLocalizationOptions));
            }

            _localizerCache = new Dictionary<string, Dictionary<string, string>>();
            foreach (var cultureInfo in requestLocalizationOptions.Value.SupportedCultures)
            {
                try
                {
                    var cultureName = cultureInfo.Name;
                    var resourceFileFullName = $"{jsonStringLocalizerOptions.Value.ResourceFileName}.{cultureName}.json";
                    var resourceFullPath = Path.Combine(jsonStringLocalizerOptions.Value.ResourceFilePath, resourceFileFullName);

                    var keyValuePairs = new ConfigurationBuilder()
                        .AddJsonFile(resourceFullPath, optional: true, reloadOnChange: true)
                        .Build()
                        .AsEnumerable();

                    _localizerCache.TryAdd(cultureName, new Dictionary<string, string>(keyValuePairs));
                }
                catch
                { }
            }
        }

        public JsonStringLocalizer(Dictionary<string, Dictionary<string, string>> cache)
        {
            _localizerCache = cache;
        }

        public string GetString(string key, string cultureName = default)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                return key;
            }

            if (key.Contains("::"))
            {
                var splittedArray = key.Split("::", StringSplitOptions.RemoveEmptyEntries);
                var messageKey = splittedArray[0];
                var parameters = splittedArray[1].Split(",", StringSplitOptions.RemoveEmptyEntries);

                return GetStringSafely(messageKey, cultureName, parameters);
            }
            else
            {
                return GetStringSafely(key, cultureName);
            }
        }

        private string GetStringSafely(string messageKey, string cultureName, params string[] parameters)
        {
            var currentCultureName = cultureName ?? CultureInfo.CurrentCulture.Name;
            if (_localizerCache.ContainsKey(currentCultureName) && _localizerCache[currentCultureName].ContainsKey(messageKey))
            {
                var messageValue = _localizerCache[currentCultureName][messageKey];
                return string.Format(messageValue, parameters);
            }
            return messageKey;
        }
    }
}
