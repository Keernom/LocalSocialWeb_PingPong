﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovik_Kocherzhenko.Model
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GetterId { get; set; }
        public User User { get; set; }
    }
}
