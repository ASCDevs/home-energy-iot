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
        void Create(User user);
        User Get(int id);
        User GetByUsername(string username);
    }
}