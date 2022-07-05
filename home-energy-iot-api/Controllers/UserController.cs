using home_energy_iot_api.Models;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace home_energy_iot_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private DataBaseContext _context;

        public UserController(ILogger<UserController> logger, DataBaseContext dbContext)
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

                List<UserView> users = _context.Users.AsNoTracking().Select(x => new UserView
                {
                    IdUser = x.Id,
                    UserName = x.Name,
                    DtRegistration = x.RegisterDate.ToString("dd/MM/yyyy"),
                    UserEmail = x.Email
                }).ToList();
                if (users.Count() == 0) throw new Exception("Nenhum usuário encontrado");

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
        public async Task<IActionResult> AddOrUpdateUser([FromBody] RegisterUserForm registerForm)
        {
            try
            {
                //Add User
                if (registerForm.IdUser == 0)
                {
                    _logger.LogInformation("Adding a new User.");
                    User userNew = new User
                    {
                        Id = registerForm.IdUser,
                        RegisterDate = DateTime.Now,
                        Username = "123",
                        Name = registerForm.UserName,
                        Email = registerForm.UserEmail,
                        CPF = registerForm.UserCPF,
                        Password = registerForm.Password,
                        SaltPassword = "123"
                    };

                    if (_context.Users.Add(userNew) == null) throw new Exception("Não foi possível cadastrar novo usuário");
                    _context.SaveChanges();

                    _logger.LogInformation("New user has been successfully added.");
                    return Ok(new { message = "Usuário cadastrado com sucesso!" });
                }

                //Update User

                _logger.LogInformation("Updating an User.");
                User userUpdate = new User
                {
                    Id = registerForm.IdUser,
                    Username = "123",
                    Password = registerForm.Password,
                    CPF = registerForm.UserCPF,
                    Email = registerForm.UserEmail,
                    Name = registerForm.UserName,
                    SaltPassword = "123"
                };

                if (_context.Users.Update(userUpdate) == null) throw new Exception("Erro ao atualizar o usuário.");
                _context.SaveChanges();

                _logger.LogInformation("User has been successfully updated.");
                return Ok(new { message = "Usuário atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error on register new user.");
                return BadRequest("Erro: " + ex.Message);
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                User? userDb = _context.Users.Find(id);
                if (userDb == null) throw new Exception("Usuário não encontrado.");

                UserView userView = new UserView
                {
                    IdUser = userDb.Id,
                    UserName = userDb.Username,
                    DtRegistration = userDb.RegisterDate.ToString("dd/MM/yyyy"),
                    UserEmail = userDb.Email
                };

                return Ok(userView);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Auth")]
        public async Task<IActionResult> AuthUser()
        {
            //Autenticação com baerer
            return Ok("Autenticação de usuário em construção.");
        }

        //Fazer rota para recuperação de senha
    }
}
