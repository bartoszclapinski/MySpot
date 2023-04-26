using MySpot.Api.Exceptions;

namespace MySpot.Api.Entities;

public class ReservationNotFoundException : CustomException
{
    public ReservationNotFoundException(Guid reservationId) : base($"Reservation with ID {reservationId} was not found.")
    {
    }

}