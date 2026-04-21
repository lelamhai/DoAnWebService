using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DoAnWebService.Models;

public partial class Dangky
{
    public int Maltc { get; set; }

    public string Masv { get; set; } = null!;

    public int? DiemCc { get; set; }

    public double? DiemGk { get; set; }

    public double? DiemCk { get; set; }

    public string? Xeploai { get; set; }

    public bool? Huydangky { get; set; }
    [JsonIgnore]
    public virtual Loptinchi MaltcNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Sinhvien MasvNavigation { get; set; } = null!;
}
