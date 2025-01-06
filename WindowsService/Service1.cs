using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Search;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using HeyRed.Mime;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using WindowsService.Models;
using static WindowsService.Models.GraphApiEmailResponse;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Graph;
using System.Management;
//using Microsoft.Graph.Models;



namespace WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer = new Timer();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000; // 5 minutes
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
            timer.Enabled = false;
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            FetchEmails();
        }

        //City Marine EMS
        //private static string tenantId = "26d892b0-3196-4399-ba55-2f2f17cf30c7"; // Azure AD tenant ID
        //private static string clientId = "55fac2e0-ff2b-4445-ad42-398b7762c3a0"; // Application (client) ID
        //private static string clientSecret = "UrR8Q~jypNvM7T6ZEY-ntpesnxqqB12EVwaqob1E"; // Application (client) secret

        //EMSCityMarine
        //private static string tenantId = "26d892b0-3196-4399-ba55-2f2f17cf30c7"; // Azure AD tenant ID
        //private static string clientId = "b79fde29-4df9-413b-b8f4-5286e1224ee4"; // Application (client) ID
        //private static string clientSecret = "Jd ~8Q ~HEWXSMQh~K2eHVeFegN1awDT0N4.zJxa4o"; // Application (client) secret



        //CityMarine
        private static string tenantId = "26d892b0-3196-4399-ba55-2f2f17cf30c7"; // Azure AD tenant ID
        private static string clientId = "c84beebd-a48c-4aad-b0c1-814fcb7fba17"; // Application (client) ID
        private static string clientSecret = "F3S8Q~RV_vbrCt-uosG.WTe8UdLth0oQOAVdwcZy"; // Application (client) secret


        private static string authority = $"https://login.microsoftonline.com/{tenantId}";

        //public static async Task<string> GetAccessTokenAsync()
        //{
        //    var confidentialClient = ConfidentialClientApplicationBuilder.Create(clientId)
        //        .WithClientSecret(clientSecret)
        //        .WithAuthority(new Uri(authority))
        //        .Build();

        //    var result = await confidentialClient.AcquireTokenForClient(new string[] { "https://graph.microsoft.com/.default" })
        //        .ExecuteAsync();

        //    return result.AccessToken; // Access token
        //}




        private void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt"; ;
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }






        public static async Task<string> GetAccessTokenAsync()
        {
            try
            {
                var confidentialClient = ConfidentialClientApplicationBuilder.Create(clientId)
                    .WithClientSecret(clientSecret)  // Use the actual client secret value here
                    .WithAuthority(new Uri(authority))
                    .Build();

                var result = await confidentialClient.AcquireTokenForClient(new string[] { "https://graph.microsoft.com/.default" })
                  .ExecuteAsync();


                Console.WriteLine($"Access Token: {result.AccessToken}");
                return result.AccessToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during token acquisition: {ex.Message}");
                throw;
            }
        }



        //private async void FetchEmails()
        //{
        //    try
        //    {
        //        string token = await GetAccessTokenAsync();
        //        string userId = "0f5fb42b-eeab-48f5-8345-5e32fa67158e";  // Use correct user ID  6245c1fb-637d-484d-864a-26bdae49a6df   admin@Citymarinebrokers.com
        //        using (var httpClient = new HttpClient())
        //        {
        //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //            // Use the correct Graph API URL for a user



        //            string inboxUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/messages?$filter=isRead eq false";
        //            var inboxResponse = await httpClient.GetAsync(inboxUrl);

        //            if (inboxResponse.IsSuccessStatusCode)
        //            {
        //                var inboxContent = await inboxResponse.Content.ReadAsStringAsync();
        //                var emails = JsonConvert.DeserializeObject<GraphApiEmailResponse>(inboxContent);

        //                foreach (var email in emails.Value)
        //                {
        //                    ProcessEmail(email);

        //                    // Simulate `inbox.AddFlags(uid, MessageFlags.Seen, true)`
        //                    await MarkEmailAsRead(httpClient, userId, email.Id);

        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Error fetching emails: {inboxResponse.StatusCode}");
        //                var errorDetails = await inboxResponse.Content.ReadAsStringAsync();
        //                Console.WriteLine($"Error details: {errorDetails}");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in FetchEmails: {ex.Message}");
        //    }
        //}

        //private async void FetchEmails()
        //{
        //    try
        //    {
        //        string token = await GetAccessTokenAsync();
        //        string userId = "0f5fb42b-eeab-48f5-8345-5e32fa67158e";  // Use correct user ID  6245c1fb-637d-484d-864a-26bdae49a6df   admin@Citymarinebrokers.com
        //        using (var httpClient = new HttpClient())
        //        {
        //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //            // Use the correct Graph API URL for a user


        //            string sentUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/mailFolders/sentitems/messages?$filter=isRead eq false";
        //            string inboxUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/messages?$filter=isRead eq false";
        //            var inboxResponse = await httpClient.GetAsync(inboxUrl);
        //            var sendResponse = await httpClient.GetAsync(sentUrl);

        //            if (inboxResponse.IsSuccessStatusCode)
        //            {
        //                var inboxContent = await inboxResponse.Content.ReadAsStringAsync();
        //                var emails = JsonConvert.DeserializeObject<GraphApiEmailResponse>(inboxContent);

        //                foreach (var email in emails.Value)
        //                {
        //                    InboxEmail(email);

        //                    // Simulate `inbox.AddFlags(uid, MessageFlags.Seen, true)`
        //                    await MarkEmailAsRead(httpClient, userId, email.Id);

        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Error fetching emails: {inboxResponse.StatusCode}");
        //                var errorDetails = await inboxResponse.Content.ReadAsStringAsync();
        //                Console.WriteLine($"Error details: {errorDetails}");
        //            }
        //            if (sendResponse.IsSuccessStatusCode)
        //            {
        //                var sendContent = await sendResponse.Content.ReadAsStringAsync();
        //                var sendemails = JsonConvert.DeserializeObject<GraphApiEmailResponse>(sendContent);

        //                foreach (var sendemail in sendemails.Value)
        //                {
        //                    SentEmail(sendemail);

        //                    // Simulate `inbox.AddFlags(uid, MessageFlags.Seen, true)`
        //                    await MarkEmailAsRead(httpClient, userId, sendemail.Id);

        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Error fetching emails: {sendResponse.StatusCode}");
        //                var errorDetails = await sendResponse.Content.ReadAsStringAsync();
        //                Console.WriteLine($"Error details: {errorDetails}");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in FetchEmails: {ex.Message}");
        //    }
        //}
        //private async Task FetchEmails()
        //{
        //    try
        //    {
        //        string token = await GetAccessTokenAsync();
        //        string userId = "0f5fb42b-eeab-48f5-8345-5e32fa67158e";  // Use correct user ID
        //        using (var httpClient = new HttpClient())
        //        {
        //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //            string sentUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/mailFolders/sentitems/messages?$filter=isRead eq false";
        //            string inboxUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/messages?$filter=isRead eq false";

        //            var inboxResponse = await httpClient.GetAsync(inboxUrl);
        //            var sendResponse = await httpClient.GetAsync(sentUrl);

        //            if (inboxResponse.IsSuccessStatusCode)
        //            {
        //                var inboxContent = await inboxResponse.Content.ReadAsStringAsync();
        //                var emails = JsonConvert.DeserializeObject<GraphApiEmailResponse>(inboxContent);

        //                if (emails?.Value?.Any() == true)
        //                {
        //                    foreach (var email in emails.Value)
        //                    {
        //                        InboxEmail(email);
        //                        await MarkEmailAsRead(httpClient, userId, email.Id);
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine("No unread emails found in inbox.");
        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Error fetching inbox emails: {inboxResponse.StatusCode}");
        //                var errorDetails = await inboxResponse.Content.ReadAsStringAsync();
        //                Console.WriteLine($"Error details: {errorDetails}");
        //            }

        //            if (sendResponse.IsSuccessStatusCode)
        //            {
        //                var sendContent = await sendResponse.Content.ReadAsStringAsync();
        //                var sendemails = JsonConvert.DeserializeObject<GraphApiEmailResponse>(sendContent);

        //                if (sendemails?.Value?.Any() == true)
        //                {
        //                    foreach (var sendemail in sendemails.Value)
        //                    {
        //                        SentEmail(sendemail);
        //                        await MarkEmailAsRead(httpClient, userId, sendemail.Id);
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine("No unread emails found in sent items.");
        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Error fetching sent emails: {sendResponse.StatusCode}");
        //                var errorDetails = await sendResponse.Content.ReadAsStringAsync();
        //                Console.WriteLine($"Error details: {errorDetails}");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in FetchEmails: {ex.Message}");
        //    }
        //}
        private async Task FetchEmails()
        {
            try
            {
                // Get the access token
                string token = await GetAccessTokenAsync();
                // string userId = "0f5fb42b-eeab-48f5-8345-5e32fa67158e"; // Replace with the correct user ID

                string userId = "";
                string connectionString = "Server=P3NWPLSK12SQL-v13.shr.prod.phx3.secureserver.net;Database=CityMarineMgmt;User Id=CityMarineMgmt;Password=bZl34u0^6;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query to fetch email rule configuration data
                    string query1 = @"SELECT userid from dbo.tbl_emsuser";

                    using (SqlCommand cmd = new SqlCommand(query1, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                userId = reader["userid"].ToString();
                            }

                        }

                    }
                }
                string vemail = "EMS@Citymarinebrokers.com";
                using (var httpClient = new HttpClient())
                {
                    // Set the Authorization header
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Fetch all folders to dynamically locate the Sent Items folder
                    string folderUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/mailFolders";
                    var folderResponse = await httpClient.GetAsync(folderUrl);

                    if (!folderResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error fetching folders: {folderResponse.StatusCode}");
                        var folderErrorDetails = await folderResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Folder error details: {folderErrorDetails}");
                        return;
                    }

                    var folderContent = await folderResponse.Content.ReadAsStringAsync();
                    var folders = JsonConvert.DeserializeObject<GraphApiFolderResponse>(folderContent);

                    // Get the Sent Items folder ID
                    var sentFolder = folders.Value.FirstOrDefault(f => f.DisplayName.Equals("Sent Items", StringComparison.OrdinalIgnoreCase));
                    if (sentFolder == null)
                    {
                        Console.WriteLine("Sent Items folder not found.");
                        return;
                    }

                    string sentFolderId = sentFolder.Id;

                    // Define API URLs for Inbox and Sent Items
                    // string inboxUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/messages";
                    //string sentUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/mailFolders/{sentFolderId}/messages?$filter=isRead eq false";
                    string inboxUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/messages?$expand=attachments";
                    string sentUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/mailFolders/{sentFolderId}/messages?$expand=attachments";

                    // Fetch and process emails from Inbox
                    var inboxResponse = await httpClient.GetAsync(inboxUrl);
                    if (inboxResponse.IsSuccessStatusCode)
                    {
                        try
                        {
                            // Read the response content
                            var inboxContent = await inboxResponse.Content.ReadAsStringAsync();
                            Console.WriteLine("Raw Inbox Response: " + inboxContent);

                            // Deserialize the JSON content into GraphApiEmailResponse
                            var inboxEmails = JsonConvert.DeserializeObject<GraphApiEmailResponse>(inboxContent);

                            // Check if emails exist
                            if (inboxEmails?.Value?.Any() == true)
                            {
                                // Filter for unread emails
                                var unreadInboxEmails = inboxEmails.Value.Where(email => !email.IsRead).ToList();
                               // var unreadInboxEmails = inboxEmails.Value.ToList();

                                if (unreadInboxEmails.Any())
                                {
                                    foreach (var email in unreadInboxEmails)
                                    {
                                        try
                                        {
                                            // Process unread Inbox email
                                            InboxEmail(email, userId);

                                            // Mark email as read after processing
                                            await MarkEmailAsRead(httpClient, userId, email.Id);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Error processing email with ID {email.Id}: {ex.Message}");
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No unread emails found in Inbox.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No emails found in Inbox.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing inbox response: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve inbox emails. Status Code: {inboxResponse.StatusCode}");
                    }
                    // Fetch and process emails from Sent Items
                    var sentResponse = await httpClient.GetAsync(sentUrl);
                    if (sentResponse.IsSuccessStatusCode)
                    {
                        var sentContent = await sentResponse.Content.ReadAsStringAsync();
                        var sentEmails = JsonConvert.DeserializeObject<GraphApiEmailResponse>(sentContent);

                        if (sentEmails?.Value?.Any() == true)
                        {
                            // Filter for unread emails
                            //var unreadEmails = sentEmails.Value.Where(email => email.IsRead == false).ToList();
                            var unreadEmails = sentEmails.Value.ToList(); // Simply take all emails
                            if (unreadEmails.Any())
                            {
                                foreach (var email in unreadEmails)
                                {
                                    // Process unread email
                                    SentEmail(email, userId);

                                    // Mark email as read
                                    await MarkEmailAsRead(httpClient, userId, email.Id);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No unread emails found in Sent Items.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No emails found in Sent Items.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error fetching Sent emails: {sentResponse.StatusCode}");
                        var sentErrorDetails = await sentResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Sent error details: {sentErrorDetails}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchEmails: {ex.Message}");
            }
        }

        private async Task InboxEmail(GraphApiEmailResponse.GraphApiMessage email, string userId)
        {
            try
            {
                // Extract email details with null checks
                string subject = email.Subject ?? string.Empty;
                string from = email.From?.EmailAddress?.Address ?? string.Empty;
                string to = string.Join(", ", email.ToRecipients?.Select(r => r.EmailAddress?.Address) ?? new List<string>());
                string body = email.Body?.Content ?? string.Empty;
                string inReplyTo = email.InReplyToId ?? string.Empty;
                string messageId = email.Id ?? string.Empty;
                DateTime receivedDate = email.ReceivedDateTime ?? DateTime.MinValue;
                string emailType = "General";

                // Create an instance of AttachmentSaver to save attachments
                var attachmentSaver = new AttachmentSaver();

                // Await the asynchronous SaveAttachments method

                using (HttpClient httpClient = new HttpClient())
                {
                    // Create an instance of AttachmentSaver to save attachments


                    // Pass the HttpClient instance to the SaveAttachments method
                    string attachmentPath = await attachmentSaver.SaveAttachments(email, userId, httpClient);

                    // Insert into the database
                    InsertInboxEmailToDatabase(subject, from, to, body, inReplyTo, messageId, receivedDate, attachmentPath, emailType);

                    // Log success
                    WriteToFile($"Email processed successfully: {subject}");
                }
                // Insert into the database

            }
            catch (Exception ex)
            {
                // Log the error with details
                WriteToFile($"Error processing email (ID: {email?.Id ?? "Unknown"}): {ex.Message}");
            }
        }
        //private async Task SentEmail(GraphApiEmailResponse.GraphApiMessage email, string userId)
        //{
        //    try
        //    {
        //        // Extract email details with null checks
        //        string subject = email.Subject ?? string.Empty;
        //        string from = email.From?.EmailAddress?.Address ?? string.Empty;
        //        string to = string.Join(", ", email.ToRecipients?.Select(r => r.EmailAddress?.Address) ?? new List<string>());
        //        string body = email.Body?.Content ?? string.Empty;
        //        string inReplyTo = email.InReplyToId ?? string.Empty;
        //        string messageId = email.Id ?? string.Empty;
        //        DateTime sentDate = email.ReceivedDateTime ?? DateTime.MinValue; // Use appropriate sent date field if available
        //        string emailType = "Sent";

        //        // Create an instance of AttachmentSaver to save attachments
        //        var attachmentSaver = new AttachmentSaver();

        //        // Await the asynchronous SaveAttachments method

        //        using (HttpClient httpClient = new HttpClient())
        //        {
        //            // Create an instance of AttachmentSaver to save attachments


        //            // Pass the HttpClient instance to the SaveAttachments method
        //            string attachmentPath = await attachmentSaver.SaveAttachments1(email, userId, httpClient);

        //            // Insert into the database
        //            InsertSentEmailToDatabase(
        //          subject,
        //          from,
        //          to,
        //          body,
        //          inReplyTo,
        //          messageId,
        //          sentDate,
        //          attachmentPath,
        //          emailType
        //      );

        //            // Log success
        //            WriteToFile($"Sent email processed successfully: {subject}");
        //        }
        //        // Insert into the database

        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error with details
        //        string emailId = email?.Id ?? "Unknown";
        //        WriteToFile($"Error processing sent email (ID: {emailId}): {ex.Message}");

        //    }
        //}

        private async Task SentEmail(GraphApiEmailResponse.GraphApiMessage email, string userId)
        {
            try
            {
                // Extract email details with null checks
                string subject = email.Subject ?? string.Empty;
                string from = email.From?.EmailAddress?.Address ?? string.Empty;
                string to = string.Join(", ", email.ToRecipients?.Select(r => r.EmailAddress?.Address) ?? new List<string>());
                string body = email.Body?.Content ?? string.Empty;
                string inReplyTo = email.InReplyToId ?? string.Empty;
                string messageId = email.Id ?? string.Empty;
                DateTime sentDate = email.ReceivedDateTime ?? DateTime.MinValue; // Use appropriate sent date field if available
                string emailType = "Sent";

                // Process attachments if available
                var attachmentSaver = new AttachmentSaver();

                using (HttpClient httpClient = new HttpClient())
                {
                    // Save attachments
                    string attachmentPath = await attachmentSaver.SaveAttachments1(email, userId, httpClient);

                    // Insert email into the database along with attachments
                    InsertSentEmailToDatabase(
                        subject,
                        from,
                        to,
                        body,
                        inReplyTo,
                        messageId,
                        sentDate,
                        attachmentPath,
                        emailType
                    );

                    // Log success
                    WriteToFile($"Sent email processed successfully: {subject} with attachments.");
                }

            }
            catch (Exception ex)
            {
                // Log the error with details
                string emailId = email?.Id ?? "Unknown";
                WriteToFile($"Error processing sent email (ID: {emailId}): {ex.Message}");
            }
        }


        private void InsertInboxEmailToDatabase(string subject, string from, string to, string body, string inReplyTo, string messageId, DateTime receivedDate, string attachmentPath, string emailType)
        {
            string connectionString = "Server=P3NWPLSK12SQL-v13.shr.prod.phx3.secureserver.net;Database=CityMarineMgmt;User Id=CityMarineMgmt;Password=bZl34u0^6;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Query to fetch email rule configuration data
                string query1 = @"SELECT E_id, pv1.pv_parametervalue as E_parameterName, pv2.pv_parametervalue as E_conditionName,
                                pv3.pv_parametervalue as E_categoryName, E_category, E_value, E_parameter, E_condition,
                                E_createdby, E_updatedby, E_updateddate, E_createddate, E_isactive 
                          FROM [dbo].[tbl_EmailRuleConfg] ec
                          JOIN tbl_parametervaluemaster pv1 ON pv1.pv_id = ec.E_parameter
                          JOIN tbl_parametervaluemaster pv2 ON pv2.pv_id = ec.E_condition
                          JOIN tbl_parametervaluemaster pv3 ON pv3.pv_id = ec.E_category 
                          WHERE E_isactive = '1'";




                using (SqlCommand cmd = new SqlCommand(query1, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Loop through each row in the result
                        while (reader.Read())
                        {
                            string E_parameterName = reader["E_parameterName"].ToString();
                            string E_conditionName = reader["E_conditionName"].ToString();
                            string E_categoryName = reader["E_categoryName"].ToString();
                            string E_value = reader["E_value"].ToString();

                            // Check if the parameter name is "Subject" or "Domain" and apply corresponding conditions
                            //if (E_parameterName == "Subject" || E_parameterName == "Domain")
                            //{
                            //    bool conditionMatched = false;

                            //    // Check the condition against subject or domain (for "Subject" or "Domain")
                            //    if (E_conditionName == "Contains")
                            //    {
                            //        conditionMatched = (subject.IndexOf(E_value, StringComparison.OrdinalIgnoreCase) >= 0);
                            //        emailType = E_categoryName;
                            //    }
                            //    else if (E_conditionName == "Begin With")
                            //    {
                            //        conditionMatched = (subject.StartsWith(E_value, StringComparison.OrdinalIgnoreCase));
                            //        emailType = E_categoryName;
                            //    }
                            //    else if (E_conditionName == "Equal To")
                            //    {
                            //        conditionMatched = subject.Equals(E_value, StringComparison.OrdinalIgnoreCase);
                            //        emailType = E_categoryName;
                            //    }

                            //    // If the condition matches, set the email type to the category name
                            //    //if (conditionMatched)
                            //    //{
                            //    //    emailType = E_categoryName;
                            //    //}
                            //}


                            if (E_parameterName == "Subject")
                            {
                                if (E_conditionName == "Contains")
                                {
                                    //if (subject.Contains(E_value))
                                    if (subject.IndexOf(E_value, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        emailType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Begin With")
                                {
                                    if (subject.StartsWith(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        emailType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Equal To")
                                {
                                    if (subject.Equals(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        emailType = E_categoryName;
                                    }
                                }

                            }
                            else if (E_parameterName == "Domain")
                            {
                                if (E_conditionName == "Contains")
                                {
                                    //if (subject.Contains(E_value))
                                    if (subject.IndexOf(E_value, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        emailType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Begin With")
                                {
                                    if (subject.StartsWith(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        emailType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Equal To")
                                {
                                    if (subject.Equals(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        emailType = E_categoryName;
                                    }
                                }
                            }
                        }
                    }
                }


                string query2 = @"SELECT COUNT(*) FROM [dbo].[tbl_customermaster] WHERE c_email = @Email";


                using (SqlCommand cmd2 = new SqlCommand(query2, conn))
                {
                    cmd2.Parameters.AddWithValue("@Email", from);

                    // Execute the query and get the count
                    int count = (int)cmd2.ExecuteScalar();

                    if (count == 0)
                    {
                        emailType = "General";
                    }
                }

                // Insert email data into the tbl_InboxEmail table
                string query = @"INSERT INTO dbo.tbl_InboxEmail (i_subject, i_from, i_to, i_body, i_replyto, i_messageid, i_receiveddate, i_attachment, i_type)
                         VALUES (@i_subject, @i_from, @i_to, @i_body, @i_replyto, @i_messageid, @i_receiveddate, @i_attachment, @i_type)";

                using (SqlCommand cmd1 = new SqlCommand(query, conn))
                {
                    // Add parameters to the insert query
                    cmd1.Parameters.AddWithValue("@i_subject", subject);
                    cmd1.Parameters.AddWithValue("@i_from", from);
                    cmd1.Parameters.AddWithValue("@i_to", to);
                    cmd1.Parameters.AddWithValue("@i_body", body);
                    cmd1.Parameters.AddWithValue("@i_replyto", inReplyTo);
                    cmd1.Parameters.AddWithValue("@i_messageid", messageId);
                    cmd1.Parameters.AddWithValue("@i_receiveddate", receivedDate);
                    cmd1.Parameters.AddWithValue("@i_attachment", attachmentPath);
                    cmd1.Parameters.AddWithValue("@i_type", emailType);

                    // Execute the query to insert the email into the database
                    cmd1.ExecuteNonQuery();
                }
            }
        }


        private void InsertSentEmailToDatabase(string subject, string from, string to, string body, string inReplyTo, string messageId, DateTime sendDate, string attachmentPath, string sType)
        {
            string connectionString = "Server=P3NWPLSK12SQL-v13.shr.prod.phx3.secureserver.net;Database=CityMarineMgmt;User Id=CityMarineMgmt;Password=bZl34u0^6;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Query to fetch email rules
                string emailRuleQuery = @"
            SELECT 
                E_id,
                pv1.pv_parametervalue AS E_parameterName, 
                pv2.pv_parametervalue AS E_conditionName, 
                pv3.pv_parametervalue AS E_categoryName,
                E_category, 
                E_value, 
                E_parameter, 
                E_condition, 
                E_createdby, 
                E_updatedby, 
                E_updateddate, 
                E_createddate, 
                E_isactive
            FROM dbo.tbl_EmailRuleConfg ec
            JOIN tbl_parametervaluemaster pv1 ON pv1.pv_id = ec.E_parameter
            JOIN tbl_parametervaluemaster pv2 ON pv2.pv_id = ec.E_condition
            JOIN tbl_parametervaluemaster pv3 ON pv3.pv_id = ec.E_category
            WHERE E_isactive = '1'";

                // Read rules and determine email type
                using (SqlCommand ruleCmd = new SqlCommand(emailRuleQuery, conn))
                using (SqlDataReader reader = ruleCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string parameterName = reader["E_parameterName"].ToString();
                        string conditionName = reader["E_conditionName"].ToString();
                        string categoryName = reader["E_categoryName"].ToString();
                        string value = reader["E_value"].ToString();

                        if (parameterName.Equals("Subject", StringComparison.OrdinalIgnoreCase))
                        {
                            sType = ApplyRule(subject, conditionName, value, categoryName, sType);
                        }
                        else if (parameterName.Equals("Domain", StringComparison.OrdinalIgnoreCase))
                        {
                            sType = ApplyRule(from, conditionName, value, categoryName, sType);
                        }
                    }
                }

                // Insert email into SentEmail table
                string insertQuery = @"
            INSERT INTO dbo.tbl_SentEmail (
                s_subject, 
                s_from, 
                s_to, 
                s_body, 
                s_replyto, 
                s_messageid, 
                s_sentdate, 
                s_attachment, 
                s_type
            ) 
            VALUES (
                @s_subject, 
                @s_from, 
                @s_to, 
                @s_body, 
                @s_replyto, 
                @s_messageid, 
                @s_sentdate, 
                @s_attachment, 
                @s_type
            )";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@s_subject", subject);
                    insertCmd.Parameters.AddWithValue("@s_from", from);
                    insertCmd.Parameters.AddWithValue("@s_to", to);
                    insertCmd.Parameters.AddWithValue("@s_body", body);
                    insertCmd.Parameters.AddWithValue("@s_replyto", inReplyTo);
                    insertCmd.Parameters.AddWithValue("@s_messageid", messageId);
                    insertCmd.Parameters.AddWithValue("@s_sentdate", sendDate);
                    insertCmd.Parameters.AddWithValue("@s_attachment", attachmentPath);
                    insertCmd.Parameters.AddWithValue("@s_type", sType);

                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        private string ApplyRule(string fieldValue, string conditionName, string ruleValue, string categoryName, string currentType)
        {
            if (conditionName.Equals("Contains", StringComparison.OrdinalIgnoreCase) &&
                fieldValue.IndexOf(ruleValue, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return categoryName;
            }
            else if (conditionName.Equals("Begin With", StringComparison.OrdinalIgnoreCase) &&
                     fieldValue.StartsWith(ruleValue, StringComparison.OrdinalIgnoreCase))
            {
                return categoryName;
            }
            else if (conditionName.Equals("Equal To", StringComparison.OrdinalIgnoreCase) &&
                     fieldValue.Equals(ruleValue, StringComparison.OrdinalIgnoreCase))
            {
                return categoryName;
            }

            return currentType; // Default to current type if no rule matches
        }


        public class AttachmentSaver
        {



            //Inbox
            public async Task<string> SaveAttachments(GraphApiEmailResponse.GraphApiMessage email, string userId, HttpClient httpClient)
            {
                List<string> filePaths = new List<string>();

                // Ensure email has attachments
                if (email.Attachments != null && email.Attachments.Count > 0)
                {
                    foreach (var attachment in email.Attachments)
                    {
                        string senderEmail = email.From?.EmailAddress?.Address ?? "UnknownSender";
                        string attachmentsFolder = Path.Combine("E:\\ServiceLog1\\Attachments", senderEmail);

                        // Ensure the folder exists
                        if (!Directory.Exists(attachmentsFolder))
                        {
                            Directory.CreateDirectory(attachmentsFolder);
                        }

                        string fileName = SanitizeFileName(attachment.Name);
                        string filePath = Path.Combine(attachmentsFolder, fileName);

                        // If the attachment is of type 'GraphApiAttachment'
                        if (attachment is GraphApiAttachment fileAttachment)
                        {
                            try
                            {
                                // Check if the attachment has a contentUrl or contentBytes
                                if (!string.IsNullOrEmpty(fileAttachment.ContentUrl))
                                {
                                    // Download the attachment using its contentUrl
                                    await DownloadAttachmentFromUrl(fileAttachment.ContentUrl, filePath, httpClient);
                                }
                                else if (!string.IsNullOrEmpty(fileAttachment.ContentBytes))
                                {
                                    // Decode and save attachment from Base64 contentBytes
                                    byte[] content = Convert.FromBase64String(fileAttachment.ContentBytes);
                                    File.WriteAllBytes(filePath, content);
                                }

                                filePaths.Add(filePath);
                                Console.WriteLine($"Attachment saved: {filePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error downloading attachment {fileAttachment.Name}: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Attachment {attachment.Name} is not a file attachment.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No attachments found.");
                    return "No attachments available.";
                }

                return string.Join(Environment.NewLine, filePaths);
            }


            private async Task DownloadAttachmentFromUrl(string contentUrl, string filePath, HttpClient httpClient)
            {
                var response = await httpClient.GetAsync(contentUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    File.WriteAllBytes(filePath, content);
                }
                else
                {
                    Console.WriteLine($"Failed to download attachment from URL: {contentUrl}");
                }
            }

            private string SanitizeFileName(string fileName)
            {

                return string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
            }
            //Send
            public async Task<string> SaveAttachments1(GraphApiEmailResponse.GraphApiMessage email, string userId, HttpClient httpClient)
            {
                List<string> filePaths = new List<string>();

                // Ensure email has attachments
                if (email.Attachments != null && email.Attachments.Any())
                {
                    foreach (var attachment in email.Attachments)
                    {
                        string senderEmail = email.From?.EmailAddress?.Address ?? "UnknownSender";
                        string attachmentsFolder = Path.Combine("E:\\ServiceLog1\\Attachments", senderEmail);

                        // Ensure the folder exists
                        if (!Directory.Exists(attachmentsFolder))
                        {
                            Directory.CreateDirectory(attachmentsFolder);
                        }

                        string fileName = SanitizeFileName(attachment.Name);
                        string filePath = Path.Combine(attachmentsFolder, fileName);

                        try
                        {
                            // Check if the attachment is of type 'GraphApiAttachment'
                            if (attachment is GraphApiEmailResponse.GraphApiAttachment graphAttachment)
                            {
                                // If the attachment has contentBytes (Base64 encoded)
                                if (!string.IsNullOrEmpty(graphAttachment.ContentBytes))
                                {
                                    byte[] content = Convert.FromBase64String(graphAttachment.ContentBytes);
                                    File.WriteAllBytes(filePath, content);
                                }
                                // If the attachment has a contentUrl, download the file
                                else if (!string.IsNullOrEmpty(graphAttachment.ContentUrl))
                                {
                                    await DownloadAttachmentFromUrl1(graphAttachment.ContentUrl, filePath, httpClient);
                                }

                                filePaths.Add(filePath);
                                Console.WriteLine($"Attachment saved: {filePath}");
                            }
                            else
                            {
                                Console.WriteLine($"Attachment {attachment.Name} is not a file attachment.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing attachment {attachment.Name}: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No attachments found.");
                    return "No attachments available.";
                }

                return string.Join(Environment.NewLine, filePaths);
            }






            public async Task DownloadAttachmentFromUrl1(string contentUrl, string filePath, HttpClient httpClient)
            {
                try
                {
                    // Get the attachment content from the URL
                    var response = await httpClient.GetAsync(contentUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsByteArrayAsync();
                        File.WriteAllBytes(filePath, content);
                        Console.WriteLine($"Attachment downloaded: {filePath}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to download attachment from {contentUrl}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading attachment: {ex.Message}");
                }
            }




        }





        private async Task MarkEmailAsRead(HttpClient httpClient, string userId, string emailId)
        {
            try
            {
                string markAsReadUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/messages/{emailId}";
                var patchData = new StringContent("{\"isRead\": true}", Encoding.UTF8, "application/json");

                var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), markAsReadUrl)
                {
                    Content = patchData
                };

                var patchResponse = await httpClient.SendAsync(patchRequest);
                if (patchResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Marked email {emailId} as read.");
                }
                else
                {
                    Console.WriteLine($"Error marking email {emailId} as read: {patchResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MarkEmailAsRead: {ex.Message}");
            }
        }


    }
}
