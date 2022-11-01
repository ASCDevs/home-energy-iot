using home_energy_iot_core.Exceptions;
using home_energy_iot_core.Helpers.Interfaces;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    public class UserManager : IUserManager
    {
        private ILogger<UserManager> _logger;
        private IHasher _hasher;

        private IUserManagerRepository _userManagerRepository;

        public UserManager(ILogger<UserManager> logger, IHasher hasher, IUserManagerRepository userManagerRepository)
        {
            _logger = logger;
            _hasher = hasher;
            _userManagerRepository = userManagerRepository;
        }

        public void Create(User user)
        {
            try
            {
                ValidateUser(user);

                _logger.LogInformation($"Criando Usuário: [{user.Username}].");
                _logger.LogInformation("Iniciando a geração do Salt da senha.");

                user.SaltPassword = _hasher.CreateSalt(20);
                user.Password = _hasher.GenerateHash(user.Password, user.SaltPassword);

                user.RegisterDate = DateTime.UtcNow.AddHours(-3);

                _userManagerRepository.Create(user);

                _logger.LogInformation($"Usuário [{user.Username}] criado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar o usuário.");
                throw;
            }
        }

        public User Get(int id)
        {
            try
            {
                ValidateUserId(id);

                _logger.LogInformation($"Consultando dados do usuário Id [{id}] na base de dados.");

                var user = _userManagerRepository.Get(id);

                if (user?.Id > 0)
                {
                    _logger.LogInformation($"Usuário Id [{id}] encontrado. Retornando resultado.");
                    return user;
                }

                throw new EntityNotFoundException($"Usuário com Id [{id}] não encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao consultar o usuário com Id [{id}].");
                throw;
            }
        }

        public User GetByUsername(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentNullException(nameof(username), "Nome do usuário nulo ou vazio.");

                _logger.LogInformation($"Buscando o usuário com Username [{username}].");

                var user = _userManagerRepository.GetByUsername(username);

                if (user?.Id > 0)
                    return user;

                throw new EntityNotFoundException($"Usuário com Username [{username}] não encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao consultar o usuário [{username}].");
                throw;
            }
        }

        private void ValidateUserId(int id)
        {
            if (id <= 0)
                throw new InvalidEntityNumericValueException($"Id [{id}] do usuário inválido.");
        }

        private void ValidateUser(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "Usuário nulo.");

            if (string.IsNullOrWhiteSpace(user.Name))
                throw new InvalidEntityTextValueException("Nome do usuário vazio ou nulo.");

            if (string.IsNullOrWhiteSpace(user.Username))
                throw new InvalidEntityTextValueException("Username do usuário vazio ou nulo.");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new InvalidEntityTextValueException("Senha do usuário vazia ou nula.");

            if (string.IsNullOrWhiteSpace(user.CPF))
                throw new InvalidEntityTextValueException("CPF do usuário vazio ou nulo.");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new InvalidEntityTextValueException("Email do usuário vazio ou nulo.");
        }
    }
}