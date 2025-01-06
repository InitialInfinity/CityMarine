function toggleDropdown() {
    const dropdownContent = document.querySelector('.dropdown-content');
    // Toggle visibility of dropdown
    dropdownContent.style.display = dropdownContent.style.display === 'block' ? 'none' : 'block';
}

function SubmitEmployeeEmail() {
    if ($('#e_email').val() == '--Select--') {
        alert("Please select email!");
        return false;
    } 
    var isActive = $('#e_isactive').is(':checked') ? 1 : 0;
    let selectedEmployees = $('#e_employee').find(":checked").map(function () {
        return $(this).val(); // Get the value of each checked checkbox
    }).get().join(",");

    if (selectedEmployees == '') {
        alert("Please select employees!");
        return false;
    }

    var data = {
        e_id: $('#e_id').val(),
        e_email: $('#e_email').val(),
        /*e_employee: $('#e_employee').find(":checked").val(),*/
        e_employee: selectedEmployees,
        e_isactive: isActive,
    };
    $.ajax({
        type: 'POST',
        url: '/EmployeeEmailMgmt/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#createModal").modal("hide");
            window.location.reload();
        },
        error: function (xhr, status, error) {
        }
    });
}

function editLinkClick(e_id,e_emailid, e_email,e_staffname, e_employee, e_isactive) {

    $("#createModal").modal("show");
    $('#e_id').val(e_id);
    $("#e_email").val(e_emailid);
    /*$("#e_employee").val(e_employee);*/
    let ids = e_employee.split(','); // Convert string to array

    // Now use forEach
    ids.forEach(id => {
        $(`#e_employee input[type="checkbox"][value="${id.trim()}"]`).prop("checked", true);
    });
    if (e_isactive === "1") {
        $('#e_isactive').prop('checked', true);
    } else {
        $('#e_isactive').prop('checked', false);
    }
}

function ToggleSwitch(e_id, e_emailid, e_email, e_staffname, e_employee, e_isactive) {
    var data = {

        e_id: e_id,
        e_email: e_emailid,
        e_employee: e_employee,
        e_isactive: e_isactive
    };
    $.ajax({
        type: 'POST',
        url: '/EmployeeEmailMgmt/Create', // Use the correct URL or endpoint
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
    var baseUrl = window.location.origin + '/EmployeeEmailMgmt';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1'; // Set a default value if 'status' is null
    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '');
    window.location.href = newUrl;
}

function Pdf() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    var baseUrl = window.location.origin + '/EmployeeEmailMgmt';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1'; // Set a default value if 'status' is null
    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');
    window.location.href = newUrl;
}
function performAction() {
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/EmployeeEmailMgmt/Index?status=" + status;
}

