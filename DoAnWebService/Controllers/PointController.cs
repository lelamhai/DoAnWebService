using DoAnWebService.DTO.Employment;
using DoAnWebService.DTO.Teacher;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PointController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> List()
        {
            return Ok();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? keyword)
        {
            return Ok();
        }

        [HttpPut("update/{masv}")]
        public async Task<IActionResult> Update(string masv, UpdateTeacherModel model)
        {
            return Ok();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateLTCModel model)
        {
            return Ok();
        }
    }
}
