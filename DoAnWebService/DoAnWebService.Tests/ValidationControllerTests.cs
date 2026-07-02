using DoAnWebService.Controllers;
using DoAnWebService.DTO.Classroom;
using DoAnWebService.DTO.Employment;
using DoAnWebService.DTO.Student;
using DoAnWebService.DTO.Teacher;
using DoAnWebService.Tests.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DoAnWebService.Tests;

public class ValidationControllerTests
{
    [Fact]
    public async Task Classroom_Create_ReturnsBadRequest_WhenMaLopIsEmpty()
    {
        var controller = new ClassroomController(TestDataFactory.CreateJwtConfiguration());
        var model = new UpdateModel
        {
            MaLop = string.Empty,
            TenLop = "CTK45A",
            KhoaHoc = "2024-2028",
            MaKhoa = "CNTT",
            NgayMoLop = "01/09/2024",
            TrangThai = 1
        };

        var result = await controller.CreateClassroom(model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Classroom_Create_ReturnsBadRequest_WhenNgayMoLopIsInvalid()
    {
        var controller = new ClassroomController(TestDataFactory.CreateJwtConfiguration());
        var model = new UpdateModel
        {
            MaLop = "CTK45A",
            TenLop = "CTK45A",
            KhoaHoc = "2024-2028",
            MaKhoa = "CNTT",
            NgayMoLop = "not-a-date",
            TrangThai = 1
        };

        var result = await controller.CreateClassroom(model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Classroom_Update_ReturnsBadRequest_WhenMaLopIsEmpty()
    {
        var controller = new ClassroomController(TestDataFactory.CreateJwtConfiguration());
        var model = new UpdateModel
        {
            MaLop = "CTK45A",
            TenLop = "CTK45A",
            KhoaHoc = "2024-2028",
            MaKhoa = "CNTT",
            NgayMoLop = "01/09/2024",
            TrangThai = 1
        };

        var result = await controller.Update(string.Empty, model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Classroom_Update_ReturnsBadRequest_WhenNgayMoLopIsInvalid()
    {
        var controller = new ClassroomController(TestDataFactory.CreateJwtConfiguration());
        var model = new UpdateModel
        {
            MaLop = "CTK45A",
            TenLop = "CTK45A",
            KhoaHoc = "2024-2028",
            MaKhoa = "CNTT",
            NgayMoLop = "invalid-date",
            TrangThai = 1
        };

        var result = await controller.Update("CTK45A", model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Student_Create_ReturnsBadRequest_WhenNgaySinhIsInvalid()
    {
        var controller = new StudentController(TestDataFactory.CreateJwtConfiguration());
        var model = new CreateStudent
        {
            Masv = "SV001",
            Ho = "Nguyen",
            Ten = "An",
            Phai = true,
            Diachi = "HN",
            Sodienthoai = "0123456789",
            Ngaysinh = "invalid-date",
            Email = "sv001@example.com",
            Malop = "CTK45A",
            Trangthai = 1
        };

        var result = await controller.Create(model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Student_Update_ReturnsBadRequest_WhenNgaySinhIsInvalid()
    {
        var controller = new StudentController(TestDataFactory.CreateJwtConfiguration());
        var model = new UpdateStudent
        {
            Ho = "Nguyen",
            Ten = "An",
            Phai = true,
            Diachi = "HN",
            Sodienthoai = "0123456789",
            Ngaysinh = "invalid-date",
            Email = "sv001@example.com",
            Malop = "CTK45A",
            Trangthai = 1
        };

        var result = await controller.Update("SV001", model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Teacher_Create_ReturnsBadRequest_WhenMagvIsEmpty()
    {
        var controller = new TeacherController(TestDataFactory.CreateJwtConfiguration());
        var model = new CreateTeacherModels
        {
            Magv = string.Empty,
            Makhoa = "CNTT",
            Ho = "Tran",
            Ten = "Binh",
            Phai = true,
            Ngaysinh = "01/01/1990",
            Trangthai = 1
        };

        var result = await controller.Create(model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Teacher_Create_ReturnsBadRequest_WhenNgaySinhIsInvalid()
    {
        var controller = new TeacherController(TestDataFactory.CreateJwtConfiguration());
        var model = new CreateTeacherModels
        {
            Magv = "GV001",
            Makhoa = "CNTT",
            Ho = "Tran",
            Ten = "Binh",
            Phai = true,
            Ngaysinh = "invalid-date",
            Trangthai = 1
        };

        var result = await controller.Create(model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Teacher_Update_ReturnsBadRequest_WhenMagvIsEmpty()
    {
        var controller = new TeacherController(TestDataFactory.CreateJwtConfiguration());
        var model = new UpdateTeacherModel
        {
            Makhoa = "CNTT",
            Ho = "Tran",
            Ten = "Binh",
            Phai = true,
            Ngaysinh = "01/01/1990",
            Trangthai = 1
        };

        var result = await controller.Update(string.Empty, model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Teacher_Update_ReturnsBadRequest_WhenNgaySinhIsInvalid()
    {
        var controller = new TeacherController(TestDataFactory.CreateJwtConfiguration());
        var model = new UpdateTeacherModel
        {
            Makhoa = "CNTT",
            Ho = "Tran",
            Ten = "Binh",
            Phai = true,
            Ngaysinh = "invalid-date",
            Trangthai = 1
        };

        var result = await controller.Update("GV001", model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ManageLTC_Create_ReturnsBadRequest_WhenStartDateIsInvalid()
    {
        var controller = new ManageLTCController(TestDataFactory.CreateJwtConfiguration());
        var model = new CreateLTCModel
        {
            NienKhoa = "2024-2025",
            HocKy = 1,
            MaMH = "MH001",
            MaGV = "GV001",
            SiSoToiDa = 30,
            DayThuTrongTuan = "2,4,6",
            ThoiGianBatDau = "invalid-date",
            ThoiGianKetThuc = "01/12/2024",
            HuyLop = false
        };

        var result = await controller.Create(model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ManageLTC_Create_ReturnsBadRequest_WhenEndDateIsInvalid()
    {
        var controller = new ManageLTCController(TestDataFactory.CreateJwtConfiguration());
        var model = new CreateLTCModel
        {
            NienKhoa = "2024-2025",
            HocKy = 1,
            MaMH = "MH001",
            MaGV = "GV001",
            SiSoToiDa = 30,
            DayThuTrongTuan = "2,4,6",
            ThoiGianBatDau = "01/09/2024",
            ThoiGianKetThuc = "invalid-date",
            HuyLop = false
        };

        var result = await controller.Create(model);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RegisterLTC_ReturnsBadRequest_WhenMaSvIsEmpty()
    {
        var controller = new RegisterLTCController(TestDataFactory.CreateJwtConfiguration());

        var result = await controller.GetLopTinChi(string.Empty, 1);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task TeacherCourse_ReturnsBadRequest_WhenMaGvIsEmpty()
    {
        var controller = new TeacherCourseController(TestDataFactory.CreateJwtConfiguration());

        var result = await controller.GetTeacherSubject(string.Empty, 1);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task TeacherInputScore_ReturnsBadRequest_WhenMaGvIsEmpty()
    {
        var controller = new TeacherInputScoreController(TestDataFactory.CreateJwtConfiguration());

        var result = await controller.GetInputScore(string.Empty, 1);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
