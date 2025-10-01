using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.ErrorHandling;
using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Models;
using Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Identity;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Web;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Extensions;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Models;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Repositories;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Entities;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Enums;

namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.BusinessLogic;

public class CompanyBrandingBusinessLogic(IPortalRepositories portalRepositories, IIdentityService identityService) : ICompanyBrandingBusinessLogic
{
    private readonly IIdentityData _identityData = identityService.IdentityData;
    private const long OneMegabyteInBytes = 1024 * 1024;

    public async Task<Guid> SaveCompanyBrandingLogoAsync(CompanyBrandingLogoData companyBrandingLogoData, CancellationToken cancellationToken)
    {
        await CheckCompanyValidityAsync(companyBrandingLogoData.CompanyId);
        await CheckIfUserCompanyHasOperatorRoleAsync();
        await CheckIfCompanyBrandingLogoAlreadyExistsAsync(companyBrandingLogoData.CompanyId);

        var fileSizeInBytes = companyBrandingLogoData.CompanyLogoFile.Length;
        ValidateFileSizeLimit(fileSizeInBytes);

        var mediaTypeId = companyBrandingLogoData.CompanyLogoFile.ContentType.ParseMediaTypeId();
        ValidateMediaType(mediaTypeId);

        var (fileContent, _) = await companyBrandingLogoData.CompanyLogoFile.GetContentAndHash(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

        var companyBrandingFile = portalRepositories.GetInstance<ICompanyBrandingRepository>().CreateCompanyBrandingFile(
            fileName: companyBrandingLogoData.CompanyLogoFile.FileName,
            companyBrandingAssetTypeId: CompanyBrandingAssetTypeId.LOGO,
            mediaTypeId: mediaTypeId,
            fileSizeInKiloBytes: fileSizeInBytes / 1024,
            fileContent: fileContent,
            companyId: companyBrandingLogoData.CompanyId);

        await portalRepositories.SaveAsync().ConfigureAwait(ConfigureAwaitOptions.None);

        return companyBrandingFile.Id;
    }

    private static void ValidateMediaType(MediaTypeId mediaTypeId)
    {
        mediaTypeId.CheckFileContentType(validMediaTypes: [MediaTypeId.SVG, MediaTypeId.PNG, MediaTypeId.JPEG]);
    }

    private static void ValidateFileSizeLimit(long fileSizeInBytes)
    {
        if (fileSizeInBytes > OneMegabyteInBytes)
        {
            throw new BadHttpRequestException(message: "File size exceeds 1 MB size limit");
        }
    }

    private async Task CheckIfCompanyBrandingLogoAlreadyExistsAsync(Guid companyId)
    {
        var companyLogoExists = await portalRepositories.GetInstance<ICompanyBrandingRepository>().DoesCompanyBrandingFileExistAsync(companyId, CompanyBrandingAssetTypeId.LOGO).ConfigureAwait(ConfigureAwaitOptions.None);

        if (companyLogoExists)
        {
            throw ConflictException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_CONFLICT_ASSET_ALREADY_EXISTS);
        }
    }

    private async Task CheckIfUserCompanyHasOperatorRoleAsync()
    {
        var userCompanyHasOperatorRole = await portalRepositories.GetInstance<ICompanyRolesRepository>().DoesCompanyHaveSpecificRoleAsync(_identityData.CompanyId, companyRoleId: CompanyRoleId.OPERATOR).ConfigureAwait(ConfigureAwaitOptions.None);

        if (!userCompanyHasOperatorRole)
        {
            throw ForbiddenException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_FORBIDDEN_USER_NOT_ALLOW_CREATE_ASSET);
        }
    }

    public async Task<Guid> SaveCompanyBrandingFooterAsync(CompanyBrandingFooterData companyBrandingFooterData)
    {
        await CheckCompanyValidityAsync(companyBrandingFooterData.CompanyId);
        await CheckIfUserCompanyHasOperatorRoleAsync();
        await CheckIfCompanyBrandingFooterAlreadyExistsAsync(companyBrandingFooterData.CompanyId);

        var companyBrandingText = portalRepositories.GetInstance<ICompanyBrandingRepository>().CreateCompanyBrandingText(
            footer: companyBrandingFooterData.Footer,
            companyBrandingAssetTypeId: CompanyBrandingAssetTypeId.FOOTER,
            companyId: companyBrandingFooterData.CompanyId);

        await portalRepositories.SaveAsync().ConfigureAwait(ConfigureAwaitOptions.None);

        return companyBrandingText.Id;
    }

    private async Task CheckIfCompanyBrandingFooterAlreadyExistsAsync(Guid companyId)
    {
        var companyFooterExists = await portalRepositories.GetInstance<ICompanyBrandingRepository>().DoesCompanyBrandingTextExistAsync(companyId, CompanyBrandingAssetTypeId.FOOTER).ConfigureAwait(ConfigureAwaitOptions.None);

        if (companyFooterExists)
        {
            throw ConflictException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_CONFLICT_ASSET_ALREADY_EXISTS);
        }
    }

    public async Task<FileData> GetCompanyBrandingFileAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId)
    {
        var companyBrandingFile = await portalRepositories.GetInstance<ICompanyBrandingRepository>().GetCompanyBrandingFileAsync(companyId, assetTypeId).ConfigureAwait(ConfigureAwaitOptions.None);

        if (companyBrandingFile.IsDefault())
        {
            throw NotFoundException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_ASSET_NOT_FOUND);
        }

        return companyBrandingFile;
    }

    private async Task CheckCompanyValidityAsync(Guid companyId)
    {
        var companyExists = await portalRepositories.GetInstance<ICompanyRepository>().IsExistingCompany(companyId).ConfigureAwait(ConfigureAwaitOptions.None);

        if (!companyExists)
        {
            throw ControllerArgumentException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_ARGUMENT_COMPANY_ID_NOT_VALID, [new ErrorParameter(nameof(companyId), companyId.ToString())]);
        }
    }

    public async Task<string> GetCompanyBrandingTextAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId)
    {
        var companyBrandingText = await portalRepositories.GetInstance<ICompanyBrandingRepository>().GetCompanyBrandingTextAsync(companyId, assetTypeId).ConfigureAwait(ConfigureAwaitOptions.None);

        if (companyBrandingText == default)
        {
            throw NotFoundException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_ASSET_NOT_FOUND);
        }

        return companyBrandingText;
    }

    public async Task UpdateCompanyBrandingLogoAsync(Guid companyId, CompanyBrandingLogoUpdateData companyBrandingLogoUpdateData, CancellationToken cancellationToken)
    {
        await CheckCompanyValidityAsync(companyId);
        await CheckIfUserCompanyHasOperatorRoleAsync();

        var fileSizeInBytes = companyBrandingLogoUpdateData.CompanyLogoFile.Length;
        ValidateFileSizeLimit(fileSizeInBytes);

        var mediaTypeId = companyBrandingLogoUpdateData.CompanyLogoFile.ContentType.ParseMediaTypeId();
        ValidateMediaType(mediaTypeId);

        var (fileContent, _) = await companyBrandingLogoUpdateData.CompanyLogoFile.GetContentAndHash(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

        var companyBrandingFileEntity = await portalRepositories.GetInstance<ICompanyBrandingRepository>().GetCompanyBrandingFileEntityAsync(companyId, CompanyBrandingAssetTypeId.LOGO);

        if (companyBrandingFileEntity == default)
        {
            throw NotFoundException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_ASSET_NOT_FOUND);
        }

        UpdateCompanyBrandingFileEntity(companyBrandingLogoUpdateData.CompanyLogoFile.FileName, fileSizeInBytes, mediaTypeId, fileContent, companyBrandingFileEntity);

        await portalRepositories.SaveAsync().ConfigureAwait(ConfigureAwaitOptions.None);
    }

    private static void UpdateCompanyBrandingFileEntity(
        string fileName,
        long fileSizeInBytes,
        MediaTypeId mediaTypeId,
        byte[] fileContent,
        CompanyBrandingFile companyBrandingFileEntity)
    {
        companyBrandingFileEntity.FileContent = fileContent;
        companyBrandingFileEntity.FileName = fileName;
        companyBrandingFileEntity.FileSizeInKiloByte = fileSizeInBytes / 1024;
        companyBrandingFileEntity.MediaTypeId = mediaTypeId;
    }

    public async Task UpdateCompanyBrandingFooterAsync(Guid companyId, CompanyBrandingFooterUpdateData companyBrandingFooterUpdateData, CancellationToken cancellationToken)
    {
        await CheckCompanyValidityAsync(companyId);
        await CheckIfUserCompanyHasOperatorRoleAsync();

        var companyBrandingTextEntity = await portalRepositories.GetInstance<ICompanyBrandingRepository>().GetCompanyBrandingTextEntityAsync(companyId, CompanyBrandingAssetTypeId.LOGO);

        if (companyBrandingTextEntity == default)
        {
            throw NotFoundException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_ASSET_NOT_FOUND);
        }

        UpdateCompanyBrandingTextEntity(companyBrandingFooterUpdateData.Footer, companyBrandingTextEntity);

        await portalRepositories.SaveAsync().ConfigureAwait(ConfigureAwaitOptions.None);
    }

    private static void UpdateCompanyBrandingTextEntity(string brandingText, CompanyBrandingText companyBrandingTextEntity) =>
        companyBrandingTextEntity.BrandingText = brandingText;
}
