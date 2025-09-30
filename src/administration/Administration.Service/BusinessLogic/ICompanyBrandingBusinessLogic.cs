using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Models;

namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.BusinessLogic;

public interface ICompanyBrandingBusinessLogic
{
    Task<Guid> SaveCompanyBrandingLogoAsync(CompanyBrandingLogoData companyBrandingLogoData, CancellationToken cancellationToken);
    Task<Guid> SaveCompanyBrandingFooterAsync(CompanyBrandingFooterData companyBrandingFooterData, CancellationToken cancellationToken);
}
