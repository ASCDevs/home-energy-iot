using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace home_energy_iot_core.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException(string msg) : base(msg)
        {
        }
    }
}
