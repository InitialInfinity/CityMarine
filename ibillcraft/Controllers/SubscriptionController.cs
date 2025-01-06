using ibillcraft.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Localization;
//using Razorpay.Api;
using System.Diagnostics.Metrics;
using DocumentFormat.OpenXml.EMMA;
namespace ibillcraft.Controllers
{
    public class SubscriptionController : Controller
    {
        Uri baseuri = new Uri("https://localhost:44355/api");
        private readonly HttpClient _httpClient;
        private readonly ILogger<SubscriptionController> _logger;
        private readonly IStringLocalizer<SubscriptionController> _localizer;

        //rushika
        [BindProperty]
        public SubscriptionModel _subscriptionmodel { get; set; }
        //rushika

        public SubscriptionController(ILogger<SubscriptionController> logger, IStringLocalizer<SubscriptionController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index(string? ComId, string? com_name, string? com_email, string country, string contact)
        {
            ViewBag.ComId = ComId;
            ViewBag.com_company_name = com_name;
            ViewBag.com_email = com_email;
            ViewBag.country = country;
            ViewBag.contact = contact;
            try
            {
                var subMList = new List<SubscriptionModel>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/RegisterCompany/getAllSub").Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<SubscriptionModel>() };
                    var dataresponse = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    subMList = dataresponse.data;
                    if (subMList != null)
                    {
                        return View(subMList);
                    }
                    else
                    {
                        var subListse = new List<SubscriptionModel>();
                        return View(subListse);
                    }
                }
                return View(subMList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        [HttpPost]
        //public IActionResult Create([FromForm] SubscriptionModel model)
        //public IActionResult Create(Guid com_id, string com_company_name, string com_email, string country, string package_name, decimal final_Amount, int Invoice, int Quotation, int Expence, int cash_order, string sm_service, int sub_duration)

        //  {
        //    //string sm_id, string package_name, string sm_service, string subAmount, string subDiscount, string final_Amount, string Invoice, string Quotation, string Expence, string cash_order, string sub_duration
        //    try
        //    {


        ////         sub_id, sm_id, UserId, com_id, com_company_name, subAmount, package_name, payment_method, sm_service, subDiscount, final_Amount, Invoice, Quotation, Expence 
        ////, cash_order, sub_duration, com_isactive, EndDate, StartDate 
        //SubscriptionModel model= new SubscriptionModel();





        //model.com_updateddate= DateTime.Now;
        //        model.com_createddate= DateTime.Now;
        //        string data = JsonConvert.SerializeObject(model);
        //        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/RegisterCompany/InsertSub", content).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            dynamic responsedata = response.Content.ReadAsStringAsync().Result;
        //            dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
        //            dynamic responsemodel = rootObject.outcome;
        //            string outcomeDetail = responsemodel.outcomeDetail;
        //            dynamic comid = rootObject.data;
        //            string sub_id = comid.sub_id;
        //            string com_id = comid.com_id;
        //            string com_name = comid.com_name;


        //            //rushika

        //            string key = "rzp_test_dwM1QoEpJEQIXn";
        //            string secret = "rbmUVRu0CzR20QpAI7UbVnWB";

        //            Random _random = new Random();
        //            string TansactionId = _random.Next(0, 10000).ToString();


        //            Dictionary<string, object> input = new Dictionary<string, object>();
        //            input.Add("amount", Convert.ToDecimal(_subscriptionmodel.final_Amount)); 
        //            input.Add("currency", "INR");
        //            input.Add("receipt", TansactionId);

        //            RazorpayClient client = new RazorpayClient(key, secret);

        //            Razorpay.Api.Order order = client.Order.Create(input);
        //            ViewBag.orderid = order["id"].ToString();


        //            //rushika



        //            if (outcomeDetail == "Subscription inserted successfully!")
        //            {
        //                TempData["successMessage"] = "Company registered successfully!";
        //            }
        //            else
        //            {
        //                TempData["successMessage"] = outcomeDetail;
        //            }

        //            return View("Payment", _subscriptionmodel);
        //            //var result = new { sub_id, com_id, com_name, outcomeDetail };
        //            //return Ok(result);
        //        }
        //        TempData["errorMessage"] = response.Headers.ToString();


        //        return View("Payment");


        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["errorMessage"] = ex.Message;
        //        return RedirectToAction("Index");
        //    }
        //}


        //public IActionResult Create(Guid com_id, string com_company_name, string com_email, string country, string package_name, string final_Amount, string Invoice, string Quotation, string Expence, string cash_order, string sm_service, string sub_duration)
        //{
        //    try

        //    {
        //        Guid ccom_id = _subscriptionmodel.com_id ?? Guid.Empty; // Use Guid.Empty if com_id is null

        //        // Create and populate the SubscriptionModel from the form data
        //        var subscriptionModel = new SubscriptionModel
        //        {
        //            com_id = com_id,
        //            com_company_name = com_company_name,
        //            package_name = package_name,
        //            final_Amount = final_Amount,
        //            Invoice = Invoice,
        //            Quotation = Quotation,
        //            Expence = Expence,
        //            cash_order = cash_order,
        //            sm_service = sm_service,
        //            sub_duration = sub_duration,
        //            com_updateddate = DateTime.Now,
        //            com_createddate = DateTime.Now
        //        };

        //        // Serialize and send the model to the external API for saving subscription
        //        string data = JsonConvert.SerializeObject(subscriptionModel);
        //        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/RegisterCompany/InsertSub", content).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Deserialize the response data from the API
        //            dynamic responseData = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
        //            dynamic outcome = responseData.outcome;
        //            string outcomeDetail = outcome.outcomeDetail;

        //            // Retrieve company information from the response
        //            dynamic comIdData = responseData.data;
        //            string sub_id = comIdData.sub_id;
        //            com_id = comIdData.com_id;
        //            string com_name = comIdData.com_name;

        //            // Razorpay integration for payment
        //            string key = "rzp_test_dwM1QoEpJEQIXn";
        //            string secret = "rbmUVRu0CzR20QpAI7UbVnWB";

        //            // Generate a random transaction ID
        //            Random random = new Random();
        //            string transactionId = random.Next(0, 10000).ToString();

        //            // Prepare data for Razorpay order
        //            Dictionary<string, object> input = new Dictionary<string, object>
        //    {
        //        { "amount", Convert.ToDecimal(subscriptionModel.final_Amount) }, // Razorpay expects amount in paise
        //        { "currency", "INR" },
        //        { "receipt", transactionId }
        //    };

        //            // Create the Razorpay order
        //            RazorpayClient client = new RazorpayClient(key, secret);
        //            Razorpay.Api.Order order = client.Order.Create(input);

        //            // Set the Razorpay order ID to ViewBag for use in the payment view
        //            ViewBag.orderid = order["id"].ToString();

        //            // Handle success message
        //            if (outcomeDetail == "Subscription inserted successfully!")
        //            {
        //                TempData["SuccessMessage"] = "Company registered successfully!";
        //            }
        //            else
        //            {
        //                TempData["SuccessMessage"] = outcomeDetail;
        //            }

        //            // Return the Payment view with the subscription model
        //            //return View("Payment", subscriptionModel);
        //            return View("Index");
        //        }

        //        // Handle failure in API response
        //        TempData["ErrorMessage"] = "Failed to register company. Please try again.";
        //        //return View("Payment");
        //        return View("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error (ex.Message) and display a generic error message
        //        TempData["ErrorMessage"] = "An error occurred while processing your request: " + ex.Message;
        //        return RedirectToAction("Index");
        //    }
        //}

        public IActionResult Payment(Guid com_id, string com_company_name, Guid sm_id, string package_name, string sm_service, string subAmount, string subDiscount, string final_Amount, string Invoice, string Quotation, string Expence, string cash_order, string sub_duration,string contact,string com_email)
        {
            //var com_id = $('#com_id').val();
            //var com_company_name = $('#com_company_name').val();
            //var email = $('#com_email').val();
            try
            {
                SubscriptionModel model = new SubscriptionModel();
                model.sm_id = sm_id;
                model.package_name = package_name;
                model.sm_service = sm_service;
                model.subAmount = subAmount;
                model.subDiscount = subDiscount;
                model.final_Amount = final_Amount;
                model.Invoice = Invoice;
                model.Quotation = Quotation;
                model.Expence = Expence;
                model.cash_order = cash_order;
                model.sub_duration = sub_duration;
                model.com_id = com_id;
                model.com_company_name = com_company_name;
                model.com_updateddate = DateTime.Now;
                model.com_createddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/RegisterCompany/InsertSub", content).Result;


                ServerAllocationModel model1 = new ServerAllocationModel();
                
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                Random random = new Random();
                int comCode = random.Next(100, 1000);

                model1.UserId = UserId;              
                model1.com_id = com_id;
                model1.com_company_name=com_company_name;
                model1.ser_createddate = DateTime.Now;
                model1.ser_updateddate = DateTime.Now;
                model1.com_email = com_email;
                model1.com_code = comCode.ToString();
                model1.com_contact = contact;
                model1.sub_id = sm_id.ToString();
                model1.Server_Key = "Server_1";
                model1.Server_Value = "Server=P3NWPLSK12SQL-v13.shr.prod.phx3.secureserver.net;Database=ibillcraft_server1;User Id=ibillcraft_server1;Password=ibillcraft_server1;Encrypt=False;";
                model1.Allotted_Date = DateTime.Now.ToString();
                string data1 = JsonConvert.SerializeObject(model1);
                StringContent content1 = new StringContent(data1, Encoding.UTF8, "application/json");
                HttpResponseMessage response1 = _httpClient.PostAsync(_httpClient.BaseAddress + "/ServerAllocation", content1).Result;




                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.outcome;
                    string outcomeDetail = responsemodel.outcomeDetail;
                    dynamic comid = rootObject.data;
                    string sub_id = comid.sub_id;
                    com_id = comid.com_id;
                    string com_name = comid.com_name;
                    return View("RegistrationSuccessfully");

                }
                TempData["errorMessage"] = response.Headers.ToString();
                return View("RegistrationSuccessfully");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
