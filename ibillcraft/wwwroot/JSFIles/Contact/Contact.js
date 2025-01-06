

function submitContact() {
   
    if ($('#c_name').val() == '') {
        alert("Please enter first name!");
        return false;
    }
    if ($('#c_lname').val() == '') {
        alert("Please enter last name!");
        return false;
    }
    if ($('#c_email').val() == '') {
        alert("Please enter email address!");
        return false;
    }
    if ($('#c_subject').val() == '') {
        alert("Please enter subject!");
        return false;
    }
    if ($('#c_message').val() == '') {
        alert("Please enter massage!");
        return false;
    }
    var data = {
        c_name: $('#c_name').val(),
        c_lname: $('#c_lname').val(),
        c_email: $('#c_email').val(),
        c_subject: $('#c_subject').val(),
        c_message: $('#c_message').val(),

    };


    $.ajax({
        type: 'POST',
        url: '/Contactus/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            alert(response);
             $('#c_name').val(''),
             $('#c_lname').val(''),
             $('#c_email').val(''),
             $('#c_subject').val(''),
             $('#c_message').val('')
            //window.location.reload();
        },

        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
