using VKVideoReviews.BL.Exceptions.Common;

namespace VKVideoReviews.BL.Exceptions;

public class NotFoundException : BusinessLogicException
{
    public NotFoundException(string entityName, Guid id) : base($"{entityName} was not found. Id: {id}",
        "ENTITY_NOT_FOUND",
        404)
    {
    }

    public NotFoundException(string entityName) : base($"{entityName} was not found.",
        "ENTITY_NOT_FOUND",
        404)
    {
    }
}