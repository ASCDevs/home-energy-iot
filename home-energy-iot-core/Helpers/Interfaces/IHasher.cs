using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace home_energy_iot_core.Helpers.Interfaces
{
    public interface IHasher
    {
        string GenerateHash(string input, string salt);
        string CreateSalt(int saltSize);
    }
}
