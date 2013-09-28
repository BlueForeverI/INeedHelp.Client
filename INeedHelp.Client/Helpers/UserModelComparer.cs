using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INeedHelp.Client.Models;

namespace INeedHelp.Client.Helpers
{
    public class UserModelComparer : IEqualityComparer<UserModel>
    {
        public bool Equals(UserModel x, UserModel y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(UserModel user)
        {
            return user.Id.GetHashCode();
        }
    }
}
