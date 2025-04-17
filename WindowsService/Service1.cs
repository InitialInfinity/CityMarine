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
using Microsoft.Graph.Models.ExternalConnectors;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using System.Globalization;
//using Microsoft.Graph.Models;



namespace WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer = new Timer();
        private readonly HttpClient _httpClient;


        public Service1()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            // timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
          //  timer.Interval = 60000*60*24; // 5 minutes
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
            timer.Enabled = false;
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            ScheduleNextRun();
            FetchEmails();
        }
        private void ScheduleNextRun()
        {
            var now = DateTime.Now;
            var nextRunTime = DateTime.Today.AddDays(1).AddHours(23).AddMinutes(30); // 11:30 PM tomorrow

            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1); // If the time has passed today, schedule for the next day
            }

            var interval = nextRunTime - now;
            timer.Interval = interval.TotalMilliseconds;
        }
        //private void ScheduleNextRun()
        //{
        //    var interval = TimeSpan.FromMinutes(3); // Run after 1 minute
        //    timer.Interval = interval.TotalMilliseconds;
        //}
        //CityMarine
        private static string tenantId = "26d892b0-3196-4399-ba55-2f2f17cf30c7"; // Azure AD tenant ID
        private static string clientId = "c84beebd-a48c-4aad-b0c1-814fcb7fba17"; // Application (client) ID
        private static string clientSecret = "F3S8Q~RV_vbrCt-uosG.WTe8UdLth0oQOAVdwcZy"; // Application (client) secret


        private static string authority = $"https://login.microsoftonline.com/{tenantId}";


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

        private async Task FetchEmails()
        {
            try
            {
                // Get the access token
                string token = await GetAccessTokenAsync();
                // string userId = "0f5fb42b-eeab-48f5-8345-5e32fa67158e"; // Replace with the correct user ID

                string userId = "";
                string connectionString = ConfigurationManager.AppSettings["constring"];
                // string connectionString = "Server=P3NWPLSK12SQL-v13.shr.prod.phx3.secureserver.net;Database=CityMarineMgmt;User Id=CityMarineMgmt;Password=bZl34u0^6;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";

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
              //  string vemail = "EMS@Citymarinebrokers.com";
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
                    var inboxFolder = folders.Value.FirstOrDefault(f => f.DisplayName.Equals("Inbox", StringComparison.OrdinalIgnoreCase));
                    if (inboxFolder == null)
                    {
                        Console.WriteLine("Sent Items folder not found.");
                        return;
                    }
                    string inboxfolderid = inboxFolder.Id;
                    string sentFolderId = sentFolder.Id;
                    string inboxUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/mailFolders/{inboxfolderid}/messages?$expand=attachments&$top=200";
                    string sentUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/mailFolders/{sentFolderId}/messages?$expand=attachments&$top=200";

                    httpClient.DefaultRequestHeaders.Add("Prefer", "outlook.timezone=\"Asia/Kolkata\"");

                    //INBOX
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
                            string time = "";
                            // Check if emails exist
                            if (inboxEmails?.Value?.Any() == true)
                            {
                                // Filter for unread emails
                                //var unreadInboxEmails = inboxEmails.Value.Where(email => !email.IsRead).ToList();

                                //latest email
                                //var Emails = inboxEmails.Value.OrderByDescending(email => email.ReceivedDateTime).GroupBy(email => email.ReceivedDateTime).FirstOrDefault();

                                //today's all emails
                                //var today = DateTime.UtcNow.Date; // Get today's date in UTC
                                //var Emails = inboxEmails.Value
                                //    .Where(email => email.ReceivedDateTime.HasValue && email.ReceivedDateTime.Value.Date == today) // Ensure the value is not null
                                //    .OrderByDescending(email => email.ReceivedDateTime); // Order by ReceivedDateTime descending

                                string connectionString2 = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";

                                using (SqlConnection conn = new SqlConnection(connectionString2))
                                {
                                    conn.Open();
                                    string query2 = @"SELECT top 1 e_time from tbl_eventlog order by e_time desc";
                                    using (SqlCommand cmd = new SqlCommand(query2, conn))
                                    {
                                        using (SqlDataReader reader = cmd.ExecuteReader())
                                        {
                                            // Loop through each row in the result
                                            while (reader.Read())
                                            {
                                                time = reader["e_time"].ToString();
                                            }
                                        }
                                    }

                                }

                                DateTime startDateTime = DateTime.ParseExact(time, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                                //DateTime currentDateTime = DateTime.UtcNow.AddHours(-5).AddMinutes(-30);// Get the current UTC time


                                // Get the current UTC time
                                DateTime currentUtcTime = DateTime.UtcNow;

                                // Convert the UTC time to UTC+05:30 (Indian Standard Time)
                                TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                DateTime currentDateTimeInIst = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, istTimeZone1);

                                // Format the converted time to the desired format: "yyyy-MM-dd HH:mm:ss.fff"
                                string formattedDateTime = currentDateTimeInIst.ToString("yyyy-MM-dd HH:mm:ss.fff");

                                // Convert the formatted string back to DateTime
                                DateTime currentDateTime = DateTime.ParseExact(formattedDateTime, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                                //DateTime adjustedDateTime = startDateTime.AddHours(-5).AddMinutes(-30);
                                DateTime adjustedDateTime = startDateTime;


                                //var Emails = inboxEmails.Value
                                //    .Where(email => email.ReceivedDateTime.HasValue &&
                                //                    email.ReceivedDateTime.Value >= adjustedDateTime &&
                                //                    email.ReceivedDateTime.Value <= currentDateTime) // Ensure email ReceivedDateTime is within the range
                                //    .OrderByDescending(email => email.ReceivedDateTime);


                                var Emails = inboxEmails.Value;

                                if (Emails.Any())
                                {
                                    foreach (var email in Emails)
                                    {
                                        try
                                        {

                                            DateTime receivedDateTimeUtc1 = email.ReceivedDateTime.Value;

                                            // Convert the UTC time to UTC+05:30 (Indian Standard Time)
                                           // TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"); // UTC+05:30

                                            DateTime receivedDateTimeInIst1 = TimeZoneInfo.ConvertTimeFromUtc(receivedDateTimeUtc1, istTimeZone1);
                                            email.ReceivedDateTime = receivedDateTimeInIst1;
                                            // Process unread Inbox email
                                            InboxEmail(email, userId);

                                            // Mark email as read after processing
                                            //await MarkEmailAsRead(httpClient, userId, email.Id);
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

                                using (SqlConnection conn = new SqlConnection(connectionString2))
                                {
                                    conn.Open();

                                    string query = @"INSERT INTO dbo.tbl_time (startdate, enddate, emaildate)
                         VALUES (@startdate, @enddate, @emaildate)";

                                    using (SqlCommand cmd1 = new SqlCommand(query, conn))
                                    {
                                        // Add parameters to the insert query
                                        cmd1.Parameters.AddWithValue("@startdate", startDateTime);
                                        cmd1.Parameters.AddWithValue("@enddate", currentDateTime);
                                        cmd1.Parameters.AddWithValue("@emaildate", "Success");


                                        // Execute the query to insert the email into the database
                                        cmd1.ExecuteNonQuery();
                                    }
                                    conn.Close();
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


                    //SENT
                    // Fetch and process emails from Sent Items
                    var sentResponse = await httpClient.GetAsync(sentUrl);
                    if (sentResponse.IsSuccessStatusCode)
                    {
                        var sentContent = await sentResponse.Content.ReadAsStringAsync();
                        var sentEmails = JsonConvert.DeserializeObject<GraphApiEmailResponse>(sentContent);

                        string time = "";

                        if (sentEmails?.Value?.Any() == true)
                        {
                            // Filter for unread emails
                            //var unreadEmails = sentEmails.Value.Where(email => email.IsRead == false).ToList();
                            //var unreadEmails = sentEmails.Value.ToList(); // Simply take all emails


                            //latest email
                            //var Emails = sentEmails.Value.OrderByDescending(email => email.ReceivedDateTime).GroupBy(email => email.ReceivedDateTime).FirstOrDefault();

                            //today's all emails
                            //var today = DateTime.UtcNow.Date; // Get today's date in UTC
                            //var Emails = sentEmails.Value
                            //    .Where(email => email.ReceivedDateTime.HasValue && email.ReceivedDateTime.Value.Date == today) // Ensure the value is not null
                            //    .OrderByDescending(email => email.ReceivedDateTime); // Order by ReceivedDateTime descending

                            string connectionString2 = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";

                            using (SqlConnection conn = new SqlConnection(connectionString2))
                            {
                                conn.Open();
                                string query2 = @"SELECT top 1 e_time from tbl_eventlog order by e_time desc";
                                using (SqlCommand cmd = new SqlCommand(query2, conn))
                                {
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        // Loop through each row in the result
                                        while (reader.Read())
                                        {
                                            time = reader["e_time"].ToString();
                                        }
                                    }
                                }

                            }

                            DateTime startDateTime = DateTime.ParseExact(time, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                            //DateTime startDateTime = DateTime.ParseExact(time, "MM/dd/yy h:mm:ss tt", CultureInfo.InvariantCulture);

                            //DateTime currentDateTime = DateTime.UtcNow.AddHours(-5).AddMinutes(-30);// Get the current UTC time


                            // Get the current UTC time
                            DateTime currentUtcTime = DateTime.UtcNow;

                            // Convert the UTC time to UTC+05:30 (Indian Standard Time)
                            TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            DateTime currentDateTimeInIst = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, istTimeZone1);

                            // Format the converted time to the desired format: "yyyy-MM-dd HH:mm:ss.fff"
                            string formattedDateTime = currentDateTimeInIst.ToString("yyyy-MM-dd HH:mm:ss.fff");

                            // Convert the formatted string back to DateTime
                            DateTime currentDateTime = DateTime.ParseExact(formattedDateTime, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                            //DateTime adjustedDateTime = startDateTime.AddHours(-5).AddMinutes(-30);
                            DateTime adjustedDateTime = startDateTime;

                            var Emails = sentEmails.Value;

                            if (Emails.Any())
                            {
                                foreach (var email in Emails)
                                {
                                    try
                                    {
                                        DateTime receivedDateTimeUtc1 = email.ReceivedDateTime.Value;

                                        // Convert the UTC time to UTC+05:30 (Indian Standard Time)
                                        //TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"); // UTC+05:30
                                        DateTime receivedDateTimeInIst1 = TimeZoneInfo.ConvertTimeFromUtc(receivedDateTimeUtc1, istTimeZone1);
                                        email.ReceivedDateTime = receivedDateTimeInIst1;

                                        if (receivedDateTimeInIst1 >= adjustedDateTime && receivedDateTimeInIst1 <= currentDateTime)
                                        {
                                            SentEmail(email, userId);
                                        }



                                        // Mark email as read after processing
                                        // await MarkEmailAsRead(httpClient, userId, email.Id);

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error processing email with ID {email.Id}: {ex.Message}");
                                    }


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
                       // var sentErrorDetails = await sentResponse.Content.ReadAsStringAsync();
                       // Console.WriteLine($"Sent error details: {sentErrorDetails}");
                    }
                }
                string connectionString1 = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";

                using (SqlConnection conn = new SqlConnection(connectionString1))
                {
                    conn.Open();

                    string query = @"INSERT INTO dbo.tbl_eventlog (e_actualtime,e_time, e_source, e_status)
                         VALUES (@e_actualtime,@e_time, @e_source, @e_status)";



                    DateTime startUtcTime = DateTime.UtcNow;

                    TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime startDateTimeInIst = TimeZoneInfo.ConvertTimeFromUtc(startUtcTime, istTimeZone1);

                    // Format the converted time to the desired format: "yyyy-MM-dd HH:mm:ss.fff"
                    string formattedDateTime = startDateTimeInIst.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    // Convert the formatted string back to DateTime
                    DateTime startDateTime = DateTime.ParseExact(formattedDateTime, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);






                    using (SqlCommand cmd1 = new SqlCommand(query, conn))
                    {
                        // Add parameters to the insert query
                        cmd1.Parameters.AddWithValue("@e_actualtime", System.DateTime.Now);
                        //  cmd1.Parameters.AddWithValue("@e_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        cmd1.Parameters.AddWithValue("@e_time", startDateTime);
                        cmd1.Parameters.AddWithValue("@e_source", "Log");
                        cmd1.Parameters.AddWithValue("@e_status", "Success");


                        // Execute the query to insert the email into the database
                        cmd1.ExecuteNonQuery();
                    }
                    conn.Close();
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
        private void InsertInboxEmailToDatabase(string subject, string from, string to, string body, string inReplyTo, string messageId, DateTime receivedDate, string attachmentPath, string emailType)
        {
            //string connectionString = "Server=P3NWPLSK12SQL-v13.shr.prod.phx3.secureserver.net;Database=CityMarineMgmt;User Id=CityMarineMgmt;Password=bZl34u0^6;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
            //string connectionString = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
            string connectionString = ConfigurationManager.AppSettings["constring"];

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


                string query2 = @"SELECT COUNT(*) FROM [dbo].[tbl_customermaster] WHERE SUBSTRING(c_email, CHARINDEX('@', c_email) + 1, LEN(c_email)) = @Email";
                string domain = from.Substring(from.IndexOf('@') + 1);

                using (SqlCommand cmd2 = new SqlCommand(query2, conn))
                {
                    cmd2.Parameters.AddWithValue("@Email", domain);

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
                string emailType = "General";

                // Process attachments if available
                var attachmentSaver = new AttachmentSaver();

                using (HttpClient httpClient = new HttpClient())
                {
                    // Save attachments
                    string attachmentPath = await attachmentSaver.SaveAttachments1(email, userId, httpClient);

                    // Insert email into the database along with attachments
                    InsertSentEmailToDatabase(subject, from, to, body, inReplyTo, messageId, sentDate, attachmentPath, emailType);

                    // Log success
                    WriteToFile($"Sent email processed successfully: {subject} with attachments.");
                }

            }
            catch (Exception ex)
            {
                // Log the error with details
                // string emailId = email?.Id ?? "Unknown";
                WriteToFile($"Error processing sent email (ID: {email}): {ex.Message}");
            }
        }


        private void InsertSentEmailToDatabase(string subject, string from, string to, string body, string inReplyTo, string messageId, DateTime sendDate, string attachmentPath, string sType)
        {
            //string connectionString = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
            string connectionString = ConfigurationManager.AppSettings["constring"];

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Query to fetch email rules
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

                            if (E_parameterName == "Subject")
                            {
                                if (E_conditionName == "Contains")
                                {
                                    //if (subject.Contains(E_value))
                                    if (subject.IndexOf(E_value, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        sType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Begin With")
                                {
                                    if (subject.StartsWith(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        sType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Equal To")
                                {
                                    if (subject.Equals(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        sType = E_categoryName;
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
                                        sType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Begin With")
                                {
                                    if (subject.StartsWith(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        sType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Equal To")
                                {
                                    if (subject.Equals(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        sType = E_categoryName;
                                    }
                                }
                            }
                        }
                    }
                }


                //string query2 = @"SELECT COUNT(*) FROM [dbo].[tbl_customermaster] WHERE SUBSTRING(c_email, CHARINDEX('@', c_email) + 1, LEN(c_email)) = @Email";

                //// string domain = from.Substring(from.IndexOf('@') + 1);
                //using (SqlCommand cmd2 = new SqlCommand(query2, conn))
                //{
                //    cmd2.Parameters.AddWithValue("@Email", from);

                //    // Execute the query and get the count
                //    int count = (int)cmd2.ExecuteScalar();

                //    if (count == 0)
                //    {
                //        sType = "General";
                //    }
                //}

                string query2 = @"SELECT COUNT(*) FROM [dbo].[tbl_customermaster] WHERE SUBSTRING(c_email, CHARINDEX('@', c_email) + 1, LEN(c_email)) = @Email";
                string domain = to.Substring(to.IndexOf('@') + 1);

                using (SqlCommand cmd2 = new SqlCommand(query2, conn))
                {
                    cmd2.Parameters.AddWithValue("@Email", domain);

                    // Execute the query and get the count
                    int count = (int)cmd2.ExecuteScalar();

                    if (count == 0)
                    {
                        sType = "General";
                    }
                }


                // Insert email into SentEmail table
                string insertQuery = @"INSERT INTO dbo.tbl_SentEmail (s_subject,s_from,s_to,s_body,s_replyto,s_messageid,s_sentdate,s_attachment,s_type) 
            VALUES (@s_subject,@s_from,@s_to,@s_body,@s_replyto,@s_messageid,@s_sentdate,@s_attachment,@s_type)";

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

        //public class AttachmentSaver
        //{

        //    //Inbox
        //    public async Task<string> SaveAttachments(GraphApiEmailResponse.GraphApiMessage email, string userId, HttpClient httpClient)
        //    {
        //        List<string> filePaths = new List<string>();
        //        string attachmentpath = "CityMarine_EmailCRM/wwwroot/Email_Attachment";

        //        // Ensure email has attachments
        //        if (email.Attachments != null && email.Attachments.Count > 0)
        //        {
        //            foreach (var attachment in email.Attachments)
        //            {
        //                string senderEmail = email.From?.EmailAddress?.Address ?? "UnknownSender";
        //                string attachmentsFolder = Path.Combine(attachmentpath, senderEmail);

        //                // Ensure the folder exists
        //                if (!Directory.Exists(attachmentsFolder))
        //                {
        //                    Directory.CreateDirectory(attachmentsFolder);
        //                }

        //                string fileName = SanitizeFileName(attachment.Name);
        //                string filePath = Path.Combine(attachmentsFolder, fileName);

        //                // If the attachment is of type 'GraphApiAttachment'
        //                if (attachment is GraphApiAttachment fileAttachment)
        //                {
        //                    try
        //                    {
        //                        // Check if the attachment has a contentUrl or contentBytes
        //                        if (!string.IsNullOrEmpty(fileAttachment.ContentUrl))
        //                        {
        //                            // Download the attachment using its contentUrl
        //                            await DownloadAttachmentFromUrl(fileAttachment.ContentUrl, filePath, httpClient);
        //                        }
        //                        else if (!string.IsNullOrEmpty(fileAttachment.ContentBytes))
        //                        {
        //                            // Decode and save attachment from Base64 contentBytes
        //                            byte[] content = Convert.FromBase64String(fileAttachment.ContentBytes);
        //                            System.IO.File.WriteAllBytes(filePath, content);
        //                        }

        //                        filePaths.Add(filePath);
        //                        Console.WriteLine($"Attachment saved: {filePath}");
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine($"Error downloading attachment {fileAttachment.Name}: {ex.Message}");
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine($"Attachment {attachment.Name} is not a file attachment.");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No attachments found.");
        //            return "No attachments available.";
        //        }

        //        return string.Join(Environment.NewLine, filePaths);
        //    }


        //    private async Task DownloadAttachmentFromUrl(string contentUrl, string filePath, HttpClient httpClient)
        //    {
        //        var response = await httpClient.GetAsync(contentUrl);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsByteArrayAsync();
        //            System.IO.File.WriteAllBytes(filePath, content);
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Failed to download attachment from URL: {contentUrl}");
        //        }
        //    }

        //    private string SanitizeFileName(string fileName)
        //    {

        //        return string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
        //    }
        //    //Send
        //    public async Task<string> SaveAttachments1(GraphApiEmailResponse.GraphApiMessage email, string userId, HttpClient httpClient)
        //    {
        //        List<string> filePaths = new List<string>();
        //        string attachmentpath = "CityMarine_EmailCRM/wwwroot/Email_Attachment";

        //        // Ensure email has attachments
        //        if (email.Attachments != null && email.Attachments.Any())
        //        {
        //            foreach (var attachment in email.Attachments)
        //            {
        //                string senderEmail = email.From?.EmailAddress?.Address ?? "UnknownSender";
        //                string attachmentsFolder = Path.Combine(attachmentpath, senderEmail);

        //                // Ensure the folder exists
        //                if (!Directory.Exists(attachmentsFolder))
        //                {
        //                    Directory.CreateDirectory(attachmentsFolder);
        //                }

        //                string fileName = SanitizeFileName(attachment.Name);
        //                string filePath = Path.Combine(attachmentsFolder, fileName);

        //                try
        //                {
        //                    // Check if the attachment is of type 'GraphApiAttachment'
        //                    if (attachment is GraphApiEmailResponse.GraphApiAttachment graphAttachment)
        //                    {
        //                        // If the attachment has contentBytes (Base64 encoded)
        //                        if (!string.IsNullOrEmpty(graphAttachment.ContentBytes))
        //                        {
        //                            byte[] content = Convert.FromBase64String(graphAttachment.ContentBytes);
        //                            System.IO.File.WriteAllBytes(filePath, content);
        //                        }
        //                        // If the attachment has a contentUrl, download the file
        //                        else if (!string.IsNullOrEmpty(graphAttachment.ContentUrl))
        //                        {
        //                            await DownloadAttachmentFromUrl1(graphAttachment.ContentUrl, filePath, httpClient);
        //                        }

        //                        filePaths.Add(filePath);
        //                        Console.WriteLine($"Attachment saved: {filePath}");
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine($"Attachment {attachment.Name} is not a file attachment.");
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"Error processing attachment {attachment.Name}: {ex.Message}");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No attachments found.");
        //            return "No attachments available.";
        //        }

        //        return string.Join(Environment.NewLine, filePaths);
        //    }

        //    public async Task DownloadAttachmentFromUrl1(string contentUrl, string filePath, HttpClient httpClient)
        //    {
        //        try
        //        {
        //            // Get the attachment content from the URL
        //            var response = await httpClient.GetAsync(contentUrl);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var content = await response.Content.ReadAsByteArrayAsync();
        //                System.IO.File.WriteAllBytes(filePath, content);
        //                Console.WriteLine($"Attachment downloaded: {filePath}");
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Failed to download attachment from {contentUrl}");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error downloading attachment: {ex.Message}");
        //        }
        //    }
        //}

        public class AttachmentSaver
        {

            public async Task<string> SaveAttachments(GraphApiEmailResponse.GraphApiMessage email, string userId, HttpClient httpClient)
            {
                List<string> filePaths = new List<string>();
                List<Task> downloadTasks = new List<Task>();
                string attachmentpath = "CityMarine_EmailCRM/wwwroot/Email_Attachment";
                //string attachmentpath = ConfigurationManager.AppSettings["attachmentpath"];

                if (email.Attachments != null && email.Attachments.Count > 0)
                {
                    foreach (var attachment in email.Attachments)
                    {
                        string senderEmail = email.From?.EmailAddress?.Address ?? "UnknownSender";
                        string attachmentsFolder = Path.Combine(attachmentpath, senderEmail);

                        if (!Directory.Exists(attachmentsFolder))
                        {
                            Directory.CreateDirectory(attachmentsFolder);
                        }

                        string fileName = SanitizeFileName(attachment.Name);
                        string filePath = Path.Combine(attachmentsFolder, fileName);

                        if (attachment is GraphApiAttachment fileAttachment)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(fileAttachment.ContentUrl))
                                {
                                    downloadTasks.Add(DownloadAttachmentFromUrl(fileAttachment.ContentUrl, filePath, httpClient));
                                }
                                else if (!string.IsNullOrEmpty(fileAttachment.ContentBytes))
                                {
                                    byte[] content = Convert.FromBase64String(fileAttachment.ContentBytes);
                                    await Task.Run(() => System.IO.File.WriteAllBytes(filePath, content));
                                    Console.WriteLine($"Attachment saved: {filePath}");
                                }

                                filePaths.Add(filePath);
                                Console.WriteLine($"Attachment queued for saving: {filePath}");
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

                    // Wait for all downloads and file writes to complete
                    await Task.WhenAll(downloadTasks);
                }
                else
                {
                    Console.WriteLine("No attachments found.");
                    return "No attachments available.";
                }

                return string.Join(",", filePaths);
            }

            private async Task DownloadAttachmentFromUrl(string contentUrl, string filePath, HttpClient httpClient)
            {
                var response = await httpClient.GetAsync(contentUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();

                    // Write file asynchronously using Task.Run
                    await Task.Run(() => System.IO.File.WriteAllBytes(filePath, content)); // ✅ Fix applied
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
                //string attachmentpath = ConfigurationManager.AppSettings["attachmentpath"];
                string attachmentpath = "CityMarine_EmailCRM/wwwroot/Email_Attachment";

                // Ensure email has attachments
                if (email.Attachments != null && email.Attachments.Any())
                {
                    foreach (var attachment in email.Attachments)
                    {
                        string senderEmail = email.From?.EmailAddress?.Address ?? "UnknownSender";
                        string attachmentsFolder = Path.Combine(attachmentpath, senderEmail);

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
                                    System.IO.File.WriteAllBytes(filePath, content);
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
                        System.IO.File.WriteAllBytes(filePath, content);
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

    }
}
