using DoAnWebService.Data;
using DoAnWebService.Models;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    public class EmploymentController : ControllerBase
    {
        private readonly QLSVContext _context;

        public EmploymentController(QLSVContext context)
        {
            _context = context;
        }

        [HttpGet("get-employments")]
        public async Task<IActionResult> GetEmployments(int page = 1)
        {
            var employments = await _context.Nhanviens.ToListAsync();
            var result = PaginationHelper.CreatePagedResult(employments, page, -1);
            return Ok(new ApiResponse<PagedResult<Nhanvien>>
            {
                Message = "Lấy danh sách nhân viên thành công.",
                Data = result
            });
        }
    }
}
