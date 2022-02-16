using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cwiczenia7.Models;
using Microsoft.EntityFrameworkCore;

namespace cwiczenia7.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TripController : ControllerBase
    {
        [HttpGet("trips")]
        public async Task<IActionResult> getTrip()
        {
            var context = new s21009Context();
            var trips = context.Trips.Select(c => new
            {
                Name = c.Name,
                Description = c.Description,
                DateFrom = c.DateFrom,
                DateTo = c.DateTo,
                MaxPeople = c.MaxPeople,
                Countries = c.CountryTrips,
                Clients = c.ClientTrips
            }).GroupBy(c => c.DateFrom).ToList();
            return Ok(trips);
        }

        [HttpDelete("clients/{idClient}")]
        public async Task<IActionResult> deleteClient(int idClient)
        {
            var context = new s21009Context();
            int count = context.ClientTrips.Where(c => c.IdClient == idClient).Count();
            if (count == 0)
            {
                context.Remove(context.Clients.Single(a => a.IdClient == idClient));
                context.SaveChanges();
                return Ok("Client has been deleted");
            }
            else
                return Ok(403);
        }

        [HttpPost("trips/{idTrip}/clients")]
        public async Task<IActionResult> addClient(Client newClient)
        {
            var context = new s21009Context();
            int checkPesel = context.Clients.Where(c => c.Pesel == newClient.Pesel).Count();
            if (checkPesel == 0)
            {
                int checkClientsTrip = context.ClientTrips.Where(c => c.IdClient == newClient.IdClient).Count();
                if (checkClientsTrip == 0)
                {
                    context.Add(newClient);
                    context.SaveChanges();
                    return Ok("Client has been added");
                }
            }
            return Ok("Client can not be added");
        }
    }
}
