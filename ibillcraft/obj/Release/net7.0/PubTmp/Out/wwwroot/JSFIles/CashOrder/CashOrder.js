function addorder() {
    window.location.href = "/CashOrder/EditIndex";
}
function OpenModel() {
    if ($('#do_name').val() == '') {
        alert("Please enter name!");
        return false;
    } else
        if ($('#do_phone').val() == '') {
            alert("Please enter phone number!");
            return false;
        }
    $("#addSubscription").modal("show")
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
    cleanedValue = cleanedValue.replaceAll(',', '');

    // Parse the cleaned string as a float
    var parsedValue = parseFloat(cleanedValue);
    //var cleanedValue = parseFloat(normalizedValue);
    return isNaN(parsedValue) ? null : parsedValue;
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
function submitCashorder(type) {
    /* event.preventDefault();*/
    var test = parseCurrency($('#do_dtp_charges').val());
    var rowCount = $('#dataTable tbody tr').length - 1;
    if ($('#do_name').val() == '') {
        alert("Please enter name!");
        return false;
    } else
        if ($('#do_phone').val() == '') {
            alert("Please enter phone number!");
            return false;
        } else
            if (rowCount == 0) {
                alert("Please add product details!");
                return false;
            }
            else if ($('#do_advance').val() == '') {
                alert("Please enter advance!");
                return false;
            }
            else if ($('#do_discount').val() == '') {
                alert("Please enter discount!");
                return false;
            }
            else if ($('#do_payment_method').val() == '') {
                alert("Please select payment method!");
                return false;
            } else if ($('#do_payment_method option:selected').text() == 'Cheque') {
                if ($('#do_bank_name').val() == '') {
                    alert("Please enter bank name!");
                    return false;
                } else if ($('#do_chq_no').val() == '') {
                    alert("Please enter cheque number!");
                    return false;
                } else if ($('#do_chq_date').val() == '') {
                    alert("Please select cheque date!");
                    return false;
                }
            } else if ($('#do_payment_method option:selected').text() == 'UPI') {
                if ($('#do_upi_id').val() == '') {
                    alert("Please enter UPI ID!");
                    return false;
                }
            }
    var orderDataArray = [];

    // Iterate over each row in the table 
    $("#dataTable tbody tr:gt(0)").each(function () {
        var rowData = {
            dod_product_id: $(this).find('td:eq(0)').text(),
            
            dod_product_name: $(this).find('td:eq(1)').text(),
            dod_desc: $(this).find('td:eq(2)').text(),
            dod_height: $(this).find('td:eq(3)').text(),
            dod_width: $(this).find('td:eq(4)').text(),
            dod_size: $(this).find('td:eq(5)').text(),
            dod_rate: parseCurrency($(this).find('td:eq(6)').text()),
            dod_samount: parseCurrency($(this).find('td:eq(7)').text()),
            dod_quantity: parseCurrency($(this).find('td:eq(8)').text()),
            dod_total: parseCurrency($(this).find('td:eq(9)').text()),
            dod_cgst: $(this).find('td:eq(10)').text(),
            dod_sgst: $(this).find('td:eq(11)').text(),
            dod_igst: $(this).find('td:eq(12)').text(),
            dod_ugst: $(this).find('td:eq(13)').text(),
            dod_cgstp: parseCurrency($(this).find('td:eq(14)').text()),
            dod_sgstp: parseCurrency($(this).find('td:eq(15)').text()),
            dod_igstp: parseCurrency($(this).find('td:eq(16)').text()),
            dod_ugstp: parseCurrency($(this).find('td:eq(17)').text()),
            dod_gst: parseCurrency($(this).find('td:eq(18)').text()),
            dod_CompleteTotal: parseCurrency($(this).find('td:eq(19)').text()),
            dod_hsn: $(this).find('td:eq(20)').text(),
            dod_unit: $(this).find('td:eq(21)').text()

        };
        orderDataArray.push(rowData);

    });
    var data = {
        do_name: $('#do_name').val(),
        do_id: $('#do_id').val(),
        do_no: $('#do_no').val(),
        do_phone: $('#do_phone').val(),
        do_dtp_charges: parseCurrency($('#do_dtp_charges').val()) || 0,
        do_pasting_charges: parseCurrency($('#do_pasting_charges').val()) || 0,
        do_framming_charges: parseCurrency($('#do_framming_charges').val()) || 0,
        do_fitting_charges: parseCurrency($('#do_fitting_charges').val()) || 0,
        do_inst_charges: parseCurrency($('#do_inst_charges').val()) || 0,
        do_shipping_charges: parseCurrency($('#do_shipping_charges').val()) || 0,
        do_discount: parseCurrency($('#do_discount').val()),
        do_advance: parseCurrency($('#do_advance').val()),
        do_payment_method: $('#do_payment_method').val(),
        do_bank_name: $('#do_bank_name').val(),
        do_chq_no: $('#do_chq_no').val(),
        do_chq_date: $('#do_chq_date').val(),
        do_upi_id: $('#do_upi_id').val(),
        do_total_withoutgst: parseCurrency($('#do_total_withoutgst').text()),
        do_total_igst: parseCurrency($('#do_total_igst').text()),
        do_total_sgst: parseCurrency($('#do_total_sgst').text()),
        do_total_cgst: parseCurrency($('#do_total_cgst').text()),
        do_total_ugst: parseCurrency($('#do_total_ugst').text()),
        do_total_gst: parseCurrency($('#do_total_gst').text()),
        do_total_quantity: $('#do_total_quantity').text(),
        do_total_withgst: parseCurrency($('#do_total_withgst').text()),
        do_withgst: parseCurrency($('#do_withgst').text()),
        do_balance: parseCurrency($('#do_balance').text()),
        OrderDetails: orderDataArray
    };
    $.ajax({
        type: 'POST',
        url: '/CashOrder/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            if (type == 'Close') {
                window.location.href = "/CashOrder/Index";
            }
            else {
                window.location.href = "/CashOrder/EditIndex";
            }
        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}

function CancelgData() {
    var $rowToEdit = $("#dataTable tbody tr.editing");
    $rowToEdit.html($rowToEdit.data("originalHTML"));

    // Remove the "editing" class from the row
    $rowToEdit.removeClass("editing");

    // Close the modal
    // $("#editSubscription").modal("hide");
    $("#addSubscription").modal("hide");
    $("#do_height").val("0");
    $("#do_desc").val("");
    $("#do_hsn").val("");
    $("#do_unit").val("");
    $("#do_width").val("0");
    $("#do_size").val("0");
    $("#do_rate").val(formatAsCurrency(0.0));
    $("#do_samount").val(formatAsCurrency(0.0));
    $("#do_quantity").val("0");
    $("#do_IGST").val("0");
    $("#do_SGST").val("0");
    $("#do_UGST").val("0");
    $("#do_CGST").val("0");
    $("#do_IGSTP").val("");
    $("#do_SGSTP").val("");
    $("#do_UGSTP").val("");
    $("#do_CGSTP").val("");
    $("#do_totalGST").val("");
    $("#do_gst").val(formatAsCurrency(0.0));
    $("#do_total").val(formatAsCurrency(0.0));
    $("#do_product_id").val("");
    $("#do_CompleteTotal").val(formatAsCurrency(0.0));
}
$(document).ready(function () {

    $("#do_product_id").change(function () {
        // Get the selected value
        var selectedProductId = $(this).val();

        // Call the GetValues function with the selected value
        GetValues(selectedProductId);
    });
    $("#dop_mode").change(function () {
        // Get the selected value
        var selectedText = $(this).find("option:selected").text();
        if (selectedText == "Cheque") {
            $("#chqdate").css("display", "block");
            $("#chqno").css("display", "block");
            $("#upiid").css("display", "none");
        }else if (selectedText == "UPI") {
            $("#chqdate").css("display", "none");
            $("#chqno").css("display", "none");
            $("#upiid").css("display", "block");
        }
        else {
            $("#chqdate").css("display", "none");
            $("#chqno").css("display", "none");
            $("#upiid").css("display", "none");
        }
    });
    $("#do_payment_method").change(function () {
        // Get the selected value
        var selectedText = $(this).find("option:selected").text();
            if (selectedText == "Cheque") {
                $("#do_bank_name1").css("display", "block");
                $("#do_chq_no1").css("display", "block");
                $("#do_chq_date1").css("display", "block");
                $("#do_upi_id1").css("display", "none");
            }
            else if (selectedText == "Credit") {
                $("#do_bank_name1").css("display", "none");
                $("#do_chq_no1").css("display", "none");
                $("#do_chq_date1").css("display", "none");
                $("#do_upi_id1").css("display", "none");
            } else if (selectedText == "UPI") {
                $("#do_bank_name1").css("display", "none");
                $("#do_chq_no1").css("display", "none");
                $("#do_chq_date1").css("display", "none");
                $("#do_upi_id1").css("display", "block");

            } else {
                $("#do_bank_name1").css("display", "none");
                $("#do_chq_no1").css("display", "none");
                $("#do_chq_date1").css("display", "none");
                $("#do_upi_id1").css("display", "none");

            }
    });
    $('#addSubscription').modal({
        backdrop: 'static', // Prevents closing on outside click
        keyboard: false // Prevents closing with the escape key
    });
});
function GetValues(do_product_id) {
    var data = {
        p_id: do_product_id
    }
    $.ajax({
        type: 'POST',
        url: '/CashOrder/GetValues', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            if (response.productData.p_id != null) {
                $("#do_rate").val(formatAsCurrency(parseFloat(response.productData.p_rate)));
                $("#do_desc").val(response.productData.p_desc);
                $("#do_hsn").val(response.productData.p_hsn_code);
                $("#do_unit").val(response.productData.p_unit);
                if (response.unitData.u_amount == '1') {
                    $("#do_samount").prop("readonly", false);
                } else {
                    $("#do_samount").prop("readonly", true);
                }

                response.unitData.u_height == '1' ? $("#do_height").prop("readonly", false) : $("#do_height").attr("readonly", true);
                response.unitData.u_size == '1' ? $("#do_size").prop("readonly", false) : $("#do_size").attr("readonly", true);
                response.unitData.u_width == '1' ? $("#do_width").prop("readonly", false) : $("#do_width").attr("readonly", true);
                $("#do_height").val("0");
                $("#do_width").val("0");
                $("#do_size").val("0");
                $("#do_samount").val(formatAsCurrency(0.0));
                $("#do_quantity").val("0");
                $("#do_IGST").val("0");
                $("#do_SGST").val("0");
                $("#do_UGST").val("0");
                $("#do_CGST").val("0");
                $("#do_IGSTP").val("");
                $("#do_SGSTP").val("");
                $("#do_UGSTP").val("");
                $("#do_CGSTP").val("");
                $("#do_totalGST").val(formatAsCurrency(0.0));
                $("#do_gst").val(formatAsCurrency(0.0));
                $("#do_total").val(formatAsCurrency(0.0));
                $("#do_CompleteTotal").val(formatAsCurrency(0.0));
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
    if ($('#do_product_id').val() == '') {
        alert("Please select product!");
        return false;
    } 
    else
        if (!$('#do_width').prop("readonly") && ($('#do_width').val() == '0' || $('#do_width').val() == '')) {
        alert("Please enter length!");
        return false;
    }else
            if (!$('#do_height').prop("readonly") && ($('#do_height').val() == '0' || $('#do_height').val() == '')) {
        alert("Please enter height!");
        return false;
    } else
    if ($('#do_quantity').val() == '' || $('#do_quantity').val() == '0') {
        alert("Please enter quantity!");
        return false;
    }
            
    var productid = $("#do_product_id").val();
    var product = $("#do_product_id option:selected").text();
    var do_hsn = $("#do_hsn").val();
    var do_unit = $("#do_unit").val();
    var do_desc = $("#do_desc").val();
    var do_width = $("#do_width").val() || 0;
    var do_height = $("#do_height").val() || 0;
    var sqrft = $("#do_size").val() || 0;
    var rate = $("#do_rate").val() || 0;
    var amount = $("#do_samount").val() || 0;
    var qty = $("#do_quantity").val();
    var IGST = $("#do_IGST").val() || 0;
    var SGST = $("#do_SGST").val() || 0;
    var UTGST = $("#do_UGST").val() || 0;
    var CGST = $("#do_CGST").val() || 0;
    var IGSTP = $("#do_IGSTP").val()|| 0;
    var SGSTP = $("#do_SGSTP").val() || 0;
    var UTGSTP = $("#do_UGSTP").val() || 0;
    var CGSTP = $("#do_CGSTP").val() || 0;
    var totalGST = $("#do_gst").val();
    var totalGSTAMT = $("#do_CompleteTotal").val();
    var total = $("#do_total").val();
    var $editingRow = $("#dataTable tbody tr.editing");

    // Remove the editing class and the row
    $editingRow.removeClass("editing");
    $editingRow.remove();
    // Create a new table row
    var newRow = "<tr style='text-align: right;'>" +
        "<td style='display:none;'>" + productid + "</td>" +
        
        "<td style='text-align: center;'>" + product + "</td>" +
        "<td>" + do_desc + "</td>" +
        "<td style='text-align: center;'>" + do_height + "</td>" +
        "<td style='text-align: center;'>" + do_width + "</td>" +
        "<td style='text-align: center;'>" + sqrft + "</td>" +
        "<td>" + rate + "</td>" +
        "<td>" + amount + "</td>" +
        "<td>" + qty + "</td>" +
        "<td>" + total + "</td>" +
        "<td>" + CGST + "</td>" +
        "<td>" + SGST + "</td>" +
        "<td>" + IGST + "</td>" +
        "<td>" + UTGST + "</td>" +
        "<td>" + CGSTP + "</td>" +
        "<td>" + SGSTP + "</td>" +
        "<td>" + IGSTP + "</td>" +
        "<td>" + UTGSTP + "</td>" +
        "<td>" + totalGST + "</td>" +
        "<td>" + totalGSTAMT + "</td>" +
        "<td style='display:none;'>" + do_hsn + "</td>" +
        "<td style='display:none;'>" + do_unit + "</td>" +

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
       
        product: product,
        do_desc: do_desc,
        do_height: do_height,
        do_width: do_width,
        sqrft: sqrft,
        rate: parseFloat(rate),
        amount: parseFloat(amount),
        qty: parseFloat(qty),
        total: parseFloat(total),
        IGST: parseFloat(IGST),
        SGST: parseFloat(SGST),
        UTGST: parseFloat(UTGST),
        CGST: parseFloat(CGST),
        IGSTP: parseFloat(IGSTP),
        SGSTP: parseFloat(SGSTP),
        UTGSTP: parseFloat(UTGSTP),
        CGSTP: parseFloat(CGSTP),
        totalGST: parseFloat(totalGST),
        totalGSTAMT: parseFloat(totalGSTAMT),
        do_hsn: do_hsn,
        do_unit: do_unit
    };

    addedRowsData.push(rowData);
    // Close the modal
    calculateTotals();
    final_total();
    $("#addSubscription").modal("hide");
    $("#do_height").val("0");
    $("#do_desc").val("");
    $("#do_hsn").val("");
    $("#do_unit").val("");
    $("#do_width").val("0");
    $("#do_size").val("0");
    $("#do_rate").val(formatAsCurrency(0.0));
    $("#do_samount").val(formatAsCurrency(0.0));
    $("#do_quantity").val("0");
    $("#do_IGST").val("0");
    $("#do_SGST").val("0");
    $("#do_UGST").val("0");
    $("#do_CGST").val("0");
    $("#do_IGSTP").val("");
    $("#do_SGSTP").val("");
    $("#do_UGSTP").val("");
    $("#do_CGSTP").val("");
    $("#do_totalGST").val("");
    $("#do_gst").val(formatAsCurrency(0.0));
    $("#do_total").val(formatAsCurrency(0.0));
    $("#do_product_id").val("");
    $("#do_CompleteTotal").val(formatAsCurrency(0.0));
    //$("#do_product_id").text("");
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
    var do_product_id = $rowToEdit.find('td:eq(0)').text();
    
    var do_product_name = $rowToEdit.find('td:eq(1)').text();
    var do_desc = $rowToEdit.find('td:eq(2)').text();
    var do_height = $rowToEdit.find('td:eq(3)').text();
    var do_width = $rowToEdit.find('td:eq(4)').text();
    var do_size = $rowToEdit.find('td:eq(5)').text();
    var do_rate = $rowToEdit.find('td:eq(6)').text();
    var do_samount = $rowToEdit.find('td:eq(7)').text();
    var do_quantity = $rowToEdit.find('td:eq(8)').text();
    var do_total = $rowToEdit.find('td:eq(9)').text();
    var do_cgst = $rowToEdit.find('td:eq(10)').text();
    var do_sgst = $rowToEdit.find('td:eq(11)').text();
    var do_igst = $rowToEdit.find('td:eq(12)').text();
    var do_ugst = $rowToEdit.find('td:eq(13)').text();
    var do_cgstp = $rowToEdit.find('td:eq(14)').text();
    var do_sgstp = $rowToEdit.find('td:eq(15)').text();
    var do_igstp = $rowToEdit.find('td:eq(16)').text();
    var do_ugstp = $rowToEdit.find('td:eq(17)').text();
    var do_gst = $rowToEdit.find('td:eq(18)').text();
    var do_CompleteTotal = $rowToEdit.find('td:eq(19)').text();
    var do_hsn = $rowToEdit.find('td:eq(20)').text();
    var do_unit = $rowToEdit.find('td:eq(21)').text();
    $rowToEdit.addClass("editing");
    // Populate the modal fields with rowData (you can adapt this to your modal structure)
    $("#do_product_id").val(do_product_id.toLowerCase());
    // $("#do_product_id option:selected").text(do_product_name);
    $("#do_hsn").val(do_hsn);
    $("#do_unit").val(do_unit);
    $("#do_desc").val(do_desc);
    $("#do_width").val(do_width);
    $("#do_height").val(do_height);
    $("#do_size").val(do_size);
    $("#do_rate").val(do_rate);
    $("#do_samount").val(do_samount);
    $("#do_quantity").val(do_quantity);
    $("#do_IGST").val(do_igst);
    $("#do_SGST").val(do_sgst);
    $("#do_UGST").val(do_ugst);
    $("#do_CGST").val(do_cgst);
    $("#do_IGSTP").val(do_igstp);
    $("#do_SGSTP").val(do_sgstp);
    $("#do_UGSTP").val(do_ugstp);
    $("#do_CGSTP").val(do_cgstp);
    $("#do_gst").val(do_gst);
    $("#do_CompleteTotal").val(do_CompleteTotal);
    $("#do_total").val(do_total);
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
        totalRate += parseFloat(parseCurrency(row.find("td:eq(6)").text())) || 0;
        totalQuantity += parseFloat(row.find("td:eq(8)").text()) || 0;
        totalGSTC += parseFloat(parseCurrency(row.find("td:eq(14)").text())) || 0;
        totalGSTS += parseFloat(parseCurrency(row.find("td:eq(15)").text())) || 0;
        totalGSTI += parseFloat(parseCurrency(row.find("td:eq(16)").text())) || 0;
        totalGSTU += parseFloat(parseCurrency(row.find("td:eq(17)").text())) || 0;
        totalGST += parseFloat(parseCurrency(row.find("td:eq(18)").text())) || 0;
        totalAmount += parseFloat(parseCurrency(row.find("td:eq(19)").text())) || 0;
        totalAmountwithoutgst += parseFloat(parseCurrency(row.find("td:eq(9)").text())) || 0;
    });
    // Update the hidden inputs with the calculated totals
    $("#do_rate2").val(totalRate.toFixed(2));
    $("#do_total_quantity").text(totalQuantity);
    $("#do_total2").val(totalAmount.toFixed(2));
    $("#do_total_withoutgst").text(formatAsCurrency(parseFloat(totalAmountwithoutgst)));
    $("#do_total_igst").text(totalGSTI);
    $("#do_total_sgst").text(totalGSTS);
    $("#do_total_cgst").text(totalGSTC);
    $("#do_total_ugst").text(totalGSTU);
    $("#do_total_gst").text(formatAsCurrency(parseFloat(totalGST)));
    $("#do_total_withgst").text(formatAsCurrency(parseFloat(totalAmount.toFixed(2))));
    $("#do_withgst").text(formatAsCurrency(parseFloat(totalAmount)));
    $("#do_balance").text(formatAsCurrency(parseFloat(totalAmount)));
}
function toggleReadOnly() {
    $("#flexCheckDefault1").prop("checked") ? $("#do_dtp_charges").prop("readonly", false) : ($("#do_dtp_charges").prop("readonly", true), $("#do_dtp_charges").val(formatAsCurrency(0.0)), final_total());
    $("#flexCheckDefault2").prop("checked") ? $("#do_pasting_charges").prop("readonly", false) : ($("#do_pasting_charges").prop("readonly", true), $("#do_pasting_charges").val(formatAsCurrency(0.0)), final_total());
    $("#flexCheckDefault3").prop("checked") ? $("#do_framming_charges").prop("readonly", false) : ($("#do_framming_charges").prop("readonly", true), $("#do_framming_charges").val(formatAsCurrency(0.0)), final_total());
    $("#flexCheckDefault4").prop("checked") ? $("#do_fitting_charges").prop("readonly", false) : ($("#do_fitting_charges").prop("readonly", true), $("#do_fitting_charges").val(formatAsCurrency(0.0)), final_total());
    $("#flexCheckDefault5").prop("checked") ? $("#do_inst_charges").prop("readonly", false) : ($("#do_inst_charges").prop("readonly", true), $("#do_inst_charges").val(formatAsCurrency(0.0)), final_total());
    $("#flexCheckDefault6").prop("checked") ? $("#do_shipping_charges").prop("readonly", false) : ($("#do_shipping_charges").prop("readonly", true), $("#do_shipping_charges").val(formatAsCurrency(0.0)), final_total());
}
function sqrft() {
    var first = $("#do_height").val() || 0;
    var second = $("#do_width").val() || 0;
    var sqrft = $("#do_size");
    var sqrft_total = (parseFloat(first) * parseFloat(second));
    sqrft.val(sqrft_total.toFixed(2));
}
function rate() {
    var rates = parseCurrency($("#do_rate").val()) || 0;
    var sqrft = $("#do_size").val() || 0;
    var amount = $("#do_samount");
    var rate_amount = (parseFloat(sqrft) * parseFloat(rates));
    amount.val(formatAsCurrency(parseFloat(rate_amount.toFixed(2))));
}
function quan_amount() {
    var amt = parseCurrency($("#do_samount").val()) == '0' || $("#do_samount").val() == '' ? parseCurrency($("#do_rate").val()) : parseCurrency($("#do_samount").val());
    var quantity = $("#do_quantity").val() || 0;
    var total = $("#do_total");
    var total_amount = (parseFloat(amt) * parseFloat(quantity))
    total.val(formatAsCurrency(parseFloat(total_amount.toFixed(2))));
    $("#do_CompleteTotal").val(formatAsCurrency(parseFloat(total_amount.toFixed(2))));
}
function gst() {
    var cgst = parseFloat($("#do_CGST").val()) || 0;
    var sgst = parseFloat($("#do_SGST").val()) || 0;
    var igst = parseFloat($("#do_IGST").val()) || 0;
    var ugst = parseFloat($("#do_UGST").val()) || 0;
    var total = parseFloat(parseCurrency($("#do_total").val())) || 0;
    // var total = $("#do_CompleteTotal").val() || 0;

    var total_amount = total;

    var rcgst = total_amount * cgst / 100;
    var rsgst = total_amount * sgst / 100;
    var rigst = total_amount * igst / 100;
    var rugst = total_amount * ugst / 100;
    $("#do_IGSTP").val(formatAsCurrency(parseFloat(rigst)));
    $("#do_SGSTP").val(formatAsCurrency(parseFloat(rsgst)));
    $("#do_UGSTP").val(formatAsCurrency(parseFloat(rugst)));
    $("#do_CGSTP").val(formatAsCurrency(parseFloat(rcgst)));
    // var amt = total.value + rcgst.value + rsgst.value + rigst.value;
    var gst = parseFloat(rcgst) + parseFloat(rsgst) + parseFloat(rigst) + parseFloat(rugst);
    var amt = parseFloat(gst) + parseFloat(total_amount);
    $("#do_gst").val(formatAsCurrency(parseFloat(gst.toFixed(2))));
    $("#do_CompleteTotal").val(formatAsCurrency(parseFloat(amt.toFixed(2))));
}
function quan_amount2() {
    var rate = $("#do_rate2").val() || 0;
    var quantity = $("#do_quantity2").val() || 0;
    var total = $("#do_total2").val();
    var total_amount = (parseFloat(rate.value) * parseFloat(quantity.value))
    total.value = total_amount.toFixed(2);
}


function final_total() {
    var subtotal = parseCurrency($("#do_total_withoutgst").text());
    var totalgst = parseCurrency($('#do_total_gst').text()) || 0;
    var discount = parseCurrency($("#do_discount").val()) || 0;
    var dtpcharges = parseCurrency($("#do_dtp_charges").val()) || 0;
    var dtp_value = parseFloat(dtpcharges);
    var advance = parseCurrency($("#do_advance").val()) || 0;
    var transport_charges = parseCurrency($("#do_shipping_charges").val()) || 0;
    var transport_value = parseFloat(transport_charges);
    var do_inst_charges = parseCurrency($("#do_inst_charges").val()) || 0;
    var instvalue = parseFloat(do_inst_charges);
    var fitting = parseCurrency($("#do_fitting_charges").val()) || 0;
    var fitting_value = parseFloat(fitting);
    var do_framming_charges = parseCurrency($("#do_framming_charges").val()) || 0;
    var framming_value = parseFloat(do_framming_charges);
    var pasting = parseCurrency($("#do_pasting_charges").val()) || 0;
    var pasting_value = parseFloat(pasting);
    var total_dtp_transport = (parseFloat(transport_value) + parseFloat(dtp_value)) + parseFloat(framming_value) + parseFloat(fitting_value) + parseFloat(instvalue) + parseFloat(pasting_value);
    var total_amount = (parseFloat(subtotal) + parseFloat(total_dtp_transport));
    var totalwithgst = (parseFloat(subtotal) + parseFloat(total_dtp_transport)) + totalgst;
    if (advance == "0" && discount == "0") {
        var balance = (parseFloat(total_amount)) + totalgst;
        var roundoff = formatAsCurrency(parseFloat(balance.toFixed(2)));
        $("#do_balance").text(roundoff);
        $("#do_total_withgst").text(formatAsCurrency(parseFloat(totalwithgst.toFixed(2))));
    }
    else if (advance != "0" || discount != "0") {
        var balance = (parseFloat(total_amount) - parseFloat(advance) - parseFloat(discount)) + totalgst;
        var roundoff = formatAsCurrency(parseFloat(balance.toFixed(2)));
        $("#do_balance").text(roundoff);
        $("#do_total_withgst").text(formatAsCurrency(parseFloat(totalwithgst.toFixed(2))));
    }

}
function Excel() {
    window.location.href = '/CashOrder/Excel';
}

function Pdf() {
    window.location.href = '/CashOrder/Pdf';
}
function payment() {
    var do_no = $('#doNoValue').text();
    var do_name = $('#do_name').text();
    var do_id = $('#doidValue').text();
    var do_balance = parseCurrency($('#do_balance').text());
    var do_payment_method = $('#do_payment_method').text();
    //if (do_balance == "0" || do_balance == "0.00" || do_balance == "00.00") {
    //    alert("Payment is already done");
    //    return false;
    //}
    //else {
        window.location.href = '/CashOrder/Payment?do_no=' + do_no + '&do_name=' + do_name + '&do_balance=' + do_balance + '&do_id=' + do_id;
    //}
}
function payamount() {
    var dop_do_balance = parseCurrency($("#dop_due").val());
    var dop_pay = parseFloat($('#dop_pay').val()) || 0;
    var dop_discount = parseCurrency($("#dop_discount").val()) || 0;
    var total = parseFloat(dop_do_balance) - (parseFloat(dop_discount) + parseFloat(dop_pay));
    $("#dop_balance").val(formatAsCurrency(total));
}
function submitpayment() {
    /* event.preventDefault();*/

    if (parseCurrency($('#dop_due').val()) == '0' || parseCurrency($('#dop_due').val()) == '0.0') {
        alert("Payment is already done!");
        return false;
    }else if ($('#dop_due').val() == $('#dop_balance').val()) {
        alert("Please enter payment details!");
        return false;
    } else if ($('#dop_discount').val() == '') {
        alert("Please enter discount!");
        return false;
    } else if ($('#dop_pay').val() == '' || $('#dop_pay').val() == '0') {
        alert("Please enter pay!");
        return false;
    } else if ($('#dop_mode').val() == '') {
        alert("Please select payment method!");
        return false;
    }
    else if ($('#dop_mode option:selected').text() == 'Cheque') {
        if ($('#dop_chq_no').val() == '') {
            alert("Please enter cheque number!");
            return false;
        } else if ($('#dop_chq_date').val() == '') {
            alert("Please select cheque date!");
            return false;
        }
    }else if ($('#dop_mode option:selected').text() == 'UPI') {
        if ($('#dop_upi_id').val() == '') {
            alert("Please enter upi id!");
            return false;
        } 
    }

    var data = {
        dop_no: $('#dop_no').val(),
        dop_do_id: $('#dop_do_id').val(),
        dop_name: $('#dop_name').val(),
        dop_due: parseCurrency($('#dop_due').val()),
        dop_discount: $('#dop_discount').val(),
        dop_pay: $('#dop_pay').val(),
        dop_mode: $('#dop_mode').val(),
        dop_chq_no: $('#dop_chq_no').val(),
        dop_balance: parseCurrency($('#dop_balance').val()),
        dop_date: $('#dop_date').val(),
        dop_chq_date: $('#dop_chq_date').val(),
        dop_upi_id: $('#dop_upi_id').val()

    };
    $.ajax({
        type: 'POST',
        url: '/CashOrder/CreatePayment', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {

            window.location.href = '/CashOrder/Payment?do_no=' + response.dop_no + '&do_name=' + response.dop_name + '&do_balance=' + response.dop_balance + '&do_id=' + response.dop_do_id;
        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}
function cancel() {
    window.location.href = "/CashOrder/Index";
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
    window.location.reload();
}
function printformat1(dataTable) {
    var printContents = $("#" + dataTable).html();
    var originalContents = $("body").html();

    $("body").html(printContents);

    window.print();

    $("body").html(originalContents);
    window.location.reload();
}
function checkDMax(inputElement, id) {
    var total = parseInt(inputElement.value) + parseFloat(parseCurrency($("#do_advance").val()) || 0);
    if (total >= parseFloat(parseCurrency($("#" + id).text()))) {
        inputElement.value = parseCurrency($("#" + id).text()) - parseFloat(parseCurrency($("#do_advance").val()) || 0); // Set the value to the maximum allowe
    }
}
function checkAMax(inputElement, id) {
    var total = parseInt(inputElement.value) + parseFloat(parseCurrency($("#do_discount").val()) || 0);
    if (total >= parseFloat(parseCurrency($("#" + id).text()))) {
        inputElement.value = parseCurrency($("#" + id).text()) - parseFloat(parseCurrency($("#do_discount").val()) || 0); // Set the value to the maximum allowed
    }
}
function setMinDate() {
    var currentDate = new Date().toISOString().split('T')[0];
    document.getElementById('dop_chq_date').min = currentDate;
    document.getElementById('do_chq_date').min = currentDate;
}

function checkDPamynetMax(inputElement, id) {
    var total = parseInt(inputElement.value) + parseFloat(parseCurrency($("#dop_pay").val()) || 0);
    if (total >= parseFloat(parseCurrency($("#" + id).val()))) {
        inputElement.value = parseCurrency($("#" + id).val()) - parseFloat(parseCurrency($("#dop_pay").val()) || 0); // Set the value to the maximum allowe
    }
}
function checkPaymentMax(inputElement, id) {
    var total = parseInt(inputElement.value) + parseFloat(parseCurrency($("#dop_discount").val()) || 0);
    if (total >= parseFloat(parseCurrency($("#" + id).val()))) {
        inputElement.value = parseCurrency($("#" + id).val()) - parseFloat(parseCurrency($("#dop_discount").val()) || 0); // Set the value to the maximum allowed
    }
}
function zero() {
    if ($('#do_width').val() === '' || $('#do_width').val() === '0') {
        $('#do_width').val('0');
        $('#do_samount').val('0');
        $('#do_size').val('0');
        $('#do_total').val('0');
        $('#do_completetotal').val('0');
    }
    else if ($('#do_height').val() === '' || $('#do_height').val() === '0') {
        $('#do_height').val('0');
    }
    else if ($('#do_quantity').val() === '' || $('#do_quantity').val() === '0') {
        $('#do_quantity').val('0');
    }
    else if ($('#do_total').val() === '' || $('#do_total').val() === '0') {
        $('#do_total').val('0');
    }
    else if ($('#do_samount').val() === '' || $('#do_samount').val() === '0') {
        $('#do_samount').val('0');
    }
    else if ($('#do_completetotal').val() === '' || $('#do_completetotal').val() === '0') {
        $('#do_completetotal').val('0');
    }

}
function zero1() {
    if ($('#do_height').val() === '' || $('#do_height').val() === '0') {
        $('#do_height').val('0');
        $('#do_samount').val('0');
        $('#do_size').val('0');
        $('#do_total').val('0');
        $('#do_completetotal').val('0');
    }
    else if ($('#do_quantity').val() === '' || $('#do_quantity').val() === '0') {
        $('#do_quantity').val('0');
    }
    else if ($('#do_total').val() === '' || $('#do_total').val() === '0') {
        $('#do_total').val('0');
    }
    else if ($('#do_samount').val() === '' || $('#do_samount').val() === '0') {
        $('#do_samount').val('0');
    }
    else if ($('#do_completetotal').val() === '' || $('#do_completetotal').val() === '0') {
        $('#do_completetotal').val('0');
    }
}
function zero2() {
    if ($('#do_quantity').val() === '' || $('#do_quantity').val() === '0') {
        $('#do_quantity').val('0');
        $('#do_samount').val('0');
        $('#do_size').val('0');
        $('#do_total').val('0');
        $('#do_completetotal').val('0');
    }
    else if ($('#do_total').val() === '' || $('#do_total').val() === '0') {
        $('#do_total').val('0');
    }
    else if ($('#do_samount').val() === '' || $('#do_samount').val() === '0') {
        $('#do_samount').val('0');
    }
    else if ($('#do_completetotal').val() === '' || $('#do_completetotal').val() === '0') {
        $('#do_completetotal').val('0');
    }
}