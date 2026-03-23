using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;

namespace VKVideoReviews.BL.Exceptions.VkAuthExceptions;

public class PkceValidationException() 
    : VkAuthException("INVALID_PKCE", 
        "PKCE data not found or expired",
        500);