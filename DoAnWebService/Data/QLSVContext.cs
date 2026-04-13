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

    public virtual DbSet<CtDonghocphi> CtDonghocphis { get; set; }

    public virtual DbSet<Dangky> Dangkies { get; set; }

    public virtual DbSet<Giangvien> Giangviens { get; set; }

    public virtual DbSet<Hocphi> Hocphis { get; set; }

    public virtual DbSet<Khoa> Khoas { get; set; }

    public virtual DbSet<Loainhanvien> Loainhanviens { get; set; }

    public virtual DbSet<Lop> Lops { get; set; }

    public virtual DbSet<Loptinchi> Loptinchis { get; set; }

    public virtual DbSet<Monhoc> Monhocs { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Sinhvien> Sinhviens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CtDonghocphi>(entity =>
        {
            entity.HasKey(e => new { e.Masv, e.Nienkhoa, e.Hocky, e.Ngaydong });

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
            entity.Property(e => e.Ngaydong)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("NGAYDONG");
            entity.Property(e => e.Sotiendong).HasColumnName("SOTIENDONG");

            entity.HasOne(d => d.Hocphi).WithMany(p => p.CtDonghocphis)
                .HasForeignKey(d => new { d.Masv, d.Nienkhoa, d.Hocky })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CT_DONGHOCPHI_HOCPHI");
        });

        modelBuilder.Entity<Dangky>(entity =>
        {
            entity.HasKey(e => new { e.Maltc, e.Masv });

            entity.ToTable("DANGKY");

            entity.HasIndex(e => new { e.Maltc, e.Masv }, "UK_DANGKY").IsUnique();

            entity.Property(e => e.Maltc).HasColumnName("MALTC");
            entity.Property(e => e.Masv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MASV");
            entity.Property(e => e.DiemCc).HasColumnName("DIEM_CC");
            entity.Property(e => e.DiemCk).HasColumnName("DIEM_CK");
            entity.Property(e => e.DiemGk).HasColumnName("DIEM_GK");
            entity.Property(e => e.Huydangky).HasColumnName("HUYDANGKY");

            entity.HasOne(d => d.MaltcNavigation).WithMany(p => p.Dangkies)
                .HasForeignKey(d => d.Maltc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DANGKY_LOPTINCHI");

            entity.HasOne(d => d.MasvNavigation).WithMany(p => p.Dangkies)
                .HasForeignKey(d => d.Masv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTLTC_SINHVIEN");
        });

        modelBuilder.Entity<Giangvien>(entity =>
        {
            entity.HasKey(e => e.Magv);

            entity.ToTable("GIANGVIEN");

            entity.Property(e => e.Magv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAGV");
            entity.Property(e => e.Chuyenmon)
                .HasMaxLength(50)
                .HasColumnName("CHUYENMON");
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
            entity.Property(e => e.Ten)
                .HasMaxLength(10)
                .HasColumnName("TEN");

            entity.HasOne(d => d.MakhoaNavigation).WithMany(p => p.Giangviens)
                .HasForeignKey(d => d.Makhoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GIANGVIEN_KHOA");
        });

        modelBuilder.Entity<Hocphi>(entity =>
        {
            entity.HasKey(e => new { e.Masv, e.Nienkhoa, e.Hocky });

            entity.ToTable("HOCPHI");

            entity.Property(e => e.Masv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MASV");
            entity.Property(e => e.Nienkhoa)
                .HasMaxLength(9)
                .IsFixedLength()
                .HasColumnName("NIENKHOA");
            entity.Property(e => e.Hocky)
                .HasDefaultValue(1)
                .HasColumnName("HOCKY");
            entity.Property(e => e.Hocphi1)
                .HasDefaultValue(6000000)
                .HasColumnName("HOCPHI");

            entity.HasOne(d => d.MasvNavigation).WithMany(p => p.Hocphis)
                .HasForeignKey(d => d.Masv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOCPHI_SINHVIEN");
        });

        modelBuilder.Entity<Khoa>(entity =>
        {
            entity.HasKey(e => e.Makhoa);

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
            entity.HasKey(e => e.MaLoaiNv).HasName("PK__LOAINHAN__12252308EB15231F");

            entity.ToTable("LOAINHANVIEN");

            entity.HasIndex(e => e.TenLoaiNv, "UQ__LOAINHAN__F43523D197BA8297").IsUnique();

            entity.Property(e => e.MaLoaiNv).HasColumnName("MaLoaiNV");
            entity.Property(e => e.TenLoaiNv)
                .HasMaxLength(100)
                .HasColumnName("TenLoaiNV");
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.Malop);

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
            entity.Property(e => e.Tenlop)
                .HasMaxLength(50)
                .HasColumnName("TENLOP");

            entity.HasOne(d => d.MakhoaNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.Makhoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LOP_KHOA");
        });

        modelBuilder.Entity<Loptinchi>(entity =>
        {
            entity.HasKey(e => e.Maltc);

            entity.ToTable("LOPTINCHI");

            entity.HasIndex(e => new { e.Nienkhoa, e.Hocky, e.Mamh, e.Nhom }, "UK_LOPTINCHI").IsUnique();

            entity.Property(e => e.Maltc).HasColumnName("MALTC");
            entity.Property(e => e.Hocky).HasColumnName("HOCKY");
            entity.Property(e => e.Huylop).HasColumnName("HUYLOP");
            entity.Property(e => e.Magv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAGV");
            entity.Property(e => e.Makhoa)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAKHOA");
            entity.Property(e => e.Mamh)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAMH");
            entity.Property(e => e.Nhom).HasColumnName("NHOM");
            entity.Property(e => e.Nienkhoa)
                .HasMaxLength(9)
                .IsFixedLength()
                .HasColumnName("NIENKHOA");
            entity.Property(e => e.Sosvtoithieu).HasColumnName("SOSVTOITHIEU");

            entity.HasOne(d => d.MagvNavigation).WithMany(p => p.Loptinchis)
                .HasForeignKey(d => d.Magv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LOPTINCHI_GIANGVIEN");

            entity.HasOne(d => d.MakhoaNavigation).WithMany(p => p.Loptinchis)
                .HasForeignKey(d => d.Makhoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LOPTINCHI_KHOA");

            entity.HasOne(d => d.MamhNavigation).WithMany(p => p.Loptinchis)
                .HasForeignKey(d => d.Mamh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LOPTINCHI_MONHOC");
        });

        modelBuilder.Entity<Monhoc>(entity =>
        {
            entity.HasKey(e => e.Mamh);

            entity.ToTable("MONHOC");

            entity.Property(e => e.Mamh)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MAMH");
            entity.Property(e => e.SotietLt).HasColumnName("SOTIET_LT");
            entity.Property(e => e.SotietTh).HasColumnName("SOTIET_TH");
            entity.Property(e => e.Tenmh)
                .HasMaxLength(50)
                .HasColumnName("TENMH");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.Manv).HasName("PK__NHANVIEN__603F5114DE9F3E83");

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.Manv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MANV");
            entity.Property(e => e.Danglam)
                .HasDefaultValue(true)
                .HasColumnName("DANGLAM");
            entity.Property(e => e.Diachi)
                .HasMaxLength(100)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Ho)
                .HasMaxLength(50)
                .HasColumnName("HO");
            entity.Property(e => e.MaLoaiNv).HasColumnName("MaLoaiNV");
            entity.Property(e => e.Ngaysinh).HasColumnName("NGAYSINH");
            entity.Property(e => e.Password)
                .HasMaxLength(40)
                .HasDefaultValue("")
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Phai)
                .HasDefaultValue(true)
                .HasColumnName("PHAI");
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
            entity.HasKey(e => e.Masv);

            entity.ToTable("SINHVIEN");

            entity.HasIndex(e => e.Malop, "IX_MALOP");

            entity.Property(e => e.Masv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MASV");
            entity.Property(e => e.Danghihoc).HasColumnName("DANGHIHOC");
            entity.Property(e => e.Diachi)
                .HasMaxLength(100)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Ho)
                .HasMaxLength(50)
                .HasColumnName("HO");
            entity.Property(e => e.Malop)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MALOP");
            entity.Property(e => e.Ngaysinh).HasColumnName("NGAYSINH");
            entity.Property(e => e.Password)
                .HasMaxLength(40)
                .HasDefaultValue("")
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Phai).HasColumnName("PHAI");
            entity.Property(e => e.Ten)
                .HasMaxLength(10)
                .HasColumnName("TEN");

            entity.HasOne(d => d.MalopNavigation).WithMany(p => p.Sinhviens)
                .HasForeignKey(d => d.Malop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SINHVIEN_LOP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
