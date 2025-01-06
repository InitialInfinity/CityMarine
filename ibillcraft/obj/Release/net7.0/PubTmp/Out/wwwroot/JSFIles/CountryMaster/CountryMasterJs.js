function AddCountry() {


    $("#createModal").modal("show");
    //$("#co_id").val('');
    $("#co_country_name").val('');
    $("#co_country_code").val('');
    $("#co_currency_name").val('');
    $("#co_currency_id").val('');
    $("#co_timezone").val('');

    $("#co_isactive").val('');
    $('#btnsubmit').prop('disabled', false);


}
function CancelData() {
    $("#createModal").modal("hide");

}
function ClearData() {

    $('#co_id').val(''),
        $('#co_country_name').val(''),
        $('#co_country_code').val(''),
        $('#co_currency_name').val(''),
    $('#co_currency_name').val(''),
        $('#co_timezone').val('')
}
/* Submit Button  */
function submitCountry() {
    /* event.preventDefault();*/

    if ($('#co_country_code').val().trim() == '') {
        alert("Please enter country code!");
        return false;
    }
    else if ($('#co_country_name').val().trim() == '') {
        alert("Please enter country name!");
        return false;
    }

    else if ($('#co_currency_name').val().trim() == '') {
        alert("Please select currency!");
        return false;
    }
    else if ($('#co_timezone').val().trim() == '') {
        alert("Please enter timezone!");
        return false;
    }
    var isActive = $('#co_isactive').is(':checked') ? 1 : 0;
    var data = {
        co_id: $('#co_id').val(),
        co_country_name: $('#co_country_name').val(),
        co_country_code: $('#co_country_code').val(),
        co_currency_name: $('#co_currency_name').find(":selected").text(),
        co_currency_id: $('#co_currency_name').val(),
        co_timezone: $('#co_timezone').val(),
        co_isactive: isActive
    };
    $.ajax({
        type: 'POST',
        url: '/CountryMaster/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            // Handle success
            // console.log(response);
            $("#createModal").modal("hide");
            //alert(response);

            window.location.reload();
        },


        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}


/* For Edit Submit Button  */
function editLinkClick(co_id) {

    $.get("/CountryMaster/Edit",
        { co_id: co_id },
        function (data) {
            console.log(data);
            $("#createModal").modal("show");


            $('#co_id').val(data.co_id);
            $("#co_country_name").val(data.co_country_name);
            $("#co_country_code").val(data.co_country_code);

            //var isActive = $('#co_isactive').is(':checked') ? 1 : 0;
            //$('#co_isactive').val(data.isActive);
            var isActive = $('#co_isactive').val(data.co_isactive);

            if (data.co_isactive === "Active") {
                $('#co_isactive').prop('checked', true);
            } else {
                $('#co_isactive').prop('checked', false);
            }



            var currencyDropdown = document.getElementById("co_currency_name");
            for (var i = 0; i < currencyDropdown.options.length; i++) {
                if (currencyDropdown.options[i].value === data.co_currency_id) {
                    currencyDropdown.selectedIndex = i;
                    break; // Exit the loop once the match is found
                }
            }

            //$("#co_currency_name").selectedIndex=data.co_currency_name;
            $("#co_currency_id").val(data.co_currency_id);
            $("#co_timezone").val(data.co_timezone);

        });
}
function GoToState(co_id, co_country_name) {
    window.location.href = '/StateMaster?CountryId=' + co_id + '&CountryName=' + co_country_name;
}


function ToggleSwitchCon(co_id, co_country_code, co_country_name, co_currency_id, co_currency_name, co_timezone, status) {
    var data = {
      
        co_id: co_id,
        co_country_code: co_country_code,
        co_country_name: co_country_name,
        co_currency_id: co_currency_id,
        co_currency_name: co_currency_name,
        co_timezone: co_timezone,
        co_isactive: status
    };

    $.ajax({
        type: 'POST',
        url: '/CountryMaster/Create', // Use the correct URL or endpoint
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
 

    var currentUrl = window.location.href;

    
    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/CountryMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;

   
}

function Pdf() {

    //window.location.href = "./CountryMaster/Pdf";
    //var baseUrl = window.location.origin + '/CountryMaster';
    //var newUrl = baseUrl + '/Pdf';
    //window.location.href = newUrl;
    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/CountryMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;
}
function performAction() {

    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';

    window.location.href = "/CountryMaster/Index?status=" + status;

}
