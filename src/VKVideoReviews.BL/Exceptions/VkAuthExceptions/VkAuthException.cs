namespace VKVideoReviews.BL.Exceptions.VkAuthExceptions;

public class VkAuthException(
    string errorCode,
    string message,
    int statusCode = 500) 
    : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
    public int StatusCode { get; } = statusCode;
}