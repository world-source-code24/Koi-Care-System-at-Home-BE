using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly KoiCareSystemDbContext _context;
        private readonly NoteRepository _noteRepository;
        public NoteController (KoiCareSystemDbContext context, NoteRepository noteRepository)
        {
            _context = context;
            _noteRepository = noteRepository;
        }
        //Get All Note
        [HttpGet]
        public async Task<IActionResult> GetAllNote()
        {
            //Get Id of Account in Token
            var accIdClaim = User.FindFirst("Id")?.Value;
            //try to tranfer idClaim to int account
            if (accIdClaim == null || !int.TryParse(accIdClaim, out int accId))
            {
                return BadRequest("User ID not found or invalid.");
            }
            var NoteList = await _noteRepository.GetListNote(accId);
            return Ok(new {Success = true, NoteList});
        }
    }
}
