using System;
using System.Collections.Generic;

namespace DoAnWebService.Models;

public partial class Monhoc
{
    public string Mamh { get; set; } = null!;

    public string Tenmh { get; set; } = null!;

    public int SotietLt { get; set; }

    public int SotietTh { get; set; }

    public int? Sotinchi { get; set; }

    public string Makhoa { get; set; } = null!;

    public virtual ICollection<Loptinchi> Loptinchis { get; set; } = new List<Loptinchi>();

    public virtual Khoa MakhoaNavigation { get; set; } = null!;
}
