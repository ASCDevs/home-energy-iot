using home_energy_iot_core.Interfaces;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    public class DeviceManager : IDeviceManager
    {
        private ILogger<DeviceManager> _logger;

        private DataBaseContext _context;

        public DeviceManager(ILogger<DeviceManager> logger, DataBaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Create(Device device)
        {
            try
            {
                ValidadeDevice(device);

                _logger.LogInformation($"Criando dispositivo: [{device.Name}]");

                await _context.Devices.AddAsync(device);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Dispositivo Id [{device.Id}] criado com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task Update(Device device)
        {
            try
            {
                ValidadeDevice(device);

                _logger.LogInformation($"Atualizando dispositivo Id [{device.Id}]");

                _context.Devices.Update(device);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Dispositivo Id [{device.Id}] atualizado com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task Delete(Device device)
        {
            try
            {
                ValidadeDevice(device);

                _logger.LogInformation($"Deletando dispositivo Id [{device.Id}]");

                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Dispositivo Id [{device.Id}] deletado com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<Device> Get(int id)
        {
            try
            {
                if(id < 0)
                    throw new ArgumentOutOfRangeException(nameof(id), $"Id do dispositivo inválido: [{id}]");

                _logger.LogInformation($"Consultando dados do dispotivivo com Id [{id}]");

                var device = _context.Devices.Find(id);

                if (device != null)
                    return Task.FromResult(device);

                var notFound = $"Dispositivo com o Id [{id}] não encontrado.";

                _logger.LogError(notFound);
                throw new Exception(notFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<IEnumerable<Device>> GetAll()
        {
            try
            {
                _logger.LogInformation("Buscando dispositivos na base de dados.");

                var devices = _context.Devices.ToList();

                if (devices.Count > 0)
                    return Task.FromResult<IEnumerable<Device>>(devices);

                var message = "Nenhum dispositivo encontrado.";

                _logger.LogInformation(message);
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private void ValidadeDevice(Device device)
        {
            if (device is null)
                throw new ArgumentNullException(nameof(device), "Dispositivo nulo");
        }
    }
}