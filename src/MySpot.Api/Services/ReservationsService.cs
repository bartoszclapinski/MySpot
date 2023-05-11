using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.Models;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Services;

public class ReservationsService
{
    private static Clock Clock = new();
    private readonly List<WeeklyParkingSpot> _weeklyParkingSpots;
    
    

    public ReservationsService(List<WeeklyParkingSpot> weeklyParkingSpots)
    {
        _weeklyParkingSpots = weeklyParkingSpots;
    }
    
    public IEnumerable<ReservationDto> GetAllWeekly()        
        => _weeklyParkingSpots.SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto {
                Id = x.Id,
                ParkingSpotId = x.ParkingSpotId,
                EmployeeName = x.EmployeeName,
                Date = x.Date.Value.Date
            });

    public ReservationDto Get(Guid id)
        => GetAllWeekly().SingleOrDefault(x => x.Id == id);

    public Guid? Create(CreateReservation command)
    {
        var parkingSpotId = new ParkingSpotId(command.ParkingSpotId);
        var weeklyParkingSpot = _weeklyParkingSpots
            .SingleOrDefault(x => x.Id == parkingSpotId);
        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(
            command.ReservationId,
            command.ParkingSpotId,
            command.EmployeeName,
            command.LicensePlate,
            new Date(command.Date));

        weeklyParkingSpot.AddReservation(reservation, new Date(Clock.Current()));

        return reservation.Id;
    }
    
    public bool Update(ChangeReservationLicensePlate command) 
    {
        //  Find the reservation with the given ID
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
        
        //  Check if the reservation was found
        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var reservationId = new ReservationId(command.ReservationId);
        //  Find the reservation with the given ID
        var existingReservation = weeklyParkingSpot.Reservations
            .SingleOrDefault(x => x.Id == reservationId);
        
        //  Check if the reservation was found
        if (existingReservation is null)
        {
            return false;
        }
        

        //  Check if the reservation is in the past
        if (existingReservation.Date.Value.Date <= Clock.Current())
        {
            return false;
        }

        // Update the reservation
        existingReservation.ChangeLicensePlate(command.LicensePlate);
        
        return true;
    }
    
    public bool Delete(DeleteReservation command)
    {
        //  Find the reservation with the given ID
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
        
        //  Check if the reservation was found
        if (weeklyParkingSpot is null)
        {
            return false;
        }
        
        
        // Find the reservation with the given ID
        var reservationId = new ReservationId(command.ReservationId);
        var existingReservation = weeklyParkingSpot.Reservations
            .SingleOrDefault(x => x.Id == reservationId); 
        
        // Check if the reservation was found
        if (existingReservation is null)
        {
            // Return from the method
            return false; 
        }
        
        // Remove the reservation
        weeklyParkingSpot.RemoveReservation(command.ReservationId);
        
        return true;
    }
    
    //  Find the reservation with the given ID
    private WeeklyParkingSpot GetWeeklyParkingSpotByReservation(ReservationId reservationId)
        => _weeklyParkingSpots.
            SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));

}