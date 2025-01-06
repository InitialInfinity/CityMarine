function AddParameterValue() {
     
    const urlParams = new URLSearchParams(window.location.search);

    // Extract the values
    const ParameterId = urlParams.get("ParameterId");
    const ParameterName = urlParams.get("ParameterName");

    $("#createModal").modal("show");
    //$("#co_id").val('');
    $("#pv_parameterid").val(ParameterId);
    $("#pv_parametervalue").val('');
    $("#pv_code").val('');
    $("#pv_isactive").val('');
    $("#pv_parametername").val(ParameterName)

    $('#btnsubmit').prop('disabled', false);

}
function CancelData() {
    $("#createModal").modal("hide");
    window.location.reload()
}
function submitParameterValue() {

    if ($('#pv_code').val() == '') {
        alert("Please enter code!");
        return false;
    } else
        if ($('#pv_parametervalue').val() == '') {
            alert("Please enter name!");
            return false;
        }


    var isActive = $('#pv_isactive').is(':checked') ? 1 : 0;

    var data = {
        pv_id: $('#pv_id').val(),
        pv_isactive: isActive,
        pv_parameterid: $('#pv_parameterid').val(),
        pv_parametervalue: $('#pv_parametervalue').val(),
        pv_code: $('#pv_code').val(),
        pv_parametername: $('#pv_parametername').val()

    };
    $.ajax({
        type: 'POST',
        url: '/ParameterValueMaster/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#createModal").modal("hide");
            window.location.reload();
        },

        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}
function editLinkClick(pv_id, pv_parameterid, pv_parametername, pv_code, pv_parametervalue, pv_isactive) {
    $("#createModal").modal("show");
    $('#pv_id').val(pv_id);
    $("#pv_parameterid").val(pv_parameterid);
    $("#pv_parametervalue").val(pv_parametervalue);
    $("#pv_parametername").val(pv_parametername);
    $("#pv_code").val(pv_code);

    var isActive = $('#pv_isactive').is(':checked') ? 1 : 0;
    $('#pv_isactive').val(isActive);
    var isActive = $('#pv_isactive').val(pv_isactive);

    if (pv_isactive === "Active") {
        $('#pv_isactive').prop('checked', true);
    } else {
        $('#pv_isactive').prop('checked', false);
    }

}
function ToggleSwitch(pv_id, pv_parameterid, pv_parametername, pv_code, pv_parametervalue, pv_isactive) {
    var data = {

        pv_id: pv_id,
        pv_code: pv_code,
        pv_parameterid: pv_parameterid,
        pv_parametername: pv_parametername,
        pv_parametervalue: pv_parametervalue,
        pv_isactive: pv_isactive
    };
    $.ajax({
        type: 'POST',
        url: '/ParameterValueMaster/Create', // Use the correct URL or endpoint
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
    const ParameterId = searchParams.get("ParameterId");
    const ParameterName = searchParams.get("ParameterName");
    var baseUrl = window.location.origin + '/ParameterValueMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '') + ('&ParameterId=' + ParameterId) + ('&ParameterName=' + ParameterName);

    window.location.href = newUrl;


}

function Pdf() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    const ParameterId = searchParams.get("ParameterId");
    const ParameterName = searchParams.get("ParameterName");
    var baseUrl = window.location.origin + '/ParameterValueMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '') + ('&ParameterId=' + ParameterId) + ('&ParameterName=' + ParameterName);

    window.location.href = newUrl;
}
function performAction() {
    const urlParams = new URLSearchParams(window.location.search);
    // Extract the values
    const ParameterId = urlParams.get("ParameterId");
    const ParameterName = urlParams.get("ParameterName");
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/ParameterValueMaster/Index?status=" + status + "&ParameterId=" + ParameterId + "&ParameterName=" + ParameterName;

}
function trimInput() {
    var code = document.getElementById("pv_code");
    code.value = code.value.trim();

    var code = document.getElementById("pv_parametervalue");
    code.value = code.value.trim();

}

function validateInput() {
    var inputElement = document.getElementById("pv_parametervalue");
    var inputValue = inputElement.value;

    // Remove non-alphabetic characters from the input
    var sanitizedValue = inputValue.replace(/[^a-zA-Z]/g, '');

    // Update the input value with the sanitized value
    inputElement.value = sanitizedValue;
}