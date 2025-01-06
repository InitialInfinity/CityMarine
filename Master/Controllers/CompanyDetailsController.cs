using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Net.Http;
using Tokens;
using Master.API.Entity;
using Master.API.Repository.Interface;
using static System.Net.WebRequestMethods;

namespace Master.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDetailsController : ControllerBase
    {
    //    private readonly ICompanyDetailsRepository _CRepo;
    //    public CompanyDetailsController(ICompanyDetailsRepository CRepo)
    //    {
    //        _CRepo = CRepo;

    //    }

    //    [HttpPost("UpdateCompany")]
    //    public async Task<IActionResult> UpdateCompany([FromBody] CompanyDetails user)
    //    {
    //        try
    //        {
    //            if (user.BaseModel == null)
    //            {
    //                user.BaseModel = new BaseModel();
    //            }
    //            user.BaseModel.OperationType = "Update";
    //            user.BaseModel.Server_Value = user.Server_Value;
    //            var parameter = await _CRepo.CompanyDetails(user);
    //            return parameter;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }

    //    [HttpPost("UpdateLogo")]
    //    public async Task<IActionResult> UpdateLogo(IFormFile? com_company_logo, IFormFile? com_company_logo2)
    //    {
    //        CompanyDetails user = new CompanyDetails();
    //        if (user.BaseModel == null)
    //        {
    //            user.BaseModel = new BaseModel();
    //        }
    //        user.BaseModel.OperationType = "UpdateLogo";
    //        try
    //        {
    //            string? com_id = HttpContext.Request.Form["com_id"];
    //            string? UserId = HttpContext.Request.Form["UserId"];
    //            string? Server_Value = HttpContext.Request.Form["Server_Value"];
    //            string? type = HttpContext.Request.Form["type"];
    //            string? com_contact = HttpContext.Request.Form["com_contact"];
    //            string? com_updateddateString = HttpContext.Request.Form["com_updateddate"];
    //            string? com_createddateString = HttpContext.Request.Form["com_createddate"];

    //            DateTime com_updateddate;
    //            DateTime com_createddate;
    //            if (com_company_logo != null)
    //            {
    //                if (com_company_logo.Length > 0)
    //                {
    //                    string[] AllowedFileExtensions = new string[] { ".gif", ".jpg", ".jpeg", ".bmp", ".png", ".PNG", ".JPG", ".GIF", ".JPEG", ".BMP" };

    //                    if (!AllowedFileExtensions.Contains(com_company_logo.FileName.Substring(com_company_logo.FileName.LastIndexOf('.'))))
    //                    {
    //                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
    //                    }
    //                    else
    //                    {
    //                        string[] Filecontent = com_company_logo.FileName.Split('.');
    //                        string ImageName = Filecontent[0];
    //                        string ImageExt = Filecontent[1];
    //                        using (var ms = new MemoryStream())
    //                        {
    //                            MemoryStream memoryStream = new MemoryStream();
    //                            com_company_logo.OpenReadStream().CopyTo(ms);
    //                            var fileBytes = ms.ToArray();
    //                            user.com_company_logoF = fileBytes;
    //                        }

    //                    }

    //                }
    //            }

    //            if (com_company_logo2 != null)
    //            {
    //                if (com_company_logo2.Length > 0)
    //                {
    //                    string[] AllowedFileExtensions = new string[] { ".gif", ".jpg", ".jpeg", ".bmp", ".png", ".PNG", ".JPG", ".GIF", ".JPEG", ".BMP" };

    //                    if (!AllowedFileExtensions.Contains(com_company_logo2.FileName.Substring(com_company_logo2.FileName.LastIndexOf('.'))))
    //                    {
    //                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
    //                    }
    //                    else
    //                    {
    //                        string[] Filecontent = com_company_logo2.FileName.Split('.');
    //                        string ImageName = Filecontent[0];
    //                        string ImageExt = Filecontent[1];
    //                        using (var ms = new MemoryStream())
    //                        {
    //                            MemoryStream memoryStream = new MemoryStream();
    //                            com_company_logo2.OpenReadStream().CopyTo(ms);
    //                            var fileBytes = ms.ToArray();
    //                            user.com_company_logo2F = fileBytes;
    //                        }

    //                    }

    //                }
    //            }

    //            if (!string.IsNullOrEmpty(com_id) && Guid.TryParse(com_id, out Guid IdGuid))
    //            {
    //                user.com_id = IdGuid;
    //            }
    //            if (!string.IsNullOrEmpty(UserId) && Guid.TryParse(UserId, out Guid IdGuid1))
    //            {
    //                user.UserId = IdGuid1;
    //            }
    //            user.BaseModel.Server_Value = Server_Value;
    //            user.type = type;
    //            user.com_contact = com_contact;
    //            if (DateTime.TryParse(com_updateddateString, out com_updateddate))
    //            {
    //                user.com_updateddate = com_updateddate;
    //            }
    //            if (DateTime.TryParse(com_createddateString, out com_createddate))
    //            {
    //                user.com_createddate = com_createddate;
    //            }
    //            var parameter = await _CRepo.CompanyDetails(user);
    //            return parameter;
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //    [HttpGet("Get")]
    //    public async Task<IActionResult> GetCompanyById(Guid? UserId, string Server_Value)
    //    {
    //        CompanyDetails user = new CompanyDetails();
    //        if (user.BaseModel == null)
    //        {
    //            user.BaseModel = new BaseModel();
    //        }
    //        user.UserId = UserId;
    //        user.com_id = UserId;
    //        user.BaseModel.OperationType = "Get";
    //        user.BaseModel.Server_Value = Server_Value;
    //        try
    //        {
    //            dynamic parameter = await _CRepo.CompanyDetails(user);
    //            var modeldata = parameter.Value.Data;
    //            if (modeldata.com_company_logo != null)
    //            {
    //                modeldata.ImageBase64 = Convert.ToBase64String(modeldata.com_company_logo);
    //                modeldata.com_company_logo = null;
    //            }
    //            if (modeldata.com_company_logo2 != null)
    //            {
    //                modeldata.ImageBase642 = Convert.ToBase64String(modeldata.com_company_logo2);
    //                modeldata.com_company_logo2 = null;
    //            }


    //            return parameter;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }
    //    [HttpGet("GetCompany")]
    //    public async Task<IActionResult> GetCompany(Guid? UserId,Guid? com_id, string Server_Value)
    //    {
    //        CompanyDetails user = new CompanyDetails();
    //        if (user.BaseModel == null)
    //        {
    //            user.BaseModel = new BaseModel();
    //        }
    //        user.UserId = UserId;
    //        user.com_id = com_id;
    //        user.BaseModel.OperationType = "GetCompany";
    //        user.BaseModel.Server_Value = Server_Value;
    //        try
    //        {
    //            dynamic parameter = await _CRepo.CompanyDetails(user);
    //            var modeldata = parameter.Value.Data;
    //            if (modeldata.com_company_logo != null)
    //            {
    //                modeldata.ImageBase64 = Convert.ToBase64String(modeldata.com_company_logo);
    //                modeldata.com_company_logo = null;
    //            }
               

    //            return parameter;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }
    //    [HttpPost("DeleteCompany")]
    //    public async Task<IActionResult> DeleteCompany([FromBody] CompanyDetails user)
    //    {
    //        if (user.BaseModel == null)
    //        {
    //            user.BaseModel = new BaseModel();
    //        }

    //        user.BaseModel.OperationType = "DeleteCompanyDetails";
    //        user.BaseModel.Server_Value = user.Server_Value;

    //        try
    //        {
    //            var parameter = await _CRepo.CompanyDetails(user);
    //            return parameter;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }
    }
}
