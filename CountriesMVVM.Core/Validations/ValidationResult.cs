namespace CountriesMVVM.Validations
{
    public class ValidationResult
    {
        public bool IsValid { get; init; }
        public string ErrorMessage { get; init; } = string.Empty;

        public static ValidationResult Success() => new() { IsValid = true };

        public static ValidationResult Failure(string message) =>
            new() { IsValid = false, ErrorMessage = message };
    }
}
