using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INeedHelp.Client.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string SessionKey { get; set; }
    }
}
