function AddProduct() {
    window.location.href = '/Product/EditIndex';
}
function GoBack() {
    $('#p_name').val('');
    $('#p_unitid').val('');
    $('#p_rate').val('');
    $('#p_cgst').val('');
    $('#p_sgst').val('');
    $('#p_igst').val('');
    $('#p_ugst').val('');
    $('#p_desc').val('');
    $('#p_hsn_code').val('');
    window.location.href = '/Product/Index';
}
function clrbtn() {
    $('#p_name').val('');
    $('#p_unitid').val('');
    $('#p_rate').val('');
    $('#p_cgst').val('');
    $('#p_sgst').val('');
    $('#p_igst').val('');
    $('#p_ugst').val('');
    $('#p_desc').val('');
    $('#p_hsn_code').val('');
}
function OnSale() {
    window.location.href = '/Product/Index?status=1&p_type=Sale';
}

function OnPurchase() {
    window.location.href = '/Product/Index?status=1&p_type=Purchase';
}

function submitProduct() {
    if ($('#p_name').val() == '') {
        alert("Please enter product name!");
        return false;
    }
    else if ($('#p_unitid').val() == '') {
        alert("Please select unit!");
        return false;
    } else if ($('#p_hsn_code').val() == '') {
        alert("Please enter hsn code!");
        return false;
    }
    else if ($('#p_rate').val() == '') {
        alert("Please enter rate!");
        return false;
    }
    else if ($('#p_cgst').val() == '--Select--') {
        alert("Please select csgt!");
        return false;
    }
    else if ($('#p_sgst').val() == '--Select--') {
        alert("Please select sgst");
        return false;
    }
    else if ($('#p_igst').val() == '--Select--') {
        alert("Please select isgt!");
        return false;
    } else if ($('#p_ugst').val() == '--Select--') {
        alert("Please select usgt!");
        return false;
    }
    if ($('input[name="inlineRadioOptions"]:checked').length === 0) {
        alert("Please select product category!");
        return false;
    }
    var p_cgst = parseFloat($('#p_cgst').val()) || 0;
    var p_sgst = parseFloat($('#p_sgst').val()) || 0;
    var p_igst = parseFloat($('#p_igst').val()) || 0;
    var p_ugst = parseFloat($('#p_ugst').val()) || 0;
    var total_gst = p_cgst + p_sgst + p_igst + p_ugst;
    var p_gst = (total_gst / 100) * 100;
    // Fetch the selected value
    var selectedValue = $('input[name="inlineRadioOptions"]:checked').val();

    var isActive = $('#p_isactive').is(':checked') ? 1 : 0;
    var data = {
        p_id: $('#p_id').val(),
        p_name: $('#p_name').val(),
        p_unit: $('#p_unitid option:selected').text(),
        p_unitid: $('#p_unitid').val(),
        p_rate: parseCurrency($('#p_rate').val()),
        p_cgst: $('#p_cgst').val()||0,
        p_sgst: $('#p_sgst').val()||0,
        p_igst: $('#p_igst').val()||0,
        p_ugst: $('#p_ugst').val()||0,
        p_gst: p_gst,
        p_type: selectedValue,
        p_desc: $('#p_desc').val(),
        p_hsn_code: $('#p_hsn_code').val(),

        p_isactive: isActive,
    };
    $.ajax({
        type: 'POST',
        url: '/Product/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {

            window.location.href = "/Product/Index";
        },


        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
function ToggleSwitchCon(p_id, p_name, p_rate, p_desc, p_unit, p_unitid, p_igst, p_cgst, p_sgst, p_ugst, p_gst, p_hsn_code, p_type, status) {
    var data = {
        p_id: p_id, p_rate: parseCurrency(p_rate), p_desc: p_desc, p_name: p_name,
        p_igst: p_igst, p_cgst: p_cgst, p_sgst: p_sgst, p_ugst: p_ugst, p_gst: p_gst,
        p_hsn_code: p_hsn_code, p_type: p_type, p_unit: p_unit, p_unitid: p_unitid,
        p_isactive: status
    };

    $.ajax({
        type: 'POST',
        url: '/Product/Create', // Use the correct URL or endpoint
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
function Excel(p_type) {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    var baseUrl = window.location.origin + '/Product';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1'; // Set a default value if 'status' is null
    var newUrl = baseUrl + '/Excel' + ('?p_type=' + p_type) + (statusValue ? '&status=' + statusValue : '');
    window.location.href = newUrl;


}

function Pdf(p_type) {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    var baseUrl = window.location.origin + '/Product';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1'; // Set a default value if 'status' is null
    var newUrl = baseUrl + '/Pdf' + ('?p_type=' + p_type) + (statusValue ? '&status=' + statusValue : '');
    window.location.href = newUrl;
}
function performAction(p_type) {
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/Product/Index?status=" + status + "&p_type=" + p_type;
}
function performActionforp(p_type) {
    var status = ($('#PtoggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/Product/Index?status=" + status + "&p_type=" + p_type;
}