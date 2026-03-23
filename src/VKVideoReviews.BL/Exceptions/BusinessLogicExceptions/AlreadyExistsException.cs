namespace VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;

public class AlreadyExistsException(string entityName)
    : BusinessLogicException(
        $"{entityName} is already exists",
        "ENTITY_ALREADY_EXISTS",
        409);