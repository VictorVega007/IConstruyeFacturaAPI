using IConstruye.Factura.Core.Entities.Base;

namespace IConstruye.Factura.Core.Entities;

public class User : Entity
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}