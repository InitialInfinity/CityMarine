function report(fromdate, todate) {

    if ($('#e_type').val() == 'Custom') {
        if ($('#fromdate').val() === '') {
            alert("Please select from date");
            return false;
        } else if ($('#todate').val() === '') {
            alert("Please select to date");
            return false;
        }
    }


    

    var sfromdate = $('#fromdate').val();
    var stodate = $('#todate').val();
    $.ajax({
        type: 'POST',
        url: 'AdvanceReport/Report?fromdate=' + sfromdate + '&todate=' + stodate + '', // Use the form's action attribute
        //data: data, // Serialize the form data
        success: function (response) {
            console.log(response);
            $('#reportqt').html('');
            var htmlrpt
            for (var i = 0; i < response.item2.length; i++) {
                htmlrpt += '<tr><td>' + response.item2[i].sl_invoice_no + '</td><td>' + response.item2[i].sl_invoice_date + '</td><td>' + response.item2[i].sl_name + '</td><td>' + response.item2[i].sl_balance + '</td><td>' + response.item2[i].sl_total_withgst + '</td></tr>';
            }
            $('#reportqt').html(htmlrpt);

            $('#reportqt1').html('');
            var htmlrpt1
            for (var i = 0; i < response.item3.length; i++) {
                htmlrpt1 += '<tr><td>' + response.item3[i].pu_invoice_no + '</td><td>' + response.item3[i].pu_invoice_date + '</td><td>' + response.item3[i].pu_name + '</td><td>' + response.item3[i].pu_balance + '</td><td>' + response.item3[i].pu_total_withgst + '</td></tr>';
            }
            $('#reportqt1').html(htmlrpt1);

            $('#reportqt2').html('');
            var htmlrpt2
            for (var i = 0; i < response.item4.length; i++) {
                htmlrpt2 += '<tr><td>' + (parsefloat(i) + 1) + '</td><td>' + response.item4[i].e_date + '</td><td>' + response.item4[i].e_category_name + '</td><td>' + response.item4[i].e_user_name + '</td><td>' + response.item4[i].e_amount +'</td></tr>';
            }
            $('#reportqt2').html(htmlrpt2);

            $('#reportqt3').html('');
            var htmlrpt3
            for (var i = 0; i < response.item5.length; i++) {
                htmlrpt3 += '<tr><td>' + response.item5[i].sp_invoice + '</td><td>' + response.item5[i].sp_date + '</td><td>' + response.item5[i].sp_name + '</td><td>' + response.item5[i].sp_pay + '</td><td>' + response.item5[i].sp_balance + '</td></tr>';
            }
            $('#reportqt3').html(htmlrpt3);
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

function period() {
    var type = $('#e_type').val();

    if (type == 'Daily') {
        const currentDate = new Date().toISOString().split('T')[0];
        $('#fromdate').val(currentDate);
        $('#todate').val(currentDate);
    } else if (type == 'Monthly') {
       
        const currentDate = new Date();
        const firstDayOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
        const lastDayOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);

        // Format dates to 'YYYY-MM-DD'
        const year = firstDayOfMonth.getFullYear();
        const month = (firstDayOfMonth.getMonth() + 1).toString().padStart(2, '0'); // Months are 0-based
        const day = firstDayOfMonth.getDate().toString().padStart(2, '0');

        const formattedFirstDay = `${year}-${month}-${day}`;


        const syear = lastDayOfMonth.getFullYear();
        const smonth = (lastDayOfMonth.getMonth() + 1).toString().padStart(2, '0'); // Months are 0-based
        const sday = lastDayOfMonth.getDate().toString().padStart(2, '0');

        //const formattedLastDay = lastDayOfMonth.toISOString().split('T')[0];
        const formattedLastDay = `${syear}-${smonth}-${sday}`;

        $('#fromdate').val(formattedFirstDay);
        $('#todate').val(formattedLastDay);
    } else if (type == 'Yearly') {
        const currentDate = new Date();
        const firstDayOfYear = new Date(currentDate.getFullYear(), 0, 1);
        const lastDayOfYear = new Date(currentDate.getFullYear(), 11, 31);

        // Format dates to 'YYYY-MM-DD'
        const year = firstDayOfYear.getFullYear();
        const month = (firstDayOfYear.getMonth() + 1).toString().padStart(2, '0');
        const day = firstDayOfYear.getDate().toString().padStart(2, '0');

        const formattedFirstDay = `${year}-${month}-${day}`;
        //const formattedLastDay = lastDayOfYear.toISOString().split('T')[0];

        const syear = lastDayOfYear.getFullYear();
        const smonth = (lastDayOfYear.getMonth() + 1).toString().padStart(2, '0');
        const sday = lastDayOfYear.getDate().toString().padStart(2, '0');

        const formattedLastDay = `${syear}-${smonth}-${sday}`;

        $('#fromdate').val(formattedFirstDay);
        $('#todate').val(formattedLastDay);


    }
    else if (type == 'Custom') {
        $('#fromdate').val('');
        $('#todate').val('');
    }

}

function excel() {
    var sfromdate = $('#from').val();
    var stodate = $('#to').val();
    var scustomer = '';
    var sstaff = '';
    var type = 'Sale';
    window.location.href = '/AdvanceReport/Excel?customer=' + scustomer + '&fromdate=' + sfromdate + '&todate=' + stodate + '&staff=' + sstaff + '&type='+type+'';
}
function excel1() {
    var sfromdate = $('#from').val();
    var stodate = $('#to').val();
    var scustomer = '';
    var sstaff = '';
    var type = 'Purchase';
    window.location.href = '/AdvanceReport/Excel?customer=' + scustomer + '&fromdate=' + sfromdate + '&todate=' + stodate + '&staff=' + sstaff + '&type=' + type + '';
}
function excel2() {
    var sfromdate = $('#from').val();
    var stodate = $('#to').val();
    var scustomer = '';
    var sstaff = '';
    var type = 'Expense';
    window.location.href = '/AdvanceReport/Excel?customer=' + scustomer + '&fromdate=' + sfromdate + '&todate=' + stodate + '&staff=' + sstaff + '&type=' + type + '';
}
function excel3() {
    var sfromdate = $('#from').val();
    var stodate = $('#to').val();
    var scustomer = '';
    var sstaff = '';
    var type = 'Expense';
    window.location.href = '/AdvanceReport/Excel?customer=' + scustomer + '&fromdate=' + sfromdate + '&todate=' + stodate + '&staff=' + sstaff + '&type=' + type + '';
}

function clear() {
    window.location.href = "/AdvanceReport/Index";
}