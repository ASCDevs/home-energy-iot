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

                    _logger.LogInformation("New user has been successfully added.");
                    return Ok(new { message = "Usuário cadastrado com sucesso!" });
                }
                //Update User
                else
                {
                    _logger.LogInformation("Updating an User.");
                    User userUpdate = new User
                    {
                        IdUser = registerForm.IdUser,
                        Password = registerForm.Password,
                        UserCPF = registerForm.UserCPF,
                        UserEmail = registerForm.UserEmail,
                        UserName = registerForm.UserName,
                        DtInactivation = null
                    };

                    
                    if (_context.users.Update(userUpdate) == null) throw new Exception("Erro ao atualizar o usuário.");
                    _context.SaveChanges();

                    _logger.LogInformation("User has been successfully updated.");
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
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                User? userDb = _context.users.Find(id);
                if (userDb == null) throw new Exception("Usuário não encontrado.");

                UserView userView = new UserView
                {
                    IdUser = userDb.IdUser,
                    UserName = userDb.UserName,
                    DtRegistration = userDb.DtRegistration.ToString("dd/MM/yyyy"),
                    UserEmail = userDb.UserEmail
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
