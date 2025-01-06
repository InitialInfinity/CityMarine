function OnTerms() {
    window.location.href = '/Configuration/Index?type=Terms';
}

function OnNote() {
    window.location.href = '/Configuration/Index?type=Note';
}
function OnSeries() {
    window.location.href = '/Configuration/Index?type=Series';
}


function submitTerms() {
    if ($('#tc_terms').val() == '') {
        alert("Please Enter Terms & Conditions!");
        return false;
    }
    
    var data = {
        tc_id: $('#tc_id').val(),
        tc_terms: tinymce.get('tc_terms').getContent(),
        tc_invoicename: $('#tc_invoicename').val(),
        type: 'Terms',
    };
    $.ajax({
        type: 'POST',
        url: '/Configuration/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {

            window.location.reload();
        },


        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
function submitSeries() {
    if ($('#s_series').val() == '') {
        alert("Please enter series!");
        return false;
    }
    
    var data = {
        s_id: $('#s_id').val(),
        s_series: $('#s_series').val(),
        s_invoicename: $('#s_invoicename').val(),
        type: 'Series',
    };
    $.ajax({
        type: 'POST',
        url: '/Configuration/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {

            window.location.reload();
        },


        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
function submitNote() {
    if ($('#in_note').val() == '') {
        alert("Please enter note!");
        return false;
    }
   
    var data = {
        in_id: $('#in_id').val(),
        in_note: tinymce.get('in_note').getContent(),
        in_invoicename: $('#in_invoicename').val(),
        type: 'Note',
    };
    $.ajax({
        type: 'POST',
        url: '/Configuration/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {

            window.location.reload();
        },


        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
function cancalTerms() {
    //$('#tc_terms').val('');
    tinymce.get('tc_terms').setContent('');

}
function cancalNote() {
    //$('#in_note').val('');
    tinymce.get('in_note').setContent( '');

}
function cancalSeries() {
    $('#s_series').val('');
}


function editSeriesClick(s_id, s_invoicename, s_series) {
    $("#seriesModal").modal("show");
    $('#s_id').val(s_id);
    $('#s_series').val(s_series);
    $("#s_invoicename").val(s_invoicename);
}
function editNoteClick(in_id, in_invoicename, in_note) {
    $("#noteModal").modal("show");
    $('#in_id').val(in_id);
    $('#in_invoicename').val(in_invoicename);
    tinymce.get('in_note').setContent(in_note || '');
}
function editTermsClick(tc_id, tc_invoicename, tc_terms) {
    $("#termsModal").modal("show");
    $('#tc_id').val(tc_id);
    tinymce.get('tc_terms').setContent(tc_terms || '');

    $("#tc_invoicename").val(tc_invoicename);
}