namespace crm_api.Shared.Infrastructure.Abstractions
{
    public interface ILocalizationService
    {
        string GetLocalizedString(string key);
        string GetLocalizedString(string key, params object[] arguments);
    }
}
