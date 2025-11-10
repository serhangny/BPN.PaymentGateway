using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;

using Microsoft.Extensions.Caching.Memory;

using BPN.PaymentGateway.Application.Orders.Commands;
using BPN.PaymentGateway.Application.Clients;
using FluentAssertions;
using Xunit;

namespace PaymentGateway.UnitTests.Application.Orders;

public class CreateOrderCommandTest : TestBase
{
    private readonly Mock<IMemoryCache> _memoryCacheMock;
    private readonly Mock<IBalanceManagementClient> _balanceManagementClientMock;
    private readonly CreateOrderCommandHandler _handler;

    /// <summary>
    /// CTOR
    /// </summary>
    public CreateOrderCommandTest(Mock<IMemoryCache> memoryCacheMock, Mock<IBalanceManagementClient> balanceManagementClientMock, CreateOrderCommandHandler handler)
    {
        _memoryCacheMock = memoryCacheMock;
        _balanceManagementClientMock = balanceManagementClientMock;
        _handler = handler;
    }
    
    [Fact]
    public async Task Handle_Should_CreateOrder_When_ValidCommand()
    {
        // Arrange
        var command = Fixture.Build<CreateOrderCommand>()
                .With(x => x.OrderId, "1112233123")
            .With(x => x.Amount, 100)
            .Create();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
    }
}