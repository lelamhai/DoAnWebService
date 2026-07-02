using System;
using System.Collections.Generic;
using DoAnWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebService.Data;

public partial class QLSVContext : DbContext
{
    public QLSVContext()
    {
    }

    public QLSVContext(DbContextOptions<QLSVContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dangky> Dangkies { get; set; }

    public virtual DbSet<Donghocphi> Donghocphis { get; set; }

    public virtual DbSet<Giangvien> Giangviens { get; set; }

    public virtual DbSet<Khoa> Khoas { get; set; }

    public virtual DbSet<Loainhanvien> Loainhanviens { get; set; }

    public virtual DbSet<Lop> Lops { get; set; }

    public virtual DbSet<Loptinchi> Loptinchis { get; set; }

    public virtual DbSet<Monhoc> Monhocs { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Sinhvien> Sinhviens { get; set; }

    public virtual DbSet<Trangthaigiangvien> Trangthaigiangviens { get; set; }

    public virtual DbSet<Trangthailop> Trangthailops { get; set; }

    public virtual DbSet<Trangthainhanvien> Trangthainhanviens { get; set; }

    public virtual DbSet<Trangthaisinhvien> Trangthaisinhviens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dangky>(entity =>
        {
            entity.HasKey(e => new { e.Maltc, e.Masv });

            entity.ToTable("DANGKY");

            entity.Property(e => e.Maltc).HasColumnName("MALTC");
            entity.Property(e => e.Masv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MASV");
            entity.Property(e => e.DiemCc).HasColumnName("DIEM_CC");
            entity.Property(e => e.DiemCk).HasColumnName("DIEM_CK");
            entity.Property(e => e.DiemGk).HasColumnName("DIEM_GK");
            entity.Property(e => e.Hocky).HasColumnName("HOCKY");
            entity.Property(e => e.Huydangky)
                .HasDefaultValue(false)
                .HasColumnName("HUYDANGKY");
            entity.Property(e => e.Nienkhoa)
                .HasMaxLength(9)
                .IsFixedLength()
                .HasColumnName("NIENKHOA");

            entity.HasOne(d => d.MaltcNavigation).WithMany(p => p.Dangkies)
                .HasForeignKey(d => d.Maltc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DANGKY_LOPTINCHI");

            entity.HasOne(d => d.MasvNavigation).WithMany(p => p.Dangkies)
                .HasForeignKey(d => d.Masv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DANGKY_SINHVIEN");
        });

        modelBuilder.Entity<Donghocphi>(entity =>
        {
            entity.HasKey(e => new { e.Masv, e.Nienkhoa, e.Hocky });

            entity.ToTable("DONGHOCPHI");

            entity.Property(e => e.Masv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MASV");
            entity.Property(e => e.Nienkhoa)
                .HasMaxLength(9)
                .IsFixedLength()
                .HasColumnName("NIENKHOA");
            entity.Property(e => e.Hocky).HasColumnName("HOCKY");
            entity.Property(e => e.Hocphi).HasColumnName("HOCPHI");
            entity.Property(e => e.Ngaydong).HasColumnName("NGAYDONG");

            entity.HasOne(d => d.MasvNavigation).WithMany(p => p.Donghocphis)
                .HasForeignKey(d => d.Masv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DONGHOCPHI_SINHVIEN");
        });

        modelBuilder.Entity<Giangvien>(entity =>
        {
            entity.HasKey(e => e.Magv).HasName("PK__GIANGVIE__603F38B1F1929780");

            entity.ToTable("GIANGVIEN");

            entity.HasIndex(e => e.Email, "UQ__GIANGVIE__161CF7247DD7418C").IsUnique();

            entity.Property(e => e.Magv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAGV");
            entity.Property(e => e.Chuyenmon)
                .HasMaxLength(50)
                .HasColumnName("CHUYENMON");
            entity.Property(e => e.Diachi)
                .HasMaxLength(100)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Ho)
                .HasMaxLength(50)
                .HasColumnName("HO");
            entity.Property(e => e.Hocham)
                .HasMaxLength(20)
                .HasColumnName("HOCHAM");
            entity.Property(e => e.Hocvi)
                .HasMaxLength(20)
                .HasColumnName("HOCVI");
            entity.Property(e => e.Makhoa)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAKHOA");
            entity.Property(e => e.Ngaysinh).HasColumnName("NGAYSINH");
            entity.Property(e => e.Phai)
                .HasDefaultValue(true)
                .HasColumnName("PHAI");
            entity.Property(e => e.Sodienthoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SODIENTHOAI");
            entity.Property(e => e.Ten)
                .HasMaxLength(50)
                .HasColumnName("TEN");
            entity.Property(e => e.Trangthai)
                .HasDefaultValue(1)
                .HasColumnName("TRANGTHAI");

            entity.HasOne(d => d.MakhoaNavigation).WithMany(p => p.Giangviens)
                .HasForeignKey(d => d.Makhoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GIANGVIEN_KHOA");
        });

        modelBuilder.Entity<Khoa>(entity =>
        {
            entity.HasKey(e => e.Makhoa).HasName("PK__KHOA__22F41770040BA72A");

            entity.ToTable("KHOA");

            entity.Property(e => e.Makhoa)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAKHOA");
            entity.Property(e => e.Tenkhoa)
                .HasMaxLength(50)
                .HasColumnName("TENKHOA");
        });

        modelBuilder.Entity<Loainhanvien>(entity =>
        {
            entity.HasKey(e => e.Maloainv).HasName("PK__LOAINHAN__AFCB36C6840CDB93");

            entity.ToTable("LOAINHANVIEN");

            entity.Property(e => e.Maloainv).HasColumnName("MALOAINV");
            entity.Property(e => e.Ten)
                .HasMaxLength(100)
                .HasColumnName("TEN");
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.Malop).HasName("PK__LOP__7A3DE211CCEEBABA");

            entity.ToTable("LOP");

            entity.Property(e => e.Malop)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MALOP");
            entity.Property(e => e.Khoahoc)
                .HasMaxLength(9)
                .IsFixedLength()
                .HasColumnName("KHOAHOC");
            entity.Property(e => e.Makhoa)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAKHOA");
            entity.Property(e => e.Manv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MANV");
            entity.Property(e => e.Ngaymolop).HasColumnName("NGAYMOLOP");
            entity.Property(e => e.Tenlop)
                .HasMaxLength(50)
                .HasColumnName("TENLOP");
            entity.Property(e => e.Trangthai)
                .HasDefaultValue(1)
                .HasColumnName("TRANGTHAI");

            entity.HasOne(d => d.MakhoaNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.Makhoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LOP_KHOA");

            entity.HasOne(d => d.ManvNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.Manv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LOP_NHANVIEN");
        });

        modelBuilder.Entity<Loptinchi>(entity =>
        {
            entity.HasKey(e => e.Maltc).HasName("PK__LOPTINCH__7A3D3BC6D772CA0A");

            entity.ToTable("LOPTINCHI");

            entity.Property(e => e.Maltc).HasColumnName("MALTC");
            entity.Property(e => e.DayThutrongtuan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DAY_THUTRONGTUAN");
            entity.Property(e => e.Hocky).HasColumnName("HOCKY");
            entity.Property(e => e.Huylop).HasColumnName("HUYLOP");
            entity.Property(e => e.Magv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAGV");
            entity.Property(e => e.Mamh)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAMH");
            entity.Property(e => e.Nienkhoa)
                .HasMaxLength(9)
                .IsFixedLength()
                .HasColumnName("NIENKHOA");
            entity.Property(e => e.SisoToida).HasColumnName("SISO_TOIDA");
            entity.Property(e => e.ThoigianBatdau).HasColumnName("THOIGIAN_BATDAU");
            entity.Property(e => e.ThoigianKetthuc).HasColumnName("THOIGIAN_KETTHUC");

            entity.HasOne(d => d.MagvNavigation).WithMany(p => p.Loptinchis)
                .HasForeignKey(d => d.Magv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LOPTINCHI_GIANGVIEN");

            entity.HasOne(d => d.MamhNavigation).WithMany(p => p.Loptinchis)
                .HasForeignKey(d => d.Mamh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LOPTINCHI_MONHOC");
        });

        modelBuilder.Entity<Monhoc>(entity =>
        {
            entity.HasKey(e => e.Mamh).HasName("PK__MONHOC__603F69EB99A79DE4");

            entity.ToTable("MONHOC");

            entity.Property(e => e.Mamh)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAMH");
            entity.Property(e => e.Makhoa)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAKHOA");
            entity.Property(e => e.SotietLt).HasColumnName("SOTIET_LT");
            entity.Property(e => e.SotietTh).HasColumnName("SOTIET_TH");
            entity.Property(e => e.Sotinchi).HasColumnName("SOTINCHI");
            entity.Property(e => e.Tenmh)
                .HasMaxLength(50)
                .HasColumnName("TENMH");

            entity.HasOne(d => d.MakhoaNavigation).WithMany(p => p.Monhocs)
                .HasForeignKey(d => d.Makhoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MONHOC_KHOA");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.Manv).HasName("PK__NHANVIEN__603F51148A09AD76");

            entity.ToTable("NHANVIEN");

            entity.HasIndex(e => e.Email, "UQ__NHANVIEN__161CF7241CEC04F6").IsUnique();

            entity.Property(e => e.Manv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MANV");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AVATAR");
            entity.Property(e => e.Diachi)
                .HasMaxLength(100)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Ho)
                .HasMaxLength(50)
                .HasColumnName("HO");
            entity.Property(e => e.MaLoaiNv).HasColumnName("MaLoaiNV");
            entity.Property(e => e.Ngaysinh).HasColumnName("NGAYSINH");
            entity.Property(e => e.Phai)
                .HasDefaultValue(true)
                .HasColumnName("PHAI");
            entity.Property(e => e.Sodienthoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SODIENTHOAI");
            entity.Property(e => e.Ten)
                .HasMaxLength(50)
                .HasColumnName("TEN");
            entity.Property(e => e.Trangthai)
                .HasDefaultValue(1)
                .HasColumnName("TRANGTHAI");

            entity.HasOne(d => d.MaLoaiNvNavigation).WithMany(p => p.Nhanviens)
                .HasForeignKey(d => d.MaLoaiNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NHANVIEN_LOAINHANVIEN");
        });

        modelBuilder.Entity<Sinhvien>(entity =>
        {
            entity.HasKey(e => e.Masv).HasName("PK__SINHVIEN__60228A287C3F071A");

            entity.ToTable("SINHVIEN");

            entity.HasIndex(e => e.Email, "UQ__SINHVIEN__161CF7246F47C7A1").IsUnique();

            entity.Property(e => e.Masv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MASV");
            entity.Property(e => e.Diachi)
                .HasMaxLength(100)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Ho)
                .HasMaxLength(50)
                .HasColumnName("HO");
            entity.Property(e => e.Malop)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MALOP");
            entity.Property(e => e.Ngaysinh).HasColumnName("NGAYSINH");
            entity.Property(e => e.Phai)
                .HasDefaultValue(true)
                .HasColumnName("PHAI");
            entity.Property(e => e.Sodienthoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SODIENTHOAI");
            entity.Property(e => e.Ten)
                .HasMaxLength(50)
                .HasColumnName("TEN");
            entity.Property(e => e.Trangthai)
                .HasDefaultValue(1)
                .HasColumnName("TRANGTHAI");

            entity.HasOne(d => d.MalopNavigation).WithMany(p => p.Sinhviens)
                .HasForeignKey(d => d.Malop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SINHVIEN_LOP");
        });

        modelBuilder.Entity<Trangthaigiangvien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TRANGTHA__3214EC276D721663");

            entity.ToTable("TRANGTHAIGIANGVIEN");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Tentrangthai)
                .HasMaxLength(50)
                .HasColumnName("TENTRANGTHAI");
        });

        modelBuilder.Entity<Trangthailop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TRANGTHA__3214EC279805A7AA");

            entity.ToTable("TRANGTHAILOP");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Tentrangthai)
                .HasMaxLength(50)
                .HasColumnName("TENTRANGTHAI");
        });

        modelBuilder.Entity<Trangthainhanvien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TRANGTHA__3214EC274FBA88EB");

            entity.ToTable("TRANGTHAINHANVIEN");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Tentrangthai)
                .HasMaxLength(50)
                .HasColumnName("TENTRANGTHAI");
        });

        modelBuilder.Entity<Trangthaisinhvien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TRANGTHA__3214EC27AB57B605");

            entity.ToTable("TRANGTHAISINHVIEN");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Tentrangthai)
                .HasMaxLength(50)
                .HasColumnName("TENTRANGTHAI");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USERS__3214EC274A6A7CE4");

            entity.ToTable("USERS");

            entity.HasIndex(e => e.Username, "UQ__USERS__B15BE12E3774B6D8").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.Expiry)
                .HasColumnType("datetime")
                .HasColumnName("EXPIRY");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Refreshtoken)
                .HasMaxLength(255)
                .HasColumnName("REFRESHTOKEN");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ROLE");
            entity.Property(e => e.Username)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
