using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DoAnWebService.Models;

public partial class Token
{
    public int TokenId { get; set; }

    public string Token1 { get; set; } = null!;

    public DateOnly Expiry { get; set; }

    public int AccountId { get; set; }

    [JsonIgnore]
    public virtual Account Account { get; set; } = null!;
}
