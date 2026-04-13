using System;
using System.Collections.Generic;

namespace DoAnWebService.Models;

public partial class Nhanvien
{
    public string Manv { get; set; } = null!;

    public string Ho { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public bool Phai { get; set; }

    public string? Diachi { get; set; }

    public DateOnly? Ngaysinh { get; set; }

    public bool Danglam { get; set; }

    public int MaLoaiNv { get; set; }

    public string? Password { get; set; }

    public virtual Loainhanvien MaLoaiNvNavigation { get; set; } = null!;
}
