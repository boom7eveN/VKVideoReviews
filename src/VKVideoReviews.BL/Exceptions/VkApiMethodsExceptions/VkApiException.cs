namespace VKVideoReviews.BL.Exceptions.VkApiMethodsExceptions;

public class VkApiException(
    string message,
    string errorCode,
    int statusCode = 500)
    : Exception(message)
{
}