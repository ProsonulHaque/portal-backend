using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.ErrorHandling;
using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Models;
using Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Identity;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Web;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Extensions;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Models;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Repositories;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Enums;

namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.BusinessLogic;

public class CompanyBrandingBusinessLogic(IPortalRepositories portalRepositories, IIdentityService identityService) : ICompanyBrandingBusinessLogic
{
    private readonly IIdentityData _identityData = identityService.IdentityData;
    private const long OneMegabyteInBytes = 1024 * 1024;

    public async Task<(Guid logoId, Guid companyId)> SaveCompanyBrandingLogoAsync(CompanyBrandingLogoData companyBrandingLogoData, CancellationToken cancellationToken)
    {
        var companyHasOperatorRole = await portalRepositories.GetInstance<ICompanyRolesRepository>().DoesCompanyHaveSpecificRoleAsync(companyId: _identityData.CompanyId, companyRoleId: CompanyRoleId.OPERATOR).ConfigureAwait(ConfigureAwaitOptions.None);

        if (!companyHasOperatorRole)
        {
            throw ForbiddenException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_FORBIDDEN_USER_NOT_ALLOW_CREATE_ASSET);
        }

        var companyLogoExists = await portalRepositories.GetInstance<ICompanyBrandingRepository>().DoesCompanyBrandingFileExistAsync(_identityData.CompanyId, CompanyBrandingAssetTypeId.LOGO).ConfigureAwait(ConfigureAwaitOptions.None);

        if (companyLogoExists)
        {
            throw ConflictException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_CONFLICT_ASSET_EXISTS);
        }

        var fileSizeInBytes = companyBrandingLogoData.CompanyLogoFile.Length;

        if (fileSizeInBytes > OneMegabyteInBytes)
        {
            throw new BadHttpRequestException(message: "File size exceeds 1 MB size limit");
        }

        var mediaTypeId = companyBrandingLogoData.CompanyLogoFile.ContentType.ParseMediaTypeId();

        mediaTypeId.CheckFileContentType(validMediaTypes: [MediaTypeId.SVG, MediaTypeId.PNG, MediaTypeId.JPEG]);

        var (fileContent, _) = await companyBrandingLogoData.CompanyLogoFile.GetContentAndHash(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

        var companyBrandingFile = portalRepositories.GetInstance<ICompanyBrandingRepository>().CreateCompanyBrandingFile(
            fileName: companyBrandingLogoData.CompanyLogoFile.FileName,
            companyBrandingAssetTypeId: CompanyBrandingAssetTypeId.LOGO,
            mediaTypeId: mediaTypeId,
            fileSizeInKiloBytes: fileSizeInBytes / 1024,
            fileContent: fileContent,
            companyId: _identityData.CompanyId);

        await portalRepositories.SaveAsync().ConfigureAwait(ConfigureAwaitOptions.None);

        return (companyBrandingFile.Id, _identityData.CompanyId);
    }

    public async Task<(Guid footerId, Guid companyId)> SaveCompanyBrandingFooterAsync(CompanyBrandingFooterData companyBrandingFooterData)
    {
        var companyHasOperatorRole = await portalRepositories.GetInstance<ICompanyRolesRepository>().DoesCompanyHaveSpecificRoleAsync(companyId: _identityData.CompanyId, companyRoleId: CompanyRoleId.OPERATOR).ConfigureAwait(ConfigureAwaitOptions.None);

        if (!companyHasOperatorRole)
        {
            throw ForbiddenException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_FORBIDDEN_USER_NOT_ALLOW_CREATE_ASSET);
        }

        var companyFooterExists = await portalRepositories.GetInstance<ICompanyBrandingRepository>().DoesCompanyBrandingTextExistAsync(_identityData.CompanyId, CompanyBrandingAssetTypeId.FOOTER).ConfigureAwait(ConfigureAwaitOptions.None);

        if (companyFooterExists)
        {
            throw ConflictException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_CONFLICT_ASSET_EXISTS);
        }

        var companyBrandingText = portalRepositories.GetInstance<ICompanyBrandingRepository>().CreateCompanyBrandingText(
            footer: companyBrandingFooterData.Footer,
            companyBrandingAssetTypeId: CompanyBrandingAssetTypeId.FOOTER,
            companyId: _identityData.CompanyId);

        await portalRepositories.SaveAsync().ConfigureAwait(ConfigureAwaitOptions.None);

        return (companyBrandingText.Id, _identityData.CompanyId);
    }

    public async Task<FileData> GetCompanyBrandingFileAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId)
    {
        var companyExists = await portalRepositories.GetInstance<ICompanyRepository>().IsExistingCompany(companyId).ConfigureAwait(ConfigureAwaitOptions.None);

        if (!companyExists)
        {
            throw ControllerArgumentException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_ARGUMENT_COMPANY_ID_NOT_VALID, [new ErrorParameter(nameof(companyId), companyId.ToString())]);
        }

        var companyBrandingFile = await portalRepositories.GetInstance<ICompanyBrandingRepository>().GetCompanyBrandingFileAsync(companyId, assetTypeId).ConfigureAwait(ConfigureAwaitOptions.None);

        if (companyBrandingFile.IsDefault())
        {
            throw ConflictException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_CONFLICT_ASSET_NOT_EXIST);
        }

        return companyBrandingFile;
    }

    public async Task<string> GetCompanyBrandingTextAsync(Guid companyId, CompanyBrandingAssetTypeId assetTypeId)
    {
        var companyExists = await portalRepositories.GetInstance<ICompanyRepository>().IsExistingCompany(companyId).ConfigureAwait(ConfigureAwaitOptions.None);

        if (!companyExists)
        {
            throw ControllerArgumentException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_ARGUMENT_COMPANY_ID_NOT_VALID, [new ErrorParameter(nameof(companyId), companyId.ToString())]);
        }

        var companyBrandingText = await portalRepositories.GetInstance<ICompanyBrandingRepository>().GetCompanyBrandingTextAsync(companyId, assetTypeId).ConfigureAwait(ConfigureAwaitOptions.None);

        if (companyBrandingText == default)
        {
            throw ConflictException.Create(AdministrationCompanyBrandingErrors.COMPANY_BRANDING_CONFLICT_ASSET_NOT_EXIST);
        }

        return companyBrandingText;
    }
}
