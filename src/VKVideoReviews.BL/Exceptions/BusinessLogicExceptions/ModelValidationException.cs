namespace VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;

public class ModelValidationException : BusinessLogicException
{
    public IDictionary<string, string[]> Errors { get; }

    public ModelValidationException(string message = "Ошибка валидации данных")
        : base(message, "VALIDATION_ERROR", 400)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ModelValidationException(IDictionary<string, string[]> errors, string message = "Ошибка валидации данных")
        : base(message, "VALIDATION_ERROR", 400)
    {
        Errors = errors;
    }

    public ModelValidationException(string propertyName, string errorMessage)
        : base("Ошибка валидации данных", "VALIDATION_ERROR", 400)
    {
        Errors = new Dictionary<string, string[]>
        {
            { propertyName, [errorMessage] }
        };
    }
}