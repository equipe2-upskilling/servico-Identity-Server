namespace Grupo2_Identity_Server.Interfaces
{
    public interface IToken
    {
        string Decrypt(string encryptValue, string secretKey);
        string Encrypt(string value, string secretKey, TimeSpan tokenLifetime);
    }
}
