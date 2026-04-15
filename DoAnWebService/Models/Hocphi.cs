namespace DoAnWebService.Models;

public partial class Hocphi
{
    public string Masv { get; set; } = null!;

    public string Nienkhoa { get; set; } = null!;

    public int Hocky { get; set; }

    public int Hocphi1 { get; set; }

    public virtual ICollection<CTDonghocphi> CtDonghocphis { get; set; } = new List<CTDonghocphi>();

    public virtual Sinhvien MasvNavigation { get; set; } = null!;
}
