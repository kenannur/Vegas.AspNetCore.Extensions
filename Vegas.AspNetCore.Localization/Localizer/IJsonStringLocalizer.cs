namespace Vegas.AspNetCore.Localization.Localizer
{
    public interface IJsonStringLocalizer
    {
        string GetString(string key, string cultureName = default);
    }
}
