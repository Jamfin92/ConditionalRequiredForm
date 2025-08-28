using System.ComponentModel.DataAnnotations;

namespace ConditionalRequiredForm.Models;

public class ConditionalFormModel
{
    [Display(Name = "Do you have any medical conditions that require special accommodations?")]
    public bool HasMedicalConditions { get; set; } = false;

    [RequiredIf("HasMedicalConditions", true, ErrorMessage = "Medical conditions details are required when you have medical conditions")]
    [Display(Name = "Please describe your medical conditions and required accommodations")]
    public string? MedicalConditionsDetails { get; set; }

    [Display(Name = "Do you have any dietary restrictions or allergies?")]
    public bool HasDietaryRestrictions { get; set; } = false;

    [RequiredIf("HasDietaryRestrictions", true, ErrorMessage = "Dietary restrictions details are required when you have dietary restrictions")]
    [Display(Name = "Please describe your dietary restrictions or allergies")]
    public string? DietaryRestrictionsDetails { get; set; }
}

public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _propertyName;
    private readonly object _value;

    public RequiredIfAttribute(string propertyName, object value)
    {
        _propertyName = propertyName;
        _value = value;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_propertyName);
        if (property == null)
        {
            return new ValidationResult($"Property {_propertyName} not found");
        }

        var propertyValue = property.GetValue(validationContext.ObjectInstance);
        
        if (Equals(propertyValue, _value))
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(ErrorMessage ?? "This field is required");
            }
        }

        return ValidationResult.Success;
    }
}