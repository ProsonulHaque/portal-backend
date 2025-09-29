using Org.Eclipse.TractusX.Portal.Backend.Framework.DBAccess;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Auditing;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Enums;

namespace Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Entities;

public class CompanyBrandingText : IBaseEntity
{
    private CompanyBrandingText()
    {
        BrandingText = null!;
    }

    public CompanyBrandingText(Guid id, string brandingText, CompanyBrandingAssetTypeId companyBrandingAssetTypeId, Guid companyId, DateTimeOffset dateCreated) : this()
    {
        Id = id;
        BrandingText = brandingText;
        CompanyBrandingAssetTypeId = companyBrandingAssetTypeId;
        CompanyId = companyId;
        DateCreated = dateCreated;
    }

    public Guid Id { get; private set; }

    public string BrandingText { get; set; }

    public CompanyBrandingAssetTypeId CompanyBrandingAssetTypeId { get; private set; }

    public Guid CompanyId { get; private set; }

    public DateTimeOffset DateCreated { get; private set; }

    [LastChangedV1]
    public DateTimeOffset? DateLastChanged { get; set; }

    [LastEditorV1]
    public Guid? LastEditorId { get; private set; }

    // Navigation properties
    public virtual CompanyBrandingAssetType? CompanyBrandingAssetType { get; set; }
    public virtual Company? Company { get; set; }
}
