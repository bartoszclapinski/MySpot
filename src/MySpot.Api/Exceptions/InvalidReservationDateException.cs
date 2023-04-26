namespace MySpot.Api.Exceptions;

public sealed class InvalidReservationDateException : CustomException
{
    public DateTime DateTime { get; }

    public InvalidReservationDateException(DateTime dateTime)
        : base($"Reservation date: {dateTime:d} is invalid")
    {
        DateTime = dateTime;
    }
}