using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DoAnWebService.Models;

public partial class Loainhanvien
{
    public int Maloainv { get; set; }

    public string Ten { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Nhanvien> Nhanviens { get; set; } = new List<Nhanvien>();
}
