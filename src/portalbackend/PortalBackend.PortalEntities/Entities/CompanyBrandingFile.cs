using Org.Eclipse.TractusX.Portal.Backend.Framework.DBAccess;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Auditing;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Enums;

namespace Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Entities;

public class CompanyBrandingFile : IBaseEntity
{
    private CompanyBrandingFile()
    {
        FileName = null!;
        FileContent = null!;
    }

    public CompanyBrandingFile(Guid id, string fileName, CompanyBrandingAssetTypeId companyBrandingAssetTypeId, MediaTypeId mediaTypeId, long fileSizeInBytes, byte[] fileContent, Guid companyId, DateTimeOffset dateCreated) : this()
    {
        Id = id;
        FileName = fileName;
        CompanyBrandingAssetTypeId = companyBrandingAssetTypeId;
        MediaTypeId = mediaTypeId;
        FileSizeInBytes = fileSizeInBytes;
        FileContent = fileContent;
        CompanyId = companyId;
        DateCreated = dateCreated;
    }

    public Guid Id { get; private set; }

    public string FileName { get; set; }

    public CompanyBrandingAssetTypeId CompanyBrandingAssetTypeId { get; private set; }

    public MediaTypeId MediaTypeId { get; set; }

    public long FileSizeInBytes { get; set; }

    public byte[] FileContent { get; set; }

    public Guid CompanyId { get; private set; }

    public DateTimeOffset DateCreated { get; private set; }

    [LastChangedV1]
    public DateTimeOffset? DateLastChanged { get; set; }

    [LastEditorV1]
    public Guid? LastEditorId { get; private set; }

    // Navigation properties
    public virtual CompanyBrandingAssetType? CompanyBrandingAssetType { get; set; }
    public virtual MediaType? MediaType { get; set; }
    public virtual Company? Company { get; set; }
}
