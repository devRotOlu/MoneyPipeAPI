// using Moq;
// using MoneyPipe.Application.Interfaces.IRepository;
// using MoneyPipe.Domain.Entities;
// using MoneyPipe.Application.Interfaces;
// using MoneyPipe.Application.Models;


// namespace MoneyPipe.Tests.Services
// {
//     public class InvoiceServiceTests
//     {
//         private readonly Mock<IUserRepository> _userRepoMock;
//         private readonly Mock<IInvoiceRepository> _invoiceRepoMock;
//         private readonly IInvoiceService _invoiceService;
//         private readonly Mock<IUnitOfWork> _mockUnitOfWork;

//         public InvoiceServiceTests()
//         {
//             _userRepoMock = new Mock<IUserRepository>();
//             _mockUnitOfWork = new Mock<IUnitOfWork>();
//             _invoiceRepoMock = new Mock<IInvoiceRepository>();
//             _invoiceService = new InvoiceService(_userRepoMock.Object, _invoiceRepoMock.Object);
//         }

//         [Fact]
//         public async Task CreateInvoice_Should_Create_Invoice_For_Valid_User()
//         {
//             // Arrange
//             var userId = Guid.NewGuid();
//             var user = new User
//             {
//                 Id = userId,
//                 Email = "test@example.com",
//                 DefaultCurrency = "NGN"
//             };

//             _userRepoMock.Setup(r => r.GetByIdAsync(userId))
//                 .ReturnsAsync(user);

//             var request = new CreateInvoiceRequest
//             {
//                 UserId = userId,
//                 Amount = 5000m,
//                 Description = "Website design"
//             };

//             // Act
//             var invoice = await _invoiceService.CreateInvoiceAsync(request);

//             // Assert
//             Assert.NotNull(invoice);
//             Assert.Equal(userId, invoice.UserId);
//             Assert.Equal(5000m, invoice.Amount);
//             Assert.Equal("NGN", invoice.Currency);
//             Assert.StartsWith("INV-", invoice.InvoiceNumber);
//             Assert.Equal("Pending", invoice.Status);

//             _invoiceRepoMock.Verify(r => r.AddAsync(It.IsAny<Invoice>()), Times.Once);
//         }
//     }
// }