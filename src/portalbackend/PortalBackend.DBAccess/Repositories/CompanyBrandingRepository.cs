using Microsoft.EntityFrameworkCore;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Entities;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Enums;

namespace Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Repositories;

public class CompanyBrandingRepository(PortalDbContext portalDbContext) : ICompanyBrandingRepository
{
    public CompanyBrandingFile CreateCompanyBrandingFile(
        string fileName,
        CompanyBrandingAssetTypeId companyBrandingAssetTypeId,
        MediaTypeId mediaTypeId,
        long fileSizeInKiloBytes,
        byte[] fileContent,
        Guid companyId)
    {
        var companyBrandingFile = new CompanyBrandingFile(
            id: Guid.NewGuid(),
            fileName: fileName,
            companyBrandingAssetTypeId: companyBrandingAssetTypeId,
            mediaTypeId: mediaTypeId,
            fileSizeInKiloBytes: fileSizeInKiloBytes,
            fileContent: fileContent,
            companyId: companyId,
            dateCreated: DateTimeOffset.UtcNow);

        return portalDbContext.CompanyBrandingFiles.Add(companyBrandingFile).Entity;
    }

    public async Task<bool> DoesCompanyBrandingFileExistAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId) =>
        await portalDbContext.CompanyBrandingFiles.AnyAsync(x => x.CompanyId == companyId && x.CompanyBrandingAssetTypeId == assetTypeId);

    public CompanyBrandingText CreateCompanyBrandingText(string footer, CompanyBrandingAssetTypeId companyBrandingAssetTypeId, Guid companyId)
    {
        var companyBrandingText = new CompanyBrandingText(
            id: Guid.NewGuid(),
            brandingText: footer,
            companyBrandingAssetTypeId: companyBrandingAssetTypeId,
            companyId: companyId,
            dateCreated: DateTimeOffset.UtcNow);

        return portalDbContext.CompanyBrandingTexts.Add(companyBrandingText).Entity;
    }

    public async Task<bool> DoesCompanyBrandingTextExistAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId) =>
        await portalDbContext.CompanyBrandingTexts.AnyAsync(x => x.CompanyId == companyId && x.CompanyBrandingAssetTypeId == assetTypeId);
}
