
function addpurchase() {
    window.location.href = "/Purchase/EditIndex1";
}
function GoBack() {
    window.location.href = "/Purchase/Index1";

}

$(document).ready(function () {
    let currentDate = new Date();


    let formattedDate = currentDate.toISOString().slice(0, 10);


    $("#pc_invoice_date").val(formattedDate);


    currentDate.setDate(currentDate.getDate() + 10);


    let formattedDueDate = currentDate.toISOString().slice(0, 10);


    $("#pc_due_date").val(formattedDueDate);

    $("#pc_invoice_date").on('input', function () {

        let invoiceDate = new Date($(this).val());


        if (!isNaN(invoiceDate.getTime())) {
            let dueDate = new Date(invoiceDate.setDate(invoiceDate.getDate() + 10));


            let formattedDueDate = dueDate.toISOString().slice(0, 10);


            $("#pc_due_date").val(formattedDueDate);
        }
    });
    $("#pc_product_id").change(function () {
        // Get the selected value
        var p_id = $(this).val();

        // Call the GetValues function with the selected value
        GetValues(p_id);
    });

    $("#pc_v_id").change(function () {
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

function ShowModal() {
    if ($('#pc_v_id').val() == '') {
        alert("Please select vendor name!");
        return false;

    }
    else {
        $("#addSubscription").modal("show");
        $("#pc_product_id").val("");
        $("#pc_rate").val("0");
        $("#pc_desc").val("");
        $("#pc_total").val("0");
        $("#pc_complete_total").val("0");
        $("#pc_quantity").val("0");
        $("#pc_igst").val("0");
        $("#pc_sgst").val("0");
        $("#pc_ugst").val("0");
        $("#pc_cgst").val("0");
        $("#pc_igstp").val("0");
        $("#pc_sgstp").val("0");
        $("#pc_ugstp").val("0");
        $("#pc_cgstp").val("0");
        $("#pc_total_gst").val("0");
        $("#pc_gst").val("0");
        //$("#pc_advance").val("0");
        //$("#pc_discount").val("0");
        //$("#pc_shipping_charges").val("0");
    }

}
function quan_amount() {
    var rate = $("#pc_rate").val();
    var quantity = $("#pc_quantity").val();
    var total = $("#pc_total");
    var total_amount = (parseFloat(rate) * parseFloat(quantity))
    total.val(total_amount.toFixed(2));
    $("#pc_total").val(total_amount.toFixed(2));
    $("#pc_complete_total").val(total_amount.toFixed(2));
    gst();
}

function gst() {
    var cgst = parseFloat($("#pc_cgst").val()) || 0;
    var sgst = parseFloat($("#pc_sgst").val()) || 0;
    var igst = parseFloat($("#pc_igst").val()) || 0;
    var ugst = parseFloat($("#pc_ugst").val()) || 0;
    var total = parseFloat($("#pc_total").val()) || 0;
    // var total = $("#do_CompleteTotal").val() || 0;

    var total_amount = total;

    var rcgst = total_amount * cgst / 100;
    var rsgst = total_amount * sgst / 100;
    var rigst = total_amount * igst / 100;
    var rugst = total_amount * ugst / 100;
    $("#pc_igstp").val(rigst);
    $("#pc_sgstp").val(rsgst);
    $("#pc_ugstp").val(rugst);
    $("#pc_cgstp").val(rcgst);
    // var amt = total.value + rcgst.value + rsgst.value + rigst.value;
    var gst = parseFloat(rcgst) + parseFloat(rsgst) + parseFloat(rigst) + parseFloat(rugst);
    var amt = parseFloat(gst) + parseFloat(total_amount);
    $("#pc_gst").val(gst.toFixed(2));
    $("#pc_complete_total").val(amt.toFixed(2));
}

function final_total() {
    var subtotal = $("#pc_total_withoutgst").text();
    var totalgst = parseFloat($('#pc_total_gst').text()) || 0;
    var discount = $("#pc_discount").val() || 0;

    var advance = $("#pc_advance").val() || 0;
    var transport_charges = $("#pc_shipping_charges").val() || 0;
    var transport_value = parseFloat(transport_charges);


    var total_amount = (parseFloat(subtotal) + parseFloat(transport_value));
    var totalwithgst = (parseFloat(subtotal) + parseFloat(transport_value)) + (parseFloat(totalgst));


    if (advance == "0" && discount == "0") {
        var balance = (parseFloat(total_amount)) + (parseFloat(totalgst));
        var roundoff = Math.round(balance);
        $("#pc_balance").text(roundoff.toFixed(2));
        $("#pc_total_withgst").text(totalwithgst.toFixed(2));
    }
    else if (advance != "0" || discount != "0") {
        var balance = (parseFloat(total_amount) - parseFloat(advance) - parseFloat(discount)) + (parseFloat(totalgst));
        var roundoff = Math.round(balance);
        $("#pc_balance").text(roundoff.toFixed(2));
        $("#pc_total_withgst").text(totalwithgst.toFixed(2));
    }

}

function GetValues(pc_product_id) {
    var data = {
        p_id: pc_product_id
    }
    $.ajax({
        type: 'POST',
        url: '/Purchase/GetValues', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            if (response.productData.p_id != null) {
                $("#pc_rate").val(response.productData.p_rate);
                $("#pc_desc").val(response.productData.p_desc);
                $("#pc_hsn").val(response.productData.p_hsn_code);
                $("#pc_unit").val(response.productData.p_unit);
                $("#pc_igst").val(response.productData.P_igst ?? 0);
                $("#pc_ugst").val(response.productData.p_ugst ?? 0);
                $("#pc_sgst").val(response.productData.p_sgst ?? 0);
                $("#pc_cgst").val(response.productData.p_cgst ?? 0);
                $("#pc_igstp").val(0);
                $("#pc_sgstp").val(0);
                $("#pc_ugstp").val(0);
                $("#pc_cgstp").val(0);
                $("#pc_quantity").val(0);
                $("#pc_total").val(0);
                $("#pc_complete_total").val(0);
                $("#pc_gst").val(0);
            }
            //window.location.reload();
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}


function Vendor(pc_v_id) {
    var data = {
        pc_v_id: pc_v_id
    }
    $.ajax({
        type: 'POST',
        url: '/Purchase/Vendor', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#v_opening_balance").text(response.v_opening_balance);
            $("#pc_order_no").val(response.pc_order_no);
            $("#pc_advance").val("0");
            $("#pc_discount").val("0");
            $("#pc_shipping_charges").val("0");
        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}


var addedRowsData = [];
function submitgrid() {

    if ($('#pc_product_id').val() == '') {
        alert("Please select product!");
        return false;
    }
    else if ($('#pc_rate').val() == '' || $('#pc_rate').val() == 0) {
        alert("Please enter rate!");
        return false;
    }
    else if ($('#pc_quantity').val() == '0' || $('#pc_quantity').val() == '') {
        alert("Please enter quantity!");
        return false;
    }


    var productid = $('#pc_product_id option:selected').val();
    //var product = $("#pc_product_id option:selected").text();
    var product = $('#pc_product_id option:selected').text();

    var do_desc = $("#pc_desc").val();

    var rate = $("#pc_rate").val();

    var qty = $("#pc_quantity").val();
    var IGST = $("#pc_igst").val();
    var SGST = $("#pc_sgst").val();
    var UGST = $("#pc_ugst").val();
    var CGST = $("#pc_cgst").val();
    var IGSTP = $("#pc_igstp").val();
    var SGSTP = $("#pc_sgstp").val();
    var UGSTP = $("#pc_ugstp").val();
    var CGSTP = $("#pc_cgstp").val();
    var totalGST = $("#pc_gst").val();
    var totalGSTAMT = $("#pc_complete_total").val();
    var total = $("#pc_total").val();
    var hsn = $("#pc_hsn").val();
    var unit = $("#pc_unit").val();
    var $editingRow = $("#dataTable tbody tr.editing");

    // Remove the editing class and the row
    $editingRow.removeClass("editing");
    $editingRow.remove();
    // Create a new table row
    var newRow = "<tr>" +
        "<td style='display:none;'>" + productid + "</td>" +
       
        "<td>" + product + "</td>" +
        "<td>" + do_desc + "</td>" +
        "<td style='text-align:right'>" + rate + "</td>" +

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
        do_desc: do_desc,
        product: product,

        rate: parseFloat(rate),

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

    $("#pc_desc").val("");

    $("#pc_rate").val("");

    $("#pc_quantity").val("");
    $("#pc_igst").val("");
    $("#pc_sgst").val("");
    $("#pc_ugst").val("");
    $("#pc_cgst").val("");
    $("#pc_igstp").val("");
    $("#pc_sgstp").val("");
    $("#pc_ugstp").val("");
    $("#pc_cgstp").val("");
    $("#pc_total_gst").val("");
    $("#pc_gst").val("");
    $("#pc_total").val("");
    $("#pc_product_id").val("");
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




$(document).on('click', '#dataTable tbody .click-edit', function () {



    var $rowToEdit = $(this).closest('tr');
    var originalHTML = $rowToEdit.html();
    var pc_product_id = $rowToEdit.find('td:eq(0)').text();
    
    var pc_product_name = $rowToEdit.find('td:eq(1)').text();
    var pc_desc = $rowToEdit.find('td:eq(2)').text();
    var pc_rate = $rowToEdit.find('td:eq(3)').text();
    var pc_quantity = $rowToEdit.find('td:eq(4)').text();
    var pc_total = $rowToEdit.find('td:eq(5)').text();

    //var pc_igst = $rowToEdit.find('td:eq(6)').text();
    //var pc_sgst = $rowToEdit.find('td:eq(7)').text();
    //var pc_ugst = $rowToEdit.find('td:eq(8)').text();
    //var pc_cgst = $rowToEdit.find('td:eq(9)').text();

    //var pc_igstp = $rowToEdit.find('td:eq(10)').text();
    //var pc_sgstp = $rowToEdit.find('td:eq(11)').text();
    //var pc_ugstp = $rowToEdit.find('td:eq(12)').text();
    //var pc_cgstp = $rowToEdit.find('td:eq(13)').text();


    var pc_cgst = $rowToEdit.find('td:eq(6)').text();
    var pc_sgst = $rowToEdit.find('td:eq(7)').text();
    var pc_igst = $rowToEdit.find('td:eq(8)').text();
    var pc_ugst = $rowToEdit.find('td:eq(9)').text();
    var pc_cgstp = $rowToEdit.find('td:eq(10)').text();
    var pc_sgstp = $rowToEdit.find('td:eq(11)').text();
    var pc_igstp = $rowToEdit.find('td:eq(12)').text();
    var pc_ugstp = $rowToEdit.find('td:eq(13)').text();

    var pc_gst = $rowToEdit.find('td:eq(14)').text();
    var pc_complete_total = $rowToEdit.find('td:eq(15)').text();
    var pc_hsn = $rowToEdit.find('td:eq(16)').text();
    var pc_unit = $rowToEdit.find('td:eq(17)').text();
    $rowToEdit.addClass("editing");



    $("#pc_product_id").val(pc_product_id.trim().toLowerCase());

    // Populate the modal fields with rowData (you can adapt this to your modal structure)
    //$("#pc_product_id").val(pc_product_id);
    // $("#pc_product_id option:selected").text(pc_product_id);

    $("#pc_rate").val(pc_rate.trim());
    $("#pc_desc").val(pc_desc.trim());

    $("#pc_hsn").val(pc_hsn.trim());
    $("#pc_unit").val(pc_unit.trim());

    $("#pc_quantity").val(pc_quantity.trim());
    $("#pc_igst").val(pc_igst.trim());
    $("#pc_sgst").val(pc_sgst.trim());
    $("#pc_ugst").val(pc_ugst.trim());
    $("#pc_cgst").val(pc_cgst.trim());
    $("#pc_igstp").val(pc_igstp.trim());
    $("#pc_sgstp").val(pc_sgstp.trim());
    $("#pc_ugstp").val(pc_ugstp.trim());
    $("#pc_cgstp").val(pc_cgstp.trim());
    $("#pc_gst").val(pc_gst.trim());
    $("#pc_complete_total").val(pc_complete_total.trim());
    $("#pc_total").val(pc_total.trim());
    $("#addSubscription").modal("show");
    // $rowToEdit.remove();

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
        totalRate += parseFloat(row.find("td:eq(3)").text()) || 0;
        totalQuantity += parseFloat(row.find("td:eq(4)").text()) || 0;
        totalGSTC += parseFloat(row.find("td:eq(10)").text()) || 0;
        totalGSTS += parseFloat(row.find("td:eq(11)").text()) || 0;
        totalGSTI += parseFloat(row.find("td:eq(12)").text()) || 0;
        totalGSTU += parseFloat(row.find("td:eq(13)").text()) || 0;
        totalGST += parseFloat(row.find("td:eq(14)").text()) || 0;
        totalAmount += parseFloat(row.find("td:eq(15)").text()) || 0;
        totalAmountwithoutgst += parseFloat(row.find("td:eq(5)").text()) || 0;

    });
    // Update the hidden inputs with the calculated totals
    $("#pc_rate").val(totalRate.toFixed(2));
    $("#pc_total_quantity").text(totalQuantity);
    $("#pc_total").val(totalAmount.toFixed(2));
    $("#pc_total_withoutgst").text(totalAmountwithoutgst.toFixed(2));
    $("#pc_total_igst").text(totalGSTI);
    $("#pc_total_sgst").text(totalGSTS);
    $("#pc_total_cgst").text(totalGSTC);
    $("#pc_total_ugst").text(totalGSTU);
    $("#pc_total_gst").text(totalGST);
    $("#pc_total_withgst").text(totalAmount.toFixed(2));
    $("#pc_balance").text(totalAmount.toFixed(2));
    $("#pc_withgst").text(totalAmount.toFixed(2));

}


function submitPurchase(type) {
    /* event.preventDefault();*/

    if ($('#pc_v_id').val() == '') {
        alert("Please select vendor name!");
        return false;
    } else
        var rowCount = $('#dataTable tbody tr').length - 1;
    if (rowCount == 0) {
        alert("Please select product!");
        return false;
    }

    var orderDataArray = [];

    // Iterate over each row in the table 
    $("#dataTable tbody tr:gt(0)").each(function () {
        var rowData = {
            pc_product_id: $(this).find('td:eq(0)').text(),
          
            pc_product_name: $(this).find('td:eq(1)').text(),
            pc_desc: $(this).find('td:eq(2)').text(),
            pc_rate: $(this).find('td:eq(3)').text(),

            pc_quantity: $(this).find('td:eq(4)').text(),
            pc_total: $(this).find('td:eq(5)').text(),
            //pc_igst: $(this).find('td:eq(6)').text(),
            //pc_sgst: $(this).find('td:eq(7)').text(),
            //pc_ugst: $(this).find('td:eq(8)').text(),
            //pc_cgst: $(this).find('td:eq(9)').text(),
            //pc_igstp: $(this).find('td:eq(10)').text(),
            //pc_sgstp: $(this).find('td:eq(11)').text(),
            //pc_ugstp: $(this).find('td:eq(12)').text(),
            //pc_cgstp: $(this).find('td:eq(13)').text(),
            //pc_gst: $(this).find('td:eq(14)').text(),

            pc_cgst: $(this).find('td:eq(6)').text(),
            pc_sgst: $(this).find('td:eq(7)').text(),
            pc_igst: $(this).find('td:eq(8)').text(),
            pc_ugst: $(this).find('td:eq(9)').text(),
            pc_cgstp: $(this).find('td:eq(10)').text(),
            pc_sgstp: $(this).find('td:eq(11)').text(),
            pc_igstp: $(this).find('td:eq(12)').text(),
            pc_ugstp: $(this).find('td:eq(13)').text(),
            pc_gst: $(this).find('td:eq(14)').text(),

            pc_complete_total: $(this).find('td:eq(15)').text(),
            pc_hsn: $(this).find('td:eq(16)').text(),
            pc_unit: $(this).find('td:eq(17)').text(),
            pc_com_id: $('#comIdLabel').val(),

        };
        orderDataArray.push(rowData);

    });
    var data = {
        //  pc_name: $('#pc_name').val(),


        pc_shipping_charges: $('#pc_shipping_charges').val() ?? 0,
        pc_discount: $('#pc_discount').val() ?? 0,
        pc_advance: $('#pc_advance').val() ?? 0,

        pc_total_withoutgst: $('#pc_total_withoutgst').text(),
        pc_total_igst: $('#pc_total_igst').text(),
        pc_total_sgst: $('#pc_total_sgst').text(),
        pc_total_cgst: $('#pc_total_cgst').text(),
        pc_total_ugst: $('#pc_total_ugst').text(),
        pc_total_gst: $('#pc_total_gst').text(),
        pc_total_quantity: $('#pc_total_quantity').text(),
        pc_total_withgst: $('#pc_total_withgst').text(),
        pc_balance: $('#pc_balance').text(),
        pc_withgst: $('#pc_withgst').text(),
        pc_complete_total: $('#pc_complete_total').val(),
        pc_invoice_no: $('#pc_invoice_no').val(),
        pc_invoice_date: $('#pc_invoice_date').val(),
        pc_due_date: $('#pc_due_date').val(),
        pc_order_no: $('#pc_order_no').val(),
        pc_v_name: $('#pc_v_id option:selected').text(),
        pc_v_id: $('#pc_v_id option:selected').val(),
        pc_product_name: $('#pc_product_id option:selected').text(),
        pc_product_id: $('#pc_product_id option:selected').val(),
        pc_id: $('#pc_id').val(),


        PurchaseInvoice: orderDataArray
    };
    $.ajax({
        type: 'POST',
        url: '/Purchase/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        //success: function (response) {

        //    window.location.href = "/Purchase/EditIndex";
        //},
        success: function (response) {
            if (type == 'Close') {
                window.location.href = "/Purchase/Index";
            }
            else {
                window.location.href = "/Purchase/EditIndex";
            }
        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}

function Excel() {
    window.location.href = '/Purchase/Excel';
}

function Pdf() {
    window.location.href = '/Purchase/Pdf';
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

function payment1() {
    var pc_id = $('#pc_id').text();
    var pc_invoice_no = $('#pcNoValue').text();
    var pc_v_name = $('#pc_v_name').text();
    var pc_v_id = $('#pc_v_id').text();
    //var pc_balance = $('#pc_balance').text();
    var paymentmethod = $('#rpc_payment_method').text();
    var pc_balance = parseCurrency($('#pc_balance').text());
    // var do_payment_method = $('#do_payment_method').text();
    if (pc_balance == "0" || pc_balance == "0.00" || pc_balance == "00.00") {
        alert("Payment is already done");
        return false;
    }
    else {
        window.location.href = '/Purchase/Payment?pc_invoice_no=' + pc_invoice_no + '&pc_v_name=' + pc_v_name + '&pc_balance=' + pc_balance + '&pc_v_id=' + pc_v_id + "&pc_id=" + pc_id + "&type=" + paymentmethod;
        //  window.location.href = '/Purchase/Payment?pc_invoice_no=' + pc_invoice_no + '&pc_v_name=' + pc_v_name + '&pc_balance=' + pc_balance + '&pc_v_id=' + pc_v_id;
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
        pcp_invoice_id: $('#pcp_pc_id').val(),
        pcp_PaymentType: $('#pcp_PaymentType').val(),
        pcp_bank_name: $('#pcp_bank_name').val()
    };
    $.ajax({
        type: 'POST',
        url: '/Purchase/CreatePayment', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            // window.location.href = '/RollPurchase/Payment?rpc_invoice_no=' + response.pcp_invoice_no + '&rpc_v_name=' + response.pcp_name + '&rpc_balance=' + response.pcp_balance + '&rpc_v_id=' + response.pcp_vendor_id + '&type=' + response.pcp_PaymentType + '&rpc_id=' + response.pcp_invoice_id;
            // window.location.href = '/Purchase/Payment?pc_invoice_no=' + pc_invoice_no + '&pc_v_name=' + pc_v_name + '&pc_balance=' + pc_balance + '&pc_v_id=' + pc_v_id;
            window.location.href = '/Purchase/Payment1?pc_invoice_no=' + response.pcp_invoice_no + '&pc_v_name=' + response.pcp_name + '&pc_balance=' + response.pcp_balance + '&pc_v_id=' + response.pcp_vendor_id + '&type=' + response.pcp_PaymentType + '&pc_id=' + response.pcp_invoice_id;
        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}
function CancelData() {


    $("#pc_product_id").val("");
    $("#pc_rate").val("0");
    $("#pc_desc").val("");
    $("#pc_total").val("0");
    $("#pc_complete_total").val("0");
    $("#pc_quantity").val("0");
    $("#pc_igst").val("0");
    $("#pc_sgst").val("0");
    $("#pc_ugst").val("0");
    $("#pc_cgst").val("0");
    $("#pc_igstp").val("0");
    $("#pc_sgstp").val("0");
    $("#pc_ugstp").val("0");
    $("#pc_cgstp").val("0");
    $("#pc_total_gst").val("0");
    $("#pc_gst").val("0");


}
function cancel() {
    window.location.href = "/Purchase/Index";
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

    var advanceInput = $("#pc_advance");
    var withGstLabel = $("#pc_withgst");


    var advanceValue = parseFloat(advanceInput.val());
    var withGstValue = parseFloat(withGstLabel.text());


    if (advanceValue > withGstValue) {
        alert("Advance cannot be greater than total amount!");

        advanceInput.val("0");
    }


}

function validateDiscount() {

    var discountInput = $("#pc_discount");
    var withGstLabel = $("#pc_withgst");


    var discountValue = parseFloat(discountInput.val());
    var withGstValue = parseFloat(withGstLabel.text());


    if (discountValue > withGstValue) {
        alert("Discount cannot be greater than total amount!");

        discountInput.val("0");
    }


}


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
    var currentDateInput = $("#pc_invoice_date");

    var currentDateValue = currentDateInput.val();


    var currentDate = new Date(currentDateValue);

    if (enteredDate < currentDate) {
        alert("Due date cannot be a previous date!");
        var invoiceDate = new Date($("#pc_invoice_date").val());
        var dueDate = new Date(invoiceDate.setDate(invoiceDate.getDate() + 10));
        var formattedDueDate = dueDate.toISOString().slice(0, 10);


        $("#pc_due_date").val(formattedDueDate);
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