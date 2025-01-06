function SubmitSubMaster() {
if ($('#package_name').val() == '') {
        alert("Please enter package name!");
    return false;
}
else
if ($('#sm_service').val() == '') {
      alert("Please enter service!");
      return false;
}
else
if ($('#subAmount').val() == '') {
    alert("Please enter subscription amount!");
    return false;
}
else
if ($('#subDiscount').val() == '') {
       alert("Please enter subscripton discount!");
      return false;
}
else
if ($('#final_Amount').val() == '') {
       alert("Please enter final amount!");
       return false;
}
else
if ($('#sub_duration').val() == '') {
    alert("Please enter duration in month!");
    return false;
}
else
if ($('#Invoice').val() == '') {
      alert("Please enter no. of invoices!");
     return false;
}
else
if ($('#Quotation').val() == '') {
       alert("Please enter no. of quotation!");
      return false;
}
else
if ($('#Expence').val() == '') {
     alert("Please enter no. of expences!");
    return false;
} else
if ($('#cash_order').val() == '') {
    alert("Please enter no. of cash orders!");
    return false;
}
    var isChecked = $('#flexCheckChecked').prop('checked') ? 1 : 0;
    var data = {
        sm_id: $('#sm_id').val(),
        package_name: $('#package_name').val(),      
        sm_service: $('#sm_service').val(),
        subAmount: $('#subAmount').val(),
        subDiscount: $('#subDiscount').val(),
        final_Amount: $('#final_Amount').val(),
        sub_duration: $('#sub_duration').val(),
        Invoice: $('#Invoice').val(),
        Quotation: $('#Quotation').val(),
        Expence: $('#Expence').val(),
        cash_order: $('#cash_order').val(),
        com_isactive: isChecked
    };
    $.ajax({
        type: 'POST',
        url: '/SubscriptionMaster/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#createModal").modal("hide");
            window.location.reload();
        },


        error: function (xhr, status, error) {
            alter(error);
        }
    });
}
function calculateFinalAmount(discount) {
    var price = parseFloat(document.getElementById('subAmount').value) || 0;
    //var discount = parseFloat(document.getElementById('subDiscount').value) || 0;
    var finalAmount = price - (price * (discount.value / 100));
    document.getElementById('final_Amount').value = finalAmount.toFixed(2);
}
function displaymodel(){
    $("#addSubscription").modal("show");
    $("#AddModalLabel").show();

}
function editLinkClick(sm_id, package_name, sm_service, subAmount, subDiscount, final_Amount, Invoice, Expence, Quotation, cash_order, sub_duration) {
    
    $("#addSubscription").modal("show");
    $("#EditModalLabel").show();
    $('#package_name').val(package_name);
    $('#sm_service').val(sm_service);
    $('#sm_id').val(sm_id);
    $('#subAmount').val(subAmount);
    $('#subDiscount').val(subDiscount);
    $('#final_Amount').val(final_Amount);
    $('#Invoice').val(Invoice);
    $('#Expence').val(Expence);
    $('#Quotation').val(Quotation);
    $('#cash_order').val(cash_order);
    $('#sub_duration').val(sub_duration);
}

function ToggleSwitchCon(sm_id, package_name, sm_service, subAmount, subDiscount, final_Amount, sub_duration, Invoice, Quotation, Expence, cash_order, com_isactive) {
    var data = {
        sm_id: sm_id,
        package_name: package_name,
        sm_service: sm_service,
        subAmount: subAmount,
        subDiscount: subDiscount,
        final_Amount: final_Amount,
        sub_duration: sub_duration,
        Invoice: Invoice,
        Quotation: Quotation,
        Expence: Expence,
        cash_order: cash_order,
        com_isactive: com_isactive
    };

    $.ajax({
        type: 'POST',
        url: '/SubscriptionMaster/Create', // Use the correct URL or endpoint
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

    var baseUrl = window.location.origin + '/SubscriptionMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel' +  (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;


}

function Pdf() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/SubscriptionMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' +  (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;
}
function performAction() {

    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';

    window.location.href = "/SubscriptionMaster/Index?status=" + status;

}