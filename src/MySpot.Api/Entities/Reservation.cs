using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Models;

public class Reservation
{
    public Guid Id { get; }
    public Guid ParkingSpotId { get; set; }
    public string EmployeeName { get; private set; }
    public LicensePlate LicensePlate { get; private set; }
    public DateTime Date { get; private set; }

    public Reservation(Guid id, Guid parkingSpotId, string employeeName, LicensePlate licensePlate, DateTime date)
    {
        Id = id;
        ParkingSpotId = parkingSpotId;
        EmployeeName = employeeName;
        ChangeLicensePlate(licensePlate);
        Date = date;
    }
    
    //  Change license plate
    public void ChangeLicensePlate(LicensePlate licensePlate)
        => LicensePlate = licensePlate;

}