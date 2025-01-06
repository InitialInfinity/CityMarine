using Master.API.Entity;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;


namespace Master.Repository.Interface
{
	public interface ICompanyDetailsRepository
	{
		public Task<IActionResult> CompanyDetails(CompanyDetails user);
		
		public Task<IActionResult> SubDetails(Subscription user);
		public Task<IActionResult> PayDetails(PaymentDetails user);
	}
}
