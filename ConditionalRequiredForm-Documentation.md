# Conditional Required Form Documentation

## Overview
This project demonstrates how to implement conditional required fields in an ASP.NET Core MVC application. The form contains two sets of radio buttons that control the visibility and validation of associated textarea fields.

## Form Purpose
The form is designed for collecting information about:
1. **Medical Conditions**: Whether the user has medical conditions requiring special accommodations
2. **Dietary Restrictions**: Whether the user has dietary restrictions or allergies

## Implementation Details

### 1. Model Structure (`Models/ConditionalFormModel.cs`)

The model contains four main properties:

```csharp
public class ConditionalFormModel
{
    // Radio button properties (default to false/No)
    public bool HasMedicalConditions { get; set; } = false;
    public bool HasDietaryRestrictions { get; set; } = false;
    
    // Conditional textarea properties
    [RequiredIf("HasMedicalConditions", true, ErrorMessage = "Medical conditions details are required when you have medical conditions")]
    public string? MedicalConditionsDetails { get; set; }
    
    [RequiredIf("HasDietaryRestrictions", true, ErrorMessage = "Dietary restrictions details are required when you have dietary restrictions")]
    public string? DietaryRestrictionsDetails { get; set; }
}
```

### 2. RequiredIf Attribute

**Location**: `Models/ConditionalFormModel.cs:22-40`

The custom `RequiredIfAttribute` provides server-side conditional validation:

```csharp
public class RequiredIfAttribute : ValidationAttribute
{
    // Validates that a field is required only when another property has a specific value
    // Parameters:
    // - propertyName: The name of the property to check
    // - value: The value that triggers the requirement
}
```

**What it does**:
- Checks if the specified property (`HasMedicalConditions` or `HasDietaryRestrictions`) equals the trigger value (`true`)
- If the condition is met, validates that the target field is not null or empty
- Returns a validation error if the field is required but empty

### 3. View Implementation (`Views/Home/ConditionalForm.cshtml`)

#### Radio Button Structure

**Medical Conditions Section (Lines 21-44)**:
```html
<div class="form-group mb-4">
    <label asp-for="HasMedicalConditions">Question Label</label>
    
    <!-- No radio button (checked by default) -->
    <div class="form-check">
        <input class="form-check-input" type="radio" asp-for="HasMedicalConditions" value="false" id="medical-no" checked>
        <label class="form-check-label" for="medical-no">No</label>
    </div>
    
    <!-- Yes radio button -->
    <div class="form-check">
        <input class="form-check-input" type="radio" asp-for="HasMedicalConditions" value="true" id="medical-yes">
        <label class="form-check-label" for="medical-yes">Yes</label>
    </div>
    
    <!-- Conditional textarea (hidden by default) -->
    <div id="medical-details" class="mt-3" style="display: none;">
        <textarea asp-for="MedicalConditionsDetails" class="form-control" rows="4"></textarea>
    </div>
</div>
```

**Dietary Restrictions Section (Lines 46-69)**:
Similar structure with different IDs and property bindings:
- Radio buttons: `dietary-no`, `dietary-yes`
- Textarea container: `dietary-details`
- Bound to `HasDietaryRestrictions` and `DietaryRestrictionsDetails`

#### Hide/Show Implementation

**Key attributes for conditional display**:
- `id="medical-details"` and `id="dietary-details"`: Container divs for the textareas
- `style="display: none;"`: Initially hidden state
- Radio button `value="true"` and `value="false"`: Trigger values for JavaScript

### 4. JavaScript Functionality (`wwwroot/js/conditional-form.js`)

**Location**: `wwwroot/js/conditional-form.js`

#### Show/Hide Functions

**toggleMedicalDetails() function (Lines 12-21)**:
```javascript
function toggleMedicalDetails() {
    const selectedValue = document.querySelector('input[name="HasMedicalConditions"]:checked').value;
    if (selectedValue === 'true') {
        medicalDetails.style.display = 'block';           // Show textarea
        medicalTextarea.setAttribute('required', 'required'); // Add HTML5 required
    } else {
        medicalDetails.style.display = 'none';            // Hide textarea
        medicalTextarea.removeAttribute('required');       // Remove required
        medicalTextarea.value = '';                        // Clear content
    }
}
```

**toggleDietaryDetails() function (Lines 24-33)**:
Similar logic for dietary restrictions section.

#### Event Listeners

**Radio button event listeners (Lines 36-43)**:
```javascript
// Add change event listeners to all radio buttons
medicalRadios.forEach(function (radio) {
    radio.addEventListener('change', toggleMedicalDetails);
});

dietaryRadios.forEach(function (radio) {
    radio.addEventListener('change', toggleDietaryDetails);
});
```

**What the JavaScript does**:
1. **Listens for radio button changes**: Detects when user selects "Yes" or "No"
2. **Shows/hides textareas**: Changes `display` style property
3. **Manages HTML5 validation**: Adds/removes `required` attribute
4. **Clears content**: Empties textarea when hidden to prevent stale data
5. **Initializes on page load**: Sets correct initial state based on model values

### 5. Controller Implementation (`Controllers/HomeController.cs`)

**GET Action (Lines 21-24)**:
```csharp
public IActionResult ConditionalForm()
{
    return View(new ConditionalFormModel()); // Returns form with default values
}
```

**POST Action (Lines 26-35)**:
```csharp
[HttpPost]
public IActionResult ConditionalForm(ConditionalFormModel model)
{
    if (ModelState.IsValid) // Validates RequiredIf attributes
    {
        TempData["Success"] = "Form submitted successfully!";
        return RedirectToAction("ConditionalForm");
    }
    return View(model); // Returns form with validation errors
}
```

## Validation Flow

### Client-Side Validation
1. **HTML5 validation**: `required` attribute added/removed by JavaScript
2. **Immediate feedback**: User sees required field indicators
3. **Form submission prevention**: Browser blocks submission if required fields empty

### Server-Side Validation
1. **RequiredIf attribute execution**: Checks conditional requirements
2. **ModelState validation**: ASP.NET Core validates all model properties
3. **Error message display**: Validation errors shown via `asp-validation-for`

## Key Features

### Default Behavior
- Both radio button groups default to "No" (`checked` attribute on "No" options)
- Both textareas are initially hidden (`style="display: none;"`)
- No validation errors on initial load

### Conditional Requirements
- Textareas become required only when corresponding radio button is "Yes"
- Server-side validation enforces requirements via `RequiredIfAttribute`
- Client-side JavaScript provides immediate visual feedback

### User Experience
- Smooth show/hide transitions when switching radio buttons
- Automatic clearing of hidden textarea content
- Clear validation error messages
- Consistent Bootstrap styling

## File Locations Summary

1. **Model**: `Models/ConditionalFormModel.cs` - Data model and validation attributes
2. **Controller**: `Controllers/HomeController.cs:21-35` - Form handling actions
3. **View**: `Views/Home/ConditionalForm.cshtml` - HTML form structure
4. **JavaScript**: `wwwroot/js/conditional-form.js` - Client-side show/hide logic
5. **Navigation**: `Views/Home/Index.cshtml:8` - Link to form