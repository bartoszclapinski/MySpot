using MySpot.Api.Exceptions;
using MySpot.Api.Models;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();

    public Guid Id { get; }
    public DateTime From { get; }
    public DateTime To { get; }
    public string Name { get; }
    
    public IEnumerable<Reservation> Reservations => _reservations;
    
    public WeeklyParkingSpot(Guid id, string name, DateTime from, DateTime to)
    {
        Id = id;
        Name = name;
        From = from;
        To = to;
    }
    
    public void AddReservation(Reservation reservation, DateTime now)
    {
        // Check if the reservation is valid
        var isInvalidDate = reservation.Date.Date < From || 
                            reservation.Date.Date > To || 
                            reservation.Date.Date < now;
         if (isInvalidDate)
         {
             throw new InvalidReservationDateException(reservation.Date);
         }
         
         // Check if the reservation already exists
         if (Reservations.Any(x => x.Date.Date == reservation.Date.Date))
         { 
             throw new ParkingSpotAlreadyReservedException(Name, reservation.Date);
         }
         
         // Add the reservation
            _reservations.Add(reservation);
    }
    
    //  Remove the reservation with the given ID
    public void RemoveReservation(Guid reservationId)
    {   
        //  Find the reservation with the given ID
        var reservation = _reservations.SingleOrDefault(x => x.Id == reservationId);
        
        //  Check if the reservation was found
        if (reservation is null)
        {
            throw new ReservationNotFoundException(reservationId);
        }
        
        _reservations.Remove(reservation);
    }
}