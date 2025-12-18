using AutoMapper;
using MoneyPipe.Application.Services.Authentication.Commands.Register;
using MoneyPipe.Application.Services.Authentication.Common;
using MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice;
using MoneyPipe.Application.Services.Invoicing.Commands.EditInvoice;
using MoneyPipe.Application.Services.Invoicing.Common;
using MoneyPipe.Application.Services.Wallet.Commands.CreateWallet;
using MoneyPipe.Domain.InvoiceAggregate;
using MoneyPipe.Domain.InvoiceAggregate.Entities;
using MoneyPipe.Domain.InvoiceAggregate.Models;
using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;
using MoneyPipe.Domain.UserAggregate;
using MoneyPipe.Domain.UserAggregate.Models;
using MoneyPipe.Domain.UserAggregate.ValueObjects;
using MoneyPipe.Domain.WalletAggregate;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Mapping
{
    public class ApplicationMappingProfile:Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<RegisterCommand,UserRegisterData>();
            CreateMap<UserId, Guid>()
                .ConvertUsing(src => src.Value);
            CreateMap<User,AuthenticationResult>()
                .ForMember(dest=> dest.Id, opt => opt.MapFrom(src=> src.Id.Value));
            CreateMap<InvoiceId, Guid>()
                .ConvertUsing(src => src.Value);
            CreateMap<Invoice,InvoiceResult>()
                .ForMember(dest=>dest.Id,opt=>opt.MapFrom(src=>src.Id.Value));
            CreateMap<InvoiceItemId,Guid>()
                .ConvertUsing(src => src.Value);
            CreateMap<InvoiceItem,InvoiceItemResult>()
                .ForMember(dest=>dest.Id,opt=>opt.MapFrom(src=>src.Id.Value));
            CreateMap<CreateInvoiceCommand,InvoiceData>();
            CreateMap<CreateInvoiceItem,InvoiceItemData>();
            CreateMap<EditInvoiceCommand,EditInvoiceData>()
                .ForMember(dest=>dest.Id,opt=>opt.MapFrom(src=>src.InvoiceId));
            CreateMap<EditInvoiceItem,EditInvoiceItemData>()
                .ForMember(dest=>dest.Id,opt=>opt.MapFrom(src=>src.InvoiceItemId));
            CreateMap<WalletId, Guid>()
                .ConvertUsing(src => src.Value);
            CreateMap<Wallet,WalletResult>()
                .ForMember(dest=>dest.Id,opt=>opt.MapFrom(src=>src.Id.Value));
        }
    }
}