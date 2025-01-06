
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
            $("#v_gst_no").val('');
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
            alert('Please enter a valid contact no !');
            $("#v_contact").val('');
            return false;
        }
    }
}
function submitVendor() {

    //event.preventDefault();



    if ($('#v_name').val().trim() == '') {
        alert("Please enter name!");
        return false;
    }
    else if ($('#v_address').val().trim() == '') {
        alert("Please enter address!");
        return false;
    }
    else if ($('#v_contact').val().trim() == '') {
        alert("Please enter contact no!");
        return false;
    }
    else if ($('#v_email').val().trim() == '') {
        alert("Please enter email!");
        return false;
    }
    var form = new FormData();
    var selectedFile = $("#imagefile").prop('files')[0];

    var openingbal = $('#v_opening_balance').val();
    if (openingbal == '' || openingbal == undefined) {
        openingbal = '0';
    }



    var isActive = $('#v_isactive').is(':checked') ? 1 : 0; // Determine if the checkbox is checked

    //var data = {
    //    v_id: $('#v_id').val(),
    //    v_name: $('#v_name').val(),
    //    v_address: $('#v_address').val(),
    //    v_contact: $('#v_contact').val(),
    //    v_contact2: $('#v_contact2').val(),
    //    v_gst_no: $('#v_gst_no').val(),
    //    v_email: $('#v_email').val(),

    //    v_isactive: isActive, // Set the v_isactive property directly

    //    v_opening_balance: $('#v_opening_balance').val()


    //};


    form.append('File', $("#imagefile").prop('files')[0]);
    form.append('v_id', $("#v_id").val());
    form.append('v_name', $("#v_name").val());
    form.append('v_address', $("#v_address").val());
    form.append('v_contact', $("#v_contact").val());
    form.append('v_contact2', $("#v_contact2").val());
    form.append('v_gst_no', $("#v_gst_no").val());
    form.append('v_email', $("#v_email").val());
    form.append('v_isactive', isActive);
    form.append('v_opening_balance', openingbal);
   

    $.ajax({
        type: 'POST',
        url: '/VendorMaster/Create', // Use the form's action attribute
        data: form,
        dataType: "json",
        contentType: false,
        processData: false,
        success: function (data) {
            window.location.href = '/VendorMaster/Index?status=1';
            window.location.reload();
        },
    

        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}

function editLinkClick(c_id) {

    $.get("./VendorMaster/Edit",
        { c_id: c_id },
        function (data) {
            console.log(data);
            $("#createModal").modal("show");

            $('#v_id').val(data.v_id);
            $("#v_name").val(data.v_name);
            $("#v_address").val(data.v_address);
            $('#v_contact').val(data.v_contact);
            $('#v_contact2').val(data.v_contact2);
            $('#v_gst_no').val(data.v_gst_no);
            $('#v_email').val(data.v_email);
            $('#v_opening_balance').val(data.v_opening_balance);


            var isActive = $('#v_isactive').is(':checked') ? 1 : 0;
            $('#v_isactive').val(data.isActive);
            var isActive = $('#v_isactive').val(data.v_isactive);

            if (data.v_isactive === "Active") {
                $('#v_isactive').prop('checked', true);
            } else {
                $('#v_isactive').prop('checked', false);
            }



        });
}
function Excel() {



    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/VendorMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;
    //window.location.href = "./CustomerMaster/Excel";


}

function Pdf() {



    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/VendorMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;
}


function ToggleSwitchCon(v_id, status) {
    var data = {

        v_id: v_id,

        v_isactive: status
    };

    $.ajax({
        type: 'POST',
        url: '/VendorMaster/UpdateStatus', // Use the correct URL or endpoint
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



    window.location.href = "/VendorMaster/Index?status=" + status;

}

function GoBack() {
    window.location.href = "/VendorMaster/Index?status=" + 1;

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



function UpdatePhoto() {

    var form = new FormData();
    var selectedFile = $("#imagefile").prop('files')[0];
    form.append('File', $("#imagefile").prop('files')[0]);
    form.append('v_id', $("#v_id").val());

    $.ajax({
        type: 'POST',
        url: '/VendorMaster/UpdatePhoto', // Use the form's action attribute
        data: form,
        contentType: false,
        processData: false,

        success: function (response) {
            window.location.href = '/VendorMaster/Index?status=1';
            window.location.reload();
        },

        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}