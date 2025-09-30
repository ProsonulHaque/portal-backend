using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Models;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Entities;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Enums;

namespace Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Repositories;

public interface ICompanyBrandingRepository
{
    CompanyBrandingFile CreateCompanyBrandingFile(
        string fileName,
        CompanyBrandingAssetTypeId companyBrandingAssetTypeId,
        MediaTypeId mediaTypeId,
        long fileSizeInKiloBytes,
        byte[] fileContent,
        Guid companyId);

    Task<bool> DoesCompanyBrandingFileExistAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId);

    CompanyBrandingText CreateCompanyBrandingText(
        string footer,
        CompanyBrandingAssetTypeId companyBrandingAssetTypeId,
        Guid companyId);

    Task<bool> DoesCompanyBrandingTextExistAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId);

    Task<FileData> GetCompanyBrandingFileAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId);

    Task<string?> GetCompanyBrandingTextAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId);
}
