using System.ComponentModel.DataAnnotations;

namespace IConstruye.Factura.Models;

public class UploadInvoiceRequest
{
    [Required]
    public IFormFile File { get; set; } = null!;
}