using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherScheduleController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TeacherScheduleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> List()
        {
            return Ok();
        }
    }
}
