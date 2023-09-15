using System.Security.Cryptography;

namespace CloudSync.Adapters;

public static class Md5Helper
{
    public static string CalculateMD5(string filename)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(filename))
            {
                var hash = md5.ComputeHash(stream);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
