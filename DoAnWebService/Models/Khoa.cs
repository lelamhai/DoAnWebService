namespace DoAnWebService.Models;

public partial class Khoa
{
    public string Makhoa { get; set; } = null!;

    public string Tenkhoa { get; set; } = null!;

    public virtual ICollection<Giangvien> Giangviens { get; set; } = new List<Giangvien>();

    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();

    public virtual ICollection<Monhoc> Monhocs { get; set; } = new List<Monhoc>();
}
