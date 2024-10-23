using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly KoiCareSystemDbContext _context;
        private readonly INoteRepository _noteRepository;
        public NoteController (KoiCareSystemDbContext context, INoteRepository noteRepository)
        {
            _context = context;
            _noteRepository = noteRepository;
        }
        //Get All Note
        [HttpGet]
        public async Task<IActionResult> GetAllNote(int accId)
        {
            var NoteList = await _noteRepository.GetListNote(accId);
            return Ok(new {Success = true, NoteList});
        }
        //Create new Note
        [HttpPost]
        public async Task<IActionResult> CreateNote(NoteDTO note, int accId)
        {
            try
            {
                var newKoi = new NotesTbl
                {
                    AccId = accId,
                    NoteName = note.NoteName,
                    NoteText = note.NoteText,
                };
                await _noteRepository.AddAsync(newKoi);
                
                return Ok(new { Success = true, noteId = newKoi.NoteId});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateNote(NoteDTO note, int noteId)
        {
            var updateNote = await _context.NotesTbls.FindAsync(noteId);
            if (updateNote == null)
            {
                return BadRequest(new {Success = false, Message = "We can't see Note to update!" });
            }

            updateNote.NoteName = note.NoteName;
            updateNote.NoteText = note.NoteText;
            await _noteRepository.UpdateAsync(updateNote);
            await _context.SaveChangesAsync();
            return Ok(new {Success = true, Message = "Update is successful!"});
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNote(int noteId)
        {
            var deleteNote = await _context.NotesTbls.FindAsync(noteId);
            if (deleteNote == null) return BadRequest(new { Success = false, Message = "We can't see Note to Delete!" });
            await _noteRepository.DeleteAsync(noteId);
            return Ok(new { Success = true, Message = "Delete successful!" });
        }
    }
}
