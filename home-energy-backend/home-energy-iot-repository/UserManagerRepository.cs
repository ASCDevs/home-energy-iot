using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace home_energy_iot_repository
{
    public class UserManagerRepository : IUserManagerRepository
    {
        private DataBaseContext _dataBaseContext;

        public UserManagerRepository(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        public async Task Create(User user)
        {
            await _dataBaseContext.Users.AddAsync(user);
            await _dataBaseContext.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _dataBaseContext.Users.Update(user);
            await _dataBaseContext.SaveChangesAsync();
        }

        public async Task ChangePassword(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Get(int id)
        {
            return _dataBaseContext.Users.Find(id);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return _dataBaseContext.Users.ToList();
        }
    }
}
