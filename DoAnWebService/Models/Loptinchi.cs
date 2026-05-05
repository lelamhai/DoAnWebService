using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DoAnWebService.Models;

public partial class Loptinchi
{
    public int Maltc { get; set; }

    public string Nienkhoa { get; set; } = null!;

    public int Hocky { get; set; }

    public string Mamh { get; set; } = null!;

    public string Magv { get; set; } = null!;

    public int SisoToida { get; set; }

    public int SisoHientai { get; set; }

    public string? DayThutrongtuan { get; set; }

    public DateOnly? ThoigianBatdau { get; set; }

    public DateOnly? ThoigianKetthuc { get; set; }

    public bool Huylop { get; set; }
    [JsonIgnore]
    public virtual ICollection<Dangky> Dangkies { get; set; } = new List<Dangky>();
    [JsonIgnore]
    public virtual Giangvien MagvNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Monhoc MamhNavigation { get; set; } = null!;
}
