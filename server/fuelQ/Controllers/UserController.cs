﻿using fuelQ.Factory;
using fuelQ.Models;
using fuelQ.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace fuelQ.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IVehicleService vehicleService;
        private readonly IFuelStatioService fuelStatioService;
        private readonly ISecurityService securityService;
        private readonly UserFactory userFactory;
        //UserFactory userFactory = new UserFactory();
        public UserController(IUserService userService , IVehicleService vehicleService , IFuelStatioService fuelStatioService , ISecurityService securityService)
        {
            this.userService = userService;
            this.vehicleService = vehicleService;
            this.fuelStatioService = fuelStatioService;
            this.securityService = securityService;
            this.userFactory = new UserFactory(userService  , vehicleService , fuelStatioService , securityService);
        }
        // GET: UserController
        [HttpGet("GetUsers")]
        public ActionResult<List<User>> Index()
        {
            return userService.Get();
        }

        // GET: UserController/GetUserById/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(string id)
        {
            var user = userService.Get(id);
            if (user == null)
            {
                return NotFound($"User with id {id} not found.");
            }
            return user;
        }

        // Post: UserController/Create
        [HttpPost("AddUser")]
        public ActionResult Create([FromBody] User user)
        {
            userService.Create(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // Put: UserController/Edit/5
        [HttpPut("{id}")]
        public ActionResult Edit(string id, User user)
        {
            var existingUser = GetUserById(id);
            if (existingUser == null)
            {
                return NotFound($"User with id {id} not found.");
            }
            userService.Update(id, user);
            return NoContent();
        }

        // Delete: UserController/Delete/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var user = GetUserById(id);
            if (user == null)
            {
                return NotFound($"User with id {id} not found.");
            }
            else
            {
                userService.Remove(id);
                return Ok($"User with id {id} is deleted."); 
            }
        }

        // Post: UserController/RegisterDriver
        [HttpPost("registerDriver")]
        public ActionResult<String> RegisterDriver([FromBody] Driver driver)
        {
            Console.WriteLine("Inside register driver");
            return userFactory.RegisterDriver(driver);
        }
        
        // Post: UserController/RegisterDriver
        [HttpPost("userLogin")]
        public ActionResult<String> UserLogin([FromBody] User user)
        {
            return userFactory.ValidateUser(user);
        }
    }
}
