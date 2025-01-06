function AddBank() {
    window.location.href = '/Bank/EditIndex';
}
function GoBack() {
    window.location.href = '/Bank/Index';
}

function submitBank() {

    if ($('#b_name').val() == '') {
        alert("Please enter bank name!");
        return false;
    }
    else if ($('#b_ac_name').val() == '') {
        alert("Please enter account name!");
        return false;
    }
    else if ($('#b_ifsc').val() == '') {
        alert("Please enter ifsc code!");
        return false;
    }
    else if ($('#b_ac_no').val() == '') {
        alert("Please enter account number!");
        return false;
    }
    else if ($('#b_opening_balance').val() == '') {
        alert("Please enter opening balance!");
        return false;
    }       
    else if ($('#b_cheqno').val() == '') {
        alert("Please enter cheque number!");
        return false;
    }
    else if ($('#b_status_of_chq').val() == '') {
        alert("Please enter status of cheque!");
        return false;
    }


    var isActive = $('#b_isactive').is(':checked') ? 1 : 0;
    var data = {
        b_id: $('#b_id').val(),
        b_name: $('#b_name').val(),
        b_ac_name: $('#b_ac_name').val(),
        b_ifsc: $('#b_ifsc').val(),
        b_ac_no: $('#b_ac_no').val(),
        b_opening_balance: $('#b_opening_balance').val(),
        b_desc: $('#b_desc').val(),
        b_cheqno: $('#b_cheqno').val(),
        b_status_of_chq: $('#b_status_of_chq').val(),
        b_isactive: isActive,
    };
    $.ajax({
        type: 'POST',
        url: '/Bank/Create',
        data: data,
        success: function (response) {

            window.location.href = "/Bank/Index";
        },


        error: function (xhr, status, error) {
        }
    });
}
function ToggleSwitchCon(b_id, b_name, b_ac_name, b_ifsc, b_ac_no, b_opening_balance, b_desc, b_isactive) {
    var isActive = $('#b_isactive').is(':checked') ? 1 : 0;
    var data = {

        b_id: b_id,
        b_name: b_name,
        b_ac_name: b_ac_name,
        b_ifsc: b_ifsc,
        b_ac_no: b_ac_no,
        b_opening_balance: b_opening_balance,
        b_desc: b_desc,
        b_isactive: b_isactive,
    };

    $.ajax({
        type: 'POST',
        url: '/Bank/Create',
        data: data,
        success: function (response) {
            window.location.reload();
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}
function Excel() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    var baseUrl = window.location.origin + '/Bank';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1';
    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '');
    window.location.href = newUrl;
}
function Pdf() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    var baseUrl = window.location.origin + '/Bank';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1';
    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');
    window.location.href = newUrl;
}

function performAction() {
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/Bank/Index?status=" + status;
}

function validateIFSC(ifsc) {
    // Remove spaces and convert to uppercase
    ifsc = ifsc.replace(/\s+/g, '').toUpperCase();

    // Check if the IFSC code length is valid
    if (ifsc.length !== 11) {
        alert("Enter Valid IFSC Code!");
        $("#b_ifsc").val('');
        return false;
    }

    // Check if the first four characters are alphabets
    if (!/^[A-Z]{4}/.test(ifsc)) {
        alert("Enter Valid IFSC Code!");
        $("#b_ifsc").val('');
        return false;
    }

    // Check if the fifth character is 0
    if (ifsc.charAt(4) !== '0') {
        alert("Enter Valid IFSC Code!");
        $("#b_ifsc").val('');
        return false;
    }

    // Check if the remaining characters are alphanumeric
    if (!/^[0-9A-Z]+$/.test(ifsc.substring(5))) {
        alert("Enter Valid IFSC Code!");
        $("#b_ifsc").val('');
        return false;
    }

}