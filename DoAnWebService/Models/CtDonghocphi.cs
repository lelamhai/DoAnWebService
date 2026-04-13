using System;
using System.Collections.Generic;

namespace DoAnWebService.Models;

public partial class CtDonghocphi
{
    public string Masv { get; set; } = null!;

    public string Nienkhoa { get; set; } = null!;

    public int Hocky { get; set; }

    public DateOnly Ngaydong { get; set; }

    public int Sotiendong { get; set; }

    public virtual Hocphi Hocphi { get; set; } = null!;
}
