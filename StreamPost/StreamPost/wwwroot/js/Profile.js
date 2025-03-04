$(document).ready(function () {
    const $editInfoForm = $('#editInfoModal form');
    const $editInfoSaveButton = $editInfoForm.find('button[type="submit"]');

    const $changePassForm = $('#changePassModal form');
    const $changePassSaveButton = $changePassForm.find('button[type="button"]').last();

    document.querySelectorAll(".passwordToggle, #currentPasswordToggle").forEach(toggle => {
        toggle.addEventListener("click", function () {
            let passwordInput = this.previousElementSibling;
            let icon = this.querySelector("i");

            if (passwordInput.type === "password") {
                passwordInput.type = "text";
                icon.classList.replace("bi-eye-slash-fill", "bi-eye-fill");
            } else {
                passwordInput.type = "password";
                icon.classList.replace("bi-eye-fill", "bi-eye-slash-fill");
            }
        });
    });


    function validateEditInfoForm() {
        let isValid = true;

        $editInfoForm.find('.text-danger').text('');

        $editInfoForm.find('input').each(function () {
            if (!this.checkValidity()) {
                isValid = false;
                const errorMessage = this.validationMessage;
                $(this).next('.text-danger').text(errorMessage);
            }
        });

        const $userNameInput = $editInfoForm.find('input[name="UserName"]');
        const userNameValue = $userNameInput.val();
        const usernameRegex = /^[a-zA-Z0-9]*$/;

        if (userNameValue.length < 3) {
            isValid = false;
            $userNameInput.next('.text-danger').text('Username must be at least 3 characters long.');
        } else if (!usernameRegex.test(userNameValue)) {
            isValid = false;
            $userNameInput.next('.text-danger').text('Username can only contain letters and numbers.');
        }

        $editInfoSaveButton.prop('disabled', !isValid);
    }

    function validateChangePassForm(field) {
        let isValid = true;

        const $newPasswordInput = $changePassForm.find('input[name="Password"]');
        const $confirmPasswordInput = $changePassForm.find('input[name="ConfirmPassword"]');
        const newPasswordValue = $newPasswordInput.val();
        const confirmPasswordValue = $confirmPasswordInput.val();

        $changePassForm.find('.text-danger').text('');

        if (field === 'Password') {
            if (newPasswordValue.length < 8) {
                isValid = false;
                $newPasswordInput.next('.text-danger').text('New password must be at least 8 characters long.');
            }
            if (newPasswordValue !== confirmPasswordValue) {
                isValid = false;
                $confirmPasswordInput.next('.text-danger').text('Passwords do not match.');
            }
        } else if (field === 'ConfirmPassword') {
            if (newPasswordValue.length < 8) {
                isValid = false;
                $newPasswordInput.next('.text-danger').text('New password must be at least 8 characters long.');
            }
            if (newPasswordValue !== confirmPasswordValue) {
                isValid = false;
                $confirmPasswordInput.next('.text-danger').text('Passwords do not match.');
            }
        }

        $changePassSaveButton.prop('disabled', !isValid);
    }

    $changePassForm.find('input[name="Password"]').on('input change', function () {
        validateChangePassForm('Password');
    });

    $changePassForm.find('input[name="ConfirmPassword"]').on('input change', function () {
        validateChangePassForm('ConfirmPassword');
    });

    $editInfoForm.on('input change', validateEditInfoForm);

    validateEditInfoForm();

    $("#changePasswordForm").submit(function (e) {
        e.preventDefault();

        var form = $(this);
        var formData = form.serialize();

        form.find(".text-danger").text("");

        $.ajax({
            type: "POST",
            url: "/Profile/UpdatePassword",
            data: formData,
            success: function (response) {
                if (response.success) {
                    const successToast = `
                    <div class="toast align-items-center border-0 bg-success mt-2 position-fixed top-0 end-0 text-white fw-2 z-1" role="alert">
                         <div class="d-flex">
                             <div class="toast-body">Password updated successfully!</div>
                             <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast"></button>
                         </div>
                     </div>
                    `;
                    $('body').append(successToast);
                    const toastElement = $('.toast').last();
                    const bsToast = new bootstrap.Toast(toastElement);

                    bsToast.show();

                    setTimeout(() => {
                        toastElement.toast('hide');
                        toastElement.remove();
                    }, 3000);

                    $("#changePassModal").modal("hide");
                    form[0].reset();
                } else {
                    if (response.field && response.message) {
                        form.find("input[name='" + response.field + "']")
                            .next(".text-danger")
                            .text(response.message);
                    }

                    if (response.errors) {
                        $.each(response.errors, function (key, values) {
                            if (Array.isArray(values) && values.length > 0) {
                                form.find("input[name='" + key + "']")
                                    .next(".text-danger")
                                    .text(values[0]);
                            } else if (typeof values === 'string') {
                                form.find("input[name='" + key + "']")
                                    .next(".text-danger")
                                    .text(values);
                            }
                        });
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error:", error);

                const errorToast = `
                   <div class="toast align-items-center border-0 bg-danger mt-2  position-fixed top-0 end-0 text-white fw-2 z-1" role="alert">
                         <div class="d-flex">
                             <div class="toast-body text-white">An error occurred while updating password!</div>
                             <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast"></button>
                         </div>
                     </div>
                    `;
                $('body').append(errorToast);
                const toastElement = $('.toast').last();
                const bsToast = new bootstrap.Toast(toastElement);

                bsToast.show();

                setTimeout(() => {
                    toastElement.toast('hide');
                    toastElement.remove();
                }, 3000);
            }
        });
    });
});
