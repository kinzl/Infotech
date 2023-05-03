using System.Security.Cryptography;
using System.Text;

namespace Questionnaire_Frontend.Pages;

public class PasswordEncryption
{
    //ToDo: Do your methods here
    const int keySize = 64;
    const int iterations = 350000;
    HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    public PasswordEncryption()
    {
        
    }
    public string HashPasword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(keySize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize);
        return Convert.ToHexString(hash);
    }
    public bool VerifyPassword(string password, string hash, byte[] salt)
    {
        var hashToCompare = Convert.ToHexString(Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, hashAlgorithm, keySize));
        Console.WriteLine(hashToCompare);
        return hashToCompare == hash;
    }

}