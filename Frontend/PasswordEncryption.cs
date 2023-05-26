using System.Security.Cryptography;
using System.Text;

namespace Questionnaire_Frontend.Pages;

public class PasswordEncryption
{
    const int keySize = 64;
    const int iterations = 350000;
    HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    public PasswordEncryption()
    {
        
    }
    public string HashPasword(string password, out string salt)
    {
        byte[] saltByte;
        saltByte = RandomNumberGenerator.GetBytes(keySize);
        salt = Convert.ToBase64String(saltByte);
        Console.WriteLine("Passwort Changed newSalt = " + salt);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            saltByte,
            iterations,
            hashAlgorithm,
            keySize);

        Console.WriteLine("Passwort Changed newSalt2.0 = " + salt);
        Console.WriteLine("Passwort Changed newHash = " + Convert.ToBase64String(hash));
        return Convert.ToBase64String(hash);
    }
    public bool VerifyPassword(string password1, string hash, string salt)
    {
        //Console.WriteLine("Hash from db: "+hash);
        //Console.WriteLine("Salt from db: "+salt);
        Console.WriteLine("Passwort: " + password1);
        /*UserName newPw = _db.UserNames.Where(x => x.Username == BodyUsername).Select(x => x).First();

                string newhash = pe.HashPasword(newPassword, out var newsalt);
                newPw.PasswordHash = newhash;
        newPw.PasswordSalt = Convert.ToHexString(newsalt);
                _db.SaveChanges();*/

        byte[] saltByte = Convert.FromBase64String(salt);
        Console.WriteLine("SaltByte = " + Convert.ToBase64String(saltByte));


        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password1),
            saltByte,
            iterations,
            hashAlgorithm,
            keySize);

        Console.WriteLine("HashToCompareFromEnteredPw = " + Convert.ToBase64String(hashToCompare));
        Console.WriteLine("SaltToCompareFromEnteredPw = " + salt);
        return Convert.ToBase64String(hashToCompare) == hash;
    }

}