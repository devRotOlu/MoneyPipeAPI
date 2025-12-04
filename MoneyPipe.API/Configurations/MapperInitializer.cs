using AutoMapper;
using MoneyPipe.API.DTOs;
using MoneyPipe.Application.Services.Authentication.Commands.Login;
using MoneyPipe.Application.Services.Authentication.Commands.PasswordReset;
using MoneyPipe.Application.Services.Authentication.Commands.Register;
using MoneyPipe.Application.Services.Authentication.Common;
using MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice;
using MoneyPipe.Domain.UserAggregate;
using MoneyPipe.Domain.UserAggregate.Models;

namespace MoneyPipe.API.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<RegisterDto, RegisterCommand>();
            CreateMap<RegisterCommand,UserRegisterData>();
            CreateMap<User,AuthenticationResult>()
                .ForMember(dest=> dest.Id, opt => opt.MapFrom(src=> src.Id.Value.ToString()));
            CreateMap<AuthenticationResult, UserDetailsDTO>();
            CreateMap<PasswordResetDTO,PasswordResetCommand>();
            CreateMap<LoginDTO,LoginCommand>();
            CreateMap<CreateInvoiceDTO,CreateInvoiceCommand>();
            CreateMap<CreateInvoiceItemDTO,CreateInvoiceItem>();

            // CreateMap<User, UserDetailsDTO>()
            //     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            // CreateMap<CreateInvoiceDTO, InvoiceRequest>();
            // CreateMap<CreateInvoiceItemsDTO, InvoiceItemRequest>();
            // CreateMap<InvoiceItem, GetInvoiceItemDTO>()
            //     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            // CreateMap<Invoice, GetInvoiceDTO>()
            //     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            // CreateMap<EditInvoiceDTO, EditInvoiceRequest>()
            //     .ForMember(dest => dest.Id, opt => opt.Ignore());
            // CreateMap<EditInvoiceItemDTO, EditInvoiceItemRequest>()
            //     .ForMember(dest => dest.InvoiceItemId,opt => opt.MapFrom(src => src.Id ))
            //     .ForMember(dest => dest.Id, opt => opt.Ignore());;
        }
    } 
}
