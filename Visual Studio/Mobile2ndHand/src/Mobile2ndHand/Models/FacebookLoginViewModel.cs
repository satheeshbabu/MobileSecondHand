﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileSecondHand.Models
{
    public class FacebookLoginViewModel
    {
        public string UserName { get; set; }
		public string Email { get; set; }
        public string FacebookAccessToken { get; set; }
    }
}
