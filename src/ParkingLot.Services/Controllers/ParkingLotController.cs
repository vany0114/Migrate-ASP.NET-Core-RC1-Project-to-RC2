namespace ParkingLot.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Model;
    using Entities = ParkingLot.Data.Model;
    using Interfaces;

    [Route("api/[controller]")]
    public class ParkingLotController : BaseController
    {
        private IParkingLotFacade _parkingLotFacade;

        public ParkingLotController(IParkingLotFacade parkingLotFacade)
        {
            _parkingLotFacade = parkingLotFacade;
        }

        [HttpGet]
        public IEnumerable<Entities.ParkingLot> GetAll()
        {
            return _parkingLotFacade.GetAllParkingLots();
        }

        [HttpGet("{id:guid}", Name = "GetById")]
        public IActionResult GetById(Guid id)
        {
            var item = _parkingLotFacade.GetParkingLot(id);
            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public void CreateParkingLote([FromBody] Entities.ParkingLot parkingLot)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
            }
            else
            {
                _parkingLotFacade.AddParkingLot(parkingLot);

                string url = Url.RouteUrl("GetById", new { id = parkingLot.Id }, Request.Scheme, Request.Host.ToUriComponent());
                Response.StatusCode = 201;
                Response.Headers["Location"] = url;
            }
        }

        [HttpPut]
        public IActionResult UpdateParkingLote([FromBody] Entities.ParkingLot parkingLot)
        {
            _parkingLotFacade.UpdateParkingLot(parkingLot);
            return new StatusCodeResult(204);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteParkingLote(Guid id)
        {
            _parkingLotFacade.DeleteParkingLot(id);
            return new StatusCodeResult(204);
        }
    }
}
