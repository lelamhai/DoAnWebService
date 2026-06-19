using System;
using System.Collections.Generic;

namespace DoAnWebService.Models;

public partial class Sinhvien
{
    public string Masv { get; set; } = null!;

    public string Avatar { get; set; } = null!;

    public string Ho { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public bool Phai { get; set; }

    public string? Diachi { get; set; }

    public string? Sodienthoai { get; set; }

    public DateOnly? Ngaysinh { get; set; }

    public string? Email { get; set; }

    public string Malop { get; set; } = null!;

    public int Trangthai { get; set; }

    public virtual ICollection<Dangky> Dangkies { get; set; } = new List<Dangky>();

    public virtual ICollection<Donghocphi> Donghocphis { get; set; } = new List<Donghocphi>();

    public virtual Lop MalopNavigation { get; set; } = null!;
}
