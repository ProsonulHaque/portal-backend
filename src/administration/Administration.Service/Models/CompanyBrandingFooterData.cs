using System.ComponentModel.DataAnnotations;

namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Models;

public record CompanyBrandingFooterData
(
    [Required(ErrorMessage = "Provide company id")]
    Guid CompanyId,

    [Required(ErrorMessage = "Footer is required")]
    [MaxLength(1000, ErrorMessage = "Maximum length of footer is 1000 characters")]
    string Footer
);
