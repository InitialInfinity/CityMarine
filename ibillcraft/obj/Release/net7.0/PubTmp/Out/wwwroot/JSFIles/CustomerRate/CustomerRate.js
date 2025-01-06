function AddRate() {
    $("#addSubscription").modal("show");
    $("#AddModalLabel").show();
    $("#EditModalLabel").hide();
    $("#r_cust_id").val('');
    $("#r_product_id").val('');
    $("#r_rate").val('');
    $('#btnsubmit').prop('disabled', false);
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

function ClearData() {
    $('#r_id').val('');
    $("#r_cust_id").val('');
    $("#r_product_id").val('');
    $("#r_rate").val('');
}
function submitRate() {

    if ($('#r_cust_id').val() == '') {
        alert("Please select customer!");
        return false;
    } else
        if ($('#r_product_id').val() == '') {
            alert("Please select product!");
            return false;
        } else
            if ($('#r_rate').val() == '') {
                alert("Please enter rate!");
                return false;
            }
    var data = {
        r_id: $('#r_id').val(),
        r_product_id: $('#r_product_id').val(),
        r_product_name: $('#r_product_id').find(":selected").text(),
        r_cust_id: $('#r_cust_id').val(),
        r_cust_name: $('#r_cust_id').find(":selected").text(),
        r_rate: parseCurrency($('#r_rate').val())
    };
    $.ajax({
        type: 'POST',
        url: '/CustomerRate/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#addSubscription").modal("hide");
            //alert(response);
            window.location.reload();
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
function editLinkClick(r_id, r_cust_id, r_product_id, r_rate) {
    $("#addSubscription").modal("show");
    $("#EditModalLabel").show();
    $("#AddModalLabel").hide();

    $('#r_id').val(r_id);
    $('#r_product_id option[value="' + r_product_id + '"]').prop('selected', true);
    $('#r_cust_id option[value="' + r_cust_id + '"]').prop('selected', true);
    $('#r_product_id').prop('disabled', true);
    $('#r_cust_id').prop('disabled', true);
    $("#r_rate").val(formatAsCurrency(parseFloat(r_rate)));
}
function Excel() {
    window.location.href = "./CustomerRate/Excel";
}

function Pdf() {

    window.location.href = "./CustomerRate/Pdf";
}
