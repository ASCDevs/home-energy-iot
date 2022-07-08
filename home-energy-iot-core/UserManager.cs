using System;
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

        public async Task UpdateUser(User user)
        {
            try
            {
                if (user is null)
                    throw new ArgumentNullException(nameof(user), "Usuário nulo.");

                _logger.LogInformation("Atualizando o usuário na base de dados.");

                _context.Users.Update(user);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Usuário atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o usuário.");
                throw;
            }
        }

        public Task ChangePassword(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(int id)
        {
            try
            {
                if(id < 0)
                    throw new ArgumentOutOfRangeException(nameof(id), "Id do usuário inválido.");

                _logger.LogInformation($"Consultando dados do usuário Id [{id}] na base de dados.");

                var result = _context.Users.Find(id);

                if(result != null)
                    return Task.FromResult(result);

                var errorMessage = $"Usuário com Id [{id}] não encontrado.";

                _logger.LogInformation(errorMessage);
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Erro ao consultar o usuário.");
                throw;
            }
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            try
            {
                _logger.LogInformation($"Consultando os usuários da base de dados.");

                var result = _context.Users.ToList();

                if(result.Count > 0)
                    return Task.FromResult<IEnumerable<User>>(result);

                var errorMessage = "Usuários não encontrados.";

                _logger.LogInformation(errorMessage);
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Erro ao consultar os usuários.");
                throw;
            }
        }
    }
}