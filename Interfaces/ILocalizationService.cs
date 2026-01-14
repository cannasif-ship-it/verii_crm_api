namespace cms_webapi.Interfaces
{
    public interface ILocalizationService
    {
        string GetLocalizedString(string key);
        string GetLocalizedString(string key, params object[] arguments);
    }
}
