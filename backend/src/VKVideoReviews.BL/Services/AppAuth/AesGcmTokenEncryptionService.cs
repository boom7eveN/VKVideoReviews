using System.Security.Cryptography;
using System.Text;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;

namespace VKVideoReviews.BL.Services.AppAuth;

public class AesGcmTokenEncryptionService(string base64EncryptionKey) : ITokenEncryptionService
{
    private const int NonceSize = 12;
    private const int TagSize = 16;
    private readonly byte[] _encryptionKey = Convert.FromBase64String(base64EncryptionKey);

    public string Encrypt(string plainText)
    {
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var nonce = new byte[NonceSize];
        var ciphertext = new byte[plainBytes.Length];
        var tag = new byte[TagSize];

        RandomNumberGenerator.Fill(nonce);

        using var aesGcm = new AesGcm(_encryptionKey, TagSize);
        aesGcm.Encrypt(nonce, plainBytes, ciphertext, tag);

        var result = new byte[NonceSize + TagSize + ciphertext.Length];
        Buffer.BlockCopy(nonce, 0, result, 0, NonceSize);
        Buffer.BlockCopy(tag, 0, result, NonceSize, TagSize);
        Buffer.BlockCopy(ciphertext, 0, result, NonceSize + TagSize, ciphertext.Length);

        return Convert.ToBase64String(result);
    }
}