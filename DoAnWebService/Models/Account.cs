using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DoAnWebService.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool Active { get; set; }
    [JsonIgnore]
    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
