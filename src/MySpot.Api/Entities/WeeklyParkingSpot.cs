using MySpot.Api.Exceptions;
using MySpot.Api.Models;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();

    public ParkingSpotId Id { get; }
    public Week Week { get; }
    public string Name { get; }
    
    public IEnumerable<Reservation> Reservations => _reservations;
    
    public WeeklyParkingSpot(ParkingSpotId id, Week week, string name)
    {
        Id = id;
        Week = week;
        Name = name;
    }
    
    public void AddReservation(Reservation reservation, Date now)
    {
        // Check if the reservation is valid
        var isInvalidDate = reservation.Date < Week.From || 
                            reservation.Date > Week.To || 
                            reservation.Date < now;
         if (isInvalidDate)
         {
             throw new InvalidReservationDateException(reservation.Date.Value.Date);
         }
         
         // Check if the reservation already exists
         if (Reservations.Any(x => x.Date == reservation.Date))
         { 
             throw new ParkingSpotAlreadyReservedException(Name, reservation.Date.Value.Date);
         }
         
         // Add the reservation
            _reservations.Add(reservation);
    }
    
    //  Remove the reservation with the given ID
    public void RemoveReservation(ReservationId reservationId)
        => _reservations.RemoveWhere(x => x.Id == reservationId);

}