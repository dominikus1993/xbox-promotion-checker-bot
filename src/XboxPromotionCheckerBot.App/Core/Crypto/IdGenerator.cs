using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IO;

namespace XboxPromotionCheckerBot.App.Core.Crypto;

public static class IdGenerator
{
    private static readonly RecyclableMemoryStreamManager Manager = new();
    
    public static async Task<string> GenerateId(params string[] keys)
    {
        if (keys is null or {Length: 0})
        {
            throw new ArgumentOutOfRangeException(nameof(keys));
        }

        var bytes = Encoding.Default.GetBytes(string.Join("-", keys));
        await using var stream = Manager.GetStream(bytes);
        using var sha = SHA256.Create();
        var hash = await sha.ComputeHashAsync(stream);
        return Convert.ToBase64String(hash);
    }
}