using System;
using System.Collections.Generic;

namespace DoAnWebService.Models;

public partial class Donghocphi
{
    public string Masv { get; set; } = null!;

    public string Nienkhoa { get; set; } = null!;

    public int Hocky { get; set; }

    public int Hocphi { get; set; }

    public DateOnly? Ngaydong { get; set; }

    public virtual Sinhvien MasvNavigation { get; set; } = null!;
}
