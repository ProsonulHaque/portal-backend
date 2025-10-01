using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.BusinessLogic;
using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Models;
using Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling.Web;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Web;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Enums;
using Org.Eclipse.TractusX.Portal.Backend.Web.Identity;
using System.ComponentModel.DataAnnotations;

namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Controllers;

[ApiController]
[EnvironmentRoute("MVC_ROUTING_BASEPATH", "branding/assets")]
public class CompanyBrandingController(ICompanyBrandingBusinessLogic businessLogic) : ControllerBase
{
    [HttpGet]
    [Route("logo", Name = nameof(GetCompanyBrandingLogoAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> GetCompanyBrandingLogoAsync([FromQuery][Required(ErrorMessage = "Provide company id")] Guid companyId)
    {
        var companyBrandingLogo = await businessLogic.GetCompanyBrandingFileAsync(companyId, CompanyBrandingAssetTypeId.LOGO).ConfigureAwait(ConfigureAwaitOptions.None);

        return File
        (
            companyBrandingLogo.FileContent,
            companyBrandingLogo.FileMediaType,
            companyBrandingLogo.FileName
        );
    }

    [HttpGet]
    [Route("footer", Name = nameof(GetCompanyBrandingFooterAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> GetCompanyBrandingFooterAsync([FromQuery][Required(ErrorMessage = "Provide company id")] Guid companyId)
    {
        var companyBrandingFooter = await businessLogic.GetCompanyBrandingTextAsync(companyId, CompanyBrandingAssetTypeId.FOOTER).ConfigureAwait(ConfigureAwaitOptions.None);

        return Ok(new { CompanyBrandingFooter = companyBrandingFooter });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> GetCompanyBrandingAssetsAsync([FromQuery][Required(ErrorMessage = "Provide company id")] Guid companyId)
    {
        var companyBrandingFooter = await businessLogic.GetCompanyBrandingTextAsync(companyId, CompanyBrandingAssetTypeId.FOOTER).ConfigureAwait(ConfigureAwaitOptions.None);

        var companyBrandingAssetsResponse = new CompanyBrandingAssetsResponse
        (
            companyId,
            CompanyBrandingLogoUrl: Url.Link(nameof(GetCompanyBrandingLogoAsync), new { companyId })!,
            companyBrandingFooter
        );

        return Ok(companyBrandingAssetsResponse);
    }

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
        var logoId = await businessLogic.SaveCompanyBrandingLogoAsync(companyBrandingLogoData, CancellationToken.None).ConfigureAwait(ConfigureAwaitOptions.None);

        return CreatedAtRoute(nameof(GetCompanyBrandingLogoAsync), new { companyBrandingLogoData.CompanyId }, logoId);
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
        var footerId = await businessLogic.SaveCompanyBrandingFooterAsync(companyBrandingFooterData).ConfigureAwait(ConfigureAwaitOptions.None);

        return CreatedAtRoute(nameof(GetCompanyBrandingFooterAsync), new { companyBrandingFooterData.CompanyId }, footerId);
    }

    [HttpPut]
    [Route("logo/{companyId}")]
    [Authorize(Roles = "manage_branding_assets")]
    [Authorize(Policy = PolicyTypes.ValidCompany)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCompanyBrandingLogoAsync([FromRoute] Guid companyId, [FromForm] CompanyBrandingLogoUpdateData companyBrandingLogoUpdateData)
    {
        await businessLogic.UpdateCompanyBrandingLogoAsync(companyId, companyBrandingLogoUpdateData, CancellationToken.None).ConfigureAwait(ConfigureAwaitOptions.None);

        return NoContent();
    }

    [HttpPut]
    [Route("footer/{companyId}")]
    [Authorize(Roles = "manage_branding_assets")]
    [Authorize(Policy = PolicyTypes.ValidCompany)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCompanyBrandingFooterAsync([FromRoute] Guid companyId, [FromForm] CompanyBrandingFooterUpdateData companyBrandingFooterUpdateData)
    {
        await businessLogic.UpdateCompanyBrandingFooterAsync(companyId, companyBrandingFooterUpdateData).ConfigureAwait(ConfigureAwaitOptions.None);

        return NoContent();
    }

    [HttpDelete]
    [Route("logo/{companyId}")]
    [Authorize(Roles = "manage_branding_assets")]
    [Authorize(Policy = PolicyTypes.ValidCompany)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompanyBrandingLogoAsync([FromRoute] Guid companyId)
    {
        await businessLogic.DeleteCompanyBrandingLogoAsync(companyId).ConfigureAwait(ConfigureAwaitOptions.None);

        return NoContent();
    }

    [HttpDelete]
    [Route("footer/{companyId}")]
    [Authorize(Roles = "manage_branding_assets")]
    [Authorize(Policy = PolicyTypes.ValidCompany)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompanyBrandingFooterAsync([FromRoute] Guid companyId)
    {
        await businessLogic.DeleteCompanyBrandingFooterAsync(companyId).ConfigureAwait(ConfigureAwaitOptions.None);

        return NoContent();
    }
}
