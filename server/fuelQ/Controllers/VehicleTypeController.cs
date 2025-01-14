﻿using fuelQ.Models;
using fuelQ.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fuelQ.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleTypeController : Controller
    {
        private readonly IVehicleTypeService vehicleTypeService;

        public VehicleTypeController(IVehicleTypeService vehicleTypeService)
        {
            this.vehicleTypeService = vehicleTypeService;
        }
        // GET: VehicleTypeController
        [HttpGet("GetVehicleTypes")]
        public ActionResult<List<VehicleType>> Index()
        {
            return vehicleTypeService.Get();
        }

        // GET: VehicleTypeController/GetVehicleTypeById/5
        [HttpGet("{id}")]
        public ActionResult<VehicleType> GetVehicleTypeById(string id)
        {
            var vehicleType = vehicleTypeService.Get(id);
            if (vehicleType == null)
            {
                return NotFound($"Vehicle Type with id {id} not found.");
            }
            return vehicleType;
        }

        // Post: VehicleTypeController/Create
        [HttpPost("AddVehicleType")]
        public ActionResult Create([FromBody] VehicleType vehicleType)
        {
            vehicleTypeService.Create(vehicleType);
            return CreatedAtAction(nameof(GetVehicleTypeById), new { id = vehicleType.Id }, vehicleType);
        }

        // Put: VehicleTypeController/Edit/5
        [HttpPut("{id}")]
        public ActionResult Edit(string id, VehicleType vehicleType)
        {
            var existingVehicleType = GetVehicleTypeById(id);
            if (existingVehicleType == null)
            {
                return NotFound($"Vehicle Type with id {id} not found.");
            }
            else
            {
                vehicleTypeService.Update(id, vehicleType);
                return StatusCode(200, Json(new
                {
                    status = "Success"
                }));
            }
        }

        // Delete: VehicleTypeController/Delete/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var vehicleType = GetVehicleTypeById(id);
            if (vehicleType == null)
            {
                return NotFound($"Vehicle Type with id {id} not found.");
            }
            vehicleTypeService.Remove(id);
            return Ok($"Vehicle Type with id {id} is deleted.");
        }

        // Put: FuelInventoryController/UpdateFuelAmount/5
        [HttpPut("UpdateAllowedFuelAmount/{vehicleTypeId}/{ammount}")]
        public ActionResult UpdateAllowedFuelAmount(string vehicleTypeId, int ammount)
        {
            VehicleType vehicleType = vehicleTypeService.Get(vehicleTypeId);
            if (vehicleType == null)
            {
                return NotFound($"Vehicle type with vehicleTypeId {vehicleTypeId} and ammount {ammount} not found.");
            }
            else
            {
                vehicleType.MaxAmmount = ammount;
                vehicleTypeService.Update(vehicleType.Id, vehicleType);
                return Ok();
            }
        }
    }
}
