using System;
using System.Collections.Generic;

namespace DoAnWebService.Models;

public partial class Dangky
{
    public int Maltc { get; set; }

    public string Masv { get; set; } = null!;

    public string Nienkhoa { get; set; } = null!;

    public int Hocky { get; set; }

    public int? DiemCc { get; set; }

    public double? DiemGk { get; set; }

    public double? DiemCk { get; set; }

    public bool? Huydangky { get; set; }

    public virtual Loptinchi MaltcNavigation { get; set; } = null!;

    public virtual Sinhvien MasvNavigation { get; set; } = null!;
}
