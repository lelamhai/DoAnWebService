using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DoAnWebService.Models;

public partial class Khoa
{
    public string Makhoa { get; set; } = null!;

    public string Tenkhoa { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Giangvien> Giangviens { get; set; } = new List<Giangvien>();
    [JsonIgnore]
    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();
    [JsonIgnore]
    public virtual ICollection<Monhoc> Monhocs { get; set; } = new List<Monhoc>();
}
