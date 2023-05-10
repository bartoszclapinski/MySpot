using System;
using MySpot.Api.Commands;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    #region Arrange

    private readonly ReservationsService _reservationsService;
    
    public ReservationServiceTests()
    {
        _reservationsService = new ReservationsService();
    }

    #endregion
    
    [Fact]
    public void given_reservation_for_not_taken_date_create_should_succeed()
    {
        //  Arrange
        var command = new CreateReservation(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Guid.NewGuid(),
            "John Doe",
            "XYZ123",
            new Date(DateTime.UtcNow.AddMinutes(5)));
        
        //  Act
        var reservationId = _reservationsService.Create(command);
        
        //  Assert
        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe<Guid>(command.ReservationId);
    }
}