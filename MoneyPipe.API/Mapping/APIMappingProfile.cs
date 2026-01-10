using AutoMapper;
using MoneyPipe.API.DTOs.Requests;
using MoneyPipe.API.DTOs.Responses;
using MoneyPipe.Application.Services.Authentication.Commands.Login;
using MoneyPipe.Application.Services.Authentication.Commands.PasswordReset;
using MoneyPipe.Application.Services.Authentication.Commands.Register;
using MoneyPipe.Application.Services.Authentication.Common;
using MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice;
using MoneyPipe.Application.Services.Invoicing.Commands.EditInvoice;
using MoneyPipe.Application.Services.Invoicing.Commands.SendInvoice;
using MoneyPipe.Application.Services.Invoicing.Common;
using MoneyPipe.Application.Services.Invoicing.Queries.GetInvoices;
using MoneyPipe.Application.Services.KYCManagement.Commands.AddDocument;
using MoneyPipe.Application.Services.KYCManagement.Commands.CompleteProfile;
using MoneyPipe.Application.Services.KYCManagement.Common;
using MoneyPipe.Application.Services.KYCManagement.Queries.GetKycStatus;
using MoneyPipe.Application.Services.WalletManagement.Commands.CreateWallet;

namespace MoneyPipe.API.Mapping
{
    class APIMappingProfile:Profile
    {
        public APIMappingProfile()
        {
            CreateMap<RegisterDto, RegisterCommand>();
            CreateMap<AuthenticationResult, UserDetailsDTO>();
            CreateMap<PasswordResetDTO,PasswordResetCommand>();
            CreateMap<LoginDTO,LoginCommand>();
            CreateMap<CreateInvoiceDTO,CreateInvoiceCommand>();
            CreateMap<CreateInvoiceItemDTO,CreateInvoiceItem>();
            CreateMap<InvoiceResult,GetInvoiceDTO>();
            CreateMap<InvoiceItemResult,GetInvoiceItemDTO>();
            CreateMap<EditInvoiceDTO,EditInvoiceCommand>();
            CreateMap<EditInvoiceItemDTO,EditInvoiceItem>();
            CreateMap<GetInvoicesResult,GetInvoicesDTO>();
            CreateMap<InvoiceResult,GetInvoiceBase>();
            CreateMap<WalletResult,GetWalletDTO>();
            CreateMap<SendInvoiceDTO,SendInvoiceCommand>();
            CreateMap<CompleteProfileDTO,CompleteProfileCommand>();
            CreateMap<BaseDocumentDTO,BaseDocumentCommand>();
            CreateMap<AddDocumentDTO,AddDocumentCommand>();
            CreateMap<GetKycSatusResult,GetKycStatusDTO>();
        }
    }
}