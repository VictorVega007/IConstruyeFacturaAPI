using IConstruye.Application.Commands;
using IConstruye.Application.Queries;
using IConstruye.Factura.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IConstruye.Factura.V1.Controllers;

public class InvoiceController(IMediator mediator) : ApiController
{
    private readonly IMediator _mediator = mediator;
    
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload([FromForm] UploadInvoiceRequest request)
    {
        var result = await _mediator.Send(new UploadInvoiceCommand { File = request.File });

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result);
    }
    
    [HttpGet("{shortUrl}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByShortUrl(string shortUrl)
    {
        try
        {
            var result = await _mediator.Send(new GetInvoiceByUrlQuery(shortUrl));
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}