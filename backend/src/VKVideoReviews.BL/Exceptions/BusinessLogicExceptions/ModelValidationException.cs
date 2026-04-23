namespace VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;

public class ModelValidationException : BusinessLogicException
{
    public ModelValidationException(string message = "Ошибка валидации данных")
        : base(message, "VALIDATION_ERROR")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ModelValidationException(IDictionary<string, string[]> errors, string message = "Ошибка валидации данных")
        : base(message, "VALIDATION_ERROR")
    {
        Errors = errors;
    }

    public ModelValidationException(string propertyName, string errorMessage)
        : base("Ошибка валидации данных", "VALIDATION_ERROR")
    {
        Errors = new Dictionary<string, string[]>
        {
            { propertyName, [errorMessage] }
        };
    }

    public IDictionary<string, string[]> Errors { get; }
}