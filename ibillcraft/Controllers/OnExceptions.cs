using Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tokens;
using common.Token;
using Microsoft.AspNetCore.Mvc.Filters;
using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;
using IExceptionFilter = Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter;
using Microsoft.Extensions.DependencyInjection;
using common;
using Common;

namespace ibillcraft.Controllers
{
	public class OnExceptions : Attribute,  IExceptionFilter	{
		private TokenRepo? _loginRepo;
		private DapperContext? _context;

		private IConfiguration? con;
		public void OnException(ExceptionContext context)
		{
			var httpContext = context.HttpContext;
			var controllerName = (string)context.RouteData.Values["controller"];
			var actionName = (string)context.RouteData.Values["action"];
			var model = new HandleErrorInfo(context.Exception, controllerName, actionName);
			con = httpContext.RequestServices.GetService<IConfiguration>();// Retrieve IConfiguration from the HttpContext

			_context = new DapperContext(con);


			//var apiPath = context.ActionDescriptor.AttributeRouteInfo.Template;
			var apiPath = $"api/{controllerName}";//{actionName}";

			//EX
			ExHandle ExHan = new ExHandle();
			ExHan.Message = model.Exception.Message;
			//ExHan.Message = httpContext.Message;
			//ExHan.Message = ex.Message;
			ExHan.Source = model.Exception.Source;
			ExHan.InnerException = model.Exception.InnerException;
			ExHan.API = apiPath;
			if (ExHan.BaseModel == null)
			{
				ExHan.BaseModel = new BaseModel();
			}

			ExHan.BaseModel.OperationType = "InsertException";
			 ExceptionHandle exceptionHandle = new ExceptionHandle(_context);
			//  var abc =  exceptionHandle.ExeptionHandle(ExHan);
			//return abc;




			//context.Result = new ObjectResult(new
			//{
			//	Message = model.Exception.Message,
			//	api = apiPath,
			//	InnerException = model.Exception.InnerException,
			//	Source = model.Exception.Source
			//	// Details = errorDetails.ToString()
			//})
			//{
			//	StatusCode = StatusCodes.Status500InternalServerError
			//};
			context.Result = new RedirectToActionResult("ErrorIndex", "Home", null);
		}

	}
}
