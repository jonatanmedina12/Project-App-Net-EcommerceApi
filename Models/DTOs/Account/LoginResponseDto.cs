﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Account
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
        public DateTime TokenExpiration { get; set; }
        public Boolean IsLoginActivo { get; set; }

    }
}
