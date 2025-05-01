using FluentAssertions;
using IConstruye.Factura.Core.Entities;

namespace IConstruye.Factura.Test;

public class InvoiceTests
{
    [Fact]
    public void Should_Track_Views_Via_QueryCounter()
    {
        var invoice = new Invoice
        {
            Issuer = "A",
            Receiver = "B",
            Amount = 1500,
            Date = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(1)
        };
    
        invoice.RegisterView();
        invoice.RegisterView();

        invoice.TimesConsulted.Should().Be(2);
        invoice.CanBeConsulted.Should().BeTrue();
    }

    [Fact]
    public void Should_Be_Expired_When_ExpiresAt_Has_Passed()
    {
        var invoice = new Invoice
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(-1),
        };

        invoice.IsExpired.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Be_Expired_When_ExpiresAt_Has_None()
    {
        var invoice = new Invoice
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
        };
        
        invoice.IsExpired.Should().BeFalse();
    }
}