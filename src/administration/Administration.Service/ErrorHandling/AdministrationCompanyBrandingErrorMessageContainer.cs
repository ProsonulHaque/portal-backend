using Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling.Service;
using System.Collections.Immutable;

namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.ErrorHandling;

public class AdministrationCompanyBrandingErrorMessageContainer : IErrorMessageContainer
{
    private static readonly IReadOnlyDictionary<int, string> _messageContainer = ImmutableDictionary.CreateRange<int, string>([
        new((int)AdministrationCompanyBrandingErrors.COMPANY_BRANDING_FORBIDDEN_USER_NOT_ALLOW_CREATE_ASSET, "User is not allowed to create branding asset"),
        new((int)AdministrationCompanyBrandingErrors.COMPANY_BRANDING_FILE_SIZE_EXCEEDS_LIMIT, "File size exceeds {fileSize} size limit"),
        new((int)AdministrationCompanyBrandingErrors.COMPANY_BRANDING_CONFLICT_ASSET_ALREADY_EXISTS, "Asset already exists"),
        new((int)AdministrationCompanyBrandingErrors.COMPANY_BRANDING_ARGUMENT_COMPANY_ID_NOT_VALID, "Invalid company id: {companyId}"),
        new((int)AdministrationCompanyBrandingErrors.COMPANY_BRANDING_ASSET_NOT_FOUND, "Company branding asset does not exist")
    ]);

    public Type Type { get => typeof(AdministrationCompanyBrandingErrors); }

    public IReadOnlyDictionary<int, string> MessageContainer { get => _messageContainer; }
}

public enum AdministrationCompanyBrandingErrors
{
    COMPANY_BRANDING_FORBIDDEN_USER_NOT_ALLOW_CREATE_ASSET,
    COMPANY_BRANDING_FILE_SIZE_EXCEEDS_LIMIT,
    COMPANY_BRANDING_CONFLICT_ASSET_ALREADY_EXISTS,
    COMPANY_BRANDING_ARGUMENT_COMPANY_ID_NOT_VALID,
    COMPANY_BRANDING_ASSET_NOT_FOUND
}
