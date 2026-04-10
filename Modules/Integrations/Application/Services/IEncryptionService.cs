namespace crm_api.Modules.Integrations.Application.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string plain);
        string Decrypt(string cipher);
    }
}
