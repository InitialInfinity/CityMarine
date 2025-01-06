function AddParameter() {
 
    $("#createModal").modal("show");
    //$("#co_id").val('');
    $("#p_parametername").val('');
    $("#p_code").val('');
    $("#p_isactive").val('');

    $('#btnsubmit').prop('disabled', false);

}
function CancelData() {
    $("#createModal").modal("hide");
    window.location.reload()
}
function submitParameter() {
    if ($('#p_code').val() == '') {
        alert("Please enter code!");
        return false;
    } else if ($('#p_parametername').val() == '') {
        alert("Please enter name!");
        return false;
    }



    var isActive = $('#p_isactive').is(':checked') ? 1 : 0;

    var data = {
        p_id: $('#p_id').val(),
        p_isactive: isActive,
        p_parametername: $('#p_parametername').val(),
        p_code: $('#p_code').val(),

    };


    $.ajax({
        type: 'POST',
        url: '/ParameterMaster/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#createModal").modal("hide");

            window.location.reload();
        },

        error: function (xhr, status, error) {

        }
    });
}
function editLinkClick(p_id, p_code, p_parametername, p_isactive) {

    $("#createModal").modal("show");

    $('#p_id').val(p_id);
    $("#p_parametername").val(p_parametername);
    $("#p_code").val(p_code);

    if (p_isactive === "Active") {
        $('#p_isactive').prop('checked', true);
    } else {
        $('#p_isactive').prop('checked', false);
    }

}
function GoToState(p_id, p_parametername) {
    window.location.href = '/ParameterValueMaster?ParameterId=' + p_id + '&ParameterName=' + p_parametername;
}
function ToggleSwitch(p_id, p_code, p_parametername, p_isactive) {
    var data = {

        p_id: p_id,
        p_code: p_code,
        p_parametername: p_parametername,
        p_isactive: p_isactive
    };
    $.ajax({
        type: 'POST',
        url: '/ParameterMaster/Create', // Use the correct URL or endpoint
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
function Excel() {
    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/ParameterMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel'  + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;


}

function Pdf() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    var baseUrl = window.location.origin + '/ParameterMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;
}
function performAction() {
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/ParameterMaster/Index?status=" + status;

}
function trimInput() {
    var code = document.getElementById("p_code");
    code.value = code.value.trim();

    var code = document.getElementById("p_parametername");
    code.value = code.value.trim();

}
function validateInput() {
    var inputElement = document.getElementById("p_parametername");
    var inputValue = inputElement.value;

    // Remove non-alphabetic characters from the input
    var sanitizedValue = inputValue.replace(/[^a-zA-Z]/g, '');

    // Update the input value with the sanitized value
    inputElement.value = sanitizedValue;


    var inputElement1 = document.getElementById("p_code");
    var inputValue1 = inputElement1.value;

    // Remove non-numeric characters from the input
    var sanitizedValue1 = inputValue1.replace(/[^0-9]/g, '');

    // Update the input value with the sanitized value
    inputElement1.value = sanitizedValue1;
}