namespace crm_api.Interfaces
{
    public interface ILocalizationService
    {
        string GetLocalizedString(string key);
        string GetLocalizedString(string key, params object[] arguments);
    }
}
