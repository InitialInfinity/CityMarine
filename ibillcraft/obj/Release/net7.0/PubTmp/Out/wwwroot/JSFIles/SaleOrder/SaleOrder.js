function addsale() {
    window.location.href = "/SaleOrder/EditIndex";
}
function parseCurrency(currencyText) {
    var locale = $('#cm_currency_format').val();
    //var formattedChar = new Intl.NumberFormat(locale).format(1111.1).charAt(1);

    // Replace symbols and separators specific to the given currency format
    //var cleanedValue = parseFloat(currencyText.replace(new RegExp('[^\d' + formattedChar + ']', 'g'), '').replace(formattedChar, '.'));

    //return isNaN(cleanedValue) ? null : cleanedValue;
    //var cleanedValue = parseFloat(currencyText.replace(/[^\d.,]|,(?=[^,]*$)|\.(?=[^.]*$)/g, '').replace(',', '.'));
    var cleanedValue = currencyText.replaceAll(/[^\d.,]/g, '');

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
function OpenModel() {
    if ($('#sl_cust_id').val() == '') {
        alert("Please select customer name!");
        return false;
    } else
        if ($('#sl_staff_id').val() == '') {
            alert("Please select user!");
            return false;
        }
    $("#addSubscription").modal("show")
}
function submitSaleorder(type) {
    /* event.preventDefault();*/
    var test = parseCurrency($('#sl_dtp_charges').val());
    var rowCount = $('#dataTable tbody tr').length - 1;
    if ($('#sl_cust_id').val() == '') {
        alert("Please select customer name!");
        return false;
    } else
        if ($('#sl_staff_id').val() == '') {
            alert("Please select user!");
            return false;
        } else
            if (rowCount == 0) {
                alert("Please add product details!");
                return false;
            }
            else if ($('#sl_advance').val() == '') {
                alert("Please enter advance!");
                return false;
            }
            else if ($('#sl_discount').val() == '') {
                alert("Please enter discount!");
                return false;
            }
            else if ($('#sl_payment_method').val() == '') {
                alert("Please select payment method!");
                return false;
            } else if (parseFloat(parseCurrency($('#sl_cr_amt').val())) < parseCurrency($('#sl_balance').text())) {
                alert("Credit balance is low!");
                return false;
            } else if ($('#sl_payment_method option:selected').text() == 'Cheque') {
                if ($('#sl_bank_name').val() == '') {
                    alert("Please enter bank name!");
                    return false;
                } else if ($('#sl_chq_no').val() == '') {
                    alert("Please enter cheque number!");
                    return false;
                } else if ($('#sl_chq_date').val() == '') {
                    alert("Please select cheque date!");
                    return false;
                }
            }else if ($('#sl_payment_method option:selected').text() == 'UPI') {
                if ($('#sl_upi_id').val() == '') {
                    alert("Please enter UPI ID!");
                    return false;
                } 
            }

    var orderDataArray = [];

    // Iterate over each row in the table 
    $("#dataTable tbody tr:gt(0)").each(function () {
        var rowData = {
            si_product_id: $(this).find('td:eq(0)').text(),
            si_material_id: $(this).find('td:eq(1)').text(),
            
            si_product_name: $(this).find('td:eq(2)').text(),
            si_desc: $(this).find('td:eq(3)').text(),
            si_height: $(this).find('td:eq(4)').text(),
            si_width: $(this).find('td:eq(5)').text(),
            si_size: $(this).find('td:eq(6)').text(),
            si_rate: parseCurrency($(this).find('td:eq(7)').text()),
            si_samount: parseCurrency($(this).find('td:eq(8)').text()),
            si_quantity: parseCurrency($(this).find('td:eq(9)').text()),
            si_total: parseCurrency($(this).find('td:eq(10)').text()),
            si_cgst: $(this).find('td:eq(11)').text(),
            si_sgst: $(this).find('td:eq(12)').text(),
            si_igst: $(this).find('td:eq(13)').text(),
            si_ugst: $(this).find('td:eq(14)').text(),
            si_cgstp: parseCurrency($(this).find('td:eq(15)').text()),
            si_sgstp: parseCurrency($(this).find('td:eq(16)').text()),
            si_igstp: parseCurrency($(this).find('td:eq(17)').text()),
            si_ugstp: parseCurrency($(this).find('td:eq(18)').text()),
            si_gst: parseCurrency($(this).find('td:eq(19)').text()),
            si_complete_total: parseCurrency($(this).find('td:eq(20)').text()),
            si_hsn: $(this).find('td:eq(21)').text(),
            si_unit: $(this).find('td:eq(22)').text()

        };
        orderDataArray.push(rowData);

    });
    var data = {
        sl_cust_name: $('#sl_cust_name').val(),
        sl_cust_id: $('#sl_cust_id').val(),
        sl_staff_id: $('#sl_staff_id').val(),
        sl_id: $('#sl_id').val(),
        sl_invoice_no: $('#sl_invoice_no').val(),
        sl_invoice_date: $('#sl_invoice_date').val(),
        sl_order_no: $('#sl_order_no').val(),
        sl_due_date: $('#sl_due_date').val(),
        sl_chq_date: $('#sl_chq_date').val(),
        sl_chq_no: $('#sl_chq_no').val(),
        sl_upi_id: $('#sl_upi_id').val(),
        sl_bank_name: $('#sl_bank_name').val(),
        sl_payment_m: $('#sl_payment_method').find(":selected").text(),
        sl_payment_method: $('#sl_payment_method').val(),
        sl_dtp_charges: parseCurrency($('#sl_dtp_charges').val()) || 0,
        sl_pasting_charges: parseCurrency($('#sl_pasting_charges').val()) || 0,
        sl_framming_charges: parseCurrency($('#sl_framming_charges').val()) || 0,
        sl_fitting_charges: parseCurrency($('#sl_fitting_charges').val()) || 0,
        sl_installation_charges: parseCurrency($('#sl_installation_charges').val()) || 0,
        sl_shipping_charges: parseCurrency($('#sl_shipping_charges').val()) || 0,
        sl_discount: parseCurrency($('#sl_discount').val()),
        sl_advance: parseCurrency($('#sl_advance').val()),
        sl_payment_method: $('#sl_payment_method').val(),
        sl_total_withoutgst: parseCurrency($('#sl_total_withoutgst').text()),
        sl_total_igst: parseCurrency($('#sl_total_igst').text()),
        sl_total_sgst: parseCurrency($('#sl_total_sgst').text()),
        sl_total_cgst: parseCurrency($('#sl_total_cgst').text()),
        sl_total_ugst: parseCurrency($('#sl_total_ugst').text()),
        sl_total_gst: parseCurrency($('#sl_total_gst').text()),
        sl_total_quantity: $('#sl_total_quantity').text(),
        sl_total_withgst: parseCurrency($('#sl_total_withgst').text()),
        sl_withgst: parseCurrency($('#sl_withgst').text()),
        sl_balance: parseCurrency($('#sl_balance').text()),
        SaleDetails: orderDataArray
    };
    $.ajax({
        type: 'POST',
        url: '/SaleOrder/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            if (type == 'Close') {
                window.location.href = "/SaleOrder/Index";
            }
            else {
                window.location.href = "/SaleOrder/EditIndex";
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
    $("#sl_height").val("0");
    $("#sl_desc").val("");
    $("#sl_hsn").val("");
    $("#sl_unit").val("");
    $("#sl_width").val("0");
    $("#sl_size").val("0");
    $("#sl_rate").val((formatAsCurrency(0.0)));
    $("#sl_samount").val((formatAsCurrency(0.0)));
    $("#sl_quantity").val("0");
    $("#sl_IGST").val("0");
    $("#sl_SGST").val("0");
    $("#sl_UGST").val("0");
    $("#sl_CGST").val("0");
    $("#sl_IGSTP").val("");
    $("#sl_SGSTP").val("");
    $("#sl_UGSTP").val("");
    $("#sl_CGSTP").val("");
    $("#sl_totalGST").val("0");
    $("#sl_gst").val((formatAsCurrency(0.0)));
    $("#sl_total").val((formatAsCurrency(0.0)));
    $("#si_product_id").val("");
    $("#si_material_id").val("");
    $("#sl_complete_total").val((formatAsCurrency(0.0)));
}
$(document).ready(function () {

    $("#si_product_id").change(function () {
        // Get the selected value
        var selectedProductId = $(this).val();
        var customer = $('#sl_cust_id').val();

        // Call the GetValues function with the selected value
        GetValues(selectedProductId, customer);
    });
    $("#sl_payment_method").change(function () {
        // Get the selected value
        var selectedText = $(this).find("option:selected").text();
        if ($(sl_cust_id).val() == "") {
            alert("Please select customer");
            $(this).val("");
            return false;
        }
        else {
            var custmerid = $(sl_cust_id).val();
            if (selectedText == "Cheque") {
                $("#sl_bank_name1").css("display", "block");
                $("#sl_chq_no1").css("display", "block");
                $("#sl_chq_date1").css("display", "block");
                $("#sl_cr_amt1").css("display", "none");
                $("#sl_upi_id1").css("display", "none");

            }
            else if (selectedText == "Credit") {
                $("#sl_bank_name1").css("display", "none");
                $("#sl_chq_no1").css("display", "none");
                $("#sl_chq_date1").css("display", "none");
                $("#sl_cr_amt1").css("display", "block");
                $("#sl_upi_id1").css("display", "none");

                var credit = getcredit(custmerid);
            }else if (selectedText == "UPI") {
                $("#sl_bank_name1").css("display", "none");
                $("#sl_chq_no1").css("display", "none");
                $("#sl_chq_date1").css("display", "none");
                $("#sl_cr_amt1").css("display", "none");
                $("#sl_upi_id1").css("display", "block");

            } else {
                $("#sl_bank_name1").css("display", "none");
                $("#sl_chq_no1").css("display", "none");
                $("#sl_chq_date1").css("display", "none");
                $("#sl_cr_amt1").css("display", "none");
                $("#sl_upi_id1").css("display", "none");

            }
        }
    });
    $("#sp_mode").change(function () {
        // Get the selected value
        var selectedText = $(this).find("option:selected").text();
        if (selectedText == "Cheque") {
            $("#chqdate").css("display", "block");
            $("#chqno").css("display", "block");
            $("#bankname").css("display", "block");
            $("#upiid").css("display", "none");

        }
        else if (selectedText == "UPI") {
            $("#chqdate").css("display", "none");
            $("#chqno").css("display", "none");
            $("#bankname").css("display", "none");
            $("#upiid").css("display", "block");
        }else {
            $("#chqdate").css("display", "none");
            $("#chqno").css("display", "none");
            $("#bankname").css("display", "none");
            $("#upiid").css("display", "none");

        }
    });
    $('#addSubscription').modal({
        backdrop: 'static', // Prevents closing on outside click
        keyboard: false // Prevents closing with the escape key
    });
});
function getcredit(custmerid) {
    var data = {
        c_id: custmerid
    }
    $.ajax({
        type: 'POST',
        url: '/SaleOrder/GetCredit', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#sl_cr_amt").val(response);
            if (response != "Credit is not given") {
                if (parseFloat(parseCurrency(response)) < parseCurrency($('#sl_balance').text())) {
                    $("#sl_cr_amt1").css("display", "none");
                    $("#sl_cr_amt").val('');
                    $("#sl_payment_method").val('');
                    alert("Your credit limit is low! please select another payment method!");
                }
            }
            //window.location.reload();
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
function GetValues(si_product_id, custid) {
     custid = $('#sl_cust_id').val();

    var data = {
        p_id: si_product_id,
        p_cust_id: custid
    }
    $.ajax({
        type: 'POST',
        url: '/SaleOrder/GetValues', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            if (response.productData.p_id != null) {
                $("#sl_rate").val(formatAsCurrency(parseFloat(response.productData.p_rate)));
                $("#sl_desc").val(response.productData.p_desc);
                $("#sl_hsn").val(response.productData.p_hsn_code);
                $("#sl_unit").val(response.productData.p_unit);
                if (response.unitData.u_amount == '1') {
                    $("#sl_samount").prop("readonly", false);
                } else {
                    $("#sl_samount").prop("readonly", true);
                }

                response.unitData.u_height == '1' ? $("#sl_height").prop("readonly", false) : $("#sl_height").attr("readonly", true);
                response.unitData.u_size == '1' ? $("#sl_size").prop("readonly", false) : $("#sl_size").attr("readonly", true);
                response.unitData.u_width == '1' ? $("#sl_width").prop("readonly", false) : $("#sl_width").attr("readonly", true);
                $("#sl_width").val("0");
                $("#sl_size").val("0");
                $("#sl_samount").val("0");
                $("#sl_quantity").val("0");
                $("#sl_IGST").val("0");
                $("#sl_SGST").val("0");
                $("#sl_UGST").val("0");
                $("#sl_CGST").val("0");
                $("#sl_IGSTP").val("");
                $("#sl_SGSTP").val("");
                $("#sl_UGSTP").val("");
                $("#sl_CGSTP").val("");
                $("#sl_totalGST").val((formatAsCurrency(0.0)));
                $("#sl_gst").val((formatAsCurrency(0.0)));
                $("#sl_total").val((formatAsCurrency(0.0)));
                $("#si_material_id").val("");
                $("#sl_complete_total").val((formatAsCurrency(0.0)));
                $("#sl_height").val("0");

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
    if ($('#si_product_id').val() == '') {
        alert("Please select product!");
        return false;
    } else if ($('#si_material_id').val() == '') {
        alert("Please select material!");
        return false;
    } else
        if (!$('#sl_width').prop("readonly") && ($('#sl_width').val() == '0' || $('#sl_width').val() == '')) {
            alert("Please enter length!");
            return false;
        } else
            if (!$('#sl_height').prop("readonly") && ($('#sl_height').val() == '0' || $('#sl_height').val() == '')) {
                alert("Please enter height!");
                return false;
            } else
        if ($('#sl_quantity').val() == '' || $('#sl_quantity').val() == '0') {
            alert("Please enter quantity!");
            return false;
        }
    var productid = $("#si_product_id").val();
    var materialid = $("#si_material_id").val();
    var product = $("#si_product_id option:selected").text();
    var sl_hsn = $("#sl_hsn").val();
    var sl_unit = $("#sl_unit").val();
    var sl_desc = $("#sl_desc").val();
    var sl_width = $("#sl_width").val() || 0;
    var sl_height = $("#sl_height").val() || 0;
    var sqrft = $("#sl_size").val() || 0;
    var rate = $("#sl_rate").val();
    var amount = $("#sl_samount").val() || 0;
    var qty = $("#sl_quantity").val();
    var IGST = $("#sl_IGST").val() || 0;
    var SGST = $("#sl_SGST").val() || 0;
    var UTGST = $("#sl_UGST").val() || 0;
    var CGST = $("#sl_CGST").val() || 0;
    var IGSTP = $("#sl_IGSTP").val();
    var SGSTP = $("#sl_SGSTP").val();
    var UTGSTP = $("#sl_UGSTP").val();
    var CGSTP = $("#sl_CGSTP").val();
    var totalGST = $("#sl_gst").val();
    var totalGSTAMT = $("#sl_complete_total").val();
    var total = $("#sl_total").val();
    var $editingRow = $("#dataTable tbody tr.editing");

    // Remove the editing class and the row
    $editingRow.removeClass("editing");
    $editingRow.remove();
    // Create a new table row
    var newRow = "<tr style='text-align: right;'>" +
        "<td style='display:none;'>" + productid + "</td>" +
        "<td style='display:none;'>" + materialid + "</td>" +
       
        "<td style='text-align: center;'>" + product + "</td>" +
        "<td>" + sl_desc + "</td>" +
        "<td style='text-align: center;'>" + sl_height + "</td>" +
        "<td style='text-align: center;'>" + sl_width + "</td>" +
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
        "<td style='display:none;'>" + sl_hsn + "</td>" +
        "<td style='display:none;'>" + sl_unit + "</td>" +

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
        materialid: materialid,
        sl_desc: sl_desc,
        product: product,
        sl_height: sl_height,
        sl_width: sl_width,
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
        sl_hsn: sl_hsn,
        sl_unit: sl_unit
    };

    addedRowsData.push(rowData);
    // Close the modal
    calculateTotals();
    final_total();
    $("#addSubscription").modal("hide");
    $("#sl_height").val("0");
    $("#sl_desc").val("");
    $("#sl_hsn").val("");
    $("#sl_unit").val("");
    $("#sl_width").val("0");
    $("#sl_size").val("");
    $("#sl_rate").val((formatAsCurrency(0.0)));
    $("#sl_samount").val((formatAsCurrency(0.0)));
    $("#sl_quantity").val("0");
    $("#sl_IGST").val("0");
    $("#sl_SGST").val("0");
    $("#sl_UGST").val("0");
    $("#sl_CGST").val("0");
    $("#sl_IGSTP").val("");
    $("#sl_SGSTP").val("");
    $("#sl_UGSTP").val("");
    $("#sl_CGSTP").val("");
    $("#sl_totalGST").val("");
    $("#sl_gst").val("0");
    $("#sl_total").val((formatAsCurrency(0.0)));
    $("#sl_complete_total").val((formatAsCurrency(0.0)));
    $("#si_product_id").val("");
    $("#si_material_id").val("");
    //$("#si_product_id").text("");
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
    var si_product_id = $rowToEdit.find('td:eq(0)').text();
    var si_material_id = $rowToEdit.find('td:eq(1)').text();
    
    var sl_product_name = $rowToEdit.find('td:eq(2)').text();
    var sl_desc = $rowToEdit.find('td:eq(3)').text();
    var sl_height = $rowToEdit.find('td:eq(4)').text();
    var sl_width = $rowToEdit.find('td:eq(5)').text();
    var sl_size = $rowToEdit.find('td:eq(6)').text();
    var sl_rate = $rowToEdit.find('td:eq(7)').text();
    var sl_samount = $rowToEdit.find('td:eq(8)').text();
    var sl_quantity = $rowToEdit.find('td:eq(9)').text();
    var sl_total = $rowToEdit.find('td:eq(10)').text();
    var sl_cgst = $rowToEdit.find('td:eq(11)').text();
    var sl_sgst = $rowToEdit.find('td:eq(12)').text();
    var sl_igst = $rowToEdit.find('td:eq(13)').text();
    var sl_ugst = $rowToEdit.find('td:eq(14)').text();
    var sl_cgstp = $rowToEdit.find('td:eq(15)').text();
    var sl_sgstp = $rowToEdit.find('td:eq(16)').text();
    var sl_igstp = $rowToEdit.find('td:eq(17)').text();
    var sl_ugstp = $rowToEdit.find('td:eq(18)').text();
    var sl_gst = $rowToEdit.find('td:eq(19)').text();
    var sl_complete_total = $rowToEdit.find('td:eq(20)').text();
    var sl_hsn = $rowToEdit.find('td:eq(21)').text();
    var sl_unit = $rowToEdit.find('td:eq(22)').text();
    $rowToEdit.addClass("editing");
    // Populate the modal fields with rowData (you can adapt this to your modal structure)
    $("#si_product_id").val(si_product_id.toLowerCase());
    $("#si_material_id").val(si_material_id.toLowerCase());
    // $("#si_product_id option:selected").text(sl_product_name);
    $("#sl_hsn").val(sl_hsn);
    $("#sl_unit").val(sl_unit);
    $("#sl_desc").val(sl_desc);
    $("#sl_width").val(sl_width);
    $("#sl_height").val(sl_height);
    $("#sl_size").val(sl_size);
    $("#sl_rate").val(sl_rate);
    $("#sl_samount").val(sl_samount);
    $("#sl_quantity").val(sl_quantity);
    $("#sl_IGST").val(sl_igst);
    $("#sl_SGST").val(sl_sgst);
    $("#sl_UGST").val(sl_ugst);
    $("#sl_CGST").val(sl_cgst);
    $("#sl_IGSTP").val(sl_igstp);
    $("#sl_SGSTP").val(sl_sgstp);
    $("#sl_UGSTP").val(sl_ugstp);
    $("#sl_CGSTP").val(sl_cgstp);
    $("#sl_gst").val(sl_gst);
    $("#sl_complete_total").val(sl_complete_total);
    $("#sl_total").val(sl_total);
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
        totalRate += parseFloat(parseCurrency(row.find("td:eq(7)").text())) || 0;
        totalQuantity += parseFloat(row.find("td:eq(9)").text()) || 0;
        totalGSTC += parseFloat(parseCurrency(row.find("td:eq(15)").text())) || 0;
        totalGSTS += parseFloat(parseCurrency(row.find("td:eq(16)").text())) || 0;
        totalGSTI += parseFloat(parseCurrency(row.find("td:eq(17)").text())) || 0;
        totalGSTU += parseFloat(parseCurrency(row.find("td:eq(18)").text())) || 0;
        totalGST += parseFloat(parseCurrency(row.find("td:eq(19)").text())) || 0;
        totalAmount += parseFloat(parseCurrency(row.find("td:eq(20)").text())) || 0;
        totalAmountwithoutgst += parseFloat(parseCurrency(row.find("td:eq(10)").text())) || 0;
    });
    // Update the hidden inputs with the calculated totals
    $("#sl_rate2").val(totalRate.toFixed(2));
    $("#sl_total_quantity").text(totalQuantity);
    $("#sl_total2").val(totalAmount.toFixed(2));
    $("#sl_total_withoutgst").text(formatAsCurrency(parseFloat(totalAmountwithoutgst)));
    $("#sl_total_igst").text(totalGSTI);
    $("#sl_total_sgst").text(totalGSTS);
    $("#sl_total_cgst").text(totalGSTC);
    $("#sl_total_ugst").text(totalGSTU);
    $("#sl_total_gst").text(formatAsCurrency(parseFloat(totalGST)));
    $("#sl_total_withgst").text(formatAsCurrency(parseFloat(totalAmount.toFixed(2))));
    $("#sl_withgst").text(formatAsCurrency(parseFloat(totalAmount)));
    $("#sl_balance").text(formatAsCurrency(parseFloat(totalAmount)));
}
function toggleReadOnly() {
    $("#flexCheckDefault1").prop("checked") ? $("#sl_dtp_charges").prop("readonly", false) : ($("#sl_dtp_charges").prop("readonly", true), $("#sl_dtp_charges").val(formatAsCurrency(0.0)), final_total());
    $("#flexCheckDefault2").prop("checked") ? $("#sl_pasting_charges").prop("readonly", false) : ($("#sl_pasting_charges").prop("readonly", true), $("#sl_pasting_charges").val(formatAsCurrency(0.0)), final_total());
    $("#flexCheckDefault3").prop("checked") ? $("#sl_framming_charges").prop("readonly", false) : ($("#sl_framming_charges").prop("readonly", true), $("#sl_framming_charges").val(formatAsCurrency(0.0)), final_total());
    $("#flexCheckDefault4").prop("checked") ? $("#sl_fitting_charges").prop("readonly", false) : ($("#sl_fitting_charges").prop("readonly", true), $("#sl_fitting_charges").val(formatAsCurrency(0.0)), final_total());
    $("#flexCheckDefault5").prop("checked") ? $("#sl_installation_charges").prop("readonly", false) : ($("#sl_installation_charges").prop("readonly", true), $("#sl_installation_charges").val(formatAsCurrency(0.0)), final_total());
    $("#flexCheckDefault6").prop("checked") ? $("#sl_shipping_charges").prop("readonly", false) : ($("#sl_shipping_charges").prop("readonly", true), $("#sl_shipping_charges").val(formatAsCurrency(0.0)), final_total());
}
function sqrft() {
    var first = $("#sl_height").val() || 0;
    var second = $("#sl_width").val() || 0;
    var sqrft = $("#sl_size");
    var sqrft_total = (parseFloat(first) * parseFloat(second));
    sqrft.val(sqrft_total.toFixed(2));
}
function rate() {
    var rates = parseCurrency($("#sl_rate").val()) || 0;
    var sqrft = $("#sl_size").val() || 0;
    var amount = $("#sl_samount");
    var rate_amount = (parseFloat(sqrft) * parseFloat(rates));
    amount.val(formatAsCurrency(parseFloat(rate_amount.toFixed(2))));
}
function quan_amount() {
    var ratep = parseCurrency($("#sl_rate").val()) || 0;
    var amtp = parseCurrency($("#sl_samount").val()) || 0
    var amt = parseCurrency($("#sl_samount").val()) == '0' ||$("#sl_samount").val() == '' ? parseFloat(ratep) : parseFloat(amtp);

    var quantity = $("#sl_quantity").val();
    var total = $("#sl_total");
    var total_amount = (parseFloat(amt) * parseFloat(quantity))
    total.val(formatAsCurrency(parseFloat(total_amount)));
    var sl_complete_total = $("#sl_complete_total");
    sl_complete_total.val(formatAsCurrency(parseFloat(total_amount)));
}
function gst() {
    var cgst = parseFloat($("#sl_CGST").val()) || 0;
    var sgst = parseFloat($("#sl_SGST").val()) || 0;
    var igst = parseFloat($("#sl_IGST").val()) || 0;
    var ugst = parseFloat($("#sl_UGST").val()) || 0;
    var total = parseFloat(parseCurrency($("#sl_total").val())) || 0;
    // var total = $("#sl_complete_total").val() || 0;

    var total_amount = total;

    var rcgst = total_amount * cgst / 100;
    var rsgst = total_amount * sgst / 100;
    var rigst = total_amount * igst / 100;
    var rugst = total_amount * ugst / 100;
    $("#sl_IGSTP").val(formatAsCurrency(parseFloat(rigst)));
    $("#sl_SGSTP").val(formatAsCurrency(parseFloat(rsgst)));
    $("#sl_UGSTP").val(formatAsCurrency(parseFloat(rugst)));
    $("#sl_CGSTP").val(formatAsCurrency(parseFloat(rcgst)));
    // var amt = total.value + rcgst.value + rsgst.value + rigst.value;
    var gst = parseFloat(rcgst) + parseFloat(rsgst) + parseFloat(rigst) + parseFloat(rugst);
    var amt = parseFloat(gst) + parseFloat(total_amount);
    $("#sl_gst").val(formatAsCurrency(parseFloat(gst.toFixed(2))));
    $("#sl_complete_total").val(formatAsCurrency(parseFloat(amt.toFixed(2))));
}
function quan_amount2() {
    var rate = $("#sl_rate2").val();
    var quantity = $("#sl_quantity2").val();
    var total = $("#sl_total2").val();
    var total_amount = (parseFloat(rate.value) * parseFloat(quantity.value))
    total.value = total_amount.toFixed(2);
}


function final_total() {
    var subtotal = parseCurrency($("#sl_total_withoutgst").text()) || 0;
    var totalgst = parseCurrency($('#sl_total_gst').text()) || 0;
    var discount = parseCurrency($("#sl_discount").val()) || 0;
    var dtpcharges = parseCurrency($("#sl_dtp_charges").val()) || 0;
    var dtp_value = parseFloat(dtpcharges);
    var advance = parseCurrency($("#sl_advance").val()) || 0;
    var transport_charges = parseCurrency($("#sl_shipping_charges").val()) || 0;
    var transport_value = parseFloat(transport_charges);
    var sl_installation_charges = parseCurrency($("#sl_installation_charges").val()) || 0;
    var instvalue = parseFloat(sl_installation_charges);
    var fitting = parseCurrency($("#sl_fitting_charges").val()) || 0;
    var fitting_value = parseFloat(fitting);
    var sl_framming_charges = parseCurrency($("#sl_framming_charges").val()) || 0;
    var framming_value = parseFloat(sl_framming_charges);
    var pasting = parseCurrency($("#sl_pasting_charges").val()) || 0;
    var pasting_value = parseFloat(pasting);
    var total_dtp_transport = (parseFloat(transport_value) + parseFloat(dtp_value)) + parseFloat(framming_value) + parseFloat(fitting_value) + parseFloat(instvalue) + parseFloat(pasting_value);
    var total_amount = (parseFloat(subtotal) + parseFloat(total_dtp_transport));
    var totalwithgst = (parseFloat(subtotal) + parseFloat(total_dtp_transport)) + totalgst;
    if (advance == "0" && discount == "0") {
        var balance = (parseFloat(total_amount)) + totalgst;
        var roundoff = formatAsCurrency(parseFloat(balance.toFixed(2)));
        $("#sl_balance").text(roundoff);
        $("#sl_total_withgst").text(formatAsCurrency(parseFloat(totalwithgst.toFixed(2))));
    }
    else if (advance != "0" || discount != "0") {
        var balance = (parseFloat(total_amount) - parseFloat(advance) - parseFloat(discount)) + totalgst;
        var roundoff = formatAsCurrency(parseFloat(balance.toFixed(2)));
        $("#sl_balance").text(roundoff);
        $("#sl_total_withgst").text(formatAsCurrency(parseFloat(totalwithgst.toFixed(2))));
    }

}
function Excel() {
    window.location.href = '/SaleOrder/Excel';
}

function Pdf() {
    window.location.href = '/SaleOrder/Pdf';
}
function payment() {
    var sl_invoice_no = $('#doNoValue').text();
    var sl_id = $('#sl_id').text();
    var sl_cust_id = $('#sl_cust_id').text();
    var sl_cust_name = $('#sl_cust_name').text();
    //var paymentmethod = $('#sl_payment_method').find("option:selected").text();
    var paymentmethod = $('#sl_payment_method').text();
    var sl_balance = parseCurrency($('#sl_balance').text());
    //if (sl_balance == "0" || sl_balance == "0.00" || sl_balance == "00.00") {
    //    alert("Payment is already done");
    //    return false;
    //}
    //else {
        window.location.href = '/SaleOrder/Payment?sl_invoice_no=' + sl_invoice_no + '&sl_cust_name=' + sl_cust_name + '&sl_balance=' + sl_balance + '&sl_cust_id=' + sl_cust_id + "&sl_id=" + sl_id + "&type=" + paymentmethod;
    //}
}
function payamount() {
    var sp_sl_balance = parseCurrency($("#sp_due").val());
    var sp_pay = parseFloat($('#sp_pay').val()) || 0;
    var sp_discount = parseCurrency($("#sp_discount").val()) || 0;
    var total = parseFloat(sp_sl_balance) - (parseFloat(sp_discount) + parseFloat(sp_pay));
    $("#sp_balance").val(formatAsCurrency(total));
}
function submitpayment() {
    /* event.preventDefault();*/
    if (parseCurrency($('#sp_due').val()) == '0' || parseCurrency($('#sp_due').val()) == '0.0') {
        alert("Payment is already done!");
        return false;
    } else
    if ($('#sp_due').val() == $('#sp_balance').val()) {
        alert("Please enter payment details!");
        return false;
    } else if ($('#sp_discount').val() == '') {
        alert("Please enter discount!");
        return false;
    } else if ($('#sp_pay').val() == '' || $('#sp_pay').val() == '0') {
        alert("Please enter pay!");
        return false;
    } else if ($('#sp_mode').val() == '') {
        alert("Please select payment method!");
        return false;
    } else if ($('#sp_mode option:selected').text() == 'Cheque') {
        if ($('#sp_bank_name').val() == '') {
            alert("Please enter bank name!");
            return false;
        } else if ($('#sp_chq_no').val() == '') {
            alert("Please enter cheque number!");
            return false;
        } else if ($('#sp_chq_date').val() == '') {
            alert("Please select cheque date!");
            return false;
        }
    } else if ($('#sp_mode option:selected').text() == 'UPI') {
        if ($('#sp_upi_id').val() == '') {
            alert("Please enter upi id!");
            return false;
        }
    }

    var data = {
        sp_invoice_no: $('#sp_invoice_no').val(),
        sp_invoice_id: $('#sp_sl_id').val(),
        sp_cust_id: $('#sp_cust_id').val(),
        sp_cust_name: $('#sp_cust_name').val(),
        sp_PaymentType: $('#sp_PaymentType').val(),
        sp_due: parseCurrency($('#sp_due').val()),
        sp_discount: $('#sp_discount').val(),
        sp_pay: $('#sp_pay').val(),
        sp_mode: $('#sp_mode').val(),
        sp_chq_no: $('#sp_chq_no').val(),
        sp_balance: parseCurrency($('#sp_balance').val()),
        sp_date: $('#sp_date').val(),
        sp_chq_date: $('#sp_chq_date').val(),
        sp_upi_id: $('#sp_upi_id').val(),
        sp_bank_name: $('#sp_bank_name').val()

    };
    $.ajax({
        type: 'POST',
        url: '/SaleOrder/CreatePayment', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {

            window.location.href = '/SaleOrder/Payment?sl_invoice_no=' + response.sp_invoice_no + '&sl_cust_name=' + response.sp_cust_name + '&sl_balance=' + response.sp_balance + '&sl_cust_id=' + response.sp_cust_id + '&type=' + response.sp_PaymentType + '&sl_id=' + response.sp_invoice_id;

        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}
function cancel() {
    window.location.href = "/SaleOrder/Index";
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
    var total = parseInt(inputElement.value) + parseFloat(parseCurrency($("#sl_advance").val()) || 0);
    if (total >= parseFloat(parseCurrency($("#" + id).text()))) {
        inputElement.value = parseCurrency($("#" + id).text()) - parseFloat(parseCurrency($("#sl_advance").val()) || 0); // Set the value to the maximum allowe
    }
}
function checkAMax(inputElement, id) {
    var total = parseInt(inputElement.value) + parseFloat(parseCurrency($("#sl_discount").val()) || 0);
    if (total >= parseFloat(parseCurrency($("#" + id).text()))) {
        inputElement.value = parseCurrency($("#" + id).text()) - parseFloat(parseCurrency($("#sl_discount").val()) || 0); // Set the value to the maximum allowed
    }
}
function setMinDate() {
    var currentDate = new Date().toISOString().split('T')[0];
    document.getElementById('sl_chq_date').min = currentDate;
    document.getElementById('sl_due_date').min = currentDate;
    document.getElementById('sp_chq_date').min = currentDate;
}

function checkDPamynetMax(inputElement, id) {
    var total = parseInt(inputElement.value) + parseFloat(parseCurrency($("#sp_pay").val()) || 0);
    if (total >= parseFloat(parseCurrency($("#" + id).val()))) {
        inputElement.value = parseCurrency($("#" + id).val()) - parseFloat(parseCurrency($("#sp_pay").val()) || 0); // Set the value to the maximum allowe
    }
}
function checkPaymentMax(inputElement, id) {
    var total = parseInt(inputElement.value) + parseFloat(parseCurrency($("#sp_discount").val()) || 0);
    if (total >= parseFloat(parseCurrency($("#" + id).val()))) {
        inputElement.value = parseCurrency($("#" + id).val()) - parseFloat(parseCurrency($("#sp_discount").val()) || 0); // Set the value to the maximum allowed
    }
}
function zero() {
    if ($('#sl_width').val() === '' || $('#sl_width').val() === '0') {
        $('#sl_width').val('0');
        $('#sl_samount').val('0');
        $('#sl_size').val('0');
        $('#sl_total').val('0');
        $('#sl_complete_total').val('0');
    }
    else if ($('#sl_height').val() === '' || $('#sl_height').val() === '0') {
        $('#sl_height').val('0');
    }
    else if ($('#sl_quantity').val() === '' || $('#sl_quantity').val() === '0') {
        $('#sl_quantity').val('0');
    }
    else if ($('#sl_total').val() === '' || $('#sl_total').val() === '0') {
        $('#sl_total').val('0');
    }
    else if ($('#sl_samount').val() === '' || $('#sl_samount').val() === '0') {
        $('#sl_samount').val('0');
    }
    else if ($('#sl_complete_total').val() === '' || $('#sl_complete_total').val() === '0') {
        $('#sl_complete_total').val('0');
    }

}
function zero1() {
    if ($('#sl_height').val() === '' || $('#sl_height').val() === '0') {
        $('#sl_height').val('0');
        $('#sl_samount').val('0');
        $('#sl_size').val('0');
        $('#sl_total').val('0');
        $('#sl_complete_total').val('0');
    }
    else if ($('#sl_quantity').val() === '' || $('#sl_quantity').val() === '0') {
        $('#sl_quantity').val('0');
    }
    else if ($('#sl_total').val() === '' || $('#sl_total').val() === '0') {
        $('#sl_total').val('0');
    }
    else if ($('#sl_samount').val() === '' || $('#sl_samount').val() === '0') {
        $('#sl_samount').val('0');
    }
    else if ($('#sl_complete_total').val() === '' || $('#sl_complete_total').val() === '0') {
        $('#sl_complete_total').val('0');
    }
}
function zero2() {
    if ($('#sl_quantity').val() === '' || $('#sl_quantity').val() === '0') {
        $('#sl_quantity').val('0');
        $('#sl_samount').val('0');
        $('#sl_size').val('0');
        $('#sl_total').val('0');
        $('#sl_complete_total').val('0');
    }
    else if ($('#sl_total').val() === '' || $('#sl_total').val() === '0') {
        $('#sl_total').val('0');
    }
    else if ($('#sl_samount').val() === '' || $('#sl_samount').val() === '0') {
        $('#sl_samount').val('0');
    }
    else if ($('#sl_complete_total').val() === '' || $('#sl_complete_total').val() === '0') {
        $('#sl_complete_total').val('0');
    }
}
