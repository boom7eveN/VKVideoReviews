namespace VKVideoReviews.BL.Exceptions.VkAuthExceptions;

public class StateValidationException() 
    : VkAuthException("INVALID_STATE",
        "State not found or expired",
        500);