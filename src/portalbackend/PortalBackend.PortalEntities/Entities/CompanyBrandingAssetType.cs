using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Entities;

public class CompanyBrandingAssetType
{
    private CompanyBrandingAssetType()
    {
        Label = null!;
        CompanyBrandingFiles = new HashSet<CompanyBrandingFile>();
        CompanyBrandingTexts = new HashSet<CompanyBrandingText>();
    }

    public CompanyBrandingAssetType(CompanyBrandingAssetTypeId companyBrandingAssetTypeId) : this()
    {
        Id = companyBrandingAssetTypeId;
        Label = companyBrandingAssetTypeId.ToString();
    }

    public CompanyBrandingAssetTypeId Id { get; private set; }

    [MaxLength(255)]
    public string Label { get; private set; }

    // Navigation properties
    public virtual ICollection<CompanyBrandingFile> CompanyBrandingFiles { get; private set; }
    public virtual ICollection<CompanyBrandingText> CompanyBrandingTexts { get; private set; }
}
