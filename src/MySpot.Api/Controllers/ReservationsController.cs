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
    private readonly ReservationsService _reservationsService;

    public ReservationsController(ReservationsService reservationsService)
    {
        _reservationsService = reservationsService;
    }

    [HttpGet]   
    public IEnumerable<ReservationDto> Get() => _reservationsService.GetAllWeekly();  
    
    [HttpGet("{id:Guid}")]
    public ActionResult<Reservation> Get(Guid id)
    {
        var reservation = _reservationsService.Get(id);
        
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
        var id = _reservationsService.Create(command with {ReservationId = Guid.NewGuid()});
        
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
        if (_reservationsService.Update(command with {ReservationId = id}))
        {
            return NoContent();
        }

        return NotFound();
    }
    
    [HttpDelete("{id:Guid}")]
    public ActionResult Delete(Guid id)
    {
        // Check if the reservation was deleted
        if (_reservationsService.Delete(new DeleteReservation(id)))
        {
            return NoContent();
        }

        return NotFound();
    }
}                                        