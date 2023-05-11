using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.Models;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private static readonly Clock Clock = new();
    private static readonly ReservationsService _service = new(new()
    {
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(Clock.Current()), "P1"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(Clock.Current()), "P2"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(Clock.Current()), "P3"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(Clock.Current()), "P4"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(Clock.Current()), "P5"),
    });

    [HttpGet]   
    public IEnumerable<ReservationDto> Get() => _service.GetAllWeekly();  
    
    [HttpGet("{id:Guid}")]
    public ActionResult<Reservation> Get(Guid id)
    {
        var reservation = _service.Get(id);
        
        // Check if the reservation was found
        if (reservation is null)
        {
            return NotFound(); 
        }
        
        // Return the reservation
        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(CreateReservation command)
    {
        // Create the reservation
        var id = _service.Create(command with {ReservationId = Guid.NewGuid()});
        
        //  Check if the reservation was created
        if (id is null)
        {
            return BadRequest();
        }
        
        // Return the ID
        return CreatedAtAction(nameof(Get), new { id }, id);
    }
    
    [HttpPut("{id:Guid}")]
    public ActionResult Put(Guid id, ChangeReservationLicensePlate command)
    {
        // Check if the reservation was updated
        if (_service.Update(command with {ReservationId = id}))
        {
            return NoContent();
        }

        return NotFound();
    }
    
    [HttpDelete("{id:Guid}")]
    public ActionResult Delete(Guid id)
    {
        // Check if the reservation was deleted
        if (_service.Delete(new DeleteReservation(id)))
        {
            return NoContent();
        }

        return NotFound();
    }
}                                        