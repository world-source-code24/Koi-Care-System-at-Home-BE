using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IPondRepository
    {
        Task<PondsTbl> ConvertDtoToTbl(PondDTO pondDTO);
        Task<PondDTO> ConvertTblToDto(PondsTbl pondTbl);

    }
}
