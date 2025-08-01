function show(ic_year, ic_fromname, ic_from, tab) {
    if (tab == "Enquiry") {
        enquirymail();
    }
    if (tab == "Claim") {

        claimmail();
    }
}


function enquirymail() {

    var tab = "Enquiry";


    $('#claim-tab').attr('class', 'nav-link');
    $('#claim-atttab').attr('class', 'nav-link');
    $('#enquiry-tab').attr('class', 'nav-link active');
    $('#enquiry-atttab').attr('class', 'nav-link ');

    $("#enquirynodiv").css("display", "contents");
    var year = $('#yearDropdown option:selected').text();
    var urlParams = new URLSearchParams(window.location.search);
    var email = $('#clientDropdown1 option:selected').val();
    //var email = urlParams.get('email');

    var enquirydropvalue = $("#enquirydropdown").val();

    var data = {
        ic_from: email,
        tab: tab,
        ic_year: year,
        dropvalue: enquirydropvalue
    };

    $.ajax({
        url: '/InboxClient/GetEnquiryClaimMail', // Controller action URL
        type: 'POST',             // HTTP method
        data: data,
        // Data to send (query parameter)
        success: function (response) {
            // Handle success - update the page dynamically
            console.log('Response:', response);
            $("#enquiry").css("display", "contents");
            $("#enquiryattach").css("display", "contents");
            $("#enquirytablehead").css("display", "contents");
            $("#enquirytablebody").css("display", "contents");
            $("#attachmenttable").css("display", "none");//attachment table
            $("#attachmenttablebody").css("display", "none");// attachment table body

            $('#enquirytablebody').html('');
            var htmltab = '';
            for (var i = 0; i < response.sentclientList.length; i++) {
                htmltab += '<tr>';
                htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + (i + 1) + '</td>';
                htmltab += '<td style="display:none">' + response.sentclientList[i].ic_id + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_from + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_to + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_subject + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_receiveddate + '</td>';
                // htmltab += '<td >' + response[i].ic_attachment + '</td>';


                if (response.sentclientList[i].ic_attachment != "No attachments available." && response.sentclientList[i].ic_attachment != null) {
                    htmltab += '<td style="width:4%" ><i class="fa fa-paperclip" aria-hidden="true"></i></td>';
                }
                else {
                    htmltab += '<td style="width:4%"></td>';
                }
                htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')"></i></td>';

                htmltab += '</tr>';


            }
            $('#enquirytablebody').html(htmltab);

            // Example: Update a specific section with the response data
            $('#content-area').html(response);

        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error('Error:', error);
        }
    });
}

function enquiryattachment() {

    $('#claim-tab').attr('class', 'nav-link');
    $('#claim-atttab').attr('class', 'nav-link');
    $('#enquiry-tab').attr('class', 'nav-link');
    $('#enquiry-atttab').attr('class', 'nav-link active');


    $("#enquirynodiv").css("display", "contents");
    var tab = "Enquiry";
    // var email = $('#clientDropdown1 option:selected').text();
    var urlParams = new URLSearchParams(window.location.search);
    //var email = urlParams.get('email');
    var email = $('#clientDropdown1 option:selected').val();
    var year = $('#yearDropdown option:selected').text();

    var enquirydropvalue = $("#enquirydropdown").val();

    if (enquirydropvalue == "") {
        var data = {

            tab: tab,
            ic_year: year,
            ic_from: email
        };

        $.ajax({
            url: '/InboxClient/FetchAttachments', // Controller action URL
            type: 'POST',             // HTTP method
            data: data,
            // Data to send (query parameter)
            success: function (response) {
                // Handle success - update the page dynamically
                console.log('Response:', response);
                $("#enquiry").css("display", "contents");           //tab name
                $("#enquiryattach").css("display", "contents");    //tab name
                $("#enquirytablehead").css("display", "none");  //table head
                $("#enquirytablebody").css("display", "none");//table body
                $("#attachmenttable").css("display", "contents");//attachment table
                $("#attachmenttablebody").css("display", "contents");// attachment table body
                var htmltab = '';
                for (var i = 0; i < response.length; i++) {
                    htmltab += '<tr>';
                    htmltab += '<td>' + (i + 1) + '</td>';
                    htmltab += '<td style="display:none">' + response[i].ic_id + '</td>';

                    htmltab += '<td >' + response[i].ic_from + '</td>';
                    htmltab += '<td >' + response[i].ic_subject + '</td>';
                    htmltab += '<td >' + response[i].ic_attachment + '</td>';
                    htmltab += '<td >' + response[i].ic_receiveddate + '</td>';
                    var filePath = response[i].icc_attachment.replace(/\\/g, '\\\\'); // Escape backslashes
                    htmltab += '<td><i class="fas fa-download text-primary" onclick="downloadFile(\'' + filePath + '\')"></i></td>';
                    htmltab += '</tr>';


                }
                $('#attachmenttablebody').html(htmltab);

                // Example: Update a specific section with the response data
                $('#content-area').html(response);



            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error('Error:', error);
            }
        });
    }
    else {
        var data = {

            tab: tab,
            ic_year: year,
            ic_from: email,
            enquirydropvalue: enquirydropvalue
        };

        $.ajax({
            url: '/InboxClient/FetchAttachmentsDropDown', // Controller action URL
            type: 'POST',             // HTTP method
            data: data,
            // Data to send (query parameter)
            success: function (response) {
                // Handle success - update the page dynamically
                console.log('Response:', response);
                $("#enquiry").css("display", "contents");           //tab name
                $("#enquiryattach").css("display", "contents");    //tab name
                $("#enquirytablehead").css("display", "none");  //table head
                $("#enquirytablebody").css("display", "none");//table body
                $("#attachmenttable").css("display", "contents");//attachment table
                $("#attachmenttablebody").css("display", "contents");// attachment table body
                var htmltab = '';
                for (var i = 0; i < response.length; i++) {
                    htmltab += '<tr>';
                    htmltab += '<td>' + (i + 1) + '</td>';
                    htmltab += '<td style="display:none">' + response[i].ic_id + '</td>';

                    htmltab += '<td >' + response[i].ic_from + '</td>';
                    htmltab += '<td >' + response[i].ic_subject + '</td>';
                    htmltab += '<td >' + response[i].ic_attachment + '</td>';
                    htmltab += '<td >' + response[i].ic_receiveddate + '</td>';
                    var filePath = response[i].icc_attachment.replace(/\\/g, '\\\\'); // Escape backslashes
                    htmltab += '<td><i class="fas fa-download text-primary" onclick="downloadFile(\'' + filePath + '\')"></i></td>';
                    htmltab += '</tr>';


                }
                $('#attachmenttablebody').html(htmltab);

                // Example: Update a specific section with the response data
                $('#content-area').html(response);



            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error('Error:', error);
            }
        });
    }
}

function claimmail() {

    var tab = "Claim";

    $('#claim-tab').attr('class', 'nav-link active');
    $('#claim-atttab').attr('class', 'nav-link');
    $('#enquiry-tab').attr('class', 'nav-link');
    $('#enquiry-atttab').attr('class', 'nav-link');

    var urlParams = new URLSearchParams(window.location.search);
    //var email = urlParams.get('email');
    var email = $('#clientDropdown1 option:selected').val();
    var year = $('#yearDropdown option:selected').text();

    var claimdropvalue = $("#claimdropdown").val();

    var data = {
        ic_from: email,
        tab: tab,
        ic_year: year,
        dropvalue: claimdropvalue
    };

    $.ajax({
        url: '/InboxClient/GetEnquiryClaimMail', // Controller action URL
        type: 'POST',             // HTTP method
        data: data,
        // Data to send (query parameter)
        success: function (response) {
            // Handle success - update the page dynamically
            console.log('Response:', response);
            $("#enquirynodiv").css("display", "none");
            $("#claimnodiv").css("display", "contents");
            $("#claim").css("display", "contents");
            $("#claimattach").css("display", "contents");
            $("#claimtablehead").css("display", "contents");
            $("#claimtablebody").css("display", "contents");
            $("#attachmenttable").css("display", "none");//attachment table
            $("#attachmenttablebody").css("display", "none");// attachment table body

            $('#claimtablebody').html('');
            var htmltab = '';
            for (var i = 0; i < response.sentclientList.length; i++) {
                htmltab += '<tr>';
                htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + (i + 1) + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_claimno + '</td>';
                htmltab += '<td style="display:none">' + response.sentclientList[i].ic_id + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_from + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_to + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_subject + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_receiveddate + '</td>';
                // htmltab += '<td >' + response[i].ic_attachment + '</td>';

                if (response.sentclientList[i].ic_attachment != "No attachments available." && response.sentclientList[i].ic_attachment != null) {
                    htmltab += '<td style="width:4%" ><i class="fa fa-paperclip" aria-hidden="true"></i></td>';
                }
                else {
                    htmltab += '<td style="width:4%"></td>';
                }

                //htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg('+response[i].sc_toemail+')"></i></td>';
                htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')"></i></td>';

                htmltab += '</tr>';




                //const claimDropdown = document.getElementById('claimdropdown');

                //$('#claimdropdown').html('<option>--Select--</option>');


                //// Loop through the claimno array and add each as an option to the dropdown
                //response.claimno.forEach(claim => {
                //    const option = document.createElement('option');

                //    option.value = claim.id;  // Set the id as the value of the option
                //    option.text = claim.value;  // Set the value as the display text
                //    claimDropdown.appendChild(option);
                //});


            }
            $('#claimtablebody').html(htmltab);

            // Example: Update a specific section with the response data
            $('#content-area').html(response);



        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error('Error:', error);
        }
    });
}
function claimattachment() {
    var tab = "Claim";

    $('#claim-tab').attr('class', 'nav-link');
    $('#claim-atttab').attr('class', 'nav-link active');
    $('#enquiry-tab').attr('class', 'nav-link');
    $('#enquiry-atttab').attr('class', 'nav-link');

    // var email = $('#clientDropdown1 option:selected').text();
    var urlParams = new URLSearchParams(window.location.search);
    //var email = urlParams.get('email');
    var email = $('#clientDropdown1 option:selected').val();
    var year = $('#yearDropdown option:selected').text();

    var claimdropvalue = $("#claimdropdown").val();

    if (claimdropvalue == "") {
        var data = {

            tab: tab,
            ic_year: year,
            ic_from: email
        };

        $.ajax({
            url: '/InboxClient/FetchAttachments', // Controller action URL
            type: 'POST',             // HTTP method
            data: data,
            // Data to send (query parameter)
            success: function (response) {
                // Handle success - update the page dynamically
                console.log('Response:', response);
                $("#enquirynodiv").css("display", "none");
                $("#claimnodiv").css("display", "contents");           //dropdown
                $("#claim").css("display", "contents");           //tab name
                $("#claimattach").css("display", "contents");    //tab name
                $("#claimtablehead").css("display", "none");  //table head
                $("#claimtablebody").css("display", "none");//table body
                $("#attachmenttable").css("display", "contents");//attachment table
                $("#attachmenttablebody").css("display", "contents");// attachment table body
                var htmltab = '';
                for (var i = 0; i < response.length; i++) {
                    htmltab += '<tr>';
                    htmltab += '<td>' + (i + 1) + '</td>';
                    htmltab += '<td style="display:none">' + response[i].ic_id + '</td>';

                    htmltab += '<td >' + response[i].ic_from + '</td>';
                    htmltab += '<td >' + response[i].ic_subject + '</td>';
                    htmltab += '<td >' + response[i].ic_attachment + '</td>';
                    htmltab += '<td >' + response[i].ic_receiveddate + '</td>';
                    var filePath = response[i].icc_attachment.replace(/\\/g, '\\\\'); // Escape backslashes
                    htmltab += '<td><i class="fas fa-download text-primary" onclick="downloadFile(\'' + filePath + '\')"></i></td>';
                    htmltab += '</tr>';


                }
                $('#attachmenttablebody').html(htmltab);

                // Example: Update a specific section with the response data
                $('#content-area').html(response);



            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error('Error:', error);
            }
        });
    }
    else {
        var data = {

            tab: tab,
            ic_year: year,
            ic_from: email,
            enquirydropvalue: claimdropvalue
        };

        $.ajax({
            url: '/InboxClient/FetchAttachmentsDropDown', // Controller action URL
            type: 'POST',             // HTTP method
            data: data,
            // Data to send (query parameter)
            success: function (response) {
                // Handle success - update the page dynamically
                console.log('Response:', response);
                $("#enquirynodiv").css("display", "none");
                $("#claimnodiv").css("display", "contents");           //dropdown
                $("#claim").css("display", "contents");           //tab name
                $("#claimattach").css("display", "contents");    //tab name
                $("#claimtablehead").css("display", "none");  //table head
                $("#claimtablebody").css("display", "none");//table body
                $("#attachmenttable").css("display", "contents");//attachment table
                $("#attachmenttablebody").css("display", "contents");// attachment table body
                var htmltab = '';
                for (var i = 0; i < response.length; i++) {
                    htmltab += '<tr>';
                    htmltab += '<td>' + (i + 1) + '</td>';
                    htmltab += '<td style="display:none">' + response[i].ic_id + '</td>';

                    htmltab += '<td >' + response[i].ic_from + '</td>';
                    htmltab += '<td >' + response[i].ic_subject + '</td>';
                    htmltab += '<td >' + response[i].ic_attachment + '</td>';
                    htmltab += '<td >' + response[i].ic_receiveddate + '</td>';
                    var filePath = response[i].icc_attachment.replace(/\\/g, '\\\\'); // Escape backslashes
                    htmltab += '<td><i class="fas fa-download text-primary" onclick="downloadFile(\'' + filePath + '\')"></i></td>';
                    htmltab += '</tr>';


                }
                $('#attachmenttablebody').html(htmltab);

                // Example: Update a specific section with the response data
                $('#content-area').html(response);



            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error('Error:', error);
            }
        });
    }

}

function claimdropdown() {
    var tab = "Claim";
    var year = $('#yearDropdown option:selected').text();
    var claim = $('#claimdropdown option:selected').text();

    var clientid = $('#clientDropdown1').val();
    var data = {
        clientid: clientid,
        tab: tab,
        year: year,
        claim: claim,
    };

    $.ajax({
        url: '/InboxClient/ClaimNo', // Controller action URL
        type: 'POST',             // HTTP method
        data: data,
        success: function (response) {
            console.log(response);

            //// If the response length is 0, do not update the table or URL
            //if (response.length === 0) {
            //    // alert("no data");
            //    $('#detailsTableBody1').html('');
            //    return; // Exit the function early
            //}

            //// Make details grid visible when there is data

            $("#claimnodiv").css("display", "contents");
            $("#enquirynodiv").css("display", "none");
            $("#claim").css("display", "contents");
            $("#claimattach").css("display", "contents");
            $("#claimtablehead").css("display", "contents");
            $("#claimtablebody").css("display", "contents");
            $("#attachmenttable").css("display", "none");//attachment table
            $("#attachmenttablebody").css("display", "none");// attachment table body

            $('#claim-tab').attr('class', 'nav-link active');
            $('#claim-atttab').attr('class', 'nav-link');

            $('#claimtablebody').html('');  // Clear previous table content

            var htmltab = '';
            for (var i = 0; i < response.length; i++) {
                htmltab += '<tr>';
                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + (i + 1) + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_claimno + '</td>';
                htmltab += '<td style="display:none">' + response[i].ic_id + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_from + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_to + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_subject + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_receiveddate + '</td>';
                // htmltab += '<td >' + response[i].ic_attachment + '</td>';

                if (response[i].ic_attachment != "No attachments available." && response[i].ic_attachment != null && response[i].i_attachment !== "null") {
                    htmltab += '<td style="width:4%" ><i class="fa fa-paperclip" aria-hidden="true"></i></td>';
                }
                else {
                    htmltab += '<td style="width:4%"></td>';
                }

                htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg(' + response[i].ic_id + ')"></i></td>';
                htmltab += '</tr>';
            }

            // Update the table body with new data
            $('#claimtablebody').html(htmltab);

            // Example: Update a specific section with the response data (if any)
            $('#content-area').html(response);
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error('Error:', error);
        }
    });


    var client = $('#clientDropdown1').val();

    // Match the text in the dropdown options and set the value
    if (client) {
        $('#clientDropdown1 option').each(function () {
            if ($(this).val() == client) {
                $('#clientDropdown1').val($(this).val());
            }
        });
    }
}

function enquirydropdown() {
    var tab = "Enquiry";
    var year = $('#yearDropdown option:selected').text();
    var enquiry = $('#enquirydropdown option:selected').text();

    var clientid = $('#clientDropdown1').val();
    var data = {
        clientid: clientid,
        tab: tab,
        year: year,
        enquiry: enquiry,
    };

    $.ajax({
        url: '/InboxClient/EnquiryNo', // Controller action URL
        type: 'POST',             // HTTP method
        data: data,
        success: function (response) {
            console.log(response);

            $("#enquirynodiv").css("display", "contents");
            $("#claimnodiv").css("display", "none");
            $("#enquiry").css("display", "contents");
            $("#enquiryattach").css("display", "contents");
            $("#enquirytablehead").css("display", "contents");
            $("#enquirytablebody").css("display", "contents");
            $("#attachmenttable").css("display", "none");//attachment table
            $("#attachmenttablebody").css("display", "none");// attachment table body

            $('#enquiry-tab').attr('class', 'nav-link active');
            $('#enquiry-atttab').attr('class', 'nav-link ');

            $('#enquirytablebody').html('');  // Clear previous table content

            var htmltab = '';
            for (var i = 0; i < response.length; i++) {
                htmltab += '<tr>';
                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + (i + 1) + '</td>';
                /*                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_claimno + '</td>';*/
                htmltab += '<td style="display:none">' + response[i].ic_id + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_from + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_to + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_subject + '</td>';
                htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_receiveddate + '</td>';
                // htmltab += '<td >' + response[i].ic_attachment + '</td>';

                if (response[i].ic_attachment != "No attachments available." && response[i].ic_attachment != null && response[i].i_attachment !== "null") {
                    htmltab += '<td style="width:4%" ><i class="fa fa-paperclip" aria-hidden="true"></i></td>';
                }
                else {
                    htmltab += '<td style="width:4%"></td>';
                }

                htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg(' + response[i].ic_id + ')"></i></td>';
                htmltab += '</tr>';
            }

            // Update the table body with new data
            $('#enquirytablebody').html(htmltab);

            // Example: Update a specific section with the response data (if any)
            $('#content-area').html(response);
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error('Error:', error);
        }
    });


    var client = $('#clientDropdown1').val();

    // Match the text in the dropdown options and set the value
    if (client) {
        $('#clientDropdown1 option').each(function () {
            if ($(this).val() == client) {
                $('#clientDropdown1').val($(this).val());
            }
        });
    }
}

function clientchange1() {
    var urlParams = new URLSearchParams(window.location.search);
    var tab = urlParams.get('tab');

    var year = $('#yearDropdown option:selected').text();

    var clientid = $('#clientDropdown1').val();
    var data = {
        clientid: clientid,
        tab: tab,
        year: year,
    };


    if (tab == "Enquiry") {

        $('#claim-tab').attr('class', 'nav-link');
        $('#claim-atttab').attr('class', 'nav-link');

        $('#enquiry-atttab').attr('class', 'nav-link');
        $('#enquiry-tab').attr('class', 'nav-link active show');

        $.ajax({
            url: '/InboxClient/Clientchange1', // Controller action URL
            type: 'POST',             // HTTP method
            data: data,
            success: function (response) {
                console.log(response);

                //// If the response length is 0, do not update the table or URL
                //if (response.length === 0) {
                //    // alert("no data");
                //    $('#detailsTableBody').html('');
                //    return; // Exit the function early
                //}

                //// Make details grid visible when there is data

                $("#enquirynodiv").css("display", "contents");
                $("#claimnodiv").css("display", "none");
                $("#enquiry").css("display", "contents");
                $("#enquiryattach").css("display", "contents");
                $("#enquirytablehead").css("display", "contents");
                $("#enquirytablebody").css("display", "contents");
                $("#attachmenttable").css("display", "none");//attachment table
                $("#attachmenttablebody").css("display", "none");// attachment table body



                $('#enquirytablebody').html('');  // Clear previous table content

                var htmltab = '';
                for (var i = 0; i < response.sentclientList.length; i++) {
                    htmltab += '<tr>';
                    htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + (i + 1) + '</td>';
                    htmltab += '<td style="display:none">' + response.sentclientList[i].ic_id + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_from + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_to + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_subject + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_receiveddate + '</td>';
                    // htmltab += '<td >' + response[i].ic_attachment + '</td>';

                    if (response.sentclientList[i].ic_attachment != "No attachments available." && response.sentclientList[i].ic_attachment != null) {
                        htmltab += '<td style="width:4%" ><i class="fa fa-paperclip" aria-hidden="true"></i></td>';
                    }
                    else {
                        htmltab += '<td style="width:4%"></td>';
                    }

                    htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')"></i></td>';
                    htmltab += '</tr>';

                }
                const enquiryDropdown = document.getElementById('enquirydropdown');

                $('#enquirydropdown').html('<option>--Select--</option>');
                if (response.claimno && response.claimno.length > 0)
                {
                    // Loop through the claimno array and add each as an option to the dropdown
                    response.enquiryno.forEach(enquiry => {
                        const option = document.createElement('option');

                        option.value = enquiry.id;  // Set the id as the value of the option
                        option.text = enquiry.value;  // Set the value as the display text
                        enquiryDropdown.appendChild(option);
                    });
                }

                // Update the table body with new data
                $('#enquirytablebody').html(htmltab);

                // Example: Update a specific section with the response data (if any)
                $('#content-area').html(response);
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error('Error:', error);
            }
        });
    }

    else {
        tab = "Claim";

        $('#claim-tab').attr('class', 'nav-link active');
        $('#claim-atttab').attr('class', 'nav-link');
        $('#enquiry-tab').attr('class', 'nav-link');
        $('#enquiry-atttab').attr('class', 'nav-link');

        $.ajax({
            url: '/InboxClient/Clientchange1', // Controller action URL
            type: 'POST',             // HTTP method
            data: data,
            success: function (response) {
                console.log(response);

                //// If the response length is 0, do not update the table or URL
                //if (response.length === 0) {
                //    // alert("no data");
                //    $('#detailsTableBody1').html('');
                //    return; // Exit the function early
                //}

                //// Make details grid visible when there is data
                $("#enquirynodiv").css("display", "none");
                $("#claimnodiv").css("display", "contents");
                $("#claim").css("display", "contents");
                $("#claimattach").css("display", "contents");
                $("#claimtablehead").css("display", "contents");
                $("#claimtablebody").css("display", "contents");
                $("#attachmenttable").css("display", "none");//attachment table
                $("#attachmenttablebody").css("display", "none");// attachment table body


                $('#claimtablebody').html('');  // Clear previous table content

                var htmltab = '';
                for (var i = 0; i < response.sentclientList.length; i++) {
                    htmltab += '<tr>';
                    htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + (i + 1) + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_claimno + '</td>';
                    htmltab += '<td style="display:none">' + response.sentclientList[i].ic_id + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_from + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_to + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_subject + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')">' + response.sentclientList[i].ic_receiveddate + '</td>';
                    // htmltab += '<td >' + response[i].ic_attachment + '</td>';

                    if (response.sentclientList[i].ic_attachment != "No attachments available." && response.sentclientList[i].ic_attachment != null) {
                        htmltab += '<td style="width:4%" ><i class="fa fa-paperclip" aria-hidden="true"></i></td>';
                    }
                    else {
                        htmltab += '<td style="width:4%"></td>';
                    }

                    htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg(' + response.sentclientList[i].ic_id + ')"></i></td>';
                    htmltab += '</tr>';



                }
                const claimDropdown = document.getElementById('claimdropdown');

                $('#claimdropdown').html('<option>--Select--</option>');


                //// Loop through the claimno array and add each as an option to the dropdown
                //response.claimno.forEach(claim => {
                //    const option = document.createElement('option');

                //    option.value = claim.id;  // Set the id as the value of the option
                //    option.text = claim.value;  // Set the value as the display text
                //    claimDropdown.appendChild(option);
                //});

                if (response.claimno && response.claimno.length > 0) {
                    response.claimno.forEach(claim => {
                        const option = document.createElement('option');
                        option.value = claim.id;
                        option.text = claim.value;
                        claimDropdown.appendChild(option);
                    });
                }

                // Update the table body with new data
                $('#claimtablebody').html(htmltab);

                // Example: Update a specific section with the response data (if any)
                $('#content-area').html(response);
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error('Error:', error);
            }
        });
    }

    var client = $('#clientDropdown1').val();

    // Match the text in the dropdown options and set the value
    if (client) {
        $('#clientDropdown1 option').each(function () {
            if ($(this).val() == client) {
                $('#clientDropdown1').val($(this).val());
            }
        });
    }
}

function filter() {
    $("#addfilter").modal("show");
}
function submitfilter1() {

    var urlParams = new URLSearchParams(window.location.search);
    //var email = $('#clientDropdown1 option:selected').text();
    var tab = urlParams.get('tab');
    //alert(tab);

    $("#addfilter").modal("hide");
    var from = $('#i_from').val() || "";
    var to = $('#i_to').val() || "";
    var subject = $('#i_subject').val() || "";
    var hasthewords = $('#i_hasthewords').val() || "";
    //  var tab = $('#i_tab').val() || "";
    var year = $('#yearDropdown1 option:selected').text() || "";



    var data = {
        from: from,
        to: to,
        subject: subject,
        hasthewords: hasthewords,
        tab: tab,
        year: year,

    };


    if (tab == "Enquiry") {
        $('#enquiry-tab').attr('class', 'nav-link active');

        $("#enquirynodiv").css("display", "contents");
        $('#claimnodiv').css('display', 'none');
        $("#claimtablehead").css("display", 'none');
        $("#claimtablebody").css("display", 'none');
        $("#enquirytablehead").css("display", 'contents');
        $("#enquirytablebody").css("display", 'contents');
        $.ajax({
            url: '/InboxClient/filter?from=' + from + 'to=' + to + 'subject' + subject + 'hasthewords=' + hasthewords + 'year=' + year + 'tab=' + tab, // Controller action URL
            type: 'POST',             // HTTP method
            data: data,      // Data to send (query parameter)
            success: function (response) {
                // Handle success - update the page dynamically
                console.log('Response:', response);
                $('#enquirytablebody').html('');
                var htmltab = '';
                for (var i = 0; i < response.length; i++) {

                    htmltab += '<tr>';
                    htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + (i + 1) + '</td>';
                    htmltab += '<td style="display:none">' + response[i].ic_id + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_from + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_to + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_subject + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_receiveddate + '</td>';

                    if (response[i].ic_attachment != "No attachments available." && response[i].i_attachment !== "null" && response[i].ic_attachment != null) {
                        htmltab += '<td style="width:4%" ><i class="fa fa-paperclip" aria-hidden="true"></i></td>';
                    }
                    else {
                        htmltab += '<td style="width:4%"></td>';
                    }
                    htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg(' + response[i].ic_id + ')"></i></td>';

                    htmltab += '</tr>';
                }
                $('#enquirytablebody').html(htmltab);

                // Example: Update a specific section with the response data
                $('#content-area').html(response);
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error('Error:', error);
            }
        });
    }
    if (tab == "Claim") {
        $('#claim-tab').attr('class', 'nav-link active');
        $("#enquirynodiv").css("display", "none");
        $('#claimnodiv').css('display', 'contents');
        $("#claimtablehead").css("display", 'contents');
        $("#claimtablebody").css("display", 'contents');
        $("#enquirytablehead").css("display", 'none');
        $("#enquirytablebody").css("display", 'none');
        $.ajax({
            url: '/InboxClient/filter?from=' + from + 'to=' + to + 'subject' + subject + 'hasthewords=' + hasthewords + 'year=' + year + 'tab=' + tab, // Controller action URL
            type: 'POST',             // HTTP method
            data: data,      // Data to send (query parameter)
            success: function (response) {
                // Handle success - update the page dynamically
                console.log('Response:', response);
                $('#claimtablebody').html('');
                var htmltab = '';
                for (var i = 0; i < response.length; i++) {

                    htmltab += '<tr>';
                    htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + (i + 1) + '</td>';
                    htmltab += '<td style="display:none">' + response[i].ic_id + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_claimno + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_from + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_to + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_subject + '</td>';
                    htmltab += '<td onclick="inboxmsg(' + response[i].ic_id + ')">' + response[i].ic_receiveddate + '</td>';

                    if (response[i].ic_attachment != "No attachments available." && response[i].ic_attachment !== "null" && response[i].ic_attachment != null) {
                        htmltab += '<td style="width:4%" ><i class="fa fa-paperclip" aria-hidden="true"></i></td>';
                    }
                    else {
                        htmltab += '<td style="width:4%"></td>';
                    }
                    htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg(' + response[i].ic_id + ')"></i></td>';

                    htmltab += '</tr>';
                }
                $('#claimtablebody').html(htmltab);

                // Example: Update a specific section with the response data
                $('#content-area').html(response);
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error('Error:', error);
            }
        });
    }

    $('#i_from').val('');
    $('#i_to').val('');
    $('#i_subject').val('');
    $('#i_hasthewords').val('');
    $('#i_tab').val('');
    $('#yearDropdown1').val('');



}


function downloadFile(filePath) {
    console.log("Original File Path:", filePath); // Log the input

    try {
        // Trim and encode the file path
        const trimmedPath = filePath.trim();
        const encodedPath = encodeURIComponent(trimmedPath);
        console.log("Encoded File Path:", encodedPath); // Log the encoded path

        // Construct the URL
        const url = `/InboxClient/DownloadFile?filePath=${encodedPath}`;
        console.log("Download URL:", url); // Log the final URL

        // Open the file in a new tab
        window.open(url, "_blank");
    } catch (error) {
        console.error("Error during file download:", error); // Log any errors
    }
}
function viewfile() {
    const filePath = $('#filebuttons1').val();

    // Encode the file path to make it safe for URLs
    const encodedPath = encodeURIComponent(filePath);

    // Construct the URL to call the controller
    const url = `/InboxClient/ViewFile?filePath=${encodedPath}`;

    // Open the file in a new tab or start downloading
    window.open(url, "_blank");
}

function CancelData() {
    $("#addSubscription").modal("hide");
}

function inboxmsg(id) {

    // Show the modal
    $("#addSubscription").modal("show");

    // Data payload for the request
    var data = { id: id };

    // Perform AJAX POST request
    $.ajax({
        url: '/InboxEmail/Showdetails', // Controller action URL
        type: 'POST',                   // HTTP method
        data: data,                     // Data to send
        success: function (response) {
            console.log(response);

            if (response) {
                const attachmentsContainer = document.getElementById("attachdiv");

                // Clear previous content before updating
                attachmentsContainer.innerHTML = '';

                if (response.i_attachment != "No attachments available." && response.i_attachment !== "null" && response.i_attachment !== null) {
                    // const fileName = response.i_attachment.split(/[/\\]/).pop();


                    const attachments = response.i_attachment;
                    if (attachments) {
                        // Split attachments using a comma
                        const attachmentArray = attachments.trim().split(',').map(a => a.trim()); // Trim extra spaces

                        console.log("Raw Attachments:", attachments);
                        console.log("Split Attachments Array:", attachmentArray); // Debugging

                        // Generate the HTML
                        let attachmentHTML = '';

                        attachmentArray.forEach((attachmentPath) => {
                            if (!attachmentPath) return; // Ignore empty values

                            const fileName = attachmentPath.split(/[/\\]/).pop(); // Extract filename
                            const safeAttachmentPath = encodeURIComponent(attachmentPath); // Encode for safety

                            attachmentHTML += `
                                    <div class="attachments" style="margin: 8px 0;">
                                        <!-- Button for filename -->
                                        <button style="display: inline-block; padding: 8px; margin-right: 8px;" title="${fileName}">
                                            ${fileName}
                                        </button>
                                        <!-- Button for download -->
                                        <button style="display: inline-block; height: 42px; width: auto;"
                                                onclick="downloadFile('${safeAttachmentPath}')">
                                            <i class="fa-solid fa-chevron-down" style="font-size: 16px; color: black;"></i>
                                        </button>
                                    </div>`;
                        });

                        // Inject into the DOM
                        // document.getElementById("attachments-container").innerHTML = attachmentHTML;
                        document.getElementById('attachdiv').innerHTML = attachmentHTML;

                        // Show the attachment container
                        document.getElementById('attachdiv').style.display = 'block';
                    }

                    // Insert the generated HTML into a container


                } else {
                    $('#attachdiv').hide();
                }


                $('#ic_tolabel').text(response.i_to || 'N/A');
                $('#ic_fromlabel').text(response.i_from || 'N/A');
                $('#ic_subjectlabel').text(response.i_subject || 'N/A');

                // const formattedBody = (response.i_body || '').replace(/<br\s*\/?>/gi, "\n").replace(/<\/?[^>]+(>|$)/g, "");
                // $('#ic_bodylabel').val(formattedBody);

                //$('#ic_bodylabel').html(response.i_body || '');
                var bodyh = '' + response.i_body + '';
                $('#ic_bodyhtml').html(bodyh || '');




                // Optional: Update a specific content area with additional information
                $('#content-area').html(response.content || 'No content available.');


                $.ajax({
                    url: '/InboxEmail/inboxdates', // Controller action URL
                    type: 'POST',                   // HTTP method
                    data: data,                     // Data to send
                    success: function (response) {
                        console.log(response);


                        $('#arrow-container').empty();

                        // Iterate through the response and dynamically create elements
                        response.forEach((item, index) => {
                            const date = item.i_receiveddate; // Get the date
                            const id = item.i_id; // Get the ID

                            // Create a new arrow div
                            const arrowDiv = $(`<div class="arrow-item" id="date${index + 1}" onclick="inboxmsg('${id}')">${date}</div>`);

                            // Append the arrow div to the container
                            $('#arrow-container').append(arrowDiv);

                            // Add spacing if needed
                            $('#arrow-container').append('<div style="padding:12px"></div>');
                        });






                        // Optional: Update a specific content area with additional information
                        $('#content-area').html(response.content || 'No content available.');







                    },
                    error: function (xhr, status, error) {
                        // Display error notification
                        console.error('Error:', error);
                        alert('An error occurred while fetching the message details. Please try again later.');
                    }
                });



            } else {
                console.error('Empty response received');
                alert('No data available.');
            }
        },
        error: function (xhr, status, error) {
            // Display error notification
            console.error('Error:', error);
            alert('An error occurred while fetching the message details. Please try again later.');
        }
    });
}