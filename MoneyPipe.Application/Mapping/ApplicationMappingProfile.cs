using AutoMapper;
using MoneyPipe.Application.Services.Authentication.Commands.Register;
using MoneyPipe.Application.Services.Authentication.Common;
using MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice;
using MoneyPipe.Application.Services.Invoicing.Commands.EditInvoice;
using MoneyPipe.Application.Services.Invoicing.Common;
using MoneyPipe.Domain.InvoiceAggregate;
using MoneyPipe.Domain.InvoiceAggregate.Models;
using MoneyPipe.Domain.UserAggregate;
using MoneyPipe.Domain.UserAggregate.Models;

namespace MoneyPipe.Application.Mapping
{
    public class ApplicationMappingProfile:Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<RegisterCommand,UserRegisterData>();
            CreateMap<User,AuthenticationResult>()
                .ForMember(dest=> dest.Id, opt => opt.MapFrom(src=> src.Id.Value.ToString()));
            CreateMap<Invoice,InvoiceResult>();
            CreateMap<CreateInvoiceCommand,InvoiceData>();
            CreateMap<CreateInvoiceItem,InvoiceItemData>();
            CreateMap<EditInvoiceCommand,EditInvoiceData>()
                .ForMember(dest=>dest.Id,opt=>opt.MapFrom(src=>Guid.Parse(src.InvoiceId)));
            CreateMap<EditInvoiceItem,EditInvoiceData>()
                .ForMember(dest=>dest.Id,opt=>opt.MapFrom(src=>Guid.Parse(src.InvoiceItemId)));
        }
    }
}