using MySpot.Api.ValueObjects;

namespace MySpot.Api.Commands;

public record CreateReservation(
    ParkingSpotId ParkingSpotId, ReservationId ReservationId, EmployeeName EmployeeName, LicensePlate LicensePlate, Date Date);