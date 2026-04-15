namespace DoAnWebService.Models;

public partial class Giangvien
{
    public string Magv { get; set; } = null!;

    public string Makhoa { get; set; } = null!;

    public string Ho { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public string? Hocvi { get; set; }

    public string? Hocham { get; set; }

    public string? Chuyenmon { get; set; }

    public string? Email { get; set; }

    public bool Dangday { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Loptinchi> Loptinchis { get; set; } = new List<Loptinchi>();

    public virtual Khoa MakhoaNavigation { get; set; } = null!;
}
