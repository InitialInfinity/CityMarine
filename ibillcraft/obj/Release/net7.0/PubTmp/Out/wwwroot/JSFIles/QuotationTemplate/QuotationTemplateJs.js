function addorder() {
    window.location.href = "/QuotationTemplate/EditIndex";
} 
function parseCurrency(currencyText) {
    var locale = $('#cm_currency_format').val();
    //var formattedChar = new Intl.NumberFormat(locale).format(1111.1).charAt(1);

    // Replace symbols and separators specific to the given currency format
    //var cleanedValue = parseFloat(currencyText.replace(new RegExp('[^\d' + formattedChar + ']', 'g'), '').replace(formattedChar, '.'));

    //return isNaN(cleanedValue) ? null : cleanedValue;
    //var cleanedValue = parseFloat(currencyText.replace(/[^\d.,]|,(?=[^,]*$)|\.(?=[^.]*$)/g, '').replace(',', '.'));
    var cleanedValue = currencyText.replace(/[^\d.,]/g, '');

    // Replace comma with dot as the decimal separator
    cleanedValue = cleanedValue.replace(',', '');

    // Parse the cleaned string as a float
    var parsedValue = parseFloat(cleanedValue);
    //var cleanedValue = parseFloat(normalizedValue);
    return isNaN(parsedValue) ? null : parsedValue;
}
function formatAsCurrency(digits) {
    var locale = $('#cm_currency_format').val();
    var usCurrency = new Intl.NumberFormat(locale);
    var usFormatted = usCurrency.format(digits);
    var seFormatted = digits.toLocaleString(locale, { style: 'currency', currency: 'GBP' });
    return seFormatted;
}

function addproforma() {
    window.location.href = '@Url.Action("EditQuot", "Quotation")';
}

function submitQuotation(type) {
    /* event.preventDefault();*/
    var rowCount = $('#dataTable tbody tr').length - 1;
    if ($('#qu_name').val() == '') {
        alert("Please enter template name!");
        return false;
    } else if (rowCount == 0) {
        alert("Please add product details!");
            return false;
        }
    var orderDataArray = [];

    // Iterate over each row in the table 
    $("#dataTable tbody tr:gt(0)").each(function () {
        var rowData = {
            qi_product_id: $(this).find('td:eq(0)').text(),
           
            qi_productname: $(this).find('td:eq(1)').text(),
            qi_desc: $(this).find('td:eq(2)').text(),
            qi_product_hsn: $(this).find('td:eq(3)').text(),
            qi_height: $(this).find('td:eq(4)').text(),
            qi_width: $(this).find('td:eq(5)').text(),
            qi_size: $(this).find('td:eq(6)').text(),
            qi_rate: $(this).find('td:eq(7)').text(),
            qi_amount: $(this).find('td:eq(8)').text(),
            qi_quantity: $(this).find('td:eq(9)').text(),
            qi_total: $(this).find('td:eq(10)').text(),
            qi_cgst: $(this).find('td:eq(11)').text(),       
            qi_sgst: $(this).find('td:eq(12)').text(),
            qi_igst: $(this).find('td:eq(13)').text(),
            qi_ugst: $(this).find('td:eq(14)').text(),           
            qi_igsta: $(this).find('td:eq(15)').text(),
            qi_sgsta: $(this).find('td:eq(16)').text(),
            qi_ugsta: $(this).find('td:eq(17)').text(),
            qi_cgsta: $(this).find('td:eq(18)').text(),
            qi_gst: $(this).find('td:eq(19)').text(),
            qi_complete_total: $(this).find('td:eq(20)').text(),

            qi_unit: $(this).find('td:eq(21)').text()

        };
        orderDataArray.push(rowData);

    });
    var data = {
        qut_id: $('#qut_id').val(),
        qu_name: $('#qu_name').val(),
        qu_dtp_charges: $('#qu_dtp_charges').val(),
        qu_pasting_charges: $('#qu_pasting_charges').val(),
        qu_framing_charges: $('#qu_framing_charges').val(),
        qu_fitting_charges: $('#qu_fitting_charges').val(),
        qu_installation_charges: $('#qu_installation_charges').val(),
        qu_shipping_charges: $('#qu_shipping_charges').val(),
        qu_discount: $('#qu_discount').val(),
        qu_total_withoutgst: $('#qu_total_withoutgst').val(),
        qu_total_igst: $('#qu_total_igst').val(),
        qu_total_sgst: $('#qu_total_sgst').val(),
        qu_total_cgst: $('#qu_total_cgst').val(),
        qu_total_ugst: $('#qu_total_ugst').val(),
        qu_total_gst: $('#qu_total_gst').val(),
        qu_total_quantity: $('#qu_total_quantity').val(),
        qu_total_withgst: $('#qu_total_withgst').val(),
        qu_withgst: $('#qu_withgst').val(),
        qu_invoice_no: $('#qu_invoice_no').val(),
        qu_order_no: $('#qu_order_no').val(),
        qu_date: $('#qu_date').val(),
        qu_payment_method: $('#qu_payment_method').val(),
        qu_bank_name: $('#qu_bank_name').val(),
        qu_chq_no: $('#qu_chq_no').val(),
        qu_chq_date: $('#qu_chq_date').val(),
        qu_payment_status: $('#qu_payment_status').val(),
        qu_isactive: "1",
        qu_complete_total: $('#qu_complete_total').val(),
        quotationTemplatedetails: orderDataArray
    };
    $.ajax({
        type: 'POST',
        url: '/QuotationTemplate/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            if (type == 'Close') {
                window.location.href = "/QuotationTemplate/Index";
            }
            else {
                window.location.href = "/QuotationTemplate/EditIndex";
            }
        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}

function CancelData() {
    $("#addSubscription").modal("hide");
    $("#qu_height").val("");
    $("#qu_desc").val("");
    $("#qu_product_hsn").val("");
    $("#qu_unit").val("");
    $("#qu_width").val("");
    $("#qu_size").val("");
    $("#qu_rate").val("");
    $("#qu_amount").val("");
    $("#qu_quantity").val("");
    $("#qu_IGST").val("");
    $("#qu_SGST").val("");
    $("#qu_UGST").val("");
    $("#qu_CGST").val("");
    $("#qu_IGSTA").val("");
    $("#qu_SGSTA").val("");
    $("#qu_UGSTA").val("");
    $("#qu_CGSTA").val("");
    $("#qu_totalGST").val("");
    $("#qu_gst").val("");
    $("#qu_total").val("");
    $("#qu_product_id").val("");
    $("#qu_complete_total").val("");
}
$(document).ready(function () {

    let currentDate = new Date();


    let formattedDate = currentDate.toISOString().slice(0, 10);
    $("#qu_date").val(formattedDate);
    $("#qu_product_id").change(function () {
        // Get the selected value
        var selectedProductId = $(this).val();
        var customer = $('#qu_c_id').val();

        // Call the GetValues function with the selected value
        GetValues(selectedProductId, customer);
    });
    if ($('#qu_dtp_charges').val() != '0' && $('#qu_dtp_charges').val() != '0.00') {
        $("#qu_dtp_charges").prop("readonly", false);
        $("#flexCheckDefault1").prop("checked", true);
    }
    if ($('#qu_pasting_charges').val() != '0' && $('#qu_pasting_charges').val() != '0.00') {
        $("#qu_pasting_charges").prop("readonly", false);
        $("#flexCheckDefault2").prop("checked", true);
    }
    if ($('#qu_framing_charges').val() != '0' && $('#qu_framing_charges').val() != '0.00') {
        $("#qu_framing_charges").prop("readonly", false);
        $("#flexCheckDefault3").prop("checked", true);
    }
    if ($('#qu_fitting_charges').val() != '0' && $('#qu_fitting_charges').val() != '0.00') {
        $("#qu_fitting_charges").prop("readonly", false);
        $("#flexCheckDefault4").prop("checked", true);
    }
    if ($('#qu_installation_charges').val() != '0' && $('#qu_installation_charges').val() != '0.00') {
        $("#qu_installation_charges").prop("readonly", false);
        $("#flexCheckDefault5").prop("checked", true);
    }
    if ($('#qu_shipping_charges').val() != '0' && $('#qu_shipping_charges').val() != '0.00') {
        $("#qu_shipping_charges").prop("readonly", false);
        $("#flexCheckDefault6").prop("checked", true);
    }


    if ($('#qut_id').val() !== '') {
        $('#addbutton').css('display', 'none');
        $('#addbutton1').css('display', 'none');
        $('#editbutton').css('display', 'block');
    }

    $('#addSubscription').modal({
        backdrop: 'static', // Prevents closing on outside click
        keyboard: false // Prevents closing with the escape key
    });

});
function GetValues(qu_product_id, qu_c_id) {
    qu_c_id = $('#qu_c_id').val();
    var data = {
        p_id: qu_product_id,

        r_cust_id: qu_c_id
    }
    $.ajax({
        type: 'POST',
        url: '/Proforma/GetValues', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            if (response.productData.p_id != null) {
                $("#qu_rate").val(response.productData.p_rate);
                $("#qu_desc").val(response.productData.p_desc);
                $("#qu_product_hsn").val(response.productData.p_hsn_code);
                $("#qu_unit").val(response.productData.p_unit);
                $("#qu_IGST").val(response.productData.P_igst ?? 0);
                $("#qu_UGST").val(response.productData.p_ugst ?? 0);
                $("#qu_SGST").val(response.productData.p_sgst ?? 0);
                $("#qu_CGST").val(response.productData.p_cgst ?? 0);
                $("#qu_size").val(0);
                $("#qu_width").val(0);
                $("#qu_height").val(0);
                $("#qu_amount").val(0);
                $("#qu_quantity").val(0);
                $("#qu_total").val(0);
                $("#qu_gst").val(0);
                $("#qu_complete_total").val(0);

                if (response.unitData.u_amount == '1') {
                    $("#qu_amount").prop("readonly", false);
                } else {
                    $("#qu_amount").prop("readonly", true);
                }

                response.unitData.u_height == '1' ? $("#qu_height").prop("readonly", false) : $("#qu_height").attr("readonly", true);
                response.unitData.u_size == '1' ? $("#qu_size").prop("readonly", false) : $("#qu_size").attr("readonly", true);
                response.unitData.u_width == '1' ? $("#qu_width").prop("readonly", false) : $("#qu_width").attr("readonly", true);
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
    var productid = $("#qu_product_id").val();
    var product = $("#qu_product_id option:selected").text();
    var qu_product_hsn = $("#qu_product_hsn").val();
    var qu_unit = $("#qu_unit").val();
    var qu_desc = $("#qu_desc").val();
    var qu_width = $("#qu_width").val();
    var qu_height = $("#qu_height").val();
    var sqrft = $("#qu_size").val();
    var rate = $("#qu_rate").val();
    var amount = $("#qu_amount").val();
    var qty = $("#qu_quantity").val();
    var IGST = $("#qu_IGST").val();
    var SGST = $("#qu_SGST").val();
    var UGST = $("#qu_UGST").val();
    var CGST = $("#qu_CGST").val();
    var IGSTA = $("#qu_IGSTA").val();
    var SGSTA = $("#qu_SGSTA").val();
    var UGSTA = $("#qu_UGSTA").val();
    var CGSTA = $("#qu_CGSTA").val();
    var totalGST = $("#qu_gst").val();
    var totalGSTAMT = $("#qu_complete_total").val();
    var total = $("#qu_total").val();
    var $editingRow = $("#dataTable tbody tr.editing");

    if ($('#qu_unit').val() == 'Meter' || $('#qu_unit').val() == 'Sqft' || $('#qu_unit').val() == 'Inch') {
        if ($('#qu_width').val() == '' || $('#qu_width').val() == 0) {
            alert("Please enter length!");
            return false;
        } else if ($('#qu_height').val() == '' || $('#qu_height').val() == 0) {
            alert("Please enter height!");
            return false;
        } else if ($('#qu_quantity').val() == '' || $('#qu_quantity').val() == 0) {
            alert("Please enter quantity!");
            return false;
        }
    }
    else {
        if ($('#qu_rate').val() == '' || $('#qu_rate').val() == 0) {
            alert("Please enter rate!");
            return false;
        } else  if ($('#qu_quantity').val() == '' || $('#qu_quantity').val() == 0) {
            alert("Please enter quantity!");
            return false;
        }
    }

    // Remove the editing class and the row
    $editingRow.removeClass("editing");
    $editingRow.remove();
    // Create a new table row
    var newRow = "<tr>" +
        "<td style='display:none;'>" + productid + "</td>" +
        "<td>" + product + "</td>" +
        "<td>" + qu_desc + "</td>" +
        
        "<td style='text-align:right'>" + qu_product_hsn + "</td>" +
        "<td style='display:none;'>" + qu_height + "</td>" +
        "<td style='display:none;'>" + qu_width + "</td>" +
        "<td style='text-align:right'>" + sqrft + "</td>" +
        "<td style='text-align:right'>" + rate + "</td>" +
        "<td style='text-align:right'>" + amount + "</td>" +
        "<td style='text-align:right'>" + qty + "</td>" +
        "<td style='text-align:right'>" + total + "</td>" +
        "<td style='text-align:right'>" + CGST + "</td>" +
        "<td style='text-align:right'>" + SGST + "</td>" +
        "<td style='text-align:right'>" + IGST + "</td>" +
        "<td style='display:none;'>" + UGST + "</td>" +
        "<td style='display:none;'>" + IGSTA + "</td>" +
        "<td style='display:none;'>" + SGSTA + "</td>" +
        "<td style='display:none;'>" + UGSTA + "</td>" +
        "<td style='display:none;'>" + CGSTA + "</td>" +
        "<td style='display:none;'>" + totalGST + "</td>" +
        "<td style='text-align:right'>" + totalGSTAMT + "</td>" +

        "<td style='display:none;'>" + qu_unit + "</td>" +
        "<td class='text-center'>" +
        "<div class='d-flex'>" +
        "<i class='fas fa-edit click-edit'></i><i class='fa fa-trash ml-2 text-danger click'></i>" +
        "</div>" +
        "</td>" +
        "</tr>";

    // Append the new row to the table body
    $("#dataTable tbody").append(newRow);
    var rowData = {
        productid: productid,
        qu_desc: qu_desc,
        product: product,
        qu_product_hsn: qu_product_hsn,
        qu_height: qu_height,
        qu_width: qu_width,
        sqrft: sqrft,
        rate: parseFloat(rate),
        amount: parseFloat(amount),
        qty: parseFloat(qty),
        total: parseFloat(total),
        IGST: parseFloat(IGST),
        SGST: parseFloat(SGST),
        UGST: parseFloat(UGST),
        CGST: parseFloat(CGST),
        IGSTA: parseFloat(IGSTA),
        SGSTA: parseFloat(SGSTA),
        UGSTA: parseFloat(UGSTA),
        CGSTA: parseFloat(CGSTA),
        totalGST: parseFloat(totalGST),
        totalGSTAMT: parseFloat(totalGSTAMT),
        qu_unit: qu_unit
    };

    addedRowsData.push(rowData);
    // Close the modal
    calculateTotals();
    final_total();
    $("#addSubscription").modal("hide");
    $("#qu_height").val("");
    $("#qu_desc").val("");
    $("#qu_product_hsn").val("");
    $("#qu_unit").val("");
    $("#qu_width").val("");
    $("#qu_size").val("");
    $("#qu_rate").val("");
    $("#qu_amount").val("");
    $("#qu_quantity").val("");
    $("#qu_IGST").val("");
    $("#qu_SGST").val("");
    $("#qu_UGST").val("");
    $("#qu_CGST").val("");
    $("#qu_IGSTA").val("");
    $("#qu_SGSTA").val("");
    $("#qu_UGSTA").val("");
    $("#qu_CGSTA").val("");
    $("#qu_totalGST").val("");
    $("#qu_gst").val("");
    $("#qu_total").val("");
    $("#qu_product_id").val("");
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

function assignzero() {

    $("#addSubscription").modal("show");
    $("#qu_IGST").val("0");
    $("#qu_SGST").val("0");
    $("#qu_UGST").val("0");
    $("#qu_CGST").val("0");
    $("#qu_IGSTA").val("0");
    $("#qu_SGSTA").val("0");
    $("#qu_UGSTA").val("0");
    $("#qu_CGSTA").val("0");
    $("#qu_totalGST").val("0");
    $("#qu_gst").val("0");
 
}

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
        totalRate += parseFloat(row.find("td:eq(7)").text()) || 0;
        totalQuantity += parseFloat(row.find("td:eq(9)").text()) || 0;
        totalGSTI += parseFloat(row.find("td:eq(15)").text()) || 0;
        totalGSTS += parseFloat(row.find("td:eq(16)").text()) || 0;
        totalGSTU += parseFloat(row.find("td:eq(17)").text()) || 0;
        totalGSTC += parseFloat(row.find("td:eq(18)").text()) || 0;
        totalGST += parseFloat(row.find("td:eq(19)").text()) || 0;
        totalAmount += parseFloat(row.find("td:eq(20)").text()) || 0;
    totalAmountwithoutgst += parseFloat(row.find("td:eq(10)").text()) || 0;
    });
    // Update the hidden inputs with the calculated totals
    $("#qu_rate2").val(totalRate.toFixed(2));
    $("#qu_total_quantity").val(totalQuantity);
    $("#qu_total2").val(totalAmount.toFixed(2));
    $("#qu_total_withoutgst").val(parseFloat(totalAmountwithoutgst));
    $("#qu_total_igst").val(totalGSTI);
    $("#qu_total_sgst").val(totalGSTS);
    $("#qu_total_cgst").val(totalGSTC);
    $("#qu_total_ugst").val(totalGSTU);
    $("#qu_total_gst").val(parseFloat(totalGST));
    $("#qu_total_withgst").val(parseFloat(totalAmount.toFixed(2)));
    $("#qu_withgst").val(parseFloat(totalAmount));
    $("#qu_balance").text(parseFloat(totalAmount));
}
function toggleReadOnly() {
    //$("#flexCheckDefault1").prop("checked") ? $("#qu_dtp_charges").prop("readonly", false) : $("#qu_dtp_charges").prop("readonly", true);
    //$("#flexCheckDefault2").prop("checked") ? $("#qu_pasting_charges").prop("readonly", false) : $("#qu_pasting_charges").prop("readonly", true);
    //$("#flexCheckDefault3").prop("checked") ? $("#qu_framing_charges").prop("readonly", false) : $("#qu_framing_charges").prop("readonly", true);
    //$("#flexCheckDefault4").prop("checked") ? $("#qu_fitting_charges").prop("readonly", false) : $("#qu_fitting_charges").prop("readonly", true);
    //$("#flexCheckDefault5").prop("checked") ? $("#qu_installation_charges").prop("readonly", false) : $("#qu_installation_charges").prop("readonly", true);
    //$("#flexCheckDefault6").prop("checked") ? $("#qu_shipping_charges").prop("readonly", false) : $("#qu_shipping_charges").prop("readonly", true);
    if ($("#flexCheckDefault1").prop("checked")) {
        $("#qu_dtp_charges").prop("readonly", false);
    } else {
        $("#qu_dtp_charges").prop("readonly", true).val("0");
    }
    if ($("#flexCheckDefault2").prop("checked")) {
        $("#qu_pasting_charges").prop("readonly", false);
    } else {
        $("#qu_pasting_charges").prop("readonly", true).val("0");
    }
    if ($("#flexCheckDefault3").prop("checked")) {
        $("#qu_framing_charges").prop("readonly", false);
    } else {
        $("#qu_framing_charges").prop("readonly", true).val("0");
    }
    if ($("#flexCheckDefault4").prop("checked")) {
        $("#qu_fitting_charges").prop("readonly", false);
    } else {
        $("#qu_fitting_charges").prop("readonly", true).val("0");
    }
    if ($("#flexCheckDefault5").prop("checked")) {
        $("#qu_installation_charges").prop("readonly", false);
    } else {
        $("#qu_installation_charges").prop("readonly", true).val("0");
    }
    if ($("#flexCheckDefault6").prop("checked")) {
        $("#qu_shipping_charges").prop("readonly", false);
    } else {
        $("#qu_shipping_charges").prop("readonly", true).val("0");
    }

    final_total();
}
function sqrft() {
    var first = $("#qu_height").val();
    var second = $("#qu_width").val();
    var sqrft = $("#qu_size");
    var sqrft_total = (parseFloat(first) * parseFloat(second));
    sqrft.val(sqrft_total.toFixed(2));
}
function rate() {
    var rates = parseCurrency($("#qu_rate").val());
    var sqrft = $("#qu_size").val();
    var amount = $("#qu_amount");
    var rate_amount = (parseFloat(sqrft) * parseFloat(rates));
    amount.val(rate_amount.toFixed(2));
}
function quan_amount() {
    var amt = $("#qu_amount").val() == '0' ? $("#qu_rate").val() : $("#qu_amount").val();
    var quantity = $("#qu_quantity").val();
    var total = $("#qu_total");
    var total_amount = (parseFloat(amt) * parseFloat(quantity))
    total.val(total_amount.toFixed(2));
    $("#qu_complete_total").val(total_amount.toFixed(2));
}
function gst() {
    var cgst = parseFloat($("#qu_CGST").val()) || 0;
    var sgst = parseFloat($("#qu_SGST").val()) || 0;
    var igst = parseFloat($("#qu_IGST").val()) || 0;
    var ugst = parseFloat($("#qu_UGST").val()) || 0;
    var total = parseFloat($("#qu_total").val()) || 0;
    // var total = $("#qu_complete_total ").val() || 0;

    var total_amount = total;

    var rcgst = total_amount * cgst / 100;
    var rsgst = total_amount * sgst / 100;
    var rigst = total_amount * igst / 100;
    var rugst = total_amount * ugst / 100;
    $("#qu_IGSTA").val(parseFloat(rigst));
    $("#qu_SGSTA").val(parseFloat(rsgst));
    $("#qu_UGSTA").val(parseFloat(rugst));
    $("#qu_CGSTA").val(parseFloat(rcgst));
    // var amt = total.value + rcgst.value + rsgst.value + rigst.value;
    var gst = parseFloat(rcgst) + parseFloat(rsgst) + parseFloat(rigst) + parseFloat(rugst);
    var amt = parseFloat(gst) + parseFloat(total_amount);
    $("#qu_gst").val(gst.toFixed(2));
    $("#qu_complete_total").val(amt.toFixed(2));
}
function quan_amount2() {
    var rate = $("#qu_rate2").val();
    var quantity = $("#qu_quantity2").val();
    var total = $("#qu_total2").val();
    var total_amount = (parseFloat(rate.value) * parseFloat(quantity.value))
    total.value = total_amount.toFixed(2);
}
function final_total() {
    var subtotal = $("#qu_total_withoutgst").val();
    var totalgst = parseFloat($('#qu_total_gst').val()) || 0;
    var discount = $("#qu_discount").val() || 0;
    var dtpcharges = $("#qu_dtp_charges").val() || 0;
    var dtp_value = parseFloat(dtpcharges);
    var advance = $("#qu_advance").val() || 0;
    var transport_charges = $("#qu_shipping_charges").val() || 0;
    var transport_value = parseFloat(transport_charges);
    var qu_installation_charges = $("#qu_installation_charges").val() || 0;
    var instvalue = parseFloat(qu_installation_charges);
    var fitting = $("#qu_fitting_charges").val() || 0;
    var fitting_value = parseFloat(fitting);
    var qu_framing_charges = $("#qu_framing_charges").val() || 0;
    var framing_value = parseFloat(qu_framing_charges);
    var pasting = $("#qu_pasting_charges").val() || 0;
    var pasting_value = parseFloat(pasting);
    var total_dtp_transport = (parseFloat(transport_value) + parseFloat(dtp_value)) + parseFloat(framing_value) + parseFloat(fitting_value) + parseFloat(instvalue) + parseFloat(pasting_value);
    var total_amount = (parseFloat(subtotal) + parseFloat(total_dtp_transport));
    var totalwithgst = (parseFloat(subtotal) + parseFloat(total_dtp_transport)) + totalgst;
    if (advance == "0" && discount == "0") {
        $('#qu_advance').text('0');
        $('#qu_discount').text('0');
        var balance = (parseFloat(total_amount)) + totalgst;
        var roundoff = Math.round(balance);
        $("#qu_balance").text(roundoff.toFixed(2));
        $("#qu_total_withgst").val(totalwithgst.toFixed(2));
        $("#qu_payment_status").val('UnPaid');
    }
    else if (advance != "0" || discount != "0") {
        var balance = (parseFloat(total_amount) - parseFloat(advance) - parseFloat(discount)) + totalgst;
        var roundoff = Math.round(balance);
        $("#qu_balance").text(roundoff.toFixed(2));
        $("#qu_total_withgst").text(totalwithgst.toFixed(2));
        if (balance == '0') {
            $("#qu_payment_status").val('Paid');
        }
        else if (advance == '0') {
            $("#qu_payment_status").val('UnPaid');
        }
        else {
            $("#qu_payment_status").val('Partially Paid');
        }
    }

}

function cheque() {
    var payment = $('#qu_payment_method').val();
    if (payment == "Cheque") {
        $('#bankname').css("display", "block");
        $('#chqno').css("display", "block");
        $('#chqdate').css("display", "block");
    }
}
function Excel() {
    window.location.href = '/QuotationTemplate/Excel';
}

function Pdf() {
    window.location.href = '/QuotationTemplate/Pdf';
}

function cancel() {
    window.location.href = "/QuotationTemplate/Index";
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

$(document).on('click', '#dataTable tbody .click-edit', function () {
    var $rowToEdit = $(this).closest('tr');
    var originalHTML = $rowToEdit.html();
    var qu_product_id = $rowToEdit.find('td:eq(0)').text();
   
    var qu_product_name = $rowToEdit.find('td:eq(1)').text();
    var qu_desc = $rowToEdit.find('td:eq(2)').text();
    var qu_hsn = $rowToEdit.find('td:eq(3)').text();
    var qu_height = $rowToEdit.find('td:eq(4)').text();
    var qu_width = $rowToEdit.find('td:eq(5)').text();
    var qu_size = $rowToEdit.find('td:eq(6)').text();
    var qu_rate = $rowToEdit.find('td:eq(7)').text();
    var qu_samount = $rowToEdit.find('td:eq(8)').text();
    var qu_quantity = $rowToEdit.find('td:eq(9)').text();
    var qu_total = $rowToEdit.find('td:eq(10)').text();
    var qu_cgst = $rowToEdit.find('td:eq(11)').text();
    var qu_sgst = $rowToEdit.find('td:eq(12)').text();
    var qu_igst = $rowToEdit.find('td:eq(13)').text();
    var qu_ugst = $rowToEdit.find('td:eq(14)').text();
    var qu_igstp = $rowToEdit.find('td:eq(15)').text();
    var qu_sgstp = $rowToEdit.find('td:eq(16)').text();
    var qu_ugstp = $rowToEdit.find('td:eq(17)').text();
    var qu_cgstp = $rowToEdit.find('td:eq(18)').text();
    var qu_gst = $rowToEdit.find('td:eq(19)').text();
    var qu_CompleteTotal = $rowToEdit.find('td:eq(20)').text();   
    var qu_unit = $rowToEdit.find('td:eq(21)').text();
    $rowToEdit.addClass("editing");
    // Populate the modal fields with rowData (you can adapt this to your modal structure)
    $("#qu_product_id").val(qu_product_id.trim().toLowerCase());
    // $("#qu_product_id option:selected").text(qu_product_name);
    var qu_hsn = qu_hsn.trim();
    var qu_unit = qu_unit.trim();
    $("#qu_product_hsn").val(qu_hsn);
    $("#qu_unit").val(qu_unit);
    $("#qu_desc").val(qu_desc.trim());
    $("#qu_width").val(qu_width.trim());
    $("#qu_height").val(qu_height.trim());
    $("#qu_size").val(qu_size.trim());
    $("#qu_rate").val(qu_rate.trim());
    $("#qu_amount").val(qu_samount.trim());
    $("#qu_quantity").val(qu_quantity.trim());
    $("#qu_IGST").val(qu_igst.trim());
    $("#qu_SGST").val(qu_sgst.trim());
    $("#qu_UGST").val(qu_ugst.trim());
    $("#qu_CGST").val(qu_cgst.trim());
    $("#qu_IGSTP").val(qu_igstp.trim());
    $("#qu_SGSTP").val(qu_sgstp.trim());
    $("#qu_UGSTP").val(qu_ugstp.trim());
    $("#qu_CGSTP").val(qu_cgstp.trim());
    $("#qu_gst").val(qu_gst.trim());
    $("#qu_complete_total").val(qu_CompleteTotal.trim());
    $("#qu_total").val(qu_total.trim());
    $("#addSubscription").modal("show");
    // $rowToEdit.remove();

});

function valid() {
    var inputElement = document.getElementById("qu_IGST");
    var inputValue = parseFloat(inputElement.value);

    var inputElement1 = document.getElementById("qu_UGST");
    var inputValue1 = parseFloat(inputElement1.value);

    var inputElement11 = document.getElementById("qu_SGST");
    var inputValue11 = parseFloat(inputElement11.value);

    var inputElement111 = document.getElementById("qu_CGST");
    var inputValue111 = parseFloat(inputElement111.value);

    // Check if the entered value is a number and is within the range
    if (!isNaN(inputValue) && inputValue >= 0 && inputValue <= 100) {
        // The value is valid, no further action needed
    } else {
        // If the entered value is not a valid number or is outside the range, clear the input
        inputElement.value = "";
    }
    if (!isNaN(inputValue1) && inputValue1 >= 0 && inputValue1 <= 100) {
        // The value is valid, no further action needed
    } else {
        // If the entered value is not a valid number or is outside the range, clear the input
        inputElement1.value = "";
    }
    if (!isNaN(inputValue11) && inputValue11 >= 0 && inputValue11 <= 100) {
        // The value is valid, no further action needed
    } else {
        // If the entered value is not a valid number or is outside the range, clear the input
        inputElement11.value = "";
    }
    if (!isNaN(inputValue111) && inputValue111 >= 0 && inputValue111 <= 100) {
        // The value is valid, no further action needed
    } else {
        // If the entered value is not a valid number or is outside the range, clear the input
        inputElement111.value = "";
    }
}
function zero() {
    if ($('#qu_width').val() === '' || $('#qu_width').val() === '0') {
        $('#qu_width').val('0');
        $('#qu_amount').val('0');
        $('#qu_size').val('0');
        $('#qu_total').val('0');
        $('#qu_complete_total').val('0');
    }
    else if ($('#qu_height').val() === '' || $('#qu_height').val() === '0') {
        $('#qu_height').val('0');
    }
    else if ($('#qu_quantity').val() === '' || $('#qu_quantity').val() === '0') {
        $('#qu_quantity').val('0');
    }
    else if ($('#qu_total').val() === '' || $('#qu_total').val() === '0') {
        $('#qu_total').val('0');
    }
    else if ($('#qu_amount').val() === '' || $('#qu_amount').val() === '0') {
        $('#qu_amount').val('0');
    }
    else if ($('#qu_complete_total').val() === '' || $('#qu_complete_total').val() === '0') {
        $('#qu_complete_total').val('0');
    }

}
function zero1() {
    if ($('#qu_height').val() === '' || $('#qu_height').val() === '0') {
        $('#qu_height').val('0');
        $('#qu_amount').val('0');
        $('#qu_size').val('0');
        $('#qu_total').val('0');
        $('#qu_complete_total').val('0');
    }
    else if ($('#qu_quantity').val() === '' || $('#qu_quantity').val() === '0') {
        $('#qu_quantity').val('0');
    }
    else if ($('#qu_total').val() === '' || $('#qu_total').val() === '0') {
        $('#qu_total').val('0');
    }
    else if ($('#qu_amount').val() === '' || $('#qu_amount').val() === '0') {
        $('#qu_amount').val('0');
    }
    else if ($('#qu_complete_total').val() === '' || $('#qu_complete_total').val() === '0') {
        $('#qu_complete_total').val('0');
    }
}
function zero2() {
    if ($('#qu_quantity').val() === '' || $('#qu_quantity').val() === '0') {
        $('#qu_quantity').val('0');
        $('#qu_amount').val('0');
        $('#qu_size').val('0');
        $('#qu_total').val('0');
        $('#qu_complete_total').val('0');
    }
    else if ($('#qu_total').val() === '' || $('#qu_total').val() === '0') {
        $('#qu_total').val('0');
    }
    else if ($('#qu_amount').val() === '' || $('#qu_amount').val() === '0') {
        $('#qu_amount').val('0');
    }
    else if ($('#qu_complete_total').val() === '' || $('#qu_complete_total').val() === '0') {
        $('#qu_complete_total').val('0');
    }
}
