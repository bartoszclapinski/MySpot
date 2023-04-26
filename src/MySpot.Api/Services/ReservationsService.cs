using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.Models;

namespace MySpot.Api.Services;

public class ReservationsService
{
    private static Clock _clock = new();
    
    private static readonly List<WeeklyParkingSpot> WeeklyParkingSpots = new()
    {
        new WeeklyParkingSpot(Guid.NewGuid(), "P1", _clock.Current(), _clock.Current().AddDays(7)),
        new WeeklyParkingSpot(Guid.NewGuid(), "P2", _clock.Current(), _clock.Current().AddDays(7)),
        new WeeklyParkingSpot(Guid.NewGuid(), "P3", _clock.Current(), _clock.Current().AddDays(7)),
        new WeeklyParkingSpot(Guid.NewGuid(), "P4", _clock.Current(), _clock.Current().AddDays(7)),
        new WeeklyParkingSpot(Guid.NewGuid(), "P5", _clock.Current(), _clock.Current().AddDays(7)),
    };
    
    
    
    public IEnumerable<ReservationDto> GetAllWeekly()        
        => WeeklyParkingSpots.SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto {
                Id = x.Id,
                ParkingSpotId = x.ParkingSpotId,
                EmployeeName = x.EmployeeName,
                Date = x.Date
            });

    public ReservationDto Get(Guid id)
        => GetAllWeekly().SingleOrDefault(x => x.Id == id);

    public Guid? Create(CreateReservation command)
    { 
        var weeklyParkingSpot = WeeklyParkingSpots
            .SingleOrDefault(x => x.Id == command.ParkingSpotId);
        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(
            command.ReservationId,
            command.ParkingSpotId,
            command.EmployeeName,
            command.LicensePlate,
            command.Date);

        weeklyParkingSpot.AddReservation(reservation, _clock.Current());

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
        
        
        //  Find the reservation with the given ID
        var existingReservation = weeklyParkingSpot.Reservations
            .SingleOrDefault(x => x.Id == command.ReservationId);
        
        //  Check if the reservation was found
        if (existingReservation is null)
        {
            return false;
        }
        

        //  Check if the reservation is in the past
        if (existingReservation.Date <= _clock.Current())
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
        var existingReservation = weeklyParkingSpot.Reservations
            .SingleOrDefault(x => x.Id == command.ReservationId); 
        
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
    private WeeklyParkingSpot GetWeeklyParkingSpotByReservation(Guid reservationId)
        => WeeklyParkingSpots.
            SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));

}