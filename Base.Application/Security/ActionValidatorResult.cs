namespace Base.Application.Security;

public class ActionValidatorResult
{
    public ActionValidationStatus Status { get; set; }
    public string? Message { get; set; }
}