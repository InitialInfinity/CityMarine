$(document).ready(function () {
    //var imap = document.getElementById("imap");
    //var pop = document.getElementById("pop");
    //if (imap.checked == true) {
    //    imap();
    //}
    //else if (pop.checked == true) {
    //    pop();
    //}
});
function imap() {
    $('#imapinput').css('display', 'block');
    $('#popinput').css('display', 'none');
}
function pop() {
    $('#imapinput').css('display', 'none');
    $('#popinput').css('display', 'block');
}


function AddParameter() {

    $("#createModal").modal("show");
    //$("#co_id").val('');
    $("#E_id").val('');
    $("#E_email").val('');
    $("#E_password").val('');
    $("#E_smtph").val('');
    $("#E_smtpp").val('');
    $("#E_imaph").val('');
    $("#E_imapp").val('');
    $("#E_poph").val('');
    $("#E_popp").val('');
    $("#E_key").val('');
    $("#E_isactive").val('');
    $("#E_issslEnable").val('');
    imap();
    $('#btnsubmit').prop('disabled', false);

}


function CancelData() {
    $("#createModal").modal("hide");
    window.location.reload()
}
function submitParameter() {
    var imap = document.getElementById("imap");
    var pop = document.getElementById("pop");
    var imapinput = document.getElementById("imapinput");
    var popinput = document.getElementById("popinput");

    //var wdv2 = document.getElementById("dpagwmethod2");
    //var slm2 = document.getElementById("dpagsmethod2");

    if ($('#E_email').val().trim() == '') {
        alert("Please enter EmailId!");
        return false;
    }
    if ($('#E_password').val().trim() == '') {
        alert("Please enter password!");
        return false;
    }
    if ($('#E_smtph').val().trim() == '') {
        alert("Please enter SMTP Host!");
        return false;
    }
    if ($('#E_smtpp').val().trim() == '') {
        alert("Please enter SMTP Port!");
        return false;
    }

    if (imap.checked == true || pop.checked == true) {
        if (imap.checked) {
            if ($('#E_imaph').val() == '') {
                alert("Please enter IMAP Host!");
                return false;
            }
            if ($('#E_imapp').val() == '') {
                alert("Please enter IMAP Port!");
                return false;
            }
            $('#E_poph').val('');
            $('#E_popp').val('');
        }



        if (pop.checked) {


            if ($('#E_poph').val() == '') {
                alert("Please enter POP3 host!");
                return false;
            }

           
            if ($('#E_popp').val() == '') {
                alert("Please enter POP3 Port!");
                return false;
            }
         
            $('#E_imaph').val('');
            $('#E_imapp').val('');
         
        }


    }

    if ($('#E_key').val().trim() == '') {
        alert("Please enter Oauth key!");
        return false;
    }

    var E_issslEnable = $('#E_issslEnable').is(':checked') ? 1 : 0;
    
    var data = {
        E_id: $('#E_id').val(),
       /* assetgroup: assetgroupvalue,*/

        E_email: $('#E_email').val(),
        E_password: $('#E_password').val(),
        E_smtph: $('#E_smtph').val(),
        E_smtpp: $('#E_smtpp').val(),
        E_imaph: $('#E_imaph').val(),
        E_imapp: $('#E_imapp').val(),
        E_poph: $('#E_poph').val(),
        E_popp: $('#E_popp').val(),
        E_key: $('#E_key').val(),
        E_issslEnable: E_issslEnable
       
    };


    $.ajax({
        type: 'POST',
        url: '/EmailList/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            window.location.href = "/EmailList/Index";


        },

        error: function (xhr, status, error) {

        }
    });
}
function editLinkClick(E_id, E_email, E_password, E_smtph, E_smtpp, E_imaph, E_imapp, E_poph, E_popp, E_key ,E_issslEnable) {
    
    $("#createModal").modal("show");

    $('#E_id').val(E_id);
    $("#E_email").val(E_email);
    $("#E_password").val(E_password);
    $("#E_smtph").val(E_smtph);
    $("#E_smtpp").val(E_smtpp);
    $("#E_imaph").val(E_imaph);
    $("#E_imapp").val(E_imapp);
    $("#E_poph").val(E_poph);
    $("#E_popp").val(E_popp);
    $("#E_key").val(E_key);

    if (E_issslEnable === "True") {
        $('#E_issslEnable').prop('checked', true);
    } else {
        $('#E_issslEnable').prop('checked', false);
    }
        if (E_imaph !='') {
            $('#imap').prop('checked', true);
            imap();
        }
        if (E_poph != '') {
            $('#pop').prop('checked', true);
            pop();
        }
}
function ToggleSwitch(E_id, E_email, E_password, E_smtph, E_smtpp, E_isactive) {
    var data = {

        E_id: E_id,
        E_email: E_email,
        E_password: E_password,
        E_smtph: E_smtph,
        E_smtpp: E_smtpp,
        E_isactive: E_isactive
    };
    $.ajax({
        type: 'POST',
        url: '/EmailList/Create', // Use the correct URL or endpoint
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
    window.location.href = '/EmailList/Excel';

   
    

}

function Pdf() {
    window.location.href = '/EmailList/Pdf';

   
}
function performAction() {
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/EmailList/Index?status=" + status;

}
function trimInput() {
    var code = document.getElementById("E_email");
    code.value = code.value.trim();

    var code = document.getElementById("E_password");
    code.value = code.value.trim();

}
function validateInput() {
    var inputElement = document.getElementById("E_password");
    var inputValue = inputElement.value;

    // Remove non-alphabetic characters from the input
    var sanitizedValue = inputValue.replace(/[^a-zA-Z]/g, '');

    // Update the input value with the sanitized value
    inputElement.value = sanitizedValue;


    var inputElement1 = document.getElementById("E_email");
    var inputValue1 = inputElement1.value;

    // Remove non-numeric characters from the input
    var sanitizedValue1 = inputValue1.replace(/[^0-9]/g, '');

    // Update the input value with the sanitized value
    inputElement1.value = sanitizedValue1;
}