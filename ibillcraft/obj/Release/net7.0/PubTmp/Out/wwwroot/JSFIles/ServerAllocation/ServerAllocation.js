function AddServer(com_company_name, com_id, com_contact, com_email, sub_id, py_id, CountryId) {
    $("#createModal").modal("show");
    $('#com_id').val(com_id);
    $('#sub_id').val(sub_id);
    $('#py_id').val(py_id);
    $('#com_email').val(com_email);
    $("#com_company_name").val(com_company_name);
    $("#com_contact").val(com_contact);
    $("#CountryId").val(CountryId);
    $('#btnsubmit').prop('disabled', false);
}
function submitServer() {
    if ($('#com_code').val() == '') {
        alert("Please enter company code!");
        return false;
    }
    if ($('#server').val() == '-- Select --') {
        alert("Please select Server!");
        return false;
    }
    var data = {
        com_id: $('#com_id').val(),
        sub_id: $('#sub_id').val(),
        CountryId: $('#CountryId').val(),
        py_id: $('#py_id').val(),
        com_email: $('#com_email').val(),
        com_code: $('#com_code').val(),
        Server_Value: $('#server').val(),
        Server_Key: $('#server').find(":selected").text(),
        com_company_name: $('#com_company_name').val(),
        com_contact: $('#com_contact').val()
    };
    $.ajax({
        type: 'POST',
        url: '/ServerAllocation/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
           
            $("#createModal").modal("hide");
            window.location.reload();
        },

        error: function (xhr, status, error) {
            //TempData["errorMessage"] = error;
            alter(error);
        }
    });
}
function ViewServer(com_id, com_company_name, com_contact, Allotted_Date, com_code) {

    $.ajax({
        type: 'GET',
        url: '/ServerAllocation/Get?com_id=' + com_id, // Use the form's action attribute
        
        success: function (response) {
            
            $("#com_company_name").val(com_company_name);
            $("#com_contact").val(com_contact);
            $("#Allotted_Date").val(Allotted_Date);
            $("#com_code").val(com_code);
            $("#Allotted_Date").closest('.form-group').show();
            $("#createModal").modal("show");
            $('#server option[value="' + response + '"]').prop('selected', true);
            $('#server').attr('readonly', true);
            $("#com_code").attr('readonly', true);
            $('#btnsubmit').attr('readonly', true);
            $('#btnsubmit').prop('disabled', true).css({
                'background-color': 'gray',
                'color': 'white',
                'cursor': 'not-allowed'
            });
            $("#Allotted_Date").attr('readonly', true);
        },
        error: function (xhr, status, error) {
            TempData["errorMessage"] = error;
        }
    });
}
function Excel() {
     window.location.href = "./ServerAllocation/Excel";
}

function Pdf() {

     window.location.href = "./ServerAllocation/Pdf";
}
