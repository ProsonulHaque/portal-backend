using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.ErrorHandling;
using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Models;
using Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Identity;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Web;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Extensions;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Repositories;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Enums;

namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.BusinessLogic;

public class CompanyBrandingBusinessLogic(IPortalRepositories portalRepositories, IIdentityService identityService) : ICompanyBrandingBusinessLogic
{
    private readonly IIdentityData _identityData = identityService.IdentityData;
    private const long OneMegabyteInBytes = 1024 * 1024;

    public async Task<Guid> SaveCompanyBrandingLogoAsync(CompanyBrandingLogoData companyBrandingLogoData, CancellationToken cancellationToken)
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

        return companyBrandingFile.Id;
    }

    public async Task<Guid> SaveCompanyBrandingFooterAsync(CompanyBrandingFooterData companyBrandingFooterData, CancellationToken cancellationToken)
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

        var companyBrandingFooter = portalRepositories.GetInstance<ICompanyBrandingRepository>().CreateCompanyBrandingText(
            footer: companyBrandingFooterData.Footer,
            companyBrandingAssetTypeId: CompanyBrandingAssetTypeId.FOOTER,
            companyId: _identityData.CompanyId);

        await portalRepositories.SaveAsync().ConfigureAwait(ConfigureAwaitOptions.None);

        return companyBrandingFooter.Id;
    }
}
