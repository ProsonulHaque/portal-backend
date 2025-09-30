using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.BusinessLogic;
using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Models;
using Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling.Web;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Web;
using Org.Eclipse.TractusX.Portal.Backend.Web.Identity;

namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Controllers;

[ApiController]
[EnvironmentRoute("MVC_ROUTING_BASEPATH", "branding/assets")]
public class CompanyBrandingController(ICompanyBrandingBusinessLogic businessLogic) : ControllerBase
{
    [HttpPost]
    [Route("logo")]
    [Authorize(Roles = "manage_branding_assets")]
    [Authorize(Policy = PolicyTypes.ValidCompany)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status415UnsupportedMediaType)]
    public async Task<IActionResult> SaveCompanyBrandingLogoAsync([FromForm] CompanyBrandingLogoData companyBrandingLogoData)
    {
        var logoFileId = await businessLogic.SaveCompanyBrandingLogoAsync(companyBrandingLogoData, CancellationToken.None).ConfigureAwait(ConfigureAwaitOptions.None);

        return Created();
        //return CreatedAtRoute(nameof(), new { logoFileId = logoFileId });
    }

    [HttpPost]
    [Route("footer")]
    [Authorize(Roles = "manage_branding_assets")]
    [Authorize(Policy = PolicyTypes.ValidCompany)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SaveCompanyBrandingFooterAsync([FromBody] CompanyBrandingFooterData companyBrandingFooterData)
    {
        var footerId = await businessLogic.SaveCompanyBrandingFooterAsync(companyBrandingFooterData, CancellationToken.None).ConfigureAwait(ConfigureAwaitOptions.None);

        return Created();
        //return CreatedAtRoute(nameof(), new { footerId = footerId });
    }
}
