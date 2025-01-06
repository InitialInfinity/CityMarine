function insertreason(type) {
    $("#createModal").modal("show");
    $("#sa_reason").val('');
    $("#type").val(type);
}
function CloseData() {
    $("#createModal").modal("hide");
    $("#sa_reason").val('');
}
function InsertApproval() {
    var status;
    if ($('#sa_reason').val() == '' && $("#type").val()=='Reject') {
        alert("Please enter reason!");
        return false;
    }
    if($("#type").val() == 'Reject') {
        status = "Rejected";
    }
    else {
        status = "Accepted";
    }
    var data = {
        sa_id: $('#sa_id').val(),
        sa_sl_id: $('#sa_sl_id').val(),
        sa_reason: $('#sa_reason').val(),
        sa_approved_status: $('#sa_approved_status').val(),
        sa_approval_stage: '1',
        sa_action:'1',
    };
    $.ajax({
        type: 'POST',
        url: '/SaleApproval/Create',
        data: data,
        success: function (response) {

            window.location.href = "/SaleApproval/Index";
        },


        error: function (xhr, status, error) {
        }
    });
}