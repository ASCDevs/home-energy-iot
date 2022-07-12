﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using home_energy_iot_core.Helpers.Interfaces;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Login
{
    public class LoginService : ILoginService
    {
        private IHasher _hasher;

        private DataBaseContext _context;

        public LoginService(IHasher hasher, DataBaseContext context)
        {
            _hasher = hasher;
            _context = context;
        }

        public User GetUser(string username)
        {
            if(string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username), "Username inválido.");

            return _context.Users.First(u => u.Name == username);
        }

        public bool ValidPassword(string providedPassword, User user)
        {
            if (_hasher.GenerateHash(providedPassword, user.SaltPassword) == user.Password)
                return true;

            return false;
        }
    }
}