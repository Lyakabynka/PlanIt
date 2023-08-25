namespace PlanIt.Identity.Application.Response;

public class ValidationError
{
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
    
    public ValidationError(string propertyName, string errorMessage)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }
}
