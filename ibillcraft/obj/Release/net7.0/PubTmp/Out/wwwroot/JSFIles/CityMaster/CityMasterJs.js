function AddCity() {

    const urlParams = new URLSearchParams(window.location.search);

    // Extract the values
    const CountryId = urlParams.get("CountryId");
    const StateId = urlParams.get("StateId");
    const CountryName = urlParams.get("CountryName");
    const StateName = urlParams.get("StateName");
    $("#createModal").modal("show");
    //$("#ci_id").val('');
    $("#ci_country_name").val(CountryName);
    $("#ci_state_name").val(StateName);
    $("#ci_country_id").val(CountryId);
    $("#ci_state_id").val(StateId);
    $("#ci_city_name").val('');
    $("#ci_city_code").val('');

    $("#ci_isactive").val('');
    $('#btnsubmit').prop('disabled', false);


}
function CancelData() {
    $("#createModal").modal("hide");

}
function ClearData() {

   $('#ci_id').val(''),
        //$('#ci_country_name').val(''),
        //$('#ci_state_name').val(''),
        //$('#ci_country_id').val(''),
        //$('#ci_state_id').val(''),
        $('#ci_city_name').val(''),
       $("#ci_city_code").val('');
       
}
/* Submit Button  */
function submitCtiy() {
    /* event.preventDefault();*/
    if ($('#ci_city_code').val().trim() == '') {
        alert("Please enter city code!");
        return false;
    }
    if ($('#ci_city_name').val().trim() == '') {
        alert("Please enter city name!");
        return false;
    }
    //else if ($('#ci_state_name').val().trim() == '') {
    //    alert("Please enter state name!");
    //    return false;
    //}
    ////else if ($('#ci_country_id').val().trim() == '') {
    ////    alert("Please enter country Id!");
    ////    return false;
    ////}
    ////else if ($('#ci_state_id').val().trim() == '') {
    ////    alert("Please enter state Id!");
    ////    return false;
    ////}
    //else if ($('#ci_country_name').val().trim() == '') {
    //        alert("Please enter country name!");
    //        return false;
    //}
    var isActive = $('#ci_isactive').is(':checked') ? 1 : 0;
    var data = {
        ci_id: $('#ci_id').val(),
        ci_city_name: $('#ci_city_name').val(),
        ci_country_id: $('#ci_country_id').val(),
        ci_country_name: $('#ci_country_name').val(),
        ci_state_id: $('#ci_state_id').val(),
        ci_city_code: $('#ci_city_code').val(),
        ci_state_name: $('#ci_state_name').val(),
        ci_isactive: isActive
    };
    $.ajax({
        type: 'POST',
        url: '/CityMaster/Create', // Use the form's action attribute
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
function editLinkClick(ci_id,ci_city_code,ci_city_name) {
    const urlParams = new URLSearchParams(window.location.search);

    // Extract the values
    const CountryId = urlParams.get("CountryId");
    const StateId = urlParams.get("StateId");
    const CountryName = urlParams.get("CountryName");
    const StateName = urlParams.get("StateName");
    
    $("#createModal").modal("show");


            $("#ci_id").val(ci_id);
            $("#ci_city_name").val(ci_city_name);
            $("#ci_country_name").val(CountryName);
            $("#ci_country_id").val(CountryId);
            $("#ci_state_id").val(StateId);
    $("#ci_state_name").val(StateName);
    $("#ci_city_code").val(ci_city_code);

    if (data.ci_isactive === "Active") {
                $('#ci_isactive').prop('checked', true);
            } else {
                $('#ci_isactive').prop('checked', false);
            }

    //$.get("./CityMaster/Edit",
    //    { ci_id: ci_id },
    //    function (data) {
    //        console.log(data);
    //        $("#createModal").modal("show");


    //        $("#ci_id").val(data.ci_id);
    //        $("#ci_city_name").val(data.ci_city_name);
    //        $("#ci_country_name").val(CountryName);
    //        $("#ci_country_id").val(CountryId);
    //        $("#ci_state_id").val(StateId);
    //        $("#ci_state_name").val(StateName);
    //        var isActive = $('#ci_isactive').is(':checked') ? 1 : 0;
    //        $('#ci_isactive').val(data.isActive);
    //        var isActive = $('#ci_isactive').val(data.ci_isactive);

    //        if (data.ci_isactive === "Active") {
    //            $('#ci_isactive').prop('checked', true);
    //        } else {
    //            $('#ci_isactive').prop('checked', false);
    //        }



    //    });
}
function GoToCity(ci_id, ci_country_name, ci_state_id, ci_state_name) {
    window.location.href = '/CityMaster?StateId=' + ci_id + '&CountryName=' + ci_country_name + '&StateId=' + ci_state_id + '&StateName=' + ci_state_name;
}


function ToggleSwitchCon(ci_id, ci_country_name, ci_state_name, ci_country_id, ci_state_id,ci_city_code,ci_city_name, status) {
    var data = {

        ci_id: ci_id,
        ci_country_name: ci_country_name,
        ci_state_name: ci_state_name,
        ci_country_id: ci_country_id,
        ci_state_id: ci_state_id,
        ci_city_code: ci_city_code,
        ci_city_name: ci_city_name,
        ci_isactive: status
    };

    $.ajax({
        type: 'POST',
        url: '/CityMaster/Create', // Use the correct URL or endpoint
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
    const urlParams = new URLSearchParams(window.location.search);

    // Extract the values
    const CountryId = urlParams.get("CountryId");
    const StateId = urlParams.get("StateId");
    const CountryName = urlParams.get("CountryName");
    const StateName = urlParams.get("StateName");
    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/CityMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '') + ('&CountryId=' + CountryId) + ('&CountryName=' + CountryName) + ('&StateId=' + StateId) + ('&StateName=' + StateName);

    window.location.href = newUrl;

}

function Pdf() {
    const urlParams = new URLSearchParams(window.location.search);

    // Extract the values
    const CountryId = urlParams.get("CountryId");
    const StateId = urlParams.get("StateId");
    const CountryName = urlParams.get("CountryName");
    const StateName = urlParams.get("StateName");
    //window.location.href = "./CountryMaster/Pdf";
    //var baseUrl = window.location.origin + '/CountryMaster';
    //var newUrl = baseUrl + '/Pdf';
    //window.location.href = newUrl;
    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/CityMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '') + ('&CountryId=' + CountryId) + ('&CountryName=' + CountryName) + ('&StateId=' + StateId) + ('&StateName=' + StateName)
    //var newUrl = baseUrl + '/Pdf' + ('?countryId=' + s_country_id) + ('&CountryName=' + countryname) + (statusValue ? '&status=' + statusValue : '');


    window.location.href = newUrl;
}
function performAction() {
    const urlParams = new URLSearchParams(window.location.search);

    // Extract the values
    const CountryId = urlParams.get("CountryId");
    const StateId = urlParams.get("StateId");
    const CountryName = urlParams.get("CountryName");
    const StateName = urlParams.get("StateName");
    //var ci_country_id = $('#ci_country_id').val();
    //var ci_country_name = $('#ci_country_name').val();
    //var ci_state_id = $('#ci_state_id').val();
    //var ci_state_name = $('#ci_state_name').val();

    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';

    window.location.href = "/CityMaster/Index?status=" + status + "&CountryId=" + CountryId + "&CountryName=" + CountryName +  "&StateId=" + StateId + "&StateName=" + StateName;

}
