using System.ComponentModel.DataAnnotations;

namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Models;

public record CompanyBrandingLogoUpdateData
(
    [Required(ErrorMessage = "Logo file is missing")]
    IFormFile CompanyLogoFile
);
