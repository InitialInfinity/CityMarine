function AddParameter() {
 
    $("#createModal").modal("show");
    //$("#co_id").val('');
    $("#E_id").val('');
    $("#E_parameter").val('');
    $("#E_condition").val('');
    $("#E_category").val('');
    $("#E_value").val('');
    $("#E_isactive").val('');

    $('#btnsubmit').prop('disabled', false);

}
function uploadfile() {

    $("#createModal1").modal("show");
   

}
function submitUpload() {
   
   


    const fileInput = document.getElementById('attachment');
    const file = fileInput.files[0];

    // Check if a file is selected
    if (!file) {
        alert('Please select a file.');
        return;
    }
    // Allowed file extensions
    const allowedExtensions = ['.xls', '.xlsx', '.xlsm', '.csv'];
    const fileExtension = file.name.split('.').pop().toLowerCase();

    // Validate file extension
    if (!allowedExtensions.includes(`.${fileExtension}`)) {
        alert(`Invalid file type!`);
        return;
    }
    var form = new FormData();
    form.append('file', file);

    $.ajax({
        

        url: '/EmailRuleConfg/UploadFile', // Replace with the actual endpoint
        type: 'POST',
        data: form,
        processData: false,
        contentType: false,
        success: function (response) {
            $("#createModal1").modal("hide");

            window.location.reload();
        },

        error: function (xhr, status, error) {

        }
    });
}
function CancelData() {
    $("#createModal").modal("hide");
    window.location.reload()
}
function submitParameter() {
    if ($('#E_parameter').val() == '') {
        alert("Please select parameter!");
        return false;
    }
    else if ($('#E_condition').val() == '') {
        alert("Please select condition!");
        return false;
    }
    else if ($('#E_category').val() == '') {
        alert("Please select category!");
        return false;
    }
    else if ($('#E_value').val() == '') {
        alert("Please enter value!");
        return false;
    }



    var isActive = $('#E_isactive').is(':checked') ? 1 : 0;

    var data = {
        E_id: $('#E_id').val(),
        E_parameter: $('#E_parameter').val(),
        E_condition: $('#E_condition').val(),
        E_category: $('#E_category').val(),
        E_value: $('#E_value').val(),
        E_isactive: isActive,
      

    };


    $.ajax({
        type: 'POST',
        url: '/EmailRuleConfg/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#createModal").modal("hide");

            window.location.reload();
        },

        error: function (xhr, status, error) {

        }
    });
}
function editLinkClick(E_id, E_parameter, E_condition, E_category, E_value, E_isactive) {

    $("#createModal").modal("show");

    $('#E_id').val(E_id);
    $("#E_condition").val(E_condition);
    $("#E_parameter").val(E_parameter);
    $("#E_category").val(E_category);
    $("#E_value").val(E_value);

    if (E_isactive === "Active") {
        $('#E_isactive').prop('checked', true);
    } else {
        $('#E_isactive').prop('checked', false);
    }

}
function ToggleSwitch(E_id, E_parameter, E_condition, E_category, E_value, E_isactive) {
    var data = {

        E_id: E_id,
        E_parameter: E_parameter,
        E_condition: E_condition,
        E_category: E_category,
        E_value: E_value,
        E_isactive: E_isactive
    };
    $.ajax({
        type: 'POST',
        url: '/EmailRuleConfg/Create', // Use the correct URL or endpoint
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

    var baseUrl = window.location.origin + '/EmailRuleConfg';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel'  + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;


}

function downloadExcel() {
    
    window.location.href = '/EmailRuleConfg/Download';

}

function Pdf() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    var baseUrl = window.location.origin + '/EmailRuleConfg';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;
}
function performAction() {
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/EmailRuleConfg/Index?status=" + status;

}
function trimInput() {
    var code = document.getElementById("E_parameter");
    code.value = code.value.trim();

    var code = document.getElementById("E_condition");
    code.value = code.value.trim();

}
function validateInput() {
    var inputElement = document.getElementById("E_condition");
    var inputValue = inputElement.value;

    // Remove non-alphabetic characters from the input
    var sanitizedValue = inputValue.replace(/[^a-zA-Z]/g, '');

    // Update the input value with the sanitized value
    inputElement.value = sanitizedValue;


    var inputElement1 = document.getElementById("E_parameter");
    var inputValue1 = inputElement1.value;

    // Remove non-numeric characters from the input
    var sanitizedValue1 = inputValue1.replace(/[^0-9]/g, '');

    // Update the input value with the sanitized value
    inputElement1.value = sanitizedValue1;
}