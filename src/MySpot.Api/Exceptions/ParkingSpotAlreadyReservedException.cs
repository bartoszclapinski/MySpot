namespace MySpot.Api.Exceptions;

public sealed class ParkingSpotAlreadyReservedException : CustomException
{
    public DateTime DateTime { get; }
    public string Name { get; }

    public ParkingSpotAlreadyReservedException(string name, DateTime dateTime)
        : base($"Parking spot: {name} is already reserved at: {dateTime:d}")  
    {
        DateTime = dateTime;
        Name = name;
    }
}