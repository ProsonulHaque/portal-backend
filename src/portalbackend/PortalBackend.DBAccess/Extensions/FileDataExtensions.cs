using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Models;

namespace Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Extensions;

public static class FileDataExtensions
{
    public static bool IsDefault(this FileData fileData)
    {
        return fileData.FileContent == null &&
            fileData.FileMediaType == null &&
            fileData.FileName == null;
    }
}
