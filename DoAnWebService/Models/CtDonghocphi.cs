using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DoAnWebService.Models;

public partial class CTDonghocphi
{
    public string Masv { get; set; } = null!;

    public string Nienkhoa { get; set; } = null!;

    public int Hocky { get; set; }

    public DateOnly Ngaydong { get; set; }

    public int Sotiendong { get; set; }
    [JsonIgnore]
    public virtual Donghocphi Donghocphi { get; set; } = null!;
}
