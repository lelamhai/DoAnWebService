using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DoAnWebService.Models;

public partial class Donghocphi
{
    public string Masv { get; set; } = null!;

    public string Nienkhoa { get; set; } = null!;

    public int Hocky { get; set; }

    public int Hocphi { get; set; }
    [JsonIgnore]
    public virtual ICollection<CTDonghocphi> CtDonghocphis { get; set; } = new List<CTDonghocphi>();
    [JsonIgnore]
    public virtual Sinhvien MasvNavigation { get; set; } = null!;
}
