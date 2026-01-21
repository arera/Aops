$(document).ready(function () {
    var table = $("#customerTable");

    if (table.find("tbody tr").length > 0) {
        table.DataTable({
            paging: true,
            searching: true,
            ordering: true,
            responsive: true
        });
    }
});



    let isEditMode = false;

    function openCustomerModal(customer = null) {
        clearValidationErrors();
    $('#customerForm')[0].reset();
    $('#CustomerId').val('');
    const modal = new bootstrap.Modal(document.getElementById('customerModal'));
    modal.show();
        }

    document.getElementById('customerForm').addEventListener('submit', async function (e) {
        e.preventDefault();

    const formData = new FormData();
    const customerId = document.getElementById('CustomerId').value;
    if (customerId) formData.append("CustomerId", customerId); // Only include if it's edit mode
    //formData.append("CustomerId", document.getElementById('CustomerId').value);
    formData.append("Name", document.getElementById('Name').value);
    formData.append("Email", document.getElementById('Email').value);
    formData.append("PrimaryMobile", document.getElementById('PrimaryMobile').value);
    formData.append("SecondaryMobile", document.getElementById('SecondaryMobile').value);
    formData.append("GST", document.getElementById('GST').value);
    formData.append("IsActive", document.getElementById('IsActive').value === "true");
    formData.append("Address.Street", document.getElementById('Address_Street').value);
    formData.append("Address.City", document.getElementById('Address_City').value);
    formData.append("Address.State", document.getElementById('Address_State').value);
    formData.append("Address.ZipCode", document.getElementById('Address_ZipCode').value);
    formData.append("Address.Country", document.getElementById('Address_Country').value);

    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
    if (token) formData.append("__RequestVerificationToken", token);
    const endpoint = isEditMode ? '/Admin/UpdateCustomer' : '/Admin/AddCustomer';

    try {
     const response = await fetch(endpoint, {
        method: 'POST',
    body: formData
                });

    const result = await response.json();
    clearValidationErrors();

    if (response.ok && result.success) {
                    // ✅ Success
     const modal = bootstrap.Modal.getInstance(document.getElementById('customerModal'));
    modal.hide();
    location.reload();
                } else {
                    if (result.errors && Array.isArray(result.errors)) {
        displayValidationErrors(result.errors); // 🔄 Use the helper we discussed earlier
                    } else {
        alert(result.message || "Failed to update user.");
                    }
    console.error("Validation failed:", result);
                }
            } catch (error) {
        console.error("Exception:", error);
    alert("An unexpected error occurred while updating user.");
            }
        });

    function displayValidationErrors(errors) {
        errors.forEach(err => {
            const field = document.querySelector(`[name="${err.field}"]`);
            if (field) {
                let errorSpan = field.parentElement.querySelector('.field-validation-error');
                if (!errorSpan) {
                    errorSpan = document.createElement('span');
                    errorSpan.className = 'field-validation-error text-danger';
                    field.parentElement.appendChild(errorSpan);
                }
                errorSpan.textContent = err.message;
            } else {
                console.warn("Field not found for:", err.field);
            }
        });
        }

    function clearValidationErrors() {
        document.querySelectorAll('.field-validation-error').forEach(el => el.remove());
        }

    async function loadCustomerForEdit(customerId) {
            try {
        // Clear existing validation errors and reset form
        clearValidationErrors();
    $('#customerForm')[0].reset();

    // Fetch customer data
    const response = await fetch(`/Admin/GetAllCustomerByID?Userid=${customerId}`);
    if (!response.ok) {
                    throw new Error("Failed to fetch customer data");
                }

    const customer = await response.json();

    // Populate form fields
    $('#CustomerId').val(customer.customerId);
    $('#Name').val(customer.name);
    $('#Email').val(customer.email);
    $('#PrimaryMobile').val(customer.primaryMobile);
    $('#SecondaryMobile').val(customer.secondaryMobile);
    $('#GST').val(customer.gst);
    $('#IsActive').val(customer.isActive.toString());

    // Address fields
    $('#Address_Street').val(customer.address.street);
    $('#Address_City').val(customer.address.city);
    $('#Address_State').val(customer.address.state);
    $('#Address_ZipCode').val(customer.address.zipCode);
    $('#Address_Country').val(customer.address.country);
    isEditMode = true
    // Open the modal

    const modal = new bootstrap.Modal(document.getElementById('customerModal'));
    modal.show();
            } catch (error) {
        console.error("Edit load failed:", error);
    alert("Something went wrong while loading customer info.");
            }
        }
