using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DoAnWebService.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }
    
    public bool Active { get; set; }
    [JsonIgnore]
    public virtual Role Role { get; set; } = null!;
}
