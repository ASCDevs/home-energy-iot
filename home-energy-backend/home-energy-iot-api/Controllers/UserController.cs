﻿using home_energy_iot_api.Models;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace home_energy_iot_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserManager _userManager;

        public UserController(ILogger<UserController> logger, IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                await _userManager.CreateUser(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            try
            {
                await _userManager.UpdateUser(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userManager.GetUser(id);

                var result = FilterUserResult(user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userManager.GetUsers();
                
                var usersFiltered = new List<UserView>();

                foreach (var user in users)
                    usersFiltered.Add(FilterUserResult(user));

                return Ok(usersFiltered);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private UserView FilterUserResult(User user)
        {
            return new UserView
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                DtRegistration = user.RegisterDate
            };
        }
    }
}
