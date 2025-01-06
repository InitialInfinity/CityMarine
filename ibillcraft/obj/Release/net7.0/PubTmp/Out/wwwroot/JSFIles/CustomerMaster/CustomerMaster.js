
function validateGSTNumber(GSTNumber) {
    var countryCode = $("#co_country_code").val();
    var input = GSTNumber.value.toUpperCase();

    var patterns = {
        '1': /^\d{2}-\d{7}$/, // United States
        '44': /^(GB)?\d{3} \d{4} \d{2}(\d{3})?$/, // United Kingdom
        '61': /^\d{2}-\d{3}-\d{3}-\d{3}$/, // Australia
        '49': /^\d{2}-\d{9}$/, // Germany
        '33': /^\d{2} \d{3} \d{3} \d{3}$/,
        '91': /^\d{2}[A-Z]{5}\d{4}[A-Z]\d{1}[A-Z]\d{1}$/
        // Add more country patterns...
    };


    if (input !== '' && countryCode in patterns) {

        if (patterns[countryCode].test(input)) {

            return true;
        } else {
            alert('Please enter a valid GST no!');

            // Clear the input field if invalid
            $("#c_gst_no").val('');
            return false;
        }
    }

}
function validateMobileBycountry(mobilenumber) {

    var input = mobilenumber.value;
    var co_country_code = $("#co_country_code").val();//"+55";//

    if (input !== '' && co_country_code !== '') {
        var countryCodeDigits = co_country_code.replace(/\D/g, ''); // Extract digits from the country code

        // Define an object to map country codes to the expected length and patterns of mobile numbers
        var countryDetails = {
            '1': { length: 10, pattern: /^[2-9]\d{9}$/ },   // United States and Canada
            '44': { length: 10, pattern: /^[2-9]\d{9}$/ },  // United Kingdom
            '61': { length: 9, pattern: /^[2-9]\d{8}$/ },   // Australia
            '49': { length: 10, pattern: /^[1-9]\d{9}$/ },  // Germany
            '33': { length: 9, pattern: /^[1-9]\d{8}$/ },   // France
            '86': { length: 11, pattern: /^1[3456789]\d{9}$/ }, // China
            '91': { length: 10, pattern: /^[6-9]\d{9}$/ },  // India
            '81': { length: 10, pattern: /^[1-9]\d{9}$/ },  // Japan
            '55': { length: 9, pattern: /^[6-9]\d{8}$/ },   // Brazil
            '27': { length: 9, pattern: /^[6-9]\d{8}$/ }    // South Africa
            // Add more country codes, lengths, and patterns as needed
        };

        var countryDetailsForCode = countryDetails[countryCodeDigits];

        if (countryDetailsForCode &&
            input.length === countryDetailsForCode.length &&
            countryDetailsForCode.pattern.test(input)) {
            return true;
        } else {
            alert('Please enter a valid contact no!');
            $("#c_contact").val('');
            return false;
        }
    }
}
function submitCustomer() {

    if ($('#c_name').val().trim() == '') {
        alert("Please enter name!");
        return false;
    }
    else if ($('#c_address').val().trim() == '') {
        alert("Please enter address!");
        return false;
    }
    else if ($('#c_contact').val().trim() == '') {
        alert("Please enter contact no!");
        return false;
    }
    else if ($('#c_email').val().trim() == '') {
        alert("Please enter email!");
        return false;
    }
    
    var isActive = $('#c_isactive').is(':checked') ? 1 : 0; // Determine if the checkbox is checked
    var form = new FormData();
    var selectedFile = $("#imagefile").prop('files')[0];

    var openingbal = $('#c_opening_balance').val();
    if (openingbal == '' || openingbal == undefined) {
        openingbal = '0';
    }

    var defaultImageSrc = $('#imgowner').prop('src');


    form.append('File', $("#imagefile").prop('files')[0]);
    form.append('c_id', $("#c_id").val());
    form.append('c_name', $("#c_name").val());
    form.append('c_address', $("#c_address").val());
    form.append('c_contact', $("#c_contact").val());
    form.append('c_contact2', $("#c_contact2").val());
    form.append('c_gst_no', $("#c_gst_no").val());
    form.append('c_email', $("#c_email").val());
    form.append('c_dob', $("#c_dob").val());
    form.append('c_anidate', $("#c_anidate").val());
    form.append('c_gstin', '0');
    form.append('c_isactive', isActive);
    form.append('c_note', $("#c_note").val());
    form.append('c_openingbalance', openingbal);
    form.append('c_isactive', $("#c_isactive").val());



    $.ajax({
        type: 'POST',
        url: '/CustomerMaster/Create', // Use the form's action attribute
        data: form,
        dataType: "json",
        contentType: false,
        processData: false,
       
        success: function (data) {
          window.location.href = '/CustomerMaster/Index?status=1';
           window.location.reload();
        },

        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

function submitCredit() {

    if ($('#totalcreditlimit').val().trim() == '') {
        alert("Please enter limit!");
        return false;
    }

    var data = {
        cd_id: $('#cd_id').val(),
        cd_c_id: $('#c_id').val(),
        totalcreditlimit: $('#totalcreditlimit').val(),
        availablebalance: $('#availablebalance').val(),
        outstandingbalance: $('#outstandingbalance').val(),
    };

    $.ajax({
        type: 'POST',
        url: '/CustomerMaster/UpdateCredit', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            window.location.href = '/CustomerMaster/Index?status=1';
        },

        error: function (xhr, status, error) {
             console.error(error);
        }
    });
}
function editLinkClick(c_id) {

    $.get("./CustomerMaster/Edit",
        { c_id: c_id },
        function (data) {
            console.log(data);
            $("#createModal").modal("show");

            $('#c_id').val(data.c_id);
            $("#c_name").val(data.c_name);
            $("#c_address").val(data.c_address);
            $('#c_contact').val(data.c_contact);
            $('#c_contact2').val(data.c_contact2);
            $('#c_gst_no').val(data.c_gst_no);
            $('#c_email').val(data.c_email);


            var rawDate = data.c_dob; // Date in the format "2023-10-01T00:00:00"
            var formattedDate = rawDate.split('T')[0]; // Extract the date part

            $('#c_dob').val(formattedDate);
            //                $('#c_dob').val(data.c_dob);
            var rawDate = data.c_anidate; // Date in the format "2023-10-01T00:00:00"
            var formattedDate = rawDate.split('T')[0]; // Extract the date part

            $('#c_anidate').val(formattedDate);
            //$('#c_anidate').val(data.c_anidate);
            $('#c_gstin').val(data.c_gstin);
            $('#c_note').val(data.c_note);
            $('#c_openingbalance').val(data.c_openingbalance);

            var isActive = $('#c_isactive').is(':checked') ? 1 : 0;
            $('#c_isactive').val(data.isActive);
            var isActive = $('#c_isactive').val(data.c_isactive);

            if (data.c_isactive === "Active") {
                $('#c_isactive').prop('checked', true);
            } else {
                $('#c_isactive').prop('checked', false);
            }



        });
}
function Excel() {



    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/CustomerMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;
    //window.location.href = "./CustomerMaster/Excel";


}

function Pdf() {

  

    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/CustomerMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;
}


function ToggleSwitchCon(c_id, status) {
    var data = {

        c_id: c_id,

        c_isactive: status
    };

    $.ajax({
        type: 'POST',
        url: '/CustomerMaster/UpdateStatus', // Use the correct URL or endpoint
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
function performAction() {

    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';

   

    window.location.href = "/CustomerMaster/Index?status=" + status;

}

function GoBack() {
    window.location.href = "/CustomerMaster/Index?status=" + 1;

}
function quan_credit() {
    var totalcredit = parseFloat($('#totalcreditlimit').val()) || 0;
    var outsatanding = parseFloat($('#outstandingbalance').val()) || 0;
    if ($('#outstandingbalance').val() == '') $('#outstandingbalance').val('0')
    var total = (parseFloat(totalcredit) - parseFloat(outsatanding)) 
    $('#availablebalance').val(total);
}
function FileUploadimage() {
    $("input[id='imagefile']").click();
}

$("#imagefile").change(function () {
    readURL(this);
});


function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        var file = input.files[0];

        // File format validation
        var fileExtension = file.name.split('.').pop().toLowerCase();
        var allowedExtensions = ['gif', 'jpg', 'jpeg', 'bmp', 'png'];

        if (allowedExtensions.indexOf(fileExtension) === -1) {
            alert('Invalid file format. Please select a proper image file.');
            return false;
        }

        reader.onload = function (e) {
            $('#imgowner').attr('src', e.target.result);
        };

        reader.readAsDataURL(file);
    }
}



//function UpdatePhoto() {

//    var form = new FormData();
//    var selectedFile = $("#imagefile").prop('files')[0];
//    form.append('File', $("#imagefile").prop('files')[0]);
//    form.append('c_id', $("#c_id").val());

//    $.ajax({
//        type: 'POST',
//        url: '/CustomerMaster/UpdatePhoto', // Use the form's action attribute
//        data: form,
//        contentType: false,
//        processData: false,

//        success: function (response) {
//            window.location.href = '/CustomerMaster/Index?status=1';
//            window.location.reload();
//        },

//        error: function (xhr, status, error) {
//            console.error(error);
//        }
//    });
//}

function UpdatePhoto() {
    // Check if the file input exists
    var imageFileInput = $("#imagefile");
    if (imageFileInput.length > 0) {
        // Check if there is a file selected
        if (imageFileInput.prop('files').length > 0) {
            // If a file is selected, handle the upload logic here
            var form = new FormData();
            form.append('file', imageFileInput.prop('files')[0]);
            form.append('c_id', $("#c_id").val());
            $.ajax({
                url: '/CustomerMaster/UpdatePhoto', // Replace with the actual endpoint
                type: 'POST',
                data: form,
                processData: false,
                contentType: false,
                success: function (response) {
                    window.location.href = '/CustomerMaster/Index?status=1';
                    window.location.reload();
                },
                error: function (xhr, status, error) {
                    // Handle error
                    console.error(error);
                }
            });
        } else {
            
        }
    } else {
        
    }
}
    function CancelData() {

       
        $("#c_name").val('');
        $("#c_address").val('');
        $('#c_contact').val('');
        $('#c_contact2').val('');
        $('#c_gst_no').val('');
        $('#c_email').val('');
        $('#c_dob').val('');
        $('#c_anidate').val('');
        $('#c_note').val('');
        $('#c_openingbalance').val('');
        $("#c_isactive").prop("checked", false);
        const img1 = document.getElementById('imgowner');
        img1.setAttribute('src', '~/Images/download1.jpg');
    }
