using home_energy_iot_entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace home_energy_iot_repository.Interfaces
{
    public interface IUserManagerRepository
    {
        Task Create(User user);

        Task Update(User user);

        Task ChangePassword(User user);

        Task<User> Get(int id);

        Task<IEnumerable<User>> GetAll();
    }
}
