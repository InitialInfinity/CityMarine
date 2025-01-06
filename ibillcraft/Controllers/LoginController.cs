using ibillcraft.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
//using NuGet.Protocol.Core.Types;
using UAParser;
using System.Net.Sockets;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace ibillcraft.Controllers
{
    public class LoginController : Controller
    {
        Uri baseuri = new Uri("https://localhost:44355/api");
        private readonly HttpClient _httpClient;
        public LoginController()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = baseuri;
        }
        public IActionResult Index()
        {
            try
            {
                string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    ipAddress = endPoint.Address.ToString();
                }
                string userAgentString = Request.Headers["User-Agent"].ToString();

                var uaParser = Parser.GetDefault();
                ClientInfo clientInfo = uaParser.Parse(userAgentString);

                string browserName = clientInfo.UserAgent.Family;
                string browserVersion = clientInfo.UserAgent.Major + "." + clientInfo.UserAgent.Minor;
                string url = $"{_httpClient.BaseAddress}/LoginDetails/Get?ip_address={ipAddress}&browserName={browserName}&browserVersion={browserVersion}";

                var Logindetails = new LoginDetailsModel();
                //Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new LoginDetailsModel() };
                    var responselogin = JsonConvert.DeserializeObject<LoginDetailsModel>(data);
                    if (responselogin != null)
                    {
                        if (responselogin.com_password == null)
                        {
                            return View(responselogin);
                        }
                        else
                        {
                            string comid = responselogin.com_id;
                            string serverValue = responselogin.server_Value;
                            string loginId = responselogin.LoginId;
                            string Baseaddress = responselogin._httpClient.BaseAddress;
                            HttpContext.Session.SetString("com_id", comid);
                            HttpContext.Session.SetString("Server_Value", serverValue);
                            HttpContext.Session.SetString("loginId", loginId);
                            HttpContext.Session.SetString("BaseAddress", Baseaddress);
                            TempData["successMessage"] = "Login successfully";
                            return RedirectToAction("Index", "Home", new { com_id = comid, Server_Key = serverValue });
                        }
                    }
                    else
                    {
                        return View();
                    }
                }

                return View();
            }
            catch (Exception)
            {
                return View();
            }
            //return View();
        }
        [HttpPost]
        public IActionResult Create([FromForm] LoginDetailsModel model)
        {

            try
            {
                string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    ipAddress = endPoint.Address.ToString();
                }
                string userAgentString = Request.Headers["User-Agent"].ToString();

                var uaParser = Parser.GetDefault();
                ClientInfo clientInfo = uaParser.Parse(userAgentString);

                string browserName = clientInfo.UserAgent.Family;
                string browserVersion = clientInfo.UserAgent.Major + "." + clientInfo.UserAgent.Minor;
                model.ip_address = ipAddress;
                model.browser_version = browserVersion;
                model.browser_name = browserName;
                model.CreatedDate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/LoginDetails", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.result.outcome;
                    string outcomeDetail = responsemodel.outcomeDetail;
                    dynamic comid = rootObject.result.data;
                    string Server_Key = comid.Server_Key;
                    string com_id = comid.com_id;
                    string loginId = comid.LoginId;
                    HttpContext.Session.SetString("com_id", com_id);
                    HttpContext.Session.SetString("Server_Value", Server_Key);
                    HttpContext.Session.SetString("loginId", loginId);
                    if (outcomeDetail == "Please enter valid credentials")
                    {
                        return Ok(outcomeDetail);
                    }
                    else
                    {
                        var result = new { Server_Key, com_id };
                        TempData["successMessage"] = outcomeDetail; //.outcome.outcomeid;
                        return Ok(result);
                    }
               }
                TempData["errorMessage"] = response.Headers.ToString();
                return Ok(response.Headers.ToString());
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpGet("Delete")]
        public IActionResult Delete()
        {
            try
            {
                LoginDetailsModel model = new LoginDetailsModel();
                string comid = HttpContext.Session.GetString("com_id");
                string serverValue = HttpContext.Session.GetString("Server_Value");
                string loginId = HttpContext.Session.GetString("loginId");
                string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    ipAddress = endPoint.Address.ToString();
                }
                string userAgentString = Request.Headers["User-Agent"].ToString();
                var uaParser = Parser.GetDefault();
                ClientInfo clientInfo = uaParser.Parse(userAgentString);
                string browserName = clientInfo.UserAgent.Family;
                string browserVersion = clientInfo.UserAgent.Major + "." + clientInfo.UserAgent.Minor;
                model.ip_address = ipAddress;
                model.browser_version = browserVersion;
                model.browser_name = browserName;
                model.CreatedDate = DateTime.Now;
                model.com_id = comid;
                model.LoginId = loginId;
                model.server_Value = serverValue;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/LoginDetails/LogOut", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responcedata = response.Content.ReadAsStringAsync().Result;
                    if(responcedata != null)
                    {
                        //dynamic rootObject = JsonConvert.DeserializeObject<object>(responcedata);
                        //dynamic responcemodel = rootObject.outcome;
                        //string outcomeDetail = responcemodel.outcomeDetail;
                        TempData["successMessage"] = responcedata;
                        return RedirectToAction("Index");
                    }
                    
                    
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }

        }
    }
}
