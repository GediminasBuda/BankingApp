﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models.ResponseModels
{
    public class SignInResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string IdToken { get; set; }

    }
}