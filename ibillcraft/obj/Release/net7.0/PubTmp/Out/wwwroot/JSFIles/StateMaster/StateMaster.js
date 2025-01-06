function GoToCity(co_id, co_country_name, s_id, s_state_name) {
    window.location.href = '/CityMaster?CountryId=' + co_id + '&CountryName=' + co_country_name + '&StateId=' + s_id + '&StateName=' + s_state_name;
}

function AddState() {
    const urlParams = new URLSearchParams(window.location.search);

    // Extract the values
    const countryId = urlParams.get("CountryId");
    const countryName = urlParams.get("CountryName");

    $("#createModal").modal("show");
    //$("#co_id").val('');
    //$("#s_country_name").val('');


    $("#s_state_name").val('');
    $("#s_country_name").val(countryName);
    $("#s_country_id").val(countryId);
    $('#btnsubmit').prop('disabled', false);

    $("#s_isactive").val('');



} 
function ClearData() {

    $('#s_id').val(''),
        $('#s_country_name').val(''),
        $('#s_country_id').val(''),
        $('#s_state_name').val('')

}
/* Submit Button  */
function submitState() {
    /* event.preventDefault();*/

    if ($('#s_state_name').val() == '') {
        alert("Please enter state name!");
        return false;
    }

    var isActive = $('#s_isactive').is(':checked') ? 1 : 0;
    var data = {
        s_id: $('#s_id').val(),
        s_country_id: $('#s_country_id').val(),
        s_country_name: $('#s_country_name').val(),
        s_state_name: $('#s_state_name').val(),
        s_state_code: $('#s_state_code').val(),
        s_isactive: isActive,
    };
    $.ajax({
        type: 'POST',
        url: '/StateMaster/Create', // Use the form's action attribute
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
function editLinkClick(s_id) {

    $.get("./StateMaster/Edit",
        { s_id: s_id },
        function (data) {
            console.log(data);
            $("#createModal").modal("show");


            $('#s_id').val(data.s_id);
            $('#s_country_id').val(data.s_country_id);
            $("#s_country_name").val(data.s_country_name);
            $("#s_state_name").val(data.s_state_name);
            $("#s_state_code").val(data.s_state_code);

            var isActive = $('#s_isactive').is(':checked') ? 1 : 0;
            $('#s_isactive').val(data.isActive);
            var isActive = $('#s_isactive').val(data.s_isactive);

            if (data.s_isactive === "Active") {
                $('#s_isactive').prop('checked', true);
            } else {
                $('#s_isactive').prop('checked', false);
            }


        });



}

function GoToCity(co_id, co_country_name, s_id, s_state_name) {
    window.location.href = '/CityMaster?CountryId=' + co_id + '&CountryName=' + co_country_name + '&StateId=' + s_id + '&StateName=' + s_state_name;
}


function ToggleSwitchCon(s_id, s_country_name, s_country_id, s_state_code,s_state_name, s_updatedby,status) {
    var data = {

        s_id: s_id,
        s_country_name: s_country_name,
        s_country_id: s_country_id,
        s_state_code: s_state_code,
        s_state_name: s_state_name,
        s_updatedby: s_updatedby,
        s_isactive: status
    };

    $.ajax({
        type: 'POST',
        url: '/StateMaster/Create', // Use the correct URL or endpoint
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
//function Excel() {
//    var s_country_id = $('#CountryId').val()
//    window.location.href = "./StateMaster/Excel?countryId=" + s_country_id;
//}

//function Pdf() {
//    var s_country_id = $('#CountryId').val()
//    window.location.href = "./StateMaster/Pdf?countryId=" + s_country_id;
//}
function Excel() {
    var s_country_id = $('#CountryId').val()
    var countryname = $("#CountryName").val();
    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/StateMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel' + ('?countryId=' + s_country_id) + ('&CountryName=' + countryname) + (statusValue ? '&status=' + statusValue : '');

    window.location.href = newUrl;


}

function Pdf() {
    var s_country_id = $('#CountryId').val();
    var countryname = $("#CountryName").val();
    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/StateMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + ('?countryId=' + s_country_id) + ('&CountryName=' + countryname) + (statusValue ? '&status=' + statusValue : '');

    window.location.href = newUrl;
}
function performAction() {
    var s_country_id = $('#CountryId').val()
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    var countryname = $("#CountryName").val();
    window.location.href = "/StateMaster/Index?CountryId=" + s_country_id + "&CountryName=" + countryname + "&status=" + status;

}