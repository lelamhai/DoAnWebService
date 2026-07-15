using DoAnWebService.DTO.Employment;
using DoAnWebService.DTO.Teacher;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace DoAnWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TuitionFeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TuitionFeeController(IConfiguration configuration)
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
