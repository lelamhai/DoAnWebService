using System;
using System.Collections.Generic;

namespace DoAnWebService.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Refreshtoken { get; set; }

    public DateTime? Expiry { get; set; }

    public string Role { get; set; } = null!;
}
