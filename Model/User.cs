using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovik_Kocherzhenko.Model
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public List<FriendRequest> FriendRequests { get; set; }
        public List<Friend> Friends { get; set; }
    }
}
