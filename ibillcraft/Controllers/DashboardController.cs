using Common;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using ibillcraft.Models;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using iTextSharp.text;

namespace ibillcraft.Controllers
{
	[OnExceptions]
	public class DashboardController : Controller
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<DashboardController> _logger;
		private readonly IStringLocalizer<DashboardController> _localizer;

		public DashboardController(ILogger<DashboardController> logger, IStringLocalizer<DashboardController> localizer, IConfiguration configuration)
		{
			var handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
			_httpClient = new HttpClient(handler);
			_httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);

			_logger = logger;
			_localizer = localizer;
		}

		public IActionResult Index()
		{
			try
			{
				var homeDataList = new List<Dashboard>();
				var subdatalist = new SubscriptionModel();
				//string RoleId = "D88D23D9-2BD2-4D68-A1F9-FBE4168B8DB1";
				GetCookies gk = new GetCookies();
				CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
				ViewBag.Format = CUtility.format;
				List<Dashboard> homeList = new List<Dashboard>();
				
				HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/GetWebMenu/GetAll?RoleId={CUtility.RoleId}&server={CUtility.serverValue}").Result;
				

				if (response.IsSuccessStatusCode)
				{
					dynamic data = response.Content.ReadAsStringAsync().Result;
					var dataObject = new { data = new List<Dashboard>() };
					var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
					homeDataList = response2.data;
					

					


					if (homeDataList != null)
					{
						HttpContext.Session.SetString("HomeDataList", JsonConvert.SerializeObject(homeDataList));
						ViewBag.menu = homeDataList;
						var localizedTitle = _localizer[""];
						//var viewModel = new Tuple<Dashboard, IEnumerable<SaleOrderModel>>(orderModel, SaleModelList);
						var viewModel = new DashboardC
						{
							//data = orderModel,
							//list6 = SaleModelList,
							//list1 = ModelList1,
							//list2 = ModelList2,
							//list3 = ModelList3,
							//list4 = ModelList4

						};
						return View(viewModel);
					}
					else
					{
						var localizedTitle = _localizer[""];
						var viewModel = new DashboardC
						{
							//data = orderModel,
							//list6 = SaleModelList,
							//list1 = ModelList1,
							//list2 = ModelList2,
							//list3 = ModelList3,
							//list4 = ModelList4

						};
						return View(viewModel);
					}
				}
				var viewModel2 = new DashboardC
				{
					//data = orderModel,
					//list6 = SaleModelList,
					//list1 = ModelList1,
					//list2 = ModelList2,
					//list3 = ModelList3,
					//list4 = ModelList4

				};
				return View(viewModel2);
			}
			catch (Exception)
			{
				throw;
			}
		}

		
	}
}
