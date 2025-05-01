using Microsoft.AspNetCore.Mvc;

namespace IConstruye.Factura.V1.Controllers;

[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ApiController : ControllerBase
{
    
}