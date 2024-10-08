using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface INoteRepository
    {
        Task<List<NoteDTO>> GetListNote(int accId);
    }
}
