namespace DoAnWebService.Models;

public partial class Loainhanvien
{
    public int MaLoaiNv { get; set; }

    public string TenLoaiNv { get; set; } = null!;

    public virtual ICollection<Nhanvien> Nhanviens { get; set; } = new List<Nhanvien>();
}
