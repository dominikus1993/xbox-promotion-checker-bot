using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IO;

namespace XboxPromotionCheckerBot.App.Core.Crypto;

public static class IdGenerator
{
    private static readonly RecyclableMemoryStreamManager Manager = new();
    
    public static Guid GenerateId(params string[] keys)
    {
        if (keys is null or {Length: 0})
        {
            throw new ArgumentOutOfRangeException(nameof(keys));
        }

        var bytes = Encoding.Default.GetBytes(string.Join("-", keys));
        using var stream = Manager.GetStream(bytes);
        using var sha = MD5.Create();
        var hash = sha.ComputeHash(stream);
        stream.Flush();
        return new Guid(hash);
    }
}