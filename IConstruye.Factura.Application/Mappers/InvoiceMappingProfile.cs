using AutoMapper;
using IConstruye.Application.Responses;
using IConstruye.Factura.Core.Entities;

namespace IConstruye.Application.Mappers;

public class InvoiceMappingProfile : Profile
{
    public InvoiceMappingProfile()
    {
        CreateMap<InvoiceResponse, Invoice>().ReverseMap();
    }
}