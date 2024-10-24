using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class NoteRepository : GenericRepository<NotesTbl>, INoteRepository
    {
        private readonly KoicareathomeContext _context;
        public NoteRepository(KoicareathomeContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<NotesTbl>> GetListNote(int accId)
        {
            var NoteList = await _context.NotesTbls
                .Where(note => note.AccId == accId)
                .Select(note => new NotesTbl
                {
                    NoteId = note.NoteId,
                    NoteName = note.NoteName,
                    NoteText = note.NoteText,
                })
                .ToListAsync();
            return NoteList;
        }
    }
}
