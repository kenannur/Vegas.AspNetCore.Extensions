using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Vegas.AspNetCore.Localization.Options;

namespace Vegas.AspNetCore.Localization.Localizer
{
    public class JsonStringLocalizer : IJsonStringLocalizer
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _localizerCache =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

        public JsonStringLocalizer(IOptions<JsonStringLocalizerOptions> jsonStringLocalizerOptions,
                                   IOptions<RequestLocalizationOptions> requestLocalizationOptions)
        {
            if (jsonStringLocalizerOptions == null)
            {
                throw new ArgumentNullException(nameof(jsonStringLocalizerOptions));
            }
            if (requestLocalizationOptions == null)
            {
                throw new ArgumentNullException(nameof(requestLocalizationOptions));
            }

            SetupLocalizerCache(requestLocalizationOptions.Value, jsonStringLocalizerOptions.Value);
        }

        private void SetupLocalizerCache(RequestLocalizationOptions requestLocalizationOptions,
                                         JsonStringLocalizerOptions jsonStringLocalizerOptions)
        {
            foreach (var cultureInfo in requestLocalizationOptions.SupportedCultures)
            {
                var cultureName = cultureInfo.Name;
                var resourceFileFullName = $"{jsonStringLocalizerOptions.ResourceFileName}.{cultureName}.json";
                var resourceFullPath = Path.Combine(jsonStringLocalizerOptions.ResourceFilePath, resourceFileFullName);

                var keyValuePairs = new ConfigurationBuilder()
                    .AddJsonFile(resourceFullPath, optional: true, reloadOnChange: true)
                    .Build()
                    .AsEnumerable();
                var dictionary = new ConcurrentDictionary<string, string>(keyValuePairs);

                _localizerCache.TryAdd(cultureName, dictionary);
            }
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
