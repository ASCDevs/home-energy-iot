using home_energy_api.Data;
using home_energy_api.Models;
using home_energy_api.Models.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace home_energy_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private AppDbContext _context;

        public UserController(ILogger<UserController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        [HttpGet]
        [Route("AllUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                _logger.LogInformation("Got user list.");

                List<UserView> users = _context.users.AsNoTracking().Select(x => new UserView
                {
                    IdUser = x.IdUser,
                    UserName = x.UserName,
                    DtRegistration = x.DtRegistration.ToString("dd/MM/yyyy"),
                    UserEmail = x.UserEmail
                }).ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error on getting user list.");
                return BadRequest("Erro: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterUserForm registerForm)
        {
            try
            {
                _logger.LogInformation("Got user list.");

                //Add User
                if (registerForm.IdUser == 0)
                {
                    User userNew = new User
                    {
                        IdUser = registerForm.IdUser,
                        DtRegistration = DateTime.Now,
                        UserName = registerForm.UserName,
                        UserEmail = registerForm.UserEmail,
                        UserCPF = registerForm.UserCPF,
                        Password = registerForm.Password,
                        DtInactivation = null
                    };

                    if (_context.users.Add(userNew) == null) throw new Exception("Não foi possível cadastrar novo usuário");
                    _context.SaveChanges();

                    return Ok(new { message = "Usuário cadastrado com sucesso!" });
                }
                //Update User
                else
                {
                    User userUpdate = new User
                    {
                        IdUser = registerForm.IdUser,
                        Password = registerForm.Password,
                        UserCPF = registerForm.UserCPF,
                        UserEmail = registerForm.UserEmail,
                        UserName = registerForm.UserName,
                        DtInactivation = null
                    };

                    if(_context.users.Update(userUpdate) == null) throw new Exception("Erro ao atualizar o usuário.");
                    _context.SaveChanges();

                    return Ok(new { message = "Usuário atualizado com sucesso!" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error on register new user.");
                return BadRequest("Erro: " + ex.Message);
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string GetUserById(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
