        $(document).ready(function () {
            // Initialize DataTable only when the table contains data
            var table = $("#rolesTable");

            if (table.find("tbody tr").length > 0) {
                table.DataTable({
                    paging: true,
                    searching: true,
                    ordering: true,
                    columns: [
                        { title: "Name" },
                        { title: "Email" },
                        { title: "Role" },
                        { title: "Mobile" },
                        { title: "Status" },
                        { title: "Actions" }
                    ]
                });
            }
        });

   // Function to handle the Roles Loading by id button click
        async function loadUserForEdit(userId) {
            const response = await fetch(`/Admin/GetAllOrgUsersByID?Userid=${userId}`, {
                method: 'GET',
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                }
            });

            if (!response.ok) {
                alert("Failed to load user data.");
                return;
            }

                const user = await response.json();
                // Populate form fields
                document.getElementById('Orguser_Name').value = user.name;
                document.getElementById('Orguser_Email').value = user.email;
                document.getElementById('Orguser_Role').value = user.role;
                document.getElementById('Orguser_Mobile').value = user.mobile;
                document.getElementById('Orguser_IsActive').value = user.isActive;
                document.getElementById('Orguser_UserID').value = user.userID;
            // Show the modal
            const modal = new bootstrap.Modal(document.getElementById('editUserModal'));
            modal.show();
            
}
        // Function to handle the Add Roles by id button click
        document.getElementById('editUserForm').addEventListener('submit', async function (e) {
            e.preventDefault(); // prevent default form submission

            const formData = new FormData();
            formData.append("UserID", document.getElementById('Orguser_UserID').value);
            formData.append("Name", document.getElementById('Orguser_Name').value);
            formData.append("Email", document.getElementById('Orguser_Email').value);
            formData.append("Role", document.getElementById('Orguser_Role').value);
            formData.append("Mobile", document.getElementById('Orguser_Mobile').value);
            formData.append("IsActive", document.getElementById('Orguser_IsActive').value === "true");

            // Add anti-forgery token
            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
            formData.append("__RequestVerificationToken", token);

            try {
                const response = await fetch('/Admin/UpdateUserRoles', {
                    method: 'POST',
                    body: formData
                });

                if (response.ok) {
                    const result = await response.json();
                    console.log("Success:", result);

                    // Optional: Close modal
                    const modal = bootstrap.Modal.getInstance(document.getElementById('editUserModal'));
                    modal.hide();

                    // Optionally refresh table or UI
                    location.reload(); // or re-fetch the data and update DOM
                } else {
                    const error = await response.text();
                    console.error("Failed:", error);
                    alert("Failed to update user");
                }
            } catch (error) {
                console.error("Error:", error);
                alert("An error occurred while updating user.");
            }
        });
        // function to handle the Change Pssword Roles by id button click
        function openPasswordModal(userId) {
            document.getElementById('ChangePassword_UserID').value = userId;
            const modal = new bootstrap.Modal(document.getElementById('changePasswordModal'));
            modal.show();
        }

        document.getElementById('changePasswordForm').addEventListener('submit', async function (e) {
            e.preventDefault();

            const formData = new FormData(this);

            try {
                const response = await fetch('/Admin/ChangePassword', {
                    method: 'POST',
                    body: formData
                });

                if (response.ok) {
                    alert("Password updated successfully");
                    const modal = bootstrap.Modal.getInstance(document.getElementById('changePasswordModal'));
                    modal.hide();
                } else {
                    const error = await response.text();
                    console.error("Failed:", error);
                    alert("Failed to change password");
                }
            } catch (err) {
                console.error("Error:", err);
                alert("An error occurred while changing password.");
            }
        });
