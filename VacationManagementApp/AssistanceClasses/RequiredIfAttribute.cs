using System.ComponentModel.DataAnnotations;

namespace VacationManagementApp.Validation
{
    public class RequiredIfAttribute: ValidationAttribute
    {
        private readonly string _comparisonProperty;
        private readonly string _expectedValue;


        public RequiredIfAttribute(string comparisonProperty, string expectedValue)
        {
            _comparisonProperty = comparisonProperty;  
            _expectedValue = expectedValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
            {
                return new ValidationResult($"Unknow property: {_comparisonProperty}");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance)?.ToString();
            if (comparisonValue == _expectedValue && value == null)
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
            }

            return ValidationResult.Success;
        }



    }
}
