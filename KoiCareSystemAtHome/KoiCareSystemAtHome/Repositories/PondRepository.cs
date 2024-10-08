using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;

namespace KoiCareSystemAtHome.Repositories
{
    public class PondRepository : IPondRepository
    {
        private readonly KoiCareSystemDbContext _context;

        public PondRepository(KoiCareSystemDbContext context)
        {
            _context = context;
        }

        public async Task<PondsTbl> ConvertDtoToTbl(PondDTO pondDto)
        {
            PondsTbl pondTbl = new PondsTbl
            {
                PondId = pondDto.PondId,
                Name = pondDto.Name,
                Image = pondDto.Image,
                Depth = pondDto.Depth,
                Volume = pondDto.Volume,
                DrainCount = pondDto.DrainCount,
                PumpCapacity = pondDto.PumpCapacity,
                AccId = pondDto.UserId,
            };
            return await Task.FromResult(pondTbl); 
        }

        public async Task<PondDTO> ConvertTblToDto(PondsTbl pondTbl)
        {
            var pondDTO = new PondDTO
            (
            pondTbl.PondId,
            pondTbl.Name,
            pondTbl.Image,
            pondTbl.Depth,
            pondTbl.Volume,
            pondTbl.DrainCount,
            pondTbl.PumpCapacity,
            pondTbl.AccId
            );
            return await Task.FromResult(pondDTO);
        }
    }
}