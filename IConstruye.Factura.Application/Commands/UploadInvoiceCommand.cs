using IConstruye.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IConstruye.Application.Commands;

public class UploadInvoiceCommand : IRequest<UploadInvoiceResponse>
{
    public IFormFile File { get; set; } = null!;
}