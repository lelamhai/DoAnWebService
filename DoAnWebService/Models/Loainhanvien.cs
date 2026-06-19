using System;
using System.Collections.Generic;

namespace DoAnWebService.Models;

public partial class Loainhanvien
{
    public int Maloainv { get; set; }

    public string Ten { get; set; } = null!;

    public virtual ICollection<Nhanvien> Nhanviens { get; set; } = new List<Nhanvien>();
}
