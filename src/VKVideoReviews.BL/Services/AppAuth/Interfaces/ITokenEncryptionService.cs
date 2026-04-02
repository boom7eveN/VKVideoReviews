namespace VKVideoReviews.BL.Services.AppAuth.Interfaces;

public interface ITokenEncryptionService
{
    string Encrypt(string plainText);
}