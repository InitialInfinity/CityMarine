function AddBankOperation() {
    window.location.href = '/BankOperation/EditIndex';
}
function GoBack() {
    window.location.href = '/BankOperation/Index';
} function ClrData() {
    $('#bo_file').val('');
    $('#bo_name').val('');
    $('#bo_amount').val('');
    $('#bo_remark').val('');
    $('#bo_category').val('');
    $('#bo_bank_id').val('');
    $('#fileContent').text('');
}

function submitBankOperation() {


    if ($('#bo_bank_id').val() == '') {
        alert("Please select bank name!");
        return false;
    }
    else if ($('#bo_category').val() == '') {
        alert("Please select category!");
        return false;
    }
    else if ($('#bo_name').val() == '') {
        alert("Please enter name!");
        return false;
    }
    else if ($('#bo_amount').val() == '') {
        alert("Please enter amount!");
        return false;
    }
    else if ($('#bo_file').val() == '') {
        alert("Please select file!");
        return false;
    }

    var formData = new FormData();
    formData.append('bo_file', $('#bo_file')[0].files[0]);
    formData.append('bo_name', $('#bo_name').val());
    formData.append('bo_amount', $('#bo_amount').val());
    formData.append('bo_remark', $('#bo_remark').val());
    formData.append('bo_category', $('#bo_category option:selected').val());
    formData.append('bo_bank_name', $('#bo_bank_id option:selected').text());
    formData.append('bo_bank_id', $('#bo_bank_id').val());

    $.ajax({
        type: 'POST',
        url: '/BankOperation/Insert',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {

            window.location.reload();
        },
        error: function (xhr, status, error) {
            console.log(error);
            //window.location.reload();
        }
    });

}
function validateInput(inputElement) {
    var inputValue = inputElement.value;
    var regex = /^[a-zA-Z]+$/;

    if (!regex.test(inputValue)) {
        inputElement.value = "";
        alert('Please enter valid creditor name.');
        return false;

    }
}
$(document).ready(function () {

    $('#bo_category').change(function () {

        var selectedOption = $(this).val();
        if (selectedOption == 'Credit') {
            $('#amount1').css("display", "block");
            $('#name1').css("display", "block");
            $('#amount2').css("display", "none");
            $('#name2').css("display", "none");
        }
        else {
            $('#amount1').css("display", "none");
            $('#name1').css("display", "none");
            $('#amount2').css("display", "block");
            $('#name2').css("display", "block");
        }
    });
});
function viewFile() {

    var fileInput = document.getElementById('bo_file');

    var file = fileInput.files[0];
    if (file) {
        var allowedExtensions = [ ".jpg", ".jpeg", ".bmp", ".png"];

        if (allowedExtensions.some(ext => file.name.toLowerCase().endsWith(ext))) {
            if (file.type.startsWith('image/')) {
                var fileContentElement = document.getElementById('fileContent');
                fileContentElement.innerHTML = '';
                var img = new Image();
                img.src = URL.createObjectURL(file);
                img.alt = 'Uploaded Image';
                img.style.width = '200px'; // Set your preferred width
                img.style.height = '150px'; // Set your preferred height
                fileContentElement.appendChild(img);
            }
            else {
                alert('Invalid file type. Please upload an image.');
                $('#fileContent').text('');
                $('#bo_file').val('');
                return false;

            }
        }
        else {
            alert('Invalid file extension. Supported extensions are: ' + allowedExtensions.join(', '));
            $('#fileContent').text('');
            $('#bo_file').val('');
            return false;

        }
    }
    else {
        alert('Please select a file before clicking "View File"!');
        return false;
    }
}
//function FileUploadimage() {
//    $("input[id='imagefile']").click();
//}

//$("#imagefile").change(function () {
//    readURL(this);
//});
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        var file = input.files[0];

        // File format validation
        var fileExtension = file.name.split('.').pop().toLowerCase();
        var allowedExtensions = [ 'jpg', 'jpeg', 'bmp', 'png'];

        if (allowedExtensions.indexOf(fileExtension) === -1) {
            alert('Invalid file format. Please select a proper image file.');
            $('#fileContent').text('');
            $('#bo_file').val('');
            return false;
        }

       
            $('#bo_file').attr('src', e.target.result);

        reader.readAsDataURL(file);
    }
}
function viewgridfile(bo_filecopy) {
    $("#createModal").modal("show");

    if (bo_filecopy && bo_filecopy.startsWith('data:image/')) {
        var fileContentElement = document.getElementById('fileContent1');
        fileContentElement.innerHTML = '';

        // Create an img element
        var img = new Image();

        // Set the src attribute to the base64 string
        img.src = bo_filecopy;
        img.alt = 'Uploaded Image';
        img.style.width = '500px'; // Set your preferred width
        img.style.height = '400px'; // Set your preferred height

        // Append the image element to the fileContentElement
        fileContentElement.appendChild(img);
    }
}
function CloseData() {
    $("#createModal").modal("hide");


}
