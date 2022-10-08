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

                user.RegisterDate = DateTime.Now;

                _userManagerRepository.Create(user);

                _logger.LogInformation($"Usuário [{user.Username}] criado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar o usuário.");
                throw;
            }
        }

        public void Update(User user)
        {
            try
            {
                ValidateUser(user);

                _logger.LogInformation($"Atualizando o Usuário Id [{user.Id}].");

                _userManagerRepository.Update(user);

                _logger.LogInformation($"Usuário Id [{user.Id}] atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o usuário.");
                throw;
            }
        }

        public User Get(int id)
        {
            try
            {
                if (id < 0)
                    throw new ArgumentOutOfRangeException(nameof(id), $"Id [{id}] do usuário inválido.");

                _logger.LogInformation($"Consultando dados do usuário Id [{id}] na base de dados.");

                var user = _userManagerRepository.Get(id);

                if (user.Id > 0)
                {
                    _logger.LogInformation($"Usuário Id [{id}] encontrado. Retornando resultado.");
                    return user;
                }

                var errorMessage = $"Usuário com Id [{id}] não encontrado.";

                _logger.LogInformation(errorMessage);
                throw new EntityNotFoundException(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Erro ao consultar o usuário com Id [{id}].");
                throw;
            }
        }

        public List<User> GetAll()
        {
            try
            {
                _logger.LogInformation($"Buscando os usuários na base de dados.");

                var result = _userManagerRepository.GetAll().ToList();

                if (result.Count > 0)
                {
                    _logger.LogInformation("Retornando os usuários encontrados.");
                    return result;
                }

                var errorMessage = "Nenhum Usuário encontrado.";

                _logger.LogInformation(errorMessage);
                throw new EntityNotFoundException(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Erro ao buscar os Usuários.");
                throw;
            }
        }

        private void ValidateUser(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "Usuário nulo.");
        }
    }
}