function Submitunit() {
    if ($('#u_unit').val() == '') {
        alert("Please enter unit!");
        return false;
    }
                            
    var isChecked = $('#flexCheckChecked').prop('checked') ? 1 : 0;
    var data = {
        u_id: $('#u_id').val(),
        u_unit: $('#u_unit').val(),
        u_height: $('#u_height').prop('checked') ? 1 : 0,
        u_amount: $('#u_amount').prop('checked') ? 1 : 0,
        u_size: $('#u_size').prop('checked') ? 1 : 0,
        u_width: $('#u_width').prop('checked') ? 1 : 0,
        u_isactive: isChecked
    };
    $.ajax({
        type: 'POST',
        url: '/UnitConfiguration/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#createmodel").modal("hide");
            window.location.reload();
        },


        error: function (xhr, status, error) {
            alert(error);
        }
    });
}
function displaymodel() {
    $("#createmodel").modal("show");
    $("#AddModalLabel").show();

}
function editLinkClick(u_id, u_unit, u_height, u_amount, u_size, u_width, u_isactive) {

    $("#createmodel").modal("show");
    $("#EditModalLabel").show();
    $('#u_unit').val(u_unit);
    
    $('#u_id').val(u_id);
    
    $('#u_size').prop('checked', u_size === "1");
    $('#u_width').prop('checked', u_width === "1");
    $('#u_amount').prop('checked', u_amount === "1");
    $('#u_height').prop('checked', u_height === "1");
    //$('#flexCheckChecked').prop('checked', u_isactive === "1");
    if (u_isactive === "Active") {
        $('#flexCheckChecked').prop('checked', true);
    } else {
        $('#flexCheckChecked').prop('checked', false);
    }
   
}

function ToggleSwitchCon(u_id, u_unit, u_height, u_amount, u_size, u_width,status) {
    var data = {
        u_id: u_id,
        u_unit: u_unit,
        u_height: u_height,
        u_amount: u_amount,
        u_size: u_size,
        u_width: u_width,
        u_isactive: status
    };

    $.ajax({
        type: 'POST',
        url: '/UnitConfiguration/Create', // Use the correct URL or endpoint
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
//function Excel() {
//    window.location.href = "./Excel";
//}

//function Pdf() {

//    window.location.href = "./Pdf";
//}
function Excel() {


    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/UnitConfiguration';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;


}

function Pdf() {

    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/UnitConfiguration';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;
}
function performAction() {

    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';

    window.location.href = "/UnitConfiguration/Index?status=" + status;

}