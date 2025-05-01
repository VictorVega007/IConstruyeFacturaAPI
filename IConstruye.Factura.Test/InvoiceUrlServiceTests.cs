using FluentAssertions;
using IConstruye.Factura.Core.Services;

namespace IConstruye.Factura.Test;

public class InvoiceUrlServiceTests
{
    [Fact]
    public void Should_Generate_Different_Urls_For_Different_Ids_Of_Invoice()
    {
        var service = new InvoiceUrlService();
        
        var url1 = service.GenerateInvoiceUrl("abc");
        var url2 = service.GenerateInvoiceUrl("def");
        var url3 = service.GenerateInvoiceUrl("ghi");
        
        url1.Should().NotBeEquivalentTo(url2);
        url2.Should().NotBeEquivalentTo(url3);
        url3.Should().NotBeEquivalentTo(url1);
        url1.Should().NotBeEquivalentTo(url2);
    }

    [Fact]
    public void Sould_Generate_Only_Base62_Characters()
    {
        var service = new InvoiceUrlService();
        var shorUrl = service.GenerateInvoiceUrl("test-id");

        shorUrl.Should().MatchRegex("^[0-9a-zA-Z]+$");
    }
}