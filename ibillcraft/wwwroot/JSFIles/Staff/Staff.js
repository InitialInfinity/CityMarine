function AddStaff() {
    window.location.href = '/Staff/EditIndex';
}
function GoBack() {
   window.location.href = '/Staff/Index';
}
function clrstf() {
   // window.location.href = '/Staff/Index';
    $('#st_staff_name').val("");
    $('#co_country_code').val("");
    $('#st_email').val("");
    $('#st_address').val("");
    $('#st_contact').val("");
    $('#st_dob').val("");
    $('#st_gender').val("");
    $('#st_designation_id').val("");
    $('#st_department_id').val("");
    $('#st_salary').val("");
    $('#st_joining_date').val("");
    $('#st_city_id').val("");
    $('#st_username').val("");
    $('#st_rolename').val("");
    $('#st_category').val("");
    $('#st_staff_code').val("");
}
/* Submit Button  */
function submitStaff() {
    /* event.preventDefault();*/

    if ($('#st_staff_name').val() == '') {
        alert("Please enter  name!");
        return false;
    }
    else if ($('#st_staff_code').val() == '') {
        alert("Please enter code!");
        return false;
    }
    else if ($('#st_email').val() == '') {
        alert("Please enter email!");
        return false;
    }
    else if ($('#st_address').val() == '') {
        alert("Please enter address!");
        return false;
    }
    else if ($('#co_country_code').val().trim() == '') {
        alert("Please enter country code!");
        return false;
    }
    else if ($('#st_contact').val() == '') {
        alert("Please enter contact no.!");
        return false;
    }
    //else if ($('#st_dob').val() == '') {
    //    alert("Please enter date of birth!");
    //    return false;
    //}
    else if ($('#st_gender').val() == '') {
        alert("Please select gender!");
        return false;
    }
    else if ($('#st_username').val() == '') {
        alert("Please enter username!");
        return false;
    }
    //else if ($('#st_designation_id').val() == '') {
    //    alert("Please select designation!");
    //    return false;
    //}
    //else if ($('#st_salary').val() == '' || isNaN($('#st_salary').val())) {
    //    alert("Please enter salary!");
    //    return false;
    //}
    else if ($('#st_joining_date').val() == '') {
        alert("Please enter joining date!");
        return false;
    }
    else if ($('#st_city_id').val() == '') {
        alert("Please select city!");
        return false;
    }
    else if ($('#st_rolename').val() == '') {
        alert("Please select role!");
        return false;
    }
    else if ($('#st_rolename').val() == '') {
        alert("Please select role!");
        return false;
    }
    var isActive = $('#st_isactive').is(':checked') ? 1 : 0;
    var data = {
        st_id: $('#st_id').val(),
        st_staff_name: $('#st_staff_name').val(),
        st_email: $('#st_email').val(),
        st_address: $('#st_address').val(),
        st_contact: $('#st_contact').val(),
        st_dob: $('#st_dob').val(),
        co_country_code: $('#co_country_code').val(),
        st_gender: $('#st_gender').val(),
        st_designation_id: $('#st_designation_id').val(),
        st_department_id: $('#st_department_id').val(),
        st_salary: $('#st_salary').val(),
        st_joining_date: $('#st_joining_date').val(),
        st_city_id: $('#st_city_id').val(),
        st_username: $('#st_username').val(),
        st_rolename: $('#st_rolename').val(),
        st_category: $('#st_category').val(),
        st_staff_code: $('#st_staff_code').val(),
        st_isactive: isActive,
    };
    $.ajax({
        type: 'POST',
        url: '/Staff/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {

            window.location.href = "/Staff/Index";
        },


        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}
function ToggleSwitchCon(st_id, status) {
    var data = {
        st_id: st_id,
        st_isactive: status
    };

    $.ajax({
        type: 'POST',
        url: '/Staff/Create', // Use the correct URL or endpoint
        data: data,
        success: function (response) {
            // Handle success
            window.location.reload(); // Reload the page on success
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
function validateJoiningDate(joiningDateString) {
    if ($('#st_dob').val() == '') {
        alert("Please enter date of birth!");
        $('#st_joining_date').val('');
        return false;
    }
    var birthDateString = $('#st_dob').val();
    var birthDateParts = birthDateString.split('-');
    var birthDay = parseInt(birthDateParts[2]);
    var birthMonth = parseInt(birthDateParts[1]) - 1; // Months are zero-based
    var birthYear = parseInt(birthDateParts[0]);

    var birthDate = new Date(birthYear, birthMonth, birthDay);

    // Parse joining date
    var joiningDateParts = joiningDateString.split('-');
    var joiningDay = parseInt(joiningDateParts[2]);
    var joiningMonth = parseInt(joiningDateParts[1]) - 1; // Months are zero-based
    var joiningYear = parseInt(joiningDateParts[0]);
    if (joiningYear < 1000 || joiningYear > 9999 || isNaN(joiningYear)) {
        alert("Invalid year format! Please enter a four-digit year.");
        $('#st_joining_date').val('');
        return null;
    }
    var joiningDate = new Date(joiningYear, joiningMonth, joiningDay);

    // Calculate the age based on the birthdate
    var age = Math.floor((joiningDate - birthDate) / (365.25 * 24 * 60 * 60 * 1000));

    if (age < 18) {
        alert("Joining date should be at least 18 years later than the birthdate!");
        $('#st_joining_date').val('');
        return false;
    }

    return true;
}
function validateLeftDate(leftDateString) {
    if ($('#st_joining_date').val() == '') {
        $('#st_left_date').val('');
        alert("Please enter joining date!");
        return false;
    }
    var joiningDateString = $('#st_joining_date').val();
    var joiningDateParts = joiningDateString.split('-');
    var joiningDay = parseInt(joiningDateParts[2]);
    var joiningMonth = parseInt(joiningDateParts[1]) - 1; // Months are zero-based
    var joiningYear = parseInt(joiningDateParts[0]);

    var joiningDate = new Date(joiningYear, joiningMonth, joiningDay);

    // Parse left date
    var leftDateParts = leftDateString.split('-');
    var leftDay = parseInt(leftDateParts[2]);
    var leftMonth = parseInt(leftDateParts[1]) - 1; // Months are zero-based
    var leftYear = parseInt(leftDateParts[0]);
    if (leftYear < 1000 || leftYear > 9999 || isNaN(leftYear)) {
        alert("Invalid year format! Please enter a four-digit year.");
        $('#st_left_date').val('');
        return null;
    }
    var leftDate = new Date(leftYear, leftMonth, leftDay);

    // Ensure that the left date is not before the joining date
    if (leftDate < joiningDate) {
        alert("Left date cannot be before joining date!");
        $('#st_left_date').val('');
        return false;
    }

    return true;
}
function Excel() {
    window.location.href = '/Staff/Excel';
}

