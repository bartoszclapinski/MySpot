using System;
using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.Models;
using MySpot.Api.ValueObjects;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    #region Arrange

    private readonly Date _now;
    private readonly WeeklyParkingSpot _weeklyParkingSpot;

    public WeeklyParkingSpotTests()
    {
        _now = new Date(new DateTime(2022, 08, 10));
        _weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_now), "P1");
    }

    #endregion
    
    
    [Theory]
    [InlineData("2022-08-09")]
    [InlineData("2022-08-17")]
    public void given_invalid_date_add_reservation_should_fail(string dateString)
    {
        // Arrange
        
        var invalidDate = DateTime.Parse(dateString);
        var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe",
            "XYZ123", new Date(invalidDate));

        //  Act
        var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(reservation, _now));

        //  Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }

    [Fact]
    public void given_reservation_for_already_existing_date_add_reservation_should_fail()
    {
        //  Arrange
        var reservationDate = _now.AddDays(1);
        var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe",
            "XYZ123", reservationDate);
        var nextReservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe",
            "XYZ123", reservationDate);
        _weeklyParkingSpot.AddReservation(reservation, _now);
        
        //  Act  
        var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(nextReservation, reservationDate));
        
        //  Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotAlreadyReservedException>();
    }
    
    [Fact]
    public void given_reservation_for_not_taken_date_add_reservation_should_succeed()
    {
        //  Arrange
        var reservationDate = _now.AddDays(1);
        var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe",
            "XYZ123", reservationDate);
        
        //  Act
        _weeklyParkingSpot.AddReservation(reservation, _now);
        
        //  Assert
        _weeklyParkingSpot.Reservations.ShouldHaveSingleItem();
    }
    
}