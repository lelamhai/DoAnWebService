using DoAnWebService.Data;
using DoAnWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly QLSVContext _context;

        public StudentsController(QLSVContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Sinhvien>>> GetStudents()
        {
            return await _context.Sinhviens.ToListAsync();
        }
    }
}
