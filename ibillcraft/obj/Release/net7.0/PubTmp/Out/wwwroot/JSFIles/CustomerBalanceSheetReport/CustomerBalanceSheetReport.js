function report(customer) {
    var scustomer = $('#customer').val();
    var data = {
        customer: $('#customer').val(),
    };
    $.ajax({
        type: 'POST',
        url: 'CustomerBalanceSheetReport/Report?customer=' + scustomer + '', // Use the form's action attribute
        //data: data, // Serialize the form data
        success: function (response) {
            console.log(response);
            $('#reportqt').html('');
            var htmlrpt
            for (var i = 0; i < response.length; i++) {
                htmlrpt += '<tr><td>' + (parsefloat(i) + 1) + '</td><td>' + response[i].c_name + '</td><td style="color:red;">' + response[i].c_balance + '</td></tr>';
            }
            $('#reportqt').html(htmlrpt);
            $('.pagination').html('');

            setTimeout(function () {
                myFunction();
                $('#table_length_select').change();
            }, 2000);
        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}

function Excel(customer) {
    var scustomer = $('#customer').val();
    window.location.href = '/CustomerBalanceSheetReport/Excel?customer=' + scustomer + '';
}
function clear() {
    window.location.href = '/CustomerBalanceSheetReport/Index';
}
