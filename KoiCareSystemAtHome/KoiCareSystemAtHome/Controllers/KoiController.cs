using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api")]
    [ApiController]
    public class KoiController : ControllerBase
    {
        private readonly IKoiRepository _koiRepository;
        private readonly KoiCareSystemDbContext _context;
        public KoiController(IKoiRepository koiRepository, KoiCareSystemDbContext context)
        {
            _koiRepository = koiRepository;
            _context = context;
        }

        //Get All Koi fish in Pond
        [HttpGet("pond/{pondId}/[controller]")]
        public async Task<IActionResult> GetKoiByPondId(int pondId)
        {
            var koiList = await _koiRepository.GetKoiByPondIdAsync(pondId);
            if (koiList.IsNullOrEmpty())
            {
                return NotFound(new { message = "No Koi fish found for the pond" });
            }
            return Ok(koiList);
        }

        //Get All Koi of User
        [HttpGet("user/{userId}/[controller]")]
        public async Task<IActionResult> GetKoiByUserId(int userId)
        {
            var koiList = await _koiRepository.GetKoiByUserIdAsync((int)userId);
            if (koiList.IsNullOrEmpty())
            {
                return NotFound(new { message = "User don't have any Koi"});
            }
            return Ok(koiList);
        }

        //Get Detail of Kois
        [HttpGet("[controller]/{koiId}")]
        public async Task<IActionResult> GetKoiDetails(int koiId)
        {
            var koiDetails = await _koiRepository.GetKoiByKoiIdAsync(koiId);
            if (koiDetails == null)
            {
                return NotFound(new { message = "Don't any information of this Koi fish"});
            }
            return Ok(koiDetails);
        }

        [HttpPost("pond/{pondId}/[controller]")]
        public async Task<IActionResult> CreateKoi(int pondId, KoiDTO koi)
        {
            var newKoi = new KoisTbl
            {
                Image = koi.Image,
                Name = koi.Name,
                Age = koi.Age,
                Physique = koi.Physique,
                Length = koi.Length,
                Weight = koi.Weight,
                Breed = koi.Breed,
                Sex = koi.Sex,
                PondId = pondId,
            };

            await _koiRepository.AddAsync(newKoi);
            await _context.SaveChangesAsync();

            koi.KoiId = newKoi.KoiId;
            await _koiRepository.SaveKoiToChartAsync(newKoi);
            return Ok(newKoi);
        }

        //Input koiId to know Koi that user want to update. In case, User want to change Pond of Koi,
        //User can input pondId
        [HttpPut("pond/{pondId}/[controller]/{koiId}")]
        public async Task<IActionResult> UpdateKoi(int  koiId, KoiDTO koi, int pondId)
        {
            var updateKoi = await _context.KoisTbls.FindAsync(koiId);
            if (updateKoi == null) return NotFound(new { message = "Can't find Koi to Update" });

            //Put new update information to User
            updateKoi.Image = koi.Image;
            updateKoi.Name = koi.Name;
            updateKoi.Age = koi.Age;
            updateKoi.Physique = koi.Physique;
            updateKoi.Length = koi.Length;
            updateKoi.Weight = koi.Weight;
            updateKoi.Breed = koi.Breed;
            updateKoi.Sex = koi.Sex;
            updateKoi.PondId = pondId;

            await _koiRepository.UpdateAsync(updateKoi);
            await _context.SaveChangesAsync();

            await _koiRepository.SaveKoiToChartAsync(updateKoi);
            return Ok(new { message = "Update Successfully!" });
        }

        [HttpDelete("[controller]/{koiId}")]
        public async Task<IActionResult> DeleteKoi(int koiId)
        {
            var deleteKoi = await _context.FindAsync<KoisTbl>(koiId);
            if(deleteKoi == null) return NotFound(new { message = "Can't find Koi to Delete" });
            await _koiRepository.DeleteAsync(koiId);
            return Ok(new { message = "Delete Successfully!" });
        }
    }
}
