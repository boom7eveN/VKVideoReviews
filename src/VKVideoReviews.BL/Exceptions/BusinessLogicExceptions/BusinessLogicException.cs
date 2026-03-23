namespace VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;

public class BusinessLogicException(
    string message,
    string errorCode,
    int statusCode = 400)
    : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
    public int StatusCode { get; } = statusCode;
}