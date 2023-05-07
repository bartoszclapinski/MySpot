﻿using System;
using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.Models;
using MySpot.Api.ValueObjects;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    [Fact]
    public void given_invalid_date_add_reservation_should_fail()
    {
        // Arrange
        var now = new DateTime(2022, 08, 10);
        var invalidDate = now.AddDays(7);
        var weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(now), "P1");
        var reservation = new Reservation(Guid.NewGuid(), weeklyParkingSpot.Id, "John Doe",
            "XYZ123", new Date(invalidDate));

        //  Act
        var exception = Record.Exception(() => weeklyParkingSpot.AddReservation(reservation, new Date(now)));

        //  Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }
}