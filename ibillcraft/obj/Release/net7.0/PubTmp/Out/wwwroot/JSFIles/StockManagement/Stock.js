function ViewProduct(id,pname,sqrt,quantity,date){
    $("#createModal").modal("show");
    $("#u_product_name").val(pname);
    $("#u_sqrft").val(sqrt);
    $("#u_quanity").val(quantity);
    $("#u_date").val(date);
}

function EditProduct(p_id, p_name, p_stock, p_unit, p_hsn_code, p_desc, p_igst, p_cgst, p_sgst, p_ugst, p_rate) {
    $("#createModal").modal("show");
    $("#p_id").val(p_id);
    $("#p_name").val(p_name);
    $("#p_stock").val(p_stock);
    $("#p_unit").val(p_unit);
    $("#p_hsn_code").val(p_hsn_code);
    $("#p_desc").val(p_desc);
    $("#p_igst").val(p_igst);
    $("#p_cgst").val(p_cgst);
    $("#p_sgst").val(p_sgst);
    $("#p_ugst").val(p_ugst);
    $("#p_rate").val(p_rate);
}
function printdiv(print) {
    var printContents = $("#print #dataTable1").clone(); // Clone the table
    printContents.find('tr').each(function () {
        $(this).find('td:last').remove();
    });
    printContents.find('th:last').remove(); // Remove the last column header and cells
    var originalContents = $("body").html();
    $("body").html(printContents);
    window.print();
    $("body").html(originalContents);
}
function cancalStock() {

    $('#p_stock').val('')
       
}
/* Submit Button  */
function submitstock() {
    /* event.preventDefault();*/

    if ($('#p_stock').val() == '') {
        alert("Please enter stock!");
        return false;
    }
    var data = {
        p_id: $("#p_id").val(),
        p_stock: $('#p_stock').val(),
    };
    $.ajax({
        type: 'POST',
        url: '/StockManagement/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            // Handle success
            // console.log(response);
            $("#createModal").modal("hide");
            //alert(response);

            window.location.reload();
        },


        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}