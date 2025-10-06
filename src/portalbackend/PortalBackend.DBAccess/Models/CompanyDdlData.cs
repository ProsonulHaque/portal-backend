namespace Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Models;

public record CompanyDdl
(
    IEnumerable<CompanyDdlData> CompanyDdlData
);

public record CompanyDdlData
(
    Guid CompanyId,
    string CompanyName
);
