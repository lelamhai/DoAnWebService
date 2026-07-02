using DoAnWebService.Controllers;
using DoAnWebService.DTO.Account;
using DoAnWebService.Models;
using DoAnWebService.Tests.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DoAnWebService.Tests;

public class AccountControllerTests
{
    [Fact]
    public async Task CreateAccount_AddsNewUser_WhenUsernameIsUnique()
    {
        await using var context = TestDataFactory.CreateContext();
        var controller = new AccountController(context);

        var model = new CreateAccountDTO
        {
            Username = "admin01",
            Password = "P@ssw0rd123",
            Role = "Admin"
        };

        var result = await controller.CreateAccount(model);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
        Assert.True(context.Users.Any(u => u.Username == model.Username));

        var saved = context.Users.Single(u => u.Username == model.Username);
        Assert.NotEqual(model.Password, saved.Password);
    }

    [Fact]
    public async Task CreateAccount_ReturnsBadRequest_WhenUsernameAlreadyExists()
    {
        await using var context = TestDataFactory.CreateContext();
        context.Users.Add(TestDataFactory.CreateHashedUser("admin01", "P@ssw0rd123", "Admin"));
        await context.SaveChangesAsync();

        var controller = new AccountController(context);

        var result = await controller.CreateAccount(new CreateAccountDTO
        {
            Username = "admin01",
            Password = "AnotherPassword1!",
            Role = "Admin"
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateAccount_ReturnsBadRequest_WhenUsernameIsEmpty()
    {
        await using var context = TestDataFactory.CreateContext();
        var controller = new AccountController(context);

        var result = await controller.CreateAccount(new CreateAccountDTO
        {
            Username = string.Empty,
            Password = "Password123!",
            Role = "Admin"
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateAccount_ReturnsBadRequest_WhenPasswordIsEmpty()
    {
        await using var context = TestDataFactory.CreateContext();
        var controller = new AccountController(context);

        var result = await controller.CreateAccount(new CreateAccountDTO
        {
            Username = "admin02",
            Password = string.Empty,
            Role = "Admin"
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task InfoAccount_ReturnsEmployeeInfo_WhenUsernameBelongsToNhanVien()
    {
        await using var context = TestDataFactory.CreateContext();
        context.Nhanviens.Add(new Nhanvien
        {
            Manv = "NV001",
            Ho = "Nguyen",
            Ten = "An",
            Trangthai = 1,
            Avatar = "avatar.png",
            Phai = true,
            MaLoaiNv = 1
        });
        await context.SaveChangesAsync();

        var controller = new AccountController(context);

        var result = await controller.InfoAccount("NV001");

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task InfoAccount_ReturnsTeacherInfo_WhenUsernameBelongsToGiangVien()
    {
        await using var context = TestDataFactory.CreateContext();
        context.Giangviens.Add(new Giangvien
        {
            Magv = "GV001",
            Makhoa = "K01",
            Ho = "Tran",
            Ten = "Binh",
            Trangthai = 1
        });
        await context.SaveChangesAsync();

        var controller = new AccountController(context);

        var result = await controller.InfoAccount("GV001");

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task InfoAccount_ReturnsStudentInfo_WhenUsernameBelongsToSinhVien()
    {
        await using var context = TestDataFactory.CreateContext();
        context.Sinhviens.Add(new Sinhvien
        {
            Masv = "SV001",
            Ho = "Le",
            Ten = "Cuong",
            Phai = true,
            Malop = "L01",
            Trangthai = 1
        });
        await context.SaveChangesAsync();

        var controller = new AccountController(context);

        var result = await controller.InfoAccount("SV001");

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task InfoAccount_ReturnsNotFound_WhenUsernameDoesNotExist()
    {
        await using var context = TestDataFactory.CreateContext();
        var controller = new AccountController(context);

        var result = await controller.InfoAccount("UNKNOWN");

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task InfoAccount_ReturnsNotFound_WhenUsernameContainsOnlySpaces()
    {
        await using var context = TestDataFactory.CreateContext();
        var controller = new AccountController(context);

        var result = await controller.InfoAccount("   ");

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
