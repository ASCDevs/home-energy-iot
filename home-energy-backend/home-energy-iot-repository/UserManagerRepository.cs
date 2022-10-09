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

        public void Create(User user)
        {
            _dataBaseContext.Users.Add(user);
            _dataBaseContext.SaveChanges();
        }

        public User Get(int id)
        {
            var user = _dataBaseContext.Users.Find(id);

            return user ?? new User();
        }

        public User GetByUsername(string username)
        {
            var user = _dataBaseContext.Users.FirstOrDefault(u => u.Username == username);

            return user ?? new User();
        }
    }
}