using IConstruye.Application.Responses;
using MediatR;

namespace IConstruye.Application.Queries;

public class GetInvoiceByUrlQuery(string shortUrl) : IRequest<InvoiceResponse>
{
    public string ShortUrl { get; } = shortUrl;
}