using System;
using System.Collections.Generic;

namespace DoAnWebService.Models;

public partial class Lop
{
    public string Malop { get; set; } = null!;

    public string Tenlop { get; set; } = null!;

    public string Khoahoc { get; set; } = null!;

    public string Makhoa { get; set; } = null!;

    public string Manv { get; set; } = null!;

    public virtual Khoa MakhoaNavigation { get; set; } = null!;

    public virtual Nhanvien ManvNavigation { get; set; } = null!;

    public virtual ICollection<Sinhvien> Sinhviens { get; set; } = new List<Sinhvien>();
}
