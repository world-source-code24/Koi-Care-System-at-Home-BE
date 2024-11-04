using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface INoteRepository : IGenericRepository<NotesTbl>
    {
        Task<List<NotesTbl>> GetListNote(int accId);
    }
}
