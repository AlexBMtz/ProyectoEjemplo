﻿using Microsoft.AspNetCore.Identity;

namespace SharedModels.Models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
