
function report(customer, fromdate, todate, staff) {

    if ($('#e_type').val() == 'Custom') {
        if ($('#from').val() === '') {
            alert("Please select from date");
            return false;
        } else if ($('#to').val() === '') {
            alert("Please select to date");
            return false;
        }
    }

    var scustomer = $('#customer').val();
    var sfromdate = $('#from').val();
    var stodate = $('#to').val();
    var sstaff = $('#order_ref').val();
    var data = {
        customer: $('#customer').val(),
        fromdate: $('#from').val(),
        todate: $('#to').val(),
        staff: $('#order_ref').val(),

    };
    $.ajax({
        type: 'POST',
        url: 'SaleReport/Report?customer=' + scustomer + '&fromdate=' + sfromdate + '&todate=' + stodate + '&staff=' + sstaff + '', // Use the form's action attribute
        //data: data, // Serialize the form data
        success: function (response) {
            console.log(response);
            
            $('#reportqt').html('');
            //cleardatatable()
            var color = '';
            var status = '';
            var htmlrpt
            for (var i = 0; i < response.length; i++) {
                status = response[i].sl_balance > 0 ? response[i].sl_balance == response[i].sl_total_withgst ? "Unpaid" : "Partially" : "Paid"
                if (status == 'Unpaid') {
                    color = 'badge-light-danger';
                }
                else if (status == 'Partially') {
                    color = 'badge-light-warning';
                }
                else {
                    color = 'badge-light-success';
                }
                htmlrpt += '<tr><td>' + (parseInt(i) +1) + '</td><td>' + response[i].sl_invoice_no + '</td><td>' + response[i].sl_invoice_date + '</td><td>' + response[i].sl_name + '</td><td> <span class="badge py-3 px-4 fs-7 ' + color + '">' + status + '</span></td><td style="text-align:right">' + response[i].sl_advance + '</td><td style="text-align:right">' + response[i].sl_balance + '</td><td>' + response[i].sl_order_ref + '</td><td style="text-align:right">' + response[i].sl_total_withgst + '</td><td><div class="d-inline-flex"><a href = "/SaleOrder/BillIndex?sl_invoice_no=' + response[i].sl_invoice_no + '&amp;sl_name=' + response[i].sl_name + '&amp;sl_id=' + response[i].sl_id + '&amp;sl_total_withgst=' + response[i].sl_total_withgst + '&amp;sl_advance=' + response[i].sl_advance + '&amp;sl_balance=' + response[i].sl_balance + '&amp;total_charges=0' + '"><span class="fa fa-eye"></span></a></div></td></tr>';
            }
            $('#reportqt').html(htmlrpt);
            //$('#dataTable1').DataTable();
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
        $('#from').val(currentDate);
        $('#to').val(currentDate);
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

        $('#from').val(formattedFirstDay);
        $('#to').val(formattedLastDay);
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

        $('#from').val(formattedFirstDay);
        $('#to').val(formattedLastDay);


    }
    else if (type == 'Custom') {
        $('#from').val('');
        $('#to').val('');
    }

}

function Excel() {

    var scustomer = $('#customer').val();
    var sfromdate = $('#from').val();
    var stodate = $('#to').val();
    var sstaff = $('#order_ref').val();
    window.location.href = '/SaleReport/Excel?customer=' + scustomer + '&fromdate=' + sfromdate + '&todate=' + stodate + '&staff=' + sstaff + '';
}