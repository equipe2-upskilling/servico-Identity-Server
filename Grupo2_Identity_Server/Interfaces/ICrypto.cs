namespace Grupo2_Identity_Server.Interfaces
{
    public interface ICrypto
    {
        string GetSalt();
        string Encrypt(string value, string salt);
    }
}
