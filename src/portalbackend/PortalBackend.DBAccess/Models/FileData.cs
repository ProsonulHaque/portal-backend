namespace Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Models;

public record FileData
(
    byte[] FileContent,
    string FileName,
    string FileMediaType
);
