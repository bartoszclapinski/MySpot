using System;
using System.Collections.Generic;
using System.Linq;
using MySpot.Api.Commands;
using MySpot.Api.Entities;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    #region Arrange

    private readonly ReservationsService _reservationsService;
    private readonly clock _clock = new();
    private readonly List<WeeklyParkingSpot> _weeklyParkingSpots;
    
    
    public ReservationServiceTests()
    {
        _weeklyParkingSpots = new List<WeeklyParkingSpot>
        {
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(_clock.Current()), "P1"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(_clock.Current()), "P2"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(_clock.Current()), "P3"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(_clock.Current()), "P4"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(_clock.Current()), "P5")
        };
        _reservationsService = new ReservationsService(_weeklyParkingSpots);
    }

    #endregion
    
    [Fact]
    public void given_reservation_for_not_taken_date_create_should_succeed()
    {
        //  Arrange
        var parkingSpot = _weeklyParkingSpots.First();
        var command = new CreateReservation(
            parkingSpot.Id,
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