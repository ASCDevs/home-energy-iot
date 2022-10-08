using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;

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

        public void Update(User user)
        {
            _dataBaseContext.Users.Update(user);
            _dataBaseContext.SaveChanges();
        }

        public async Task ChangePassword(User user)
        {
            throw new NotImplementedException();
        }

        public User Get(int id)
        {
            var user = _dataBaseContext.Users.Find(id);

            return user ?? new User();
        }

        public List<User> GetAll()
        {
            return _dataBaseContext.Users.ToList();
        }
    }
}