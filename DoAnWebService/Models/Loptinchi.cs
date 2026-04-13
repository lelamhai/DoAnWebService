using System;
using System.Collections.Generic;

namespace DoAnWebService.Models;

public partial class Loptinchi
{
    public int Maltc { get; set; }

    public string Nienkhoa { get; set; } = null!;

    public int Hocky { get; set; }

    public string Mamh { get; set; } = null!;

    public int Nhom { get; set; }

    public string Magv { get; set; } = null!;

    public string Makhoa { get; set; } = null!;

    public int Sosvtoithieu { get; set; }

    public bool Huylop { get; set; }

    public virtual ICollection<Dangky> Dangkies { get; set; } = new List<Dangky>();

    public virtual Giangvien MagvNavigation { get; set; } = null!;

    public virtual Khoa MakhoaNavigation { get; set; } = null!;

    public virtual Monhoc MamhNavigation { get; set; } = null!;
}
