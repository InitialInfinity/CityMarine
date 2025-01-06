using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Dapper;
//using Abp.Domain.Entities;

using System.Net;
using System.Web.Http;
using System.Data;
using Microsoft.Extensions.ObjectPool;
//using System.Web.Http.Results;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
//using System.Linq.Dynamic.Core.Tokenizer;

using Newtonsoft.Json.Linq;

using Microsoft.Extensions.Configuration;

//using System.Web.Http.Filters;
using IActionFilter = Microsoft.AspNetCore.Mvc.Filters.IActionFilter;
using Context;
using common.Token;
using Tokens;

using ActionExecutedContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext;
using ActionExecutingContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;
using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;
using System.Diagnostics.Metrics;
using Common;
using IExceptionFilter = Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter;
using System.Diagnostics;
using System.Web.Mvc;
//using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using static common.ExampleFilterAttribute;
using System.Net.Http;
using Newtonsoft.Json;
using ibillcraft.Models;
using Microsoft.Extensions.Localization;
using DocumentFormat.OpenXml.Spreadsheet;
using common;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.InkML;
//using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace ibillcraft.Controllers
{
	public class ExampleFilter1Attribute : Attribute, IActionFilter
    {
		private DapperContext? _context;

		public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public HttpClient _httpClient;//= new HttpClient();


        public void OnActionExecuting(ActionExecutingContext context)
        {
   //         try
   //         {
   //             var handler = new HttpClientHandler();
   //             handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
   //             _httpClient = new HttpClient(handler);
   //             var httpContext = context.HttpContext;
   //             string server = httpContext.Session.GetString("Server_Value");
   //             string comid = httpContext.Session.GetString("com_id");
   //             var controllerName = (string)context.RouteData.Values["controller"];
   //             string roleid = httpContext.Session.GetString("RoleId");

   //             string baseaddress = httpContext.Session.GetString("BaseAddressUser");
   //             var Home1 = new Home1();

   //             string home = $"{baseaddress}/Home/GetAll?UserId={CUtility.comid}&Server_Value={server}&controller={controllerName}&role={roleid}";
   //             string nurl = $"{baseaddress}/InvoiceNote/Get?UserId={CUtility.comid}&Server_Value={server}&In_com_id={CUtility.comid}&invoicename={controllerName}";
   //             string nurl1 = $"{baseaddress}/InvoiceNote/GetDate?UserId={CUtility.comid}&Server_Value={server}";
   //             HttpResponseMessage nresponse1 = _httpClient.GetAsync(nurl1).Result;
   //             HttpResponseMessage nresponse = _httpClient.GetAsync(nurl).Result;
   //             HttpResponseMessage response = _httpClient.GetAsync(home).Result;
   //             if (response.IsSuccessStatusCode)
   //             {
   //                 dynamic data = response.Content.ReadAsStringAsync().Result;
   //                 var dataObject = new { data = new Home1() };
   //                 var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
   //                 Home1 = response2.data;

   //                 dynamic ndata1 = nresponse1.Content.ReadAsStringAsync().Result;
   //                 var ndataObject1 = new { data = new InvoiceNote() };
   //                 var nresponses1 = JsonConvert.DeserializeAnonymousType(ndata1, ndataObject1);
   //                 var InvoiceNote1 = new InvoiceNote();
   //                 InvoiceNote1 = nresponses1.data;

   //                 string Dateformat = InvoiceNote1.df_format;
   //                 httpContext.Session.SetString("DateFormat", Dateformat);

   //                 // string addaccess = Home1.a_addaccess == "1" ? "block" : "none";
   //                 // string viewaccess = Home1.a_viewaccess == "1" ? "block" : "none";
   //                 // string deleteaccess = Home1.a_deleteaccess == "1" ? "block" : "none";
   //                 // string editaccess = Home1.a_editaccess == "1" ? "block" : "none";
   //                 // httpContext.Session.SetString("AddAccess", addaccess);
   //                 // httpContext.Session.SetString("ViewAccess", viewaccess);
   //                 // httpContext.Session.SetString("DeleteAccess", deleteaccess);
   //                 // httpContext.Session.SetString("EditAccess", editaccess);

   //                 dynamic ndata = nresponse.Content.ReadAsStringAsync().Result;
   //                 var ndataObject = new { data = new InvoiceNote() };
   //                 var nresponses = JsonConvert.DeserializeAnonymousType(ndata, ndataObject);

   //                 var InvoiceNote = new InvoiceNote();
   //                 InvoiceNote = nresponses.data;
   //                 //string sms = InvoiceNote.con_sms == "1" ? "block" : "none";
   //                 //string wp = InvoiceNote.con_wp == "1" ? "block" : "none";
   //                 //string email = InvoiceNote.con_email == "1" ? "block" : "none";
   //                 //httpContext.Session.SetString("wp", wp);
   //                 //httpContext.Session.SetString("email", email);
   //                 //httpContext.Session.SetString("sms", sms);

   //                 if (Home1 != null)
   //                 {
   //                     //return (Home1);
   //                 }
   //                 else
   //                 {
   //                     var Home12 = new List<Home1>();
   //                     // return View(Home12);
   //                 }
   //             }
			//}
			//catch (Exception ex)
   //         {
			//	var httpContext = context.HttpContext;
			//	var controllerName = (string)context.RouteData.Values["controller"];
			//	var actionName = (string)context.RouteData.Values["action"];
			//	var model = new HandleErrorInfo(ex, controllerName, actionName);
			//	var apiPath = $"api/{controllerName}";
			//	ExHandle ExHan = new ExHandle();
			//	ExHan.Message = ex.Message;
			//	ExHan.Source = ex.Source;
			//	ExHan.InnerException = ex.InnerException;
			//	ExHan.API = apiPath;
			//	if (ExHan.BaseModel == null)
			//	{
			//		ExHan.BaseModel = new BaseModel();
			//	}

			//	ExHan.BaseModel.OperationType = "InsertException";
			//	ExceptionHandle exceptionHandle = new ExceptionHandle(_context);
			//	//var abc = exceptionHandle.ExeptionHandle(ExHan);
			//	//context.Result = new RedirectToActionResult("ErrorIndex", "Home", null);

			//}
		}

    }
}
