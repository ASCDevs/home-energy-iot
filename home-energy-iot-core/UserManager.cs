using home_energy_iot_core.Helpers.Interfaces;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    public class UserManager : IUserManager
    {
        private ILogger<UserManager> _logger;
        private IHasher _hasher;

        private DataBaseContext _context;

        public UserManager(ILogger<UserManager> logger, IHasher hasher, DataBaseContext context)
        {
            _logger = logger;
            _hasher = hasher;
            _context = context;
        }

        public async Task CreateUser(User user)
        {
            try
            {
                if (user is null)
                    throw new ArgumentNullException(nameof(user), "Usuário nulo.");

                _logger.LogInformation("Iniciando a geração do Salt da senha.");

                user.SaltPassword = _hasher.CreateSalt(20);
                user.Password = _hasher.GenerateHash(user.Password, user.SaltPassword);

                _logger.LogInformation("Salvando o usuário na base de dados.");

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Usuário criado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar o usuário.");
                throw;
            }
        }
    }
}