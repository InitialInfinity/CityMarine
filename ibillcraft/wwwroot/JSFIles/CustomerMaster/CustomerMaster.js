function AddParameter() {

    $("#createModal").modal("show");
    //$("#co_id").val('');
  
    $('#btnsubmit').prop('disabled', false);

}
document.addEventListener("DOMContentLoaded", () => {
    // Attach event listener to all elements with the 'download-link' class
    document.querySelectorAll(".download-link").forEach(link => {
        link.addEventListener("click", () => {
            // Get the file path from the data-file attribute
            const filePath = link.getAttribute("data-file");

            if (filePath) {
                // Encode the file path to make it safe for URLs
                const encodedPath = encodeURIComponent(filePath);

                // Construct the URL to call the controller
                const url = `/CustomerMaster/DownloadFile?filePath=${encodedPath}`;

                // Open the file in a new tab or start downloading
                window.open(url, "_blank");
            } else {
                console.error("File path is missing.");
            }
        });
    });
});




var addedRowsData = [];

function validateGSTNumber(GSTNumber) {
    var countryCode = $("#co_country_code").val();
    var input = GSTNumber.value.toUpperCase();

    var patterns = {
        '1': /^\d{2}-\d{7}$/, // United States
        '44': /^(GB)?\d{3} \d{4} \d{2}(\d{3})?$/, // United Kingdom
        '61': /^\d{2}-\d{3}-\d{3}-\d{3}$/, // Australia
        '49': /^\d{2}-\d{9}$/, // Germany
        '33': /^\d{2} \d{3} \d{3} \d{3}$/,
        '91': /^\d{2}[A-Z]{5}\d{4}[A-Z]\d{1}[A-Z]\d{1}$/
        // Add more country patterns...
    };


    if (input !== '' && countryCode in patterns) {

        if (patterns[countryCode].test(input)) {

            return true;
        } else {
            alert('Please enter a valid GST no!');

            // Clear the input field if invalid
            $("#c_gst_no").val('');
            return false;
        }
    }

}
function validateMobileBycountry(mobilenumber) {

    var input = mobilenumber.value;
    var co_country_code = $("#co_country_code").val();//"+55";//

    if (input !== '' && co_country_code !== '') {
        var countryCodeDigits = co_country_code.replace(/\D/g, ''); // Extract digits from the country code

        // Define an object to map country codes to the expected length and patterns of mobile numbers
        var countryDetails = {
            '1': { length: 10, pattern: /^[2-9]\d{9}$/ },   // United States and Canada
            '44': { length: 10, pattern: /^[2-9]\d{9}$/ },  // United Kingdom
            '61': { length: 9, pattern: /^[2-9]\d{8}$/ },   // Australia
            '49': { length: 10, pattern: /^[1-9]\d{9}$/ },  // Germany
            '33': { length: 9, pattern: /^[1-9]\d{8}$/ },   // France
            '86': { length: 11, pattern: /^1[3456789]\d{9}$/ }, // China
            '91': { length: 10, pattern: /^[6-9]\d{9}$/ },  // India
            '81': { length: 10, pattern: /^[1-9]\d{9}$/ },  // Japan
            '55': { length: 9, pattern: /^[6-9]\d{8}$/ },   // Brazil
            '27': { length: 9, pattern: /^[6-9]\d{8}$/ }    // South Africa
            // Add more country codes, lengths, and patterns as needed
        };

        var countryDetailsForCode = countryDetails[countryCodeDigits];

        if (countryDetailsForCode &&
            input.length === countryDetailsForCode.length &&
            countryDetailsForCode.pattern.test(input)) {
            return true;
        } else {
            alert('Please enter a valid contact no!');
            $("#c_contact").val('');
            return false;
        }
    }
}

function isNumber(event) {
    var num = event.keyCode;
    var allowedKeys = [37, 39, 46, 8, 9]; // Arrow keys, delete, backspace, tab
    var allowedChars = ['+', '-', '(', ')', ',', '_']; // Allowed special characters

    if ((num >= 48 && num <= 57) ||(num > 95 && num < 106) || (num > 36 && num < 41) || allowedKeys.includes(num)) {
        return; // Allow number keys, arrow keys, delete, backspace, and tab
    }
    
    var charCode = event.which || event.keyCode;
    var charTyped = String.fromCharCode(charCode);
    
    if (num>=65 && num<=122) {
        event.preventDefault();
    }
}


function submit() {
    const tableBody = document.querySelector("#attachmentTable");
    const name = document.querySelector("#name").value.trim();
    const typeDropdown = document.querySelector("#c_type");
    const type = typeDropdown.options[typeDropdown.selectedIndex].text.trim();
    const typeid = typeDropdown.options[typeDropdown.selectedIndex].value.trim();
    const vdate = document.querySelector("#vdate").value.trim();
    const idate = document.querySelector("#idate").value.trim();
    const fileInput = document.querySelector("#attachment");
    const attachment = fileInput.files[0];
    const currentDate = new Date().toLocaleDateString();

    const issueDate = new Date(idate);
    const formattedIssueDate = `${issueDate.getDate().toString().padStart(2, "0")}/${(issueDate.getMonth() + 1).toString().padStart(2, "0")}/${issueDate.getFullYear()}`;




    if ($('#name').val().trim() == '') {
        alert("Please enter name!");
        return false;
    }
    else if ($('#c_type').val().trim() == '') {
        alert("Please select type!");
        return false;
    }
    else if ($('#idate').val().trim() == '') {
        alert("Please select issue date!");
        return false;
    }

    else if ($('#vdate').val().trim() == '') {
        alert("Please enter valid date!");
        return false;
    }
    else if (attachment == '' || attachment == undefined) {
        alert("Please select attachment!");
        return false;
    }

    // File type validation
    const allowedExtensions = ["jpg", "jpeg", "png", "pdf", "xls", "xlsx"];
    const fileExtension = attachment.name.split(".").pop().toLowerCase();

    if (!allowedExtensions.includes(fileExtension)) {
        alert("Invalid file type! Only JPG, PNG, PDF, and Excel files are allowed.");
        return;
    }

    // Upload file via $.ajax
    const formData = new FormData();
    formData.append("file", attachment);

    $.ajax({
        url: "/CustomerMaster/UploadFile", // Update with your upload method's URL
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.filePath) {
                // Create a new row in the table
                const tr = document.createElement("tr");
                // tr.classList.add("text-center");

                // Type ID
                const typeTd = document.createElement("td");
                typeTd.textContent = typeid;
                tr.appendChild(typeTd);
                typeTd.style.display = "none";
                // Name
                const nameTd = document.createElement("td");
                nameTd.textContent = name;
                tr.appendChild(nameTd);

                // Type
                const typeColumn = document.createElement("td");
                typeColumn.textContent = type;
                tr.appendChild(typeColumn);

                // Attachment Column
                const attachmentTd = document.createElement("td");

                // File Path Display (Hidden)
                const filePathSpan = document.createElement("span");
                filePathSpan.textContent = response.filePath;
                filePathSpan.style.display = "none";

                // Create the Download Icon
                const downloadIcon = document.createElement("i");
                downloadIcon.className = "fa-solid fa-download  text-primary";
                downloadIcon.style.cursor = "pointer";
                downloadIcon.style.marginLeft = "10px";


                downloadIcon.addEventListener("click", () => {
                    // Replace with the actual file path from your backend response
                    const filePath = response.filePath;

                    // Encode the file path to make it safe for URLs
                    const encodedPath = encodeURIComponent(filePath);

                    // Construct the URL to call the controller
                    const url = `/CustomerMaster/DownloadFile?filePath=${encodedPath}`;

                    // Open the file in a new tab or start downloading
                    window.open(url, "_blank");
                });
                // Append path and icon to the TD
                attachmentTd.appendChild(filePathSpan);
                attachmentTd.appendChild(downloadIcon);

                // Append to the table row
                tr.appendChild(attachmentTd);

                // Date Column
                const dateTd = document.createElement("td");
                dateTd.textContent = currentDate;
                tr.appendChild(dateTd);

                // Issue Date
                const idateTd = document.createElement("td");
                idateTd.textContent = formattedIssueDate;
                tr.appendChild(idateTd);

                // Valid Date
                const vdateTd = document.createElement("td");
                vdateTd.textContent = vdate;
                tr.appendChild(vdateTd);



                const actionTd = document.createElement("td");

                // Create the Edit icon
                const editIcon = document.createElement("i");
                editIcon.className = "fa fa-edit  click-edit";
                editIcon.style.cursor = "pointer";
                editIcon.title = "Edit";

                const separator = document.createElement("span");
                separator.innerHTML = "&nbsp;&nbsp;";  // Adds some space between icons
                actionTd.appendChild(separator);

                actionTd.appendChild(editIcon); // Append the Edit icon
                // Create the Delete icon
                const deleteIcon = document.createElement("i");
                deleteIcon.className = "fa fa-trash text-danger";
                deleteIcon.style.cursor = "pointer";
                deleteIcon.title = "Delete";
                deleteIcon.addEventListener("click", () => {
                    if (confirm("Are you sure you want to delete this row?")) {
                        tr.remove();
                    }
                });
                actionTd.appendChild(deleteIcon);


                // Append the actionTd to the row (tr)
                tr.appendChild(actionTd);

                // Append row to the table
                tableBody.appendChild(tr);

                // Clear form inputs
                document.querySelector("#name").value = "";
                document.querySelector("#c_type").value = "";
                document.querySelector("#idate").value = "";
                document.querySelector("#vdate").value = "";
                document.querySelector("#attachment").value = "";
                $("#createModal").modal("hide");
            } else {
                alert("File upload failed!");
            }
        },
        error: function (error) {
            console.error("Error uploading file:", error);
            alert("File upload failed! Please try again.");
        },
    });
}

function submitCustomer() {
    var formData = new FormData();
    var isActive = $('#c_isactive').is(':checked') ? 1 : 0;
    //var fileInput = $("#imagefile").prop('files');
    //var defaultImageSrc = $('#imgowner').prop('src');
    //    formData.append('File', $("#imagefile").prop('files')[0]);
    //    formData.append('c_id', $("#c_id").val());
    //    formData.append('c_name', $("#c_name").val());
    //    formData.append('co_country_code', $("#co_country_code").val());
    //    formData.append('c_address', $("#c_address").val());
    //    formData.append('c_contact', $("#c_contact").val());
    //    formData.append('c_contact2', $("#c_contact2").val());
    //    formData.append('c_gst_no', $("#c_gst_no").val());
    //    formData.append('c_email', $("#c_email").val());
    //    formData.append('c_dob', $("#c_dob").val());
    //    formData.append('c_anidate', $("#c_anidate").val());
    //    formData.append('c_gstin', '0');
    //    formData.append('c_isactive', isActive);
    //    formData.append('c_note', $("#c_note").val());
    //    formData.append('c_isactive', $("#c_isactive").val());

        if ($('#c_name').val().trim() == '') {
            alert("Please enter name!");
            return false;
        }
        else if ($('#c_ccode').val().trim() == '') {
        alert("Please enter code!");
        return false;
    }
        else if ($('#c_address').val().trim() == '') {
            alert("Please enter address!");
            return false;
        }
        else if ($('#c_contact').val().trim() == '') {
            alert("Please enter contact no!");
            return false;
        }
        else if ($('#c_email').val().trim() == '') {
            alert("Please enter email!");
            return false;
        }

    //var rowCount = $('#dataTable tbody tr').length;
    //if (rowCount == 0) {
    //    alert("Please select attachment!");
    //    return false;
    //}

    var orderDataArray = [];
    $("#dataTable tbody tr").each(function () {
        var row = $(this);
        var rowData = {
            c_type_id: row.find('td:eq(0)').text().trim(),
            name: row.find('td:eq(1)').text().trim(),
            c_type: row.find('td:eq(2)').text().trim(),
            attachment: row.find('td:eq(3)').text().trim(),
            mdate: row.find('td:eq(4)').text().trim(),
            idate: row.find('td:eq(5)').text().trim(),
            vdate: row.find('td:eq(6)').text().trim(),
        };
        orderDataArray.push(rowData);
    });


    //formData.append('CustomerAttachment',orderDataArray);
        var data = {
            c_id: $("#c_id").val(),
            c_name: $("#c_name").val(),
            c_ccode: $("#c_ccode").val(),
            co_country_code: $("#co_country_code").val(),
            c_address: $("#c_address").val(),
            c_contact: $("#c_contact").val(),
            c_contact2: $("#c_contact2").val(),
            c_email: $("#c_email").val(),
            c_dob: $("#c_dob").val(),
            c_anidate: $("#c_anidate").val(),
            c_isactive: isActive,
            c_note: $("#c_note").val(),
          
            CustomerAttachment: orderDataArray
        };


    console.log("CustomerAttachment", orderDataArray);
   
    $.ajax({
        type: 'POST',
        url: '/CustomerMaster/Create', // Use the form's action attribute
       // data: formData,
        data: data,
        //dataType: "json",
        //contentType: false,
        //processData: false,
        success: function (response) {
            console.log("Server Response:", response);
            window.location.href = "/CustomerMaster/Index";
            /*alert("Data saved successfully!");*/
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
           /* alert("Error saving data. Please try again.");*/
        }
    });
}
$(document).on('click', '#dataTable tbody .click-edit', function () {



    var $rowToEdit = $(this).closest('tr');
    var originalHTML = $rowToEdit.html();
    var c_type_id = $rowToEdit.find('td:eq(0)').text();
    var name = $rowToEdit.find('td:eq(1)').text();
    var c_type = $rowToEdit.find('td:eq(2)').text();
   
    var attachment = $rowToEdit.find('td:eq(3) .download-link').attr('data-file');
    var mdate = $rowToEdit.find('td:eq(4)').text();
    var idate = $rowToEdit.find('td:eq(5)').text().trim();
    var vdate = $rowToEdit.find('td:eq(5)').text();

    $rowToEdit.addClass("editing");



    $("#c_type_id").val(c_type_id.trim().toLowerCase());
    $("#c_type").val(c_type_id.trim().toLowerCase());


    $("#name").val(name.trim());
   /* $("#attachment").val(attachment.trim());*/

    $("#mdate").val(mdate.trim());

    let formattedFDate = formatDate(idate);
  

    $('#idate').val(formattedFDate);
   
    $("#vdate").val(vdate.trim());
   
    $("#createModal").modal("show");
    

});


$(document).on('click', '#dataTable tbody .click', function () {
    var $rowToDelete = $(this).closest('tr');
    var deletedRowIndex = $rowToDelete.index();

    // Remove the data of the deleted row
    addedRowsData.splice(deletedRowIndex, 1);

    // Remove the row from the table
    $rowToDelete.remove();

   
});

function formatDate(dateTimeString) {
    // Split the date string by "/"
    const dateParts = dateTimeString.split('/');

    // Check if the date string is in the correct format (DD/MM/YYYY)
    if (dateParts.length === 3) {
        const day = String(dateParts[0]).padStart(2, '0');
        const month = String(dateParts[1]).padStart(2, '0');
        const year = dateParts[2];

        // Return the date in YYYY-MM-DD format
        return `${year}-${month}-${day}`;
    }
    return ""; // Return an empty string if the date is invalid
}

function submitCredit() {

    if ($('#totalcreditlimit').val().trim() == '') {
        alert("Please enter limit!");
        return false;
    }

    var data = {
        cd_id: $('#cd_id').val(),
        cd_c_id: $('#c_id').val(),
        totalcreditlimit: $('#totalcreditlimit').val(),
        availablebalance: $('#availablebalance').val(),
        outstandingbalance: $('#outstandingbalance').val(),
    };

    $.ajax({
        type: 'POST',
        url: '/CustomerMaster/UpdateCredit', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            window.location.href = '/CustomerMaster/Index?status=1';
        },

        error: function (xhr, status, error) {
             console.error(error);
        }
    });
}
function editLinkClick(c_id) {

    $.get("./CustomerMaster/Edit",
        { c_id: c_id },
        function (data) {
            console.log(data);
            $("#createModal").modal("show");

            $('#c_id').val(data.c_id);
            $("#c_name").val(data.c_name);
            $("#c_address").val(data.c_address);
            $('#c_contact').val(data.c_contact);
            $('#c_contact2').val(data.c_contact2);
            $('#c_gst_no').val(data.c_gst_no);
            $('#c_email').val(data.c_email);
            $('#co_country_code').val(data.co_country_code);


            var rawDate = data.c_dob; // Date in the format "2023-10-01T00:00:00"
            var formattedDate = rawDate.split('T')[0]; // Extract the date part

            $('#c_dob').val(formattedDate);
            //                $('#c_dob').val(data.c_dob);
            var rawDate = data.c_anidate; // Date in the format "2023-10-01T00:00:00"
            var formattedDate = rawDate.split('T')[0]; // Extract the date part

            $('#c_anidate').val(formattedDate);
            //$('#c_anidate').val(data.c_anidate);
            $('#c_gstin').val(data.c_gstin);
            $('#c_note').val(data.c_note);
            $('#c_openingbalance').val(data.c_openingbalance);

            var isActive = $('#c_isactive').is(':checked') ? 1 : 0;
            $('#c_isactive').val(data.isActive);
            var isActive = $('#c_isactive').val(data.c_isactive);

            if (data.c_isactive === "Active") {
                $('#c_isactive').prop('checked', true);
            } else {
                $('#c_isactive').prop('checked', false);
            }



        });
}
function Excel() {



    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/CustomerMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;
    //window.location.href = "./CustomerMaster/Excel";


}

function Pdf() {

  

    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;

    var baseUrl = window.location.origin + '/CustomerMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');

    window.location.href = newUrl;
}


function ToggleSwitchCon(c_id, status) {
    var data = {

        c_id: c_id,

        c_isactive: status
    };

    $.ajax({
        type: 'POST',
        url: '/CustomerMaster/UpdateStatus', // Use the correct URL or endpoint
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
function performAction() {

    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';

   

    window.location.href = "/CustomerMaster/Index?status=" + status;

}

function GoBack() {
    window.location.href = "/CustomerMaster/Index?status=" + 1;

}



function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        var file = input.files[0];

        // File format validation
        var fileExtension = file.name.split('.').pop().toLowerCase();
        var allowedExtensions = ['gif', 'jpg', 'jpeg', 'bmp', 'png'];

        if (allowedExtensions.indexOf(fileExtension) === -1) {
            alert('Invalid file format. Please select a proper image file.');
            return false;
        }

        reader.onload = function (e) {
            $('#imgowner').attr('src', e.target.result);
        };

        reader.readAsDataURL(file);
    }
}




    function CancelData() {

       
        $("#c_name").val('');
        $("#c_address").val('');
        $('#c_contact').val('');
        $('#c_contact2').val('');
        $('#c_ccode').val('');
     
        $('#c_email').val('');
        $('#c_dob').val('');
        $('#c_anidate').val('');
        $('#c_note').val('');
       
      
        $("#c_isactive").prop("checked", true);
        const tableBody = document.querySelector("#attachmentTable");
        tableBody.innerHTML = "";
       
}

function CancelModelData() {


    $("#c_type").val('');
    $("#name").val('');
    $('#idate').val('');
    $('#vdate').val('');
    document.querySelector("#attachment").value = "";
   // $("#createModal").modal("hide");
}
function CloseData() {


    $("#c_type").val('');
    $("#name").val('');
    $('#idate').val('');
    $('#vdate').val('');
    document.querySelector("#attachment").value = "";
   
   
    $("#createModal").modal("hide");
}