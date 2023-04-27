namespace MySpot.Api.Exceptions;

public sealed class InvalidEmployeeNameException : CustomException
{
    public InvalidEmployeeNameException() : base("Employee name is invalid")
    {
    }        
}

public sealed class InvalidParkingSpotNameException : CustomException
{
    public InvalidParkingSpotNameException() : base("Parking spot name is invalid.")
    {
    }        
}