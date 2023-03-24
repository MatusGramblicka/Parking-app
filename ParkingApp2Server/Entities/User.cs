using Microsoft.AspNetCore.Identity;
using System;

namespace Entities;

public class User : IdentityUser
{
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public bool Priviledged { get; set; }
}