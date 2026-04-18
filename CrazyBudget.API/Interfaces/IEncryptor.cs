namespace CrazyBudget.API.Interfaces;

public interface IEncryptor
{
    string GenerateHash(string password, byte[] salt);
    byte[] GenerateSalt();
    string Base64EncodedHash(string hash);

}
