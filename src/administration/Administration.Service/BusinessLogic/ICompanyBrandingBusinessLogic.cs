using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Models;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Models;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Enums;

namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.BusinessLogic;

public interface ICompanyBrandingBusinessLogic
{
    Task<Guid> SaveCompanyBrandingLogoAsync(CompanyBrandingLogoData companyBrandingLogoData, CancellationToken cancellationToken);
    Task<Guid> SaveCompanyBrandingFooterAsync(CompanyBrandingFooterData companyBrandingFooterData);
    Task<FileData> GetCompanyBrandingFileAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId);
    Task<string> GetCompanyBrandingTextAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId);
    Task UpdateCompanyBrandingLogoAsync(Guid companyId, CompanyBrandingLogoUpdateData companyBrandingLogoUpdateData, CancellationToken cancellationToken);
    Task UpdateCompanyBrandingFooterAsync(Guid companyId, CompanyBrandingFooterUpdateData companyBrandingFooterUpdateData);
    Task DeleteCompanyBrandingLogoAsync(Guid companyId);
}
