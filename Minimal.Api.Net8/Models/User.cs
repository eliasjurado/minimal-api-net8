﻿namespace Minimal.Api.Net8.Models
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
