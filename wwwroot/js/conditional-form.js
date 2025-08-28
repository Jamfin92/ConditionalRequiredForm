document.addEventListener('DOMContentLoaded', function () {
    // Medical conditions radio button handling
    const medicalRadios = document.querySelectorAll('input[name="HasMedicalConditions"]');
    const medicalDetails = document.getElementById('medical-details');
    const medicalTextarea = document.querySelector('textarea[name="MedicalConditionsDetails"]');

    // Dietary restrictions radio button handling
    const dietaryRadios = document.querySelectorAll('input[name="HasDietaryRestrictions"]');
    const dietaryDetails = document.getElementById('dietary-details');
    const dietaryTextarea = document.querySelector('textarea[name="DietaryRestrictionsDetails"]');

    // Function to show/hide medical conditions textarea
    function toggleMedicalDetails() {
        const selectedValue = document.querySelector('input[name="HasMedicalConditions"]:checked').value;
        if (selectedValue === 'true') {
            medicalDetails.style.display = 'block';
            medicalTextarea.setAttribute('required', 'required');
        } else {
            medicalDetails.style.display = 'none';
            medicalTextarea.removeAttribute('required');
            medicalTextarea.value = ''; // Clear the textarea when hidden
        }
    }

    // Function to show/hide dietary restrictions textarea
    function toggleDietaryDetails() {
        const selectedValue = document.querySelector('input[name="HasDietaryRestrictions"]:checked').value;
        if (selectedValue === 'true') {
            dietaryDetails.style.display = 'block';
            dietaryTextarea.setAttribute('required', 'required');
        } else {
            dietaryDetails.style.display = 'none';
            dietaryTextarea.removeAttribute('required');
            dietaryTextarea.value = ''; // Clear the textarea when hidden
        }
    }

    // Add event listeners to medical conditions radio buttons
    medicalRadios.forEach(function (radio) {
        radio.addEventListener('change', toggleMedicalDetails);
    });

    // Add event listeners to dietary restrictions radio buttons
    dietaryRadios.forEach(function (radio) {
        radio.addEventListener('change', toggleDietaryDetails);
    });

    // Initialize the display state based on current values (for form persistence)
    toggleMedicalDetails();
    toggleDietaryDetails();
});