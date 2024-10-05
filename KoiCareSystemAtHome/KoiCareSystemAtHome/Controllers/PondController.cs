using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Entities;
using System.Threading.Tasks;
using KoiCareSystemAtHome.Repositories.IRepositories;
using System.Linq;

namespace KoiCareSystemAtHome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PondController : ControllerBase
    {
        private readonly KoiCareSystemDbContext _context;

        //private readonly IAccountRepository _user;
        //public PondController(KoiCareSystemDbContext context, IAccountRepository user)
        //{
        //    _context = context;
        //    _user = user;
        //}

        public PondController(KoiCareSystemDbContext context)
        {
            _context = context;
        }

        [HttpGet("Show-All-Ponds")]
        public async Task<ActionResult<IEnumerable<PondDTO>>> GetPonds()
        {
            List<PondsTbl> listPond = await _context.PondsTbls.ToListAsync();
            return Ok(new { Message = "Success \n", listPond });
        }


        //Show all list of user Ponds
        [HttpGet("Show-All-Ponds-UserID/{AccId}")]
        public async Task<ActionResult<IEnumerable<PondDTO>>> GetUserPonds(int AccId)
        {
            List<PondsTbl> listPond = await _context.PondsTbls.Where(p => p.AccId == AccId).ToListAsync();
            return Ok(new { Message = "Success \n", listPond });
        }


        // GET: User for when user choose a specific pond
        [HttpGet("Show-Specific-Pond/{id} ")]
        public async Task<ActionResult<PondDTO>> GetPond(int id)
        {
            var pond = await _context.PondsTbls.FindAsync(id);
            if (pond == null)
            {
                return NotFound();
            }
            return Ok(new { status = "Success", message = "Get Pond Successful", pond });
        }

        

        // POST: Create Ponds 
        [HttpPost("Create-Pond")]
        public async Task<ActionResult<PondDTO>> CreatePond(int accId, PondDTO pondDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pondTbl = new PondsTbl
            {
                PondId = pondDto.PondId,
                Name = pondDto.Name,
                Image = pondDto.Image,
                Depth = pondDto.Depth,
                Volume = pondDto.Volume,
                DrainCount = pondDto.DrainCount,
                PumpCapacity = pondDto.PumpCapacity,
                AccId = accId
            };
            _context.PondsTbls.Add(pondTbl);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPond), new { id = pondTbl.PondId }, pondTbl);

        }

      
        // PUT : Update pond for user
        [HttpPut("Update-Pond/{id}")]
        public async Task<IActionResult> UpdatePond(int id, PondDTO pondDto)
        {
            var pond = await _context.PondsTbls.FindAsync(id);
            if (pond == null) return BadRequest("Pond not found.");

            pond.Name = pondDto.Name;
            pond.Image = pondDto.Image;
            pond.Depth = pondDto.Depth;
            pond.Volume = pondDto.Volume;
            pond.DrainCount = pondDto.DrainCount;
            pond.PumpCapacity = pondDto.PumpCapacity;

            _context.Entry(pond).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PondExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { status = true, message = $"Update Successful | {pond.ToString()}" });
        }


        // DELETE: Delete User Pond
        [HttpDelete("Delete-Pond/{id}")]
        public async Task<IActionResult> DeletePond(int id)
        {
            var pond = await _context.PondsTbls.FindAsync(id);
            if (pond == null)
            {
                return NotFound();
            }

            _context.PondsTbls.Remove(pond);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Delete Succesful| ID: {pond.PondId} Name: {pond.Name}" });
        }

        private bool PondExists(int id)
        {
            return _context.PondsTbls.Any(e => e.PondId == id);
        }
    }
}