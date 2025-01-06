function OnExpense() {
    window.location.href = '/Expense/Index?e_type=Expense';
}

function OnCategory() {
    window.location.href = '/Expense/Index?e_type=Category';
}
function OnUser() {
    window.location.href = '/Expense/Index?e_type=User';
}

function setMinDate() {
    var currentDate = new Date().toISOString().split('T')[0];
    document.getElementById('e_date').min = currentDate;
   // document.getElementById('cat_date').min = currentDate;
}

function validatechar(inputElement) {
    // Regular expression to allow only characters and special characters, but not numbers
    var regex = /^[a-zA-Z\s!#$%^&*()_,@\-./:;'"?<>|[\]{\}~`\\]*$/;

    var inputValue = inputElement.value;

    if (!regex.test(inputValue)) {
        alert("Invalid characters. Please use only characters and special characters..");
        inputElement.value = '';
        return false;
    }
}

function submitExpense() {
    if ($('#e_category_name').val() == '') {
        alert("Please select Category name!");
        return false;
    }
    else if ($('#e_user_name').val() == '') {
        alert("Please select user!");
        return false;
    }
    else if ($('#e_amount').val() == '' || isNaN($('#e_amount').val())) {
        $('#e_amount').val('');
        alert("Please enter valid amount!");
        return false;
    }
    else if ($('#e_date').val() == '') {
        alert("Please select date!");
        return false;
    }
    var data = {
        e_id: $('#e_id').val(),
        e_category_name: $('#e_category_name option:selected').text(),
        e_user_name: $('#e_user_name option:selected').text(),
        e_amount: $('#e_amount').val(),
        e_date: $('#e_date').val(),
        e_desc: $('#e_desc').val(), 
        e_type: 'Expense',
    };
    $.ajax({
        type: 'POST',
        url: '/Expense/Create', // Use the form's action attribute
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
function submitCategory() {
    if ($('#cat_category_name').val() == '') {
        alert("Please enter category name!");
        return false;
    }
    else if ($('#cat_date').val() == '') {
        alert("Please select date!");
        return false;
    }
    var data = {
        cat_id: $('#cat_id').val(),
        cat_category_name: $('#cat_category_name').val(),
        cat_date: $('#cat_date').val(),
        e_type: 'Category',
    };
    $.ajax({
        type: 'POST',
        url: '/Expense/Create', // Use the form's action attribute
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
function submitUser() {
    if ($('#ex_user_name').val() == '') {
        alert("Please enter user name!");
        return false;
    }
    else if ($('#ex_contact').val() == '') {
        alert("Please enter contact no.!");
        return false;
    }
    var data = {
        ex_id: $('#ex_id').val(),
        ex_user_name: $('#ex_user_name').val(),
        ex_contact: $('#ex_contact').val(),
        ex_desc: $('#ex_desc').val(),
        e_type: 'User',
    };
    $.ajax({
        type: 'POST',
        url: '/Expense/Create', // Use the form's action attribute
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
function cancalEx() {
    $('#e_category_name').val('');
    $('#e_user_name').val('');
    $('#e_amount').val('');
    $('#e_date').val('');
    $('#e_desc').val('');

}
function cancalCat() {
    $('#cat_category_name').val('');
    $('#cat_date').val('');
}
function cancalUser() {
    $('#ex_user_name').val('');
    $('#ex_contact').val('');
    $('#ex_desc').val('');
}

function report(category, fromdate, todate) {

    if ($('#e_type').val() == 'Custom') {
        if ($('#fromdate').val() === '') {
            alert("Please select from date");
            return false;
        } else if ($('#todate').val() === '') {
            alert("Please select to date");
            return false;
        }
    }
    var scategory = $('#e_category_name option:selected').text();
    if (scategory == "-- Select --") {
        scategory= "";
    }


    

    var sfromdate = $('#fromdate').val();
    var stodate = $('#todate').val();
    $.ajax({
        type: 'POST',
        url: 'ExpenseReport/Report?e_category_name=' + scategory + '&fromdate=' + sfromdate + '&todate=' + stodate + '', // Use the form's action attribute
        //data: data, // Serialize the form data
        success: function (response) {
            console.log(response);
            $('#reportqt').html('');
            var htmlrpt
            for (var i = 0; i < response.length; i++) {
                /*htmlrpt += '<tr><td>' + i + 1 + '</td><td style=display:none>' + response[i].e_id + '</td><td>' + response[i].e_date + '</td><td>' + response[i].e_category_name + '</td><td>' + response[i].e_user_name + '</td><td>' + response[i].e_amount + '</td><td style=display:none>' + response[i].e_desc + '</td><td onclick="editLinkClick(''' + response[i].e_id + ''',''' + response[i].e_date + ''',''' + response[i].e_category_name + ''',''' + response[i].e_user_name + ''',''' + response[i].e_amount + ''',''' + response[i].e_desc +''')"> <div class="d-inline-flex"><a><span class="fa fa-eye"></span></a></div > </td></tr>';*/
                htmlrpt += "<tr><td>" + (parsefloat(i) + 1) + "</td><td style=display:none>" + response[i].e_id + "</td><td>" + response[i].e_date + "</td><td>" + response[i].e_category_name + "</td><td>" + response[i].e_user_name + "</td><td>" + response[i].e_amount + "</td><td style=display:none>" + response[i].e_desc + "</td><td onclick='editLinkClick(" + "'"+ response[i].e_id + "','" + response[i].e_date + "','" + response[i].e_category_name + "','" + response[i].e_user_name + "','" + response[i].e_amount + "','" + response[i].e_desc + "')'> <div class='d-inline-flex'><a><span class='fa fa-eye'></span></a></div > </td></tr>";
            }
            $('#reportqt').html(htmlrpt);
            //myFunction();
            //$('#table_length_select').change();
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
        //const currentDate = new Date();
        //const firstDayOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
        //const lastDayOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);

        //// Format dates to 'YYYY-MM-DD'
        ////const formattedFirstDay = firstDayOfMonth.toISOString().split('T')[0];
        //const formattedLastDay = lastDayOfMonth.toISOString().split('T')[0];


        //const year = firstDayOfMonth.getFullYear();
        //const month = (firstDayOfMonth.getMonth() + 1).toString().padStart(2, '0'); // Months are 0-based
        //const day = firstDayOfMonth.getDate().toString().padStart(2, '0');

        //const formattedFirstDay = `${year}-${day}-${month}`;

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

function Excel(category, fromdate, todate) {

    var scategory = $('#e_category_name option:selected').text();
    if (scategory == "-- Select --") {
        scategory = "";
    }
    window.location.href = '/ExpenseReport/Excel?e_category_name=' + scategory + '&fromdate=' + sfromdate + '&todate=' + stodate + '';
}

function editLinkClick(e_id, e_date, e_category_name, e_user_name, e_amount, e_desc) {
    $("#addSubscription").modal("show");
    $('#e_id').val(e_id);
    $("#e_date").val(e_date);
    $("#e_category_name1").val(e_category_name);
    $("#e_user_name").val(e_user_name);
    $("#e_amount").val(e_amount);
    $("#e_desc").val(e_desc);
}