using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly KoiCareSystemDbContext _context;
        public NoteRepository(KoiCareSystemDbContext context)
        {
            _context = context;
        }
        public async Task<List<NoteDTO>> GetListNote(int accId)
        {
            var NoteList = await _context.NotesTbls
                .Where(note => note.AccId == accId)
                .Select(note => new NoteDTO
                {
                    NoteName = note.NoteName,
                    NoteText = note.NoteText,
                })
                .ToListAsync();
            return NoteList;
        }
    }
}
