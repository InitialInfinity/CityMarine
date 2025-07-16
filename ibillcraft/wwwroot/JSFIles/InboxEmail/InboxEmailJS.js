function clienttab() {
    var tab = "Client";

    $.ajax({
        url: '/InboxEmail/ClientGenaral?tab=' + tab, // Controller action URL
        type: 'POST',             // HTTP method
        data: { tab: tab },      // Data to send (query parameter)
        success: function (response) {
            // Handle success - update the page dynamically
            console.log('Response:', response);
            $('#clientgeneral').css('display', 'contents');
            $('#enquiryclaim').css('display', 'none');
            $('#clientgeneralbody').html('');
            var htmltab = '';
            for (var i = 0; i < response.length; i++) {
                htmltab += '<tr>';
                htmltab += '<td style="width:6%" onclick="inboxmsg(' + response[i].i_id + ')">' + (i + 1) + '</td>';
                htmltab += '<td style="display:none">' + response[i].i_id + '</td>';
                htmltab += '<td  style="width:12%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_from + '</td>';
                htmltab += '<td  style="width:12%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_to + '</td>';
                htmltab += '<td style="width:10%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_subject + '</td>';


                if (response[i].i_attachment != "No attachments available." && response[i].i_attachment !== "null" && response[i].i_attachment !== null) {
                    htmltab += '<td style="width:4%" ><i class="fa fa-paperclip" aria-hidden="true"></i></td>';
                }
                else {
                    htmltab += '<td style="width:4%"></td>';
                }


                htmltab += '<td  style="width:12%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_receiveddate + '</td>';
                htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg(' + response[i].i_id + ')"></i></td>';
                htmltab += '</tr>';
            }
            $('#clientgeneralbody').html(htmltab);

            // Example: Update a specific section with the response data
            $('#content-area').html(response);
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error('Error:', error);
        }
    });
}

function generaltab() {
    var tab = "General";

    $.ajax({
        url: '/InboxEmail/ClientGenaral?tab=' + tab, // Controller action URL
        type: 'POST',             // HTTP method
        data: { tab: tab },      // Data to send (query parameter)
        success: function (response) {
            // Handle success - update the page dynamically
            console.log('Response:', response);
            $('#clientgeneral').css('display', 'contents');
            $('#enquiryclaim').css('display', 'none');
            $('#clientgeneralbody').html('');
            var htmltab = '';
            for (var i = 0; i < response.length; i++) {
                htmltab += '<tr>';
                htmltab += '<td style="width:6%" onclick="inboxmsg(' + response[i].i_id + ')">' + (i + 1) + '</td>';
                htmltab += '<td style="display:none">' + response[i].i_id + '</td>';
                htmltab += '<td  style="width:12%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_from + '</td>';
                htmltab += '<td  style="width:12%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_to + '</td>';
                htmltab += '<td style="width:10%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_subject + '</td>';


                if (response[i].i_attachment != "No attachments available." && response[i].i_attachment !== "null" && response[i].i_attachment !== null) {
                    htmltab += '<td style="width:4%" ><i class="fa fa-paperclip" aria-hidden="true"></i></td>';
                }
                else {
                    htmltab += '<td style="width:4%"></td>';
                }


                htmltab += '<td  style="width:12%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_receiveddate + '</td>';
                htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg(' + response[i].i_id + ')"></i></td>';
                htmltab += '</tr>';
            }
            $('#clientgeneralbody').html(htmltab);

            // Example: Update a specific section with the response data
            $('#content-area').html(response);
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error('Error:', error);
        }
    });
}

function enquirytab() {
    var tab = "Enquiry";

    $.ajax({
        url: '/InboxEmail/EnquiryClaim?tab=' + tab, // Controller action URL
        type: 'POST',             // HTTP method
        data: { tab: tab },      // Data to send (query parameter)
        success: function (response) {
            // Handle success - update the page dynamically
            console.log('Response:', response);
            $('#enquiryclaim').css('display', 'contents');
            $('#clientgeneral').css('display', 'none');
            $('#enquiryclaimbody').html('');
            var htmltab = '';
            for (var i = 0; i < response.length; i++) {
                htmltab += '<tr>';
                htmltab += '<td>' + (i + 1) + '</td>';
                let i_from = response[i].i_from.trim();
                htmltab += '<td><a href="/InboxClient?email=' + response[i].i_from + '&client=' + response[i].i_fromname + '&tab=' + tab + '">' + response[i].i_fromname + '</a></td>';

                htmltab += '<td>' + response[i].i_receiveddate + '</td>';
                htmltab += '</tr>';
            }
            $('#enquiryclaimbody').html(htmltab);

            // Example: Update a specific section with the response data
            $('#content-area').html(response);
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error('Error:', error);
        }
    });
}

function claimtab() {
    var tab = "Claim";

    $.ajax({
        url: '/InboxEmail/EnquiryClaim?tab=' + tab, // Controller action URL
        type: 'POST',             // HTTP method
        data: { tab: tab },      // Data to send (query parameter)
        success: function (response) {
            // Handle success - update the page dynamically
            console.log('Response:', response);
            $('#enquiryclaim').css('display', 'contents');
            $('#clientgeneral').css('display', 'none');
            $('#enquiryclaimbody').html('');
            var htmltab = '';
            for (var i = 0; i < response.length; i++) {
                htmltab += '<tr>';
                htmltab += '<td>' + (i + 1) + '</td>';
                let i_from = response[i].i_from.trim();
                htmltab += '<td><a href="/InboxClient?email=' + response[i].i_from + '&client=' + response[i].i_fromname + '&tab=' + tab + '">' + response[i].i_fromname + '</a></td>';
                htmltab += '<td>' + response[i].i_receiveddate + '</td>';
                htmltab += '</tr>';
            }
            $('#enquiryclaimbody').html(htmltab);

            // Example: Update a specific section with the response data
            $('#content-area').html(response);
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error('Error:', error);
        }
    });
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


function filter() {
    $("#addfilter").modal("show");
}

function submitfilter() {

    $("#addfilter").modal("hide");
    var from = $('#i_from').val();
    var to = $('#i_to').val();
    var subject = $('#i_subject').val();
    var hasthewords = $('#i_hasthewords').val();
    var tab = $('#i_tab').val();

    var data = {
        from: from,
        to: to,
        subject: subject,
        hasthewords: hasthewords,
        tab: tab,

    };
    if (tab == 'Enquiry' || tab == "Claim") {
        if (tab == "Enquiry") {
            $('#enquiry-tab').attr('class', 'nav-link active');
        }
        if (tab == "Claim") {
            $('#claim-tab').attr('class', 'nav-link active');
            $('#general-tab').attr('class', 'nav-link');
            $('#client-tab').attr('class', 'nav-link');
            $('#enquiry-tab').attr('class', 'nav-link');
        }
        
        $.ajax({
            url: '/InboxEmail/EnquiryClaimfilter?from=' + from + 'to=' + to + 'subject' + subject + 'hasthewords=' + hasthewords + 'tab=' + tab, // Controller action URL
            type: 'POST',             // HTTP method
            data: data,      // Data to send (query parameter)
            success: function (response) {
                // Handle success - update the page dynamically
                console.log('Response:', response);
                $('#enquiryclaim').css('display', 'contents');
                $('#enquiryclaimbody').css('display', 'contents');
                $('#clientgeneral').css('display', 'none');
                $('#clientgeneralbody').css('display', 'none');
                $('#enquiryclaimbody').html('');
                var htmltab = '';
                for (var i = 0; i < response.length; i++) {
                    htmltab += '<tr>';
                    htmltab += '<td>' + (i + 1) + '</td>';
                    let i_from = response[i].i_from.trim();
                    htmltab += '<td><a href="/InboxClient?email=' + response[i].i_from + '&client=' + response[i].i_fromname + '&tab=' + tab + '">' + response[i].i_fromname + '</a></td>';

                    htmltab += '<td>' + response[i].i_receiveddate + '</td>';
                    htmltab += '</tr>';
                }
                $('#enquiryclaimbody').html(htmltab);

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
        if (tab == "General") {
            $('#general-tab').attr('class', 'nav-link active');
            $('#client-tab').attr('class', 'nav-link');
            $('#enquiry-tab').attr('class', 'nav-link');
            $('#claim-tab').attr('class', 'nav-link');
        }
        else {
            $('#client-tab').attr('class', 'nav-link active');
            $('#general-tab').attr('class', 'nav-link');
            $('#enquiry-tab').attr('class', 'nav-link');
            $('#claim-tab').attr('class', 'nav-link');
        }
        $.ajax({
            url: '/InboxEmail/EnquiryClaimfilter?from=' + from + 'to=' + to + 'subject' + subject + 'hasthewords=' + hasthewords + 'tab=' + tab, // Controller action URL
           // url: '/InboxEmail/Generalfilter?from=' + from + 'to=' + to + 'subject' + subject + 'hasthewords=' + hasthewords + 'tab=' + tab, // Controller action URL
            type: 'POST',             // HTTP method
            data: data,      // Data to send (query parameter)
            success: function (response) {
                // Handle success - update the page dynamically
                console.log('Response:', response);
                $('#enquiryclaim').css('display', 'none');
                $('#enquiryclaimbody').css('display', 'none');
                $('#clientgeneral').css('display', 'contents');
                $('#clientgeneralbody').css('display', 'contents');
                $('#clientgeneralbody').html('');
                var htmltab = '';
                for (var i = 0; i < response.length; i++) {
                    htmltab += '<tr>';
                    htmltab += '<td style="width:6%" onclick="inboxmsg(' + response[i].i_id + ')">' + (i + 1) + '</td>';
                    htmltab += '<td style="display:none">' + response[i].i_id + '</td>';
                    htmltab += '<td  style="width:12%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_from + '</td>';
                    htmltab += '<td  style="width:12%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_to + '</td>';
                    htmltab += '<td style="width:10%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_subject + '</td>';

                    if (response[i].i_attachment != "No attachments available." && response[i].i_attachment !== "null" && response[i].i_attachment !== null) {
                        htmltab += '<td style="width:4%" ><i class="fa fa-paperclip" aria-hidden="true"></i></td>';
                    }
                    else {
                        htmltab += '<td style="width:4%"></td>';
                    }
                    htmltab += '<td  style="width:12%" onclick="inboxmsg(' + response[i].i_id + ')">' + response[i].i_receiveddate + '</td>';
                    htmltab += '<td><i class="fas fa-eye text-primary" onclick="inboxmsg(' + response[i].i_id + ')"></i></td>';
                    htmltab += '</tr>';
                }
                $('#clientgeneralbody').html(htmltab);

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
}