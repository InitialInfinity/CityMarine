
//function printdiv(print) {
//    var printContents = $("#print #dataTable1").clone(); // Clone the table
//    printContents.find('tr').each(function () {
//        $(this).find('td:last').remove();
//    });
//    printContents.find('th:last').remove(); // Remove the last column header and cells
//    var originalContents = $("body").html();
//    $("body").html(printContents);
//    window.print();
//    $("body").html(originalContents);
//}
//function ViewProduct(u_id, u_product_name, u_sqrft, u_quanity, u_date) {
//    $("#createModal").modal("show");
//    $("#u_id").val(u_id);
//    $("#u_product_name").val(u_product_name);
//    $("#u_sqrft").val(u_sqrft);
//    $("#u_date").val(u_date);
//    $("#u_quanity").val(u_quanity);

//}

function GoBack() {
    window.location.href = "/RollPurchase/Index";

}
function addpurchase() {
    window.location.href = "/RollPurchase/EditIndex";
}
$(document).ready(function () {
    let currentDate = new Date();


    let formattedDate = currentDate.toISOString().slice(0, 10);


    $("#rpc_invoice_date").val(formattedDate);


    currentDate.setDate(currentDate.getDate() + 10);


    let formattedDueDate = currentDate.toISOString().slice(0, 10);


    $("#rpc_due_date").val(formattedDueDate);

    $("#rpc_invoice_date").on('input', function () {

        let invoiceDate = new Date($(this).val());


        if (!isNaN(invoiceDate.getTime())) {
            let dueDate = new Date(invoiceDate.setDate(invoiceDate.getDate() + 10));


            let formattedDueDate = dueDate.toISOString().slice(0, 10);


            $("#rpc_due_date").val(formattedDueDate);
        }
    });
    $("#rpc_product_id").change(function () {
        // Get the selected value
        var p_id = $(this).val();

        // Call the GetValues function with the selected value
        GetValues(p_id);
    });


    $("#rpc_feet").change(function () {
        // Get the selected value
        var p_id = $(this).val();

        // Call the GetValues function with the selected value
        GetFeetValues(p_id);
    });



    $("#rpc_v_id").change(function () {
        var selectedvendor = $(this).val();
        Vendor(selectedvendor);
    });

    $("#pcp_mode").change(function () {
        // Get the selected value
        var selectedText = $(this).find("option:selected").text();
        if (selectedText == "Cheque") {
            $("#chqdate").css("display", "block");
            $("#chqno").css("display", "block");
            $("#bankname").css("display", "block");
        }
       else if (selectedText == "UPI") {
            $("#chqdate").css("display", "none");
            $("#chqno").css("display", "none");
            $("#bankname").css("display", "none");
            $("#pcp_upi").css("display", "block");
        }
        else {
            $("#chqdate").css("display", "none");
            $("#chqno").css("display", "none");
            $("#bankname").css("display", "none");
        }
    });








    $('#addSubscription').modal({
        backdrop: 'static', // Prevents closing on outside click
        keyboard: false // Prevents closing with the escape key
    });
});


function validateDueDate() {

    //var dueDateInput = $(this);
    //var enteredDate = new Date(dueDateInput.val());
    //var currentDate = new Date();

    //if (enteredDate < currentDate) {

    //    alert(" Due date cannot be a previous date.!");
    //    var invoiceDate = new Date($("#rpc_invoice_date").val());
    //    var dueDate = new Date(invoiceDate.setDate(invoiceDate.getDate() + 10));
    //    var formattedDueDate = dueDate.toISOString().slice(0, 10);


    //    $("#rpc_due_date").val(formattedDueDate);
    //}


    var dueDateInput = $(this);
    var enteredDate = new Date(dueDateInput.val());
    var currentDateInput = $("#rpc_invoice_date");

    var currentDateValue = currentDateInput.val();


    var currentDate = new Date(currentDateValue);

    if (enteredDate < currentDate) {
        alert("Due date cannot be a previous date!");
        var invoiceDate = new Date($("#rpc_invoice_date").val());
        var dueDate = new Date(invoiceDate.setDate(invoiceDate.getDate() + 10));
        var formattedDueDate = dueDate.toISOString().slice(0, 10);


        $("#rpc_due_date").val(formattedDueDate);
    }
}


function validateInputNO(textBox) {

    var textBoxValue = parseFloat(textBox.value);


    if (textBoxValue < 0) {
        alert(" Please enter a non - negative value.!");
        textBox.value = 0;
        $("#rpc_feet").val("");
        return false;

    }



}
function ShowModal() {
    if ($('#rpc_v_id').val() == '') {
        alert("Please select vendor name!");
        return false;
    }
    else {
        $("#addSubscription").modal("show");
        $("#rpc_quantity").val("0");
        $("#rpc_rate").val("0");
        $("#rpc_product_id").val("");
        $("#rpc_desc").val("");
        $("#rpc_feet").val("");
        $("#rpc_complete_total").val("0");
        $("#rpc_total").val("0");
        $("#rpc_igst").val("0");
        $("#rpc_sqmeter").val("0");
        $("#rpc_sqfeet").val("0");
        $("#rpc_samount").val("0");
        $("#rpc_meter").val("0");
        $("#rpc_length").val("0");
        $("#rpc_sgst").val("0");
        $("#rpc_ugst").val("0");
        $("#rpc_cgst").val("0");
        $("#rpc_igstp").val("0");
        $("#rpc_sgstp").val("0");
        $("#rpc_ugstp").val("0");
        $("#rpc_cgstp").val("0");
        $("#rpc_total_gst").val("0");
        $("#rpc_gst").val("0");
        //$("#rpc_advance").val("0");
        //$("#rpc_discount").val("0");
        //$("#rpc_shipping_charges").val("0");

    }

   

}
function CancelData() {
    $("#rpc_quantity").val("0");
    $("#rpc_rate").val("0");
    $("#rpc_product_id").val("");
    $("#rpc_feet").val("");
    $("#rpc_complete_total").val("0");
    $("#rpc_total").val("0");
    $("#rpc_igst").val("0");
    $("#rpc_sqmeter").val("0");
    $("#rpc_sqfeet").val("0");
    $("#rpc_samount").val("0");
    $("#rpc_meter").val("0");
    $("#rpc_length").val("0");
    $("#rpc_sgst").val("0");
    $("#rpc_ugst").val("0");
    $("#rpc_cgst").val("0");
    $("#rpc_igstp").val("0");
    $("#rpc_sgstp").val("0");
    $("#rpc_ugstp").val("0");
    $("#rpc_cgstp").val("0");
    $("#rpc_total_gst").val("0");
    $("#rpc_gst").val("0");
    $("#rpc_desc").val("");

}
function Total_sqmtr() {

    var meter = $("#rpc_meter").val();
    var length = $("#rpc_length").val();
    var totalSqMr = $("#rpc_sqmeter");
    var totalSqfeet = $("#rpc_sqfeet");

    if (meter == null) {
        meter = 0;
    }
    if (length == null) {
        length = 0;
    }


    var total_amount = (parseFloat(meter) * parseFloat(length))
    totalSqMr.val(total_amount.toFixed(2));


    //var Final_sqrmtr = (parseFloat(total_amount));

    var sqrft_formula = 10.764;

    totalSqfeetamount = (parseFloat(sqrft_formula) * parseFloat(total_amount));

    totalSqfeet.val(totalSqfeetamount.toFixed(2));


    var rate = $("#rpc_rate").val();

    var total_amount = (parseFloat(totalSqfeetamount) * parseFloat(rate))


    $("#rpc_samount").val(total_amount.toFixed(2));
   

}

//function rate() {
//    var totalsqfeet = $("#rpc_sqfeet").val();
//    var rate = $("#rpc_rate").val();

//    var total_amount = (parseFloat(totalsqfeet) * parseFloat(rate))


//    $("#rpc_samount").val(total_amount.toFixed(2));

//}


function quan_amount() {
    var rpcamount = $("#rpc_samount").val();
    var quantity = $("#rpc_quantity").val();
    //var total = $("#rpc_total");
    var total_amount = (parseFloat(rpcamount) * parseFloat(quantity))
    //total.val(total_amount.toFixed(2));

    $("#rpc_total").val(total_amount.toFixed(2));
    $("#rpc_complete_total").val(total_amount.toFixed(2));
    gst();
}

function gst() {
    var cgst = parseFloat($("#rpc_cgst").val()) || 0;
    var sgst = parseFloat($("#rpc_sgst").val()) || 0;
    var igst = parseFloat($("#rpc_igst").val()) || 0;
    var ugst = parseFloat($("#rpc_ugst").val()) || 0;
    var total = parseFloat($("#rpc_total").val()) || 0;
    // var total = $("#do_CompleteTotal").val() || 0;

    var total_amount = total;

    var rcgst = total_amount * cgst / 100;
    var rsgst = total_amount * sgst / 100;
    var rigst = total_amount * igst / 100;
    var rugst = total_amount * ugst / 100;
    $("#rpc_igstp").val(rigst);
    $("#rpc_sgstp").val(rsgst);
    $("#rpc_ugstp").val(rugst);
    $("#rpc_cgstp").val(rcgst);
    // var amt = total.value + rcgst.value + rsgst.value + rigst.value;
    var gst = parseFloat(rcgst) + parseFloat(rsgst) + parseFloat(rigst) + parseFloat(rugst);
    var amt = parseFloat(gst) + parseFloat(total_amount);
    $("#rpc_gst").val(gst.toFixed(2));
    $("#rpc_complete_total").val(amt.toFixed(2));
}


function final_total() {
    var subtotal = $("#rpc_total_withoutgst").text();
    var totalgst = parseFloat($('#rpc_total_gst').text()) || 0;
    var discount = $("#rpc_discount").val() || 0;

    var advance = $("#rpc_advance").val() || 0;
    var transport_charges = $("#rpc_shipping_charges").val() || 0;
    var transport_value = parseFloat(transport_charges);


    var total_amount = (parseFloat(subtotal) + parseFloat(transport_value));
    var totalwithgst = (parseFloat(subtotal) + parseFloat(transport_value)) + parseFloat(totalgst);


    if (advance == "0" && discount == "0") {
        var balance = (parseFloat(total_amount)) + parseFloat(totalgst);
        var roundoff = Math.round(balance);
        $("#rpc_balance").text(roundoff.toFixed(2));
        $("#rpc_total_withgst").text(totalwithgst.toFixed(2));
    }
    else if (advance != "0" || discount != "0") {
        var balance = (parseFloat(total_amount) - parseFloat(advance) - parseFloat(discount)) + parseFloat(totalgst);
        var roundoff = Math.round(balance);
        $("#rpc_balance").text(roundoff.toFixed(2));
        $("#rpc_total_withgst").text(totalwithgst.toFixed(2));
    }

}




function GetValues(rpc_product_id) {
    var data = {
        p_id: rpc_product_id
    }
    $.ajax({
        type: 'POST',
        url: '/RollPurchase/GetValues', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            if (response.productData.p_id != null) {
                $("#rpc_rate").val(response.productData.p_rate);
                $("#rpc_desc").val(response.productData.p_desc);
                $("#rpc_hsn").val(response.productData.p_hsn_code);
                $("#rpc_unit").val(response.productData.p_unit);
                $("#rpc_igst").val(response.productData.P_igst ?? 0);
                $("#rpc_ugst").val(response.productData.p_ugst ?? 0);
                $("#rpc_sgst").val(response.productData.p_sgst ?? 0);
                $("#rpc_cgst").val(response.productData.p_cgst ?? 0);
               
                $("#rpc_length").val(0);
                $("#rpc_quantity").val(0);
                $("#rpc_total").val(0);
                $("#rpc_complete_total").val(0);
                $("#rpc_igstp").val(0);
                $("#rpc_sgstp").val(0);
                $("#rpc_ugstp").val(0);
                $("#rpc_cgstp").val(0);
                $("#rpc_meter").val(0);
                $("#rpc_feet").val("");
               
                $("#rpc_sqmeter").val(0);
                $("#rpc_sqfeet").val(0);
                $("#rpc_samount").val(0);
                $("#rpc_gst").val(0);
              
               
            }
            //window.location.reload();
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
function Vendor(rpc_v_id) {
    var data = {
        rpc_v_id: rpc_v_id
    }
    $.ajax({
        type: 'POST',
        url: '/RollPurchase/Vendor', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#v_opening_balance").text(response.v_opening_balance);
            $("#rpc_order_no").val(response.rpc_order_no);
            $("#rpc_advance").val("0");
            $("#rpc_discount").val("0");
            $("#rpc_shipping_charges").val("0");
        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}











function GetFeetValues(rpc_feet) {
    var data = {
        p_id: rpc_feet
    }
    $.ajax({
        type: 'POST',
        url: '/RollPurchase/GetFeetValues', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            if (response.productData.f_id != null) {
                $("#rpc_meter").val(response.productData.f_mtr);


            }
            //window.location.reload();
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
var addedRowsData = [];
function submitgrid() {


  
    if ($('#rpc_product_id').val() == '') {
        alert("Please select product!");
        return false;
    }
    else if ($('#rpc_feet').val() == '') {
        alert("Please select feet!");
        return false;
    }
    else if ($('#rpc_meter').val() == '0' || $('#rpc_meter').val() == '') {
        alert("Please enter meter!");
        return false;
    }
   // else if (!isNaN(numericValue) && numericValue <= 0) {
    else if ($('#rpc_length').val() == '0' || $('#rpc_length').val() == '') {
        alert("Please enter length!");
        return false;
    }
   
    else if ($('#rpc_rate').val() == '' || $('#rpc_rate').val() == 0) {
        alert("Please enter rate!");
        return false;
    }
    else if ($('#rpc_quantity').val() == '0' || $('#rpc_quantity').val() == '') {
        alert("Please enter quantity!");
        return false;
    }
    var productid = $('#rpc_product_id option:selected').val();
    //var product = $("#pc_product_id option:selected").text();
    var product = $('#rpc_product_id option:selected').text();

    var desc = $("#rpc_desc").val();

    var rate = $("#rpc_rate").val();


    var feetid = $('#rpc_feet option:selected').val();
    var feet = $('#rpc_feet option:selected').text();

    var meter = $("#rpc_meter").val();
    var length = $("#rpc_length").val();
    var sqmeter = $("#rpc_sqmeter").val();
    var sqfeet = $("#rpc_sqfeet").val();
    var samount = $("#rpc_samount").val();


    var qty = $("#rpc_quantity").val();
    var IGST = $("#rpc_igst").val();
    var SGST = $("#rpc_sgst").val();
    var UGST = $("#rpc_ugst").val();
    var CGST = $("#rpc_cgst").val();
    var IGSTP = $("#rpc_igstp").val();
    var SGSTP = $("#rpc_sgstp").val();
    var UGSTP = $("#rpc_ugstp").val();
    var CGSTP = $("#rpc_cgstp").val();
    var totalGST = $("#rpc_gst").val();
    var totalGSTAMT = $("#rpc_complete_total").val();
    var total = $("#rpc_total").val();
    var hsn = $("#rpc_hsn").val();
    var unit = $("#rpc_unit").val();

    var $editingRow = $("#dataTable tbody tr.editing");

    // Remove the editing class and the row
    $editingRow.removeClass("editing");
    $editingRow.remove();
    // Create a new table row
    var newRow = "<tr>" +
        "<td style='display:none;'>" + productid + "</td>" +
       
        "<td style='display:none;'>" + feetid + "</td>" +
        "<td>" + product + "</td>" +
        "<td>" + desc + "</td>" +
        "<td style='text-align:right'>" + meter + "</td>" +
        "<td style='text-align:right'>" + length + "</td>" +
        "<td style='text-align:right'>" + sqmeter + "</td>" +
        "<td style='text-align:right'>" + sqfeet + "</td>" +
        "<td style='text-align:right'>" + rate + "</td>" +
        "<td style='text-align:right'>" + samount + "</td>" +
        "<td style='text-align:right'>" + qty + "</td>" +
        "<td style='text-align:right'>" + total + "</td>" +
        //"<td>" + IGST + "</td>" +
        //"<td>" + SGST + "</td>" +
        //"<td>" + UGST + "</td>" +
        //"<td>" + CGST + "</td>" +
        //"<td>" + IGSTP + "</td>" +
        //"<td>" + SGSTP + "</td>" +
        //"<td>" + UGSTP + "</td>" +
        //"<td>" + CGSTP + "</td>" +
        "<td style='text-align:right'>" + CGST + "</td>" +
        "<td style='text-align:right'>" + SGST + "</td>" +
        "<td style='text-align:right'>" + IGST + "</td>" +
        "<td style='text-align:right'>" + UGST + "</td>" +
        "<td style='display:none;'>" + CGSTP + "</td>" +
        "<td style='display:none;'>" + SGSTP + "</td>" +
        "<td style='display:none;'>" + IGSTP + "</td>" +
        "<td style='display:none;'>" + UGSTP + "</td>" +
        "<td style='text-align:right'>" + totalGST + "</td>" +
        "<td style='text-align:right'>" + totalGSTAMT + "</td>" +
        "<td style='display:none;'>" + hsn + "</td>" +
        "<td style='display:none;'>" + unit + "</td>" +

        "<td class='text-center'>" +
        "<div class='d-flex'>" +
        " <i class='fas fa-edit click-edit'></i><i class='fa fa-trash ml-2 text-danger click'></i>" +
        "</div>" +
        "</td>" +
        "</tr>";






    // Append the new row to the table body
    $("#dataTable tbody").append(newRow);
    var rowData = {
        productid: productid,
        desc: desc,
        feetid: feetid,
        product: product,
        meter: meter,
        length: parseFloat(length),
        sqmeter: parseFloat(sqmeter),
        sqfeet: parseFloat(sqfeet),

        rate: parseFloat(rate),
        samount: parseFloat(samount),

        qty: parseFloat(qty),
        total: parseFloat(total),
        IGST: parseFloat(IGST),
        SGST: parseFloat(SGST),
        UGST: parseFloat(UGST),
        CGST: parseFloat(CGST),
        IGSTP: parseFloat(IGSTP),
        SGSTP: parseFloat(SGSTP),
        UGSTP: parseFloat(UGSTP),
        CGSTP: parseFloat(CGSTP),
        totalGST: parseFloat(totalGST),
        totalGSTAMT: parseFloat(totalGSTAMT),
        hsn: parseFloat(hsn),
        unit: parseFloat(unit)
    };

    addedRowsData.push(rowData);
    // Close the modal
    calculateTotals();
    final_total();
    $("#addSubscription").modal("hide");

    $("#rpc_desc").val("");

    $("#rpc_rate").val("0");

    $("#rpc_quantity").val("0");
    $("#rpc_igst").val("0");
    $("#rpc_sgst").val("0");
    $("#rpc_ugst").val("0");
    $("#rpc_cgst").val("0");
    $("#rpc_igstp").val("0");
    $("#rpc_sgstp").val("0");
    $("#rpc_ugstp").val("0");
    $("#rpc_cgstp").val("0");
    $("#rpc_total_gst").val("0");
    $("#rpc_gst").val("0");
   
    $("#rpc_product_id").val("");

  
    $("#rpc_feet").val("");
    $("#rpc_complete_total").val("0");
    $("#rpc_total").val("0");
    
    $("#rpc_sqmeter").val("0");
    $("#rpc_sqfeet").val("0");
    $("#rpc_samount").val("0");
    $("#rpc_meter").val("0");
    $("#rpc_length").val("0");
    $("#rpc_desc").val("");
}

$(document).on('click', '#dataTable tbody .click', function () {
    var $rowToDelete = $(this).closest('tr');
    var deletedRowIndex = $rowToDelete.index();

    // Remove the data of the deleted row
    addedRowsData.splice(deletedRowIndex, 1);

    // Remove the row from the table
    $rowToDelete.remove();

    // Recalculate totals
    calculateTotals();
    final_total();
});

function calculateTotals() {
    // Calculate totals from the stored data
    var totalRate = 0;
    var totalQuantity = 0;
    var totalAmount = 0;
    var totalGSTI = 0;
    var totalGSTS = 0;
    var totalGSTU = 0;
    var totalGSTC = 0;
    var totalGST = 0;
    var totalAmountwithoutgst = 0;
    $("#dataTable tbody tr").each(function () {
        var row = $(this);
        totalRate += parseFloat(row.find("td:eq(8)").text()) || 0;
        totalQuantity += parseFloat(row.find("td:eq(10)").text()) || 0;
        totalGSTI += parseFloat(row.find("td:eq(18)").text()) || 0;
        totalGSTS += parseFloat(row.find("td:eq(17)").text()) || 0;
        totalGSTU += parseFloat(row.find("td:eq(19)").text()) || 0;
        totalGSTC += parseFloat(row.find("td:eq(16)").text()) || 0;
        totalGST += parseFloat(row.find("td:eq(20)").text()) || 0;
        totalAmount += parseFloat(row.find("td:eq(21)").text()) || 0;
        totalAmountwithoutgst += parseFloat(row.find("td:eq(11)").text()) || 0;

    });
    // Update the hidden inputs with the calculated totals
    $("#rpc_rate").val(totalRate.toFixed(2));
    $("#rpc_total_quantity").text(totalQuantity);
    $("#rpc_total").val(totalAmount.toFixed(2));
    $("#rpc_total_withoutgst").text(totalAmountwithoutgst.toFixed(2));
    $("#rpc_total_igst").text(totalGSTI);
    $("#rpc_total_sgst").text(totalGSTS);
    $("#rpc_total_cgst").text(totalGSTC);
    $("#rpc_total_ugst").text(totalGSTU);
    $("#rpc_total_gst").text(totalGST.toFixed(2));
    $("#rpc_total_withgst").text(totalAmount.toFixed(2));
    $("#rpc_balance").text(totalAmount.toFixed(2));
    $("#rpc_withgst").text(totalAmount.toFixed(2));

}

function final_total() {
    var subtotal = $("#rpc_total_withoutgst").text();
    var totalgst = parseFloat($('#rpc_total_gst').text()) || 0;
    var discount = $("#rpc_discount").val() || 0;

    var advance = $("#rpc_advance").val() || 0;
    var transport_charges = $("#rpc_shipping_charges").val() || 0;
    var transport_value = parseFloat(transport_charges);


    var total_amount = (parseFloat(subtotal) + parseFloat(transport_value));
    var totalwithgst = (parseFloat(subtotal) + parseFloat(transport_value)) + totalgst;


    if (advance == "0" && discount == "0") {
        var balance = (parseFloat(total_amount)) + totalgst;
        var roundoff = Math.round(balance);
        $("#rpc_balance").text(roundoff.toFixed(2));
        $("#rpc_total_withgst").text(totalwithgst.toFixed(2));
    }
    else if (advance != "0" || discount != "0") {
        var balance = (parseFloat(total_amount) - parseFloat(advance) - parseFloat(discount)) + totalgst;
        var roundoff = Math.round(balance);
        $("#rpc_balance").text(roundoff.toFixed(2));
        $("#rpc_total_withgst").text(totalwithgst.toFixed(2));
    }

}



function submitPurchase(type) {
    /* event.preventDefault();*/
    if ($('#rpc_v_id').val() == '') {
        alert("Please select vendor name!");
        return false;
    }
   
   
    else
        var rowCount = $('#dataTable tbody tr').length - 1;
    if (rowCount == 0) {
        alert("Please select product!");
        return false;
    }
    else if ($('#rpc_payment_method').val() == '') {
        alert("Please select payment method!");
        return false;
    }
   
    var orderDataArray = [];

    // Iterate over each row in the table 
    $("#dataTable tbody tr:gt(0)").each(function () {
        var rowData = {
            rpc_product_id: $(this).find('td:eq(0)').text(),
           
            rpc_feet: $(this).find('td:eq(1)').text(),
            rpc_product_name: $(this).find('td:eq(2)').text(),
            rpc_desc: $(this).find('td:eq(3)').text(),
            rpc_meter: $(this).find('td:eq(4)').text(),
            rpc_length: $(this).find('td:eq(5)').text(),

            rpc_sqmeter: $(this).find('td:eq(6)').text(),
            rpc_sqfeet: $(this).find('td:eq(7)').text(),
            rpc_rate: $(this).find('td:eq(8)').text(),
            rpc_samount: $(this).find('td:eq(9)').text(),
            rpc_quantity: $(this).find('td:eq(10)').text(),
            rpc_total: $(this).find('td:eq(11)').text(),

            //rpc_igst: $(this).find('td:eq(12)').text(),
            //rpc_sgst: $(this).find('td:eq(13)').text(),
            //rpc_ugst: $(this).find('td:eq(14)').text(),
            //rpc_cgst: $(this).find('td:eq(15)').text(),
            //rpc_igstp: $(this).find('td:eq(16)').text(),
            //rpc_sgstp: $(this).find('td:eq(17)').text(),
            //rpc_ugstp: $(this).find('td:eq(18)').text(),
            //rpc_cgstp: $(this).find('td:eq(19)').text(),

            rpc_cgst: $(this).find('td:eq(12)').text(),
           rpc_sgst: $(this).find('td:eq(13)').text(),
            rpc_igst: $(this).find('td:eq(14)').text(),
            rpc_ugst: $(this).find('td:eq(15)').text(),
            rpc_cgstp: $(this).find('td:eq(16)').text(),
            rpc_sgstp: $(this).find('td:eq(17)').text(),
            rpc_igstp: $(this).find('td:eq(18)').text(),
            rpc_ugstp: $(this).find('td:eq(19)').text(),
            


            rpc_gst: $(this).find('td:eq(20)').text(),
            rpc_complete_total: $(this).find('td:eq(21)').text(),
            rpc_hsn: $(this).find('td:eq(22)').text(),
            rpc_unit: $(this).find('td:eq(23)').text(),
            rpc_invoice_no: $('#rpc_invoice_no').val(),
            rpc_order_no: $('#rpc_order_no').val(),
            rpc_com_id: $('#comIdLabel').val(),






        };
        orderDataArray.push(rowData);

    });
    var data = {
        //  pc_name: $('#pc_name').val(),


        rpc_shipping_charges: $('#rpc_shipping_charges').val() ?? 0,
        rpc_discount: $('#rpc_discount').val() ?? 0,
        rpc_advance: $('#rpc_advance').val() ?? 0,
        rpc_payment_method: $('#rpc_payment_method').val(),
        rpc_total_withoutgst: $('#rpc_total_withoutgst').text(),
        rpc_total_igst: $('#rpc_total_igst').text(),
        rpc_total_sgst: $('#rpc_total_sgst').text(),
        rpc_total_cgst: $('#rpc_total_cgst').text(),
        rpc_total_ugst: $('#rpc_total_ugst').text(),
        rpc_total_gst: $('#rpc_total_gst').text(),
        rpc_total_quantity: $('#rpc_total_quantity').text(),
        rpc_total_withgst: $('#rpc_total_withgst').text(),
        rpc_balance: $('#rpc_balance').text(),
        rpc_withgst: $('#rpc_withgst').text(),
        rpc_complete_total: $('#rpc_complete_total').val(),
        rpc_invoice_no: $('#rpc_invoice_no').val(),
        rpc_invoice_date: $('#rpc_invoice_date').val(),
        rpc_due_date: $('#rpc_due_date').val(),
        rpc_order_no: $('#rpc_order_no').val(),
        rpc_v_name: $('#rpc_v_id option:selected').text(),
        rpc_v_id: $('#rpc_v_id option:selected').val(),
        rpc_product_name: $('#rpc_product_id option:selected').text(),
        rpc_product_id: $('#rpc_product_id option:selected').val(),
        rpc_id: $('#rpc_id').val(),

      RollPurchaseInvoice: orderDataArray
    };
    $.ajax({
        type: 'POST',
        url: '/RollPurchase/Create', // Use the form's action attribute
        data: data, // Serialize the form data
       
        success: function (response) {
            if (type == 'Close') {
                window.location.href = "/RollPurchase/Index";
            }
            else {
                window.location.href = "/RollPurchase/EditIndex";
            }
        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}


$(document).on('click', '#dataTable tbody .click-edit', function () {



    var $rowToEdit = $(this).closest('tr');
    var originalHTML = $rowToEdit.html();
    var rpc_product_id = $rowToEdit.find('td:eq(0)').text();
   
    var rpc_feet = $rowToEdit.find('td:eq(1)').text();
    var rpc_product_name = $rowToEdit.find('td:eq(2)').text();
    var rpc_desc = $rowToEdit.find('td:eq(3)').text();
    var rpc_meter = $rowToEdit.find('td:eq(4)').text();
    var rpc_length = $rowToEdit.find('td:eq(5)').text();
    var rpc_sqmeter = $rowToEdit.find('td:eq(6)').text();
    var rpc_sqfeet = $rowToEdit.find('td:eq(7)').text();
    var rpc_rate = $rowToEdit.find('td:eq(8)').text();
    var rpc_samount = $rowToEdit.find('td:eq(9)').text();
    var rpc_quantity = $rowToEdit.find('td:eq(10)').text();
    var rpc_total = $rowToEdit.find('td:eq(11)').text();

    //var rpc_igst = $rowToEdit.find('td:eq(12)').text();
    //var rpc_sgst = $rowToEdit.find('td:eq(13)').text();
    //var rpc_ugst = $rowToEdit.find('td:eq(14)').text();
    //var rpc_cgst = $rowToEdit.find('td:eq(15)').text();

    //var rpc_igstp = $rowToEdit.find('td:eq(16)').text();
    //var rpc_sgstp = $rowToEdit.find('td:eq(17)').text();
    //var rpc_ugstp = $rowToEdit.find('td:eq(18)').text();
    //var rpc_cgstp = $rowToEdit.find('td:eq(19)').text();


    var rpc_cgst = $rowToEdit.find('td:eq(12)').text();
    var rpc_sgst = $rowToEdit.find('td:eq(13)').text();
    var rpc_igst = $rowToEdit.find('td:eq(14)').text();
    var rpc_ugst = $rowToEdit.find('td:eq(15)').text();
    var rpc_cgstp = $rowToEdit.find('td:eq(16)').text();
    var rpc_sgstp = $rowToEdit.find('td:eq(17)').text();
    var rpc_igstp = $rowToEdit.find('td:eq(18)').text();
    var rpc_ugstp = $rowToEdit.find('td:eq(19)').text();

    var rpc_gst = $rowToEdit.find('td:eq(20)').text();
    var rpc_complete_total = $rowToEdit.find('td:eq(21)').text();
    var rpc_hsn = $rowToEdit.find('td:eq(22)').text();
    var rpc_unit = $rowToEdit.find('td:eq(23)').text();
    $rowToEdit.addClass("editing");







    $("#rpc_product_id").val(rpc_product_id.trim().toLowerCase());

    // Populate the modal fields with rowData (you can adapt this to your modal structure)
    //$("#pc_product_id").val(pc_product_id);
    // $("#pc_product_id option:selected").text(pc_product_id);

    $("#rpc_rate").val(rpc_rate.trim());
    $("#rpc_feet").val(rpc_feet.trim());
    $("#rpc_meter").val(rpc_meter.trim());
    $("#rpc_length").val(rpc_length.trim());
    $("#rpc_sqmeter").val(rpc_sqmeter.trim());
    $("#rpc_sqfeet").val(rpc_sqfeet.trim());
    $("#rpc_samount").val(rpc_samount.trim());




    $("#rpc_desc").val(rpc_desc.trim());

    $("#rpc_hsn").val(rpc_hsn.trim());
    $("#rpc_unit").val(rpc_unit.trim());

    $("#rpc_quantity").val(rpc_quantity.trim());
    $("#rpc_igst").val(rpc_igst.trim());
    $("#rpc_sgst").val(rpc_sgst.trim());
    $("#rpc_ugst").val(rpc_ugst.trim());
    $("#rpc_cgst").val(rpc_cgst.trim());
    $("#rpc_igstp").val(rpc_igstp.trim());
    $("#rpc_sgstp").val(rpc_sgstp.trim());
    $("#rpc_ugstp").val(rpc_ugstp.trim());
    $("#rpc_cgstp").val(rpc_cgstp.trim());
    $("#rpc_gst").val(rpc_gst.trim());
    $("#rpc_complete_total").val(rpc_complete_total.trim());
    $("#rpc_total").val(rpc_total.trim());
    $("#addSubscription").modal("show");
    // $rowToEdit.remove();

});

function Excel() {
    window.location.href = '/RollPurchase/Excel';
}
var locales = [
    { code: 'en-US', style: 'currency', currency: 'USD' },   // United States Dollar
    { code: 'en-GB', style: 'currency', currency: 'GBP' },   // British Pound Sterling
    { code: 'en-IN', style: 'currency', currency: 'INR' },   // Indian Rupee
    { code: 'es-ES', style: 'currency', currency: 'EUR' },   // Euro (Spain)
    // Add more locales as needed
];
function formatAsCurrency(digits) {
    var locale = $('#cm_currency_format').val();
    var usCurrency = new Intl.NumberFormat(locale);
    var usFormatted = usCurrency.format(digits);
    var selectedLocale = locales.find(function (item) {
        return item.code === locale;
    });
    var seFormatted = digits.toLocaleString(locale, { style: 'currency', currency: selectedLocale.currency });
    return seFormatted;
}
function Pdf() {
    window.location.href = '/RollPurchase/Pdf';
}
function payment() {
    var rpc_id = $('#rpc_id').text();
    var rpc_invoice_no = $('#pcNoValue').text();
    var rpc_v_name = $('#rpc_v_name').text();
    var rpc_v_id = $('#rpc_v_id').text();
   // var rpc_balance = $('#rpc_balance').text();
    var rpc_balance = parseCurrency($('#rpc_balance').text());
    var paymentmethod = $('#rpc_payment_method').text();
    // var do_payment_method = $('#do_payment_method').text();
    if (rpc_balance == "0" || rpc_balance == "0.00" || rpc_balance == "00.00") {
        alert("Payment is already done");
        return false;
    }
    else {
        window.location.href = '/RollPurchase/Payment?rpc_invoice_no=' + rpc_invoice_no + '&rpc_v_name=' + rpc_v_name + '&rpc_balance=' + rpc_balance + '&rpc_v_id=' + rpc_v_id + "&rpc_id=" + rpc_id + "&type=" + paymentmethod;
    }
}
function payamount() {
   

    var pcp_pc_balance = parseCurrency($("#pcp_due").val());
    var pcp_pay = parseFloat($('#pcp_pay').val()) || 0;
    var pcp_discount = parseCurrency($("#pcp_discount").val()) || 0;
    var total = parseFloat(pcp_pc_balance) - (parseFloat(pcp_discount) + parseFloat(pcp_pay));
    $("#pcp_balance").val(formatAsCurrency(total));
}
function submitpayment() {
    /* event.preventDefault();*/

    //if ($('#pcp_due').val() == $('#pcp_balance').val()) {
    //    alert("Please enter payment details!");
    //    return false;
    //}
    if (parseCurrency($('#pcp_due').val()) == '0' || parseCurrency($('#pcp_due').val()) == '0.0') {
        alert("Payment is already done!");
        return false;
    } 

    else if ($('#pcp_discount').val() == '') {
        alert("Please enter discount!");
        return false;
    } else if ($('#pcp_pay').val() == '' || $('#pcp_pay').val() == '0') {
        alert("Please enter pay!");
        return false;
    } else if ($('#pcp_mode').val() == '') {
        alert("Please select payment method!");
        return false;
    } 
    
    else if ($('#pcp_mode option:selected').text() == 'Cheque') {
        if ($('#pcp_bank_name').val() == '') {
            alert("Please enter bank name!");
            return false;
        } else if ($('#pcp_chq_no').val() == '') {
            alert("Please enter cheque number!");
            return false;
        } else if ($('#pcp_chq_date').val() == '') {
            alert("Please select cheque date!");
            return false;
        }
    }
    var data = {
        pcp_invoice_no: $('#pcp_invoice_no').val(),
        pcp_invoice_id: $('#pcp_rpc_id').val(),
        pcp_name: $('#pcp_name').val(),
       
        pcp_due: parseCurrency($('#pcp_due').val()),
        pcp_discount: $('#pcp_discount').val(),
       
        pcp_pay: $('#pcp_pay').val(),
        pcp_mode: $('#pcp_mode').val(),
        pcp_chq_no: $('#pcp_chq_no').val(),
      
        pcp_balance: parseCurrency($('#pcp_balance').val()),
        pcp_date: $('#pcp_date').val(),
        pcp_chq_date: $('#pcp_chq_date').val(),
        pcp_vendor_id: $('#pcp_vendor_id').val(),
        pcp_PaymentType: $('#pcp_PaymentType').val(),
        pcp_bank_name: $('#pcp_bank_name').val(),
        pcp_upi_id: $('#pcp_upi_id').val(),
    };
    $.ajax({
        type: 'POST',
        url: '/RollPurchase/CreatePayment', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {

           // window.location.href = "/RollPurchase/Index";
            //window.location.href = '/RollPurchase/Payment?rpc_invoice_no=' + rpc_invoice_no + '&rpc_v_name=' + rpc_v_name + '&rpc_balance=' + rpc_balance + '&rpc_v_id=' + rpc_v_id;

            window.location.href = '/RollPurchase/Payment?rpc_invoice_no=' + response.pcp_invoice_no + '&rpc_v_name=' + response.pcp_name + '&rpc_balance=' + response.pcp_balance + '&rpc_v_id=' + response.pcp_vendor_id + '&type=' + response.pcp_PaymentType + '&rpc_id=' + response.pcp_invoice_id;
        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}
function cancel() {
    window.location.href = "/RollPurchase/Index";
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
function printformat1(dataTable) {
    var printContents = $("#" + dataTable).html();
    var originalContents = $("body").html();

    $("body").html(printContents);

    window.print();

    $("body").html(originalContents);
}

function validateAdvance() {
    
    var advanceInput = $("#rpc_advance");
    var withGstLabel = $("#rpc_withgst");

   
    var advanceValue = parseFloat(advanceInput.val());
    var withGstValue = parseFloat(withGstLabel.text());

  
    if (advanceValue > withGstValue) {
        alert("Advance cannot be greater than total amount!");
      
        advanceInput.val("0");
    }

    
}

function validateDiscount() {

    var discountInput = $("#rpc_discount");
    var withGstLabel = $("#rpc_withgst");


    var discountValue = parseFloat(discountInput.val());
    var withGstValue = parseFloat(withGstLabel.text());


    if (discountValue > withGstValue) {
        alert("Discount cannot be greater than total amount!");

        discountInput.val("0");
    }


}

function checkDPamynetMax(inputElement, id) {
    var total = parseInt(inputElement.value) + parseFloat(parseCurrency($("#pcp_pay").val()) || 0);
    if (total >= parseFloat(parseCurrency($("#" + id).val()))) {
        inputElement.value = parseCurrency($("#" + id).val()) - parseFloat(parseCurrency($("#pcp_pay").val()) || 0); // Set the value to the maximum allowe
    }
}
function checkPaymentMax(inputElement, id) {
    var total = parseInt(inputElement.value) + parseFloat(parseCurrency($("#pcp_discount").val()) || 0);
    if (total >= parseFloat(parseCurrency($("#" + id).val()))) {
        inputElement.value = parseCurrency($("#" + id).val()) - parseFloat(parseCurrency($("#pcp_discount").val()) || 0); // Set the value to the maximum allowed
    }
}