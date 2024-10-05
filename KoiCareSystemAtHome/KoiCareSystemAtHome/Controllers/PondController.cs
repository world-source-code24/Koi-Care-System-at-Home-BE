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
        private readonly IAccountRepository _accountRepository;
        public PondController(KoiCareSystemDbContext context, IAccountRepository accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        //Show all list of user Ponds
        [HttpGet("Show-All-Ponds-(TEST)/")]
        public async Task<ActionResult<IEnumerable<PondDTO>>> GetPonds(int accId)
        {
            List<PondsTbl> listPond = await _context.PondsTbls.Where(p => p.AccId == accId).ToListAsync();
            return Ok(new { Message = "Success \n", listPond });
        }


        //Lay ID User dang login
        [HttpGet("Show-All-Ponds-(REAL)/")]
        public async Task<ActionResult<IEnumerable<PondDTO>>> GetPonds()
        {
            //Get Id of Account in Token
            var idClaim = User.FindFirst("Id")?.Value;
            //try to tranfer idClaim to int account
            if (idClaim == null || !int.TryParse(idClaim, out int accId))
            {
                return BadRequest("User ID not found or invalid.");
            }
            List<PondsTbl> listPond = await _context.PondsTbls.Where(p => p.AccId == accId).ToListAsync();
            return Ok(new { Message = "Success \n", listPond });
        }

        // GET: User for when user choose a specific pond
        [HttpGet("Show-Specific-Pond/{id} (TEST)")]
        public async Task<ActionResult<PondDTO>> GetPond(int id)
        {
            var pond = await _context.PondsTbls.FindAsync(id);
            if (pond == null)
            {
                return NotFound();
            }
            return Ok(new { status = "Success", message = "Get Pond Successful", pond });
        }

        // GET: User for when user choose a specific pond
        [HttpGet("Show-Specific-Pond-(REAL)")]
        public async Task<ActionResult<PondDTO>> GetPond()
        {
            //Get Id of Account in Token
            var idClaim = User.FindFirst("Id")?.Value;
            //try to tranfer idClaim to int account
            if (idClaim == null || !int.TryParse(idClaim, out int accId))
            {
                return BadRequest("User ID not found or invalid.");
            }
            var pond = await _context.PondsTbls.FindAsync(accId);

            if (pond == null)
            {
                return NotFound();
            }
            if (pond.AccId != accId) return NotFound("This pond don't belong to you");

            return Ok(new { status = "Success", message = "Get Pond Successful", pond });
        }

        // POST: Create Ponds 
        [HttpPost("Create-Pond (TEST)")]
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
                AccId = accId,
            };
            _context.PondsTbls.Add(pondTbl);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPond), new { id = pondTbl.PondId }, pondTbl);

        }

        // POST: Create Ponds 
        [HttpPost("Create-Pond (REAL)")]
        public async Task<ActionResult<PondDTO>> CreatePond(PondDTO pondDto)
        {
            //Get Id of Account in Token
            var idClaim = User.FindFirst("Id")?.Value;
            //try to tranfer idClaim to int account
            if (idClaim == null || !int.TryParse(idClaim, out int accId))
            {
                return BadRequest("User ID not found or invalid.");
            }
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