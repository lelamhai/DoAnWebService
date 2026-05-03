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

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<CTDonghocphi> CtDonghocphis { get; set; }

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

    public virtual DbSet<Token> Tokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__ACCOUNT__05B22F6024D803B8");

            entity.ToTable("ACCOUNT");

            entity.HasIndex(e => e.Username, "UQ__ACCOUNT__B15BE12E3CE8049B").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("ACCOUNT_ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ROLE");
            entity.Property(e => e.Username)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
        });

        modelBuilder.Entity<CTDonghocphi>(entity =>
        {
            entity.HasKey(e => new { e.Masv, e.Nienkhoa, e.Hocky, e.Ngaydong }).HasName("PK_CTHOCPHI");

            entity.ToTable("CT_DONGHOCPHI");

            entity.Property(e => e.Masv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MASV");
            entity.Property(e => e.Nienkhoa)
                .HasMaxLength(9)
                .IsFixedLength()
                .HasColumnName("NIENKHOA");
            entity.Property(e => e.Hocky).HasColumnName("HOCKY");
            entity.Property(e => e.Ngaydong).HasColumnName("NGAYDONG");
            entity.Property(e => e.Sotiendong).HasColumnName("SOTIENDONG");

            entity.HasOne(d => d.Donghocphi).WithMany(p => p.CtDonghocphis)
                .HasForeignKey(d => new { d.Masv, d.Nienkhoa, d.Hocky })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDONGHOCPHI_HOCPHI");
        });

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
            entity.Property(e => e.Huydangky)
                .HasDefaultValue(false)
                .HasColumnName("HUYDANGKY");
            entity.Property(e => e.Xeploai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("XEPLOAI");

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

            entity.HasOne(d => d.MasvNavigation).WithMany(p => p.Donghocphis)
                .HasForeignKey(d => d.Masv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DONGHOCPHI_SINHVIEN");
        });

        modelBuilder.Entity<Giangvien>(entity =>
        {
            entity.HasKey(e => e.Magv).HasName("PK__GIANGVIE__603F38B18439182A");

            entity.ToTable("GIANGVIEN");

            entity.HasIndex(e => e.Email, "UQ__GIANGVIE__161CF724AEA808A1").IsUnique();

            entity.Property(e => e.Magv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAGV");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AVATAR");
            entity.Property(e => e.Chuyenmon)
                .HasMaxLength(50)
                .HasColumnName("CHUYENMON");
            entity.Property(e => e.Dangday)
                .HasDefaultValue(true)
                .HasColumnName("DANGDAY");
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

            entity.HasOne(d => d.MakhoaNavigation).WithMany(p => p.Giangviens)
                .HasForeignKey(d => d.Makhoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GIANGVIEN_KHOA");
        });

        modelBuilder.Entity<Khoa>(entity =>
        {
            entity.HasKey(e => e.Makhoa).HasName("PK__KHOA__22F417700A537E8E");

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
            entity.HasKey(e => e.Maloainv).HasName("PK__LOAINHAN__AFCB36C640099D79");

            entity.ToTable("LOAINHANVIEN");

            entity.Property(e => e.Maloainv).HasColumnName("MALOAINV");
            entity.Property(e => e.Ten)
                .HasMaxLength(100)
                .HasColumnName("TEN");
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.Malop).HasName("PK__LOP__7A3DE211BBFCDEEF");

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
            entity.Property(e => e.Tenlop)
                .HasMaxLength(50)
                .HasColumnName("TENLOP");

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
            entity.HasKey(e => e.Maltc).HasName("PK__LOPTINCH__7A3D3BC6C2310B46");

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
            entity.Property(e => e.SisoHientai).HasColumnName("SISO_HIENTAI");
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
            entity.HasKey(e => e.Mamh).HasName("PK__MONHOC__603F69EBE2F947A1");

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
            entity.HasKey(e => e.Manv).HasName("PK__NHANVIEN__603F5114EF0890F8");

            entity.ToTable("NHANVIEN");

            entity.HasIndex(e => e.Email, "UQ__NHANVIEN__161CF7242D830D8A").IsUnique();

            entity.Property(e => e.Manv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MANV");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AVATAR");
            entity.Property(e => e.Danglam)
                .HasDefaultValue(true)
                .HasColumnName("DANGLAM");
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

            entity.HasOne(d => d.MaLoaiNvNavigation).WithMany(p => p.Nhanviens)
                .HasForeignKey(d => d.MaLoaiNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NHANVIEN_LOAINHANVIEN");
        });

        modelBuilder.Entity<Sinhvien>(entity =>
        {
            entity.HasKey(e => e.Masv).HasName("PK__SINHVIEN__60228A289213D4DE");

            entity.ToTable("SINHVIEN");

            entity.HasIndex(e => e.Email, "UQ__SINHVIEN__161CF7242D808803").IsUnique();

            entity.Property(e => e.Masv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MASV");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AVATAR");
            entity.Property(e => e.Danghoc)
                .HasDefaultValue(true)
                .HasColumnName("DANGHOC");
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

            entity.HasOne(d => d.MalopNavigation).WithMany(p => p.Sinhviens)
                .HasForeignKey(d => d.Malop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SINHVIEN_LOP");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__TOKEN__8481DA1E65C7DF69");

            entity.ToTable("TOKEN");

            entity.Property(e => e.TokenId).HasColumnName("TOKEN_ID");
            entity.Property(e => e.AccountId).HasColumnName("ACCOUNT_ID");
            entity.Property(e => e.Expiry).HasColumnName("EXPIRY");
            entity.Property(e => e.Token1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TOKEN");

            entity.HasOne(d => d.Account).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TOKEN_USER");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
