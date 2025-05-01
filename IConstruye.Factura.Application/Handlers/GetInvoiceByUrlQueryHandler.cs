using AutoMapper;
using IConstruye.Application.Queries;
using IConstruye.Application.Responses;
using IConstruye.Factura.Core.Repositories;
using MediatR;

namespace IConstruye.Application.Handlers;

public class GetInvoiceByUrlQueryHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
    : IRequestHandler<GetInvoiceByUrlQuery, InvoiceResponse>
{
    public async Task<InvoiceResponse> Handle(GetInvoiceByUrlQuery request, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetInvoiceByUrlAsync(request.ShortUrl);

        if (invoice.IsExpired)
            throw new InvalidOperationException("The url for consulting the invoice is expired");
    
        if (!invoice.CanBeConsulted)
            throw new InvalidOperationException("Invoice view limit reached.");

        invoice.RegisterView();

        await invoiceRepository.SaveChangesAsync();

        var response = mapper.Map<InvoiceResponse>(invoice);
        return response;
    }
}