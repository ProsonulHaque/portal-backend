namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Models;

public record CompanyBrandingAssetsResponse
(
    Guid CompanyId,
    string CompanyBrandingLogoUrl,
    string CompanyBrandingFooter
);
