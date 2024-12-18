﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Entities;
using System.Threading.Tasks;
using KoiCareSystemAtHome.Repositories.IRepositories;
using System.Linq;
using System.Text;

namespace KoiCareSystemAtHome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PondController : ControllerBase
    {
        private readonly KoicareathomeContext _context;
        public PondController(KoicareathomeContext context)
        {
            _context = context;
        }

        [HttpGet("/api/Show-All-Ponds")]
        public async Task<IActionResult> GetPonds()
        {
            // Retrieve the list of ponds from the database
            List<PondsTbl> listPond = await _context.PondsTbls.ToListAsync();

            // Check if the list is empty
            if (listPond == null || !listPond.Any())
            {
                return NotFound(new { success = false, message = "No ponds found." });
            }

            // Map PondsTbl to PondDTO
            var pondDtos = listPond.Select(p => new PondDTO(
                p.PondId,
                p.Name,
                p.Image, // Assuming PondsTbl has an Image property
                p.Depth,
                p.Volume,
                p.DrainCount,
                p.PumpCapacity,
                p.AccId // Assuming AccId is the UserId
            )).ToList();

            // Return the result with a success message
            return Ok(new { success = true, listPond = pondDtos });
        }


        //Show all list of user Ponds
        [HttpGet("/api/Show-All-Ponds-UserID/{AccId}")]
        public async Task<IActionResult> GetUserPonds(int AccId)
        {
            List<PondsTbl> listPond = await _context.PondsTbls.Where(p => p.AccId == AccId).ToListAsync();
            if (listPond.Count == 0)
                return NotFound(new { Message = "You don't have any ponds, try to create pond" });
            return Ok(new { Message = "Success \n", listPond });
        }


        // GET: User for when user choose a specific pond
        [HttpGet("/api/Show-Specific-Pond/{id}")]
        public async Task<ActionResult<PondDTO>> GetPond(int id)
        {
            var pond = await _context.PondsTbls.Where(p => p.PondId == id).FirstOrDefaultAsync();
            if (pond == null)
            {
                return NotFound(new { status = "Fail", message = "This pond didn't exist" });
            }
            return Ok(new { status = "Success", message = "Get Pond Successful", pond });
        }

        

        // POST: Create Ponds 
        [HttpPost("/api/Create-Pond")]
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
        [HttpPut("/api/Update-Pond/{id}")]
        public async Task<IActionResult> UpdatePond(int id, PondDTO pondDto)
        {
            var pond = await _context.PondsTbls.Where(p => p.PondId == id).FirstOrDefaultAsync();
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
        [HttpDelete("/api/Delete-Pond/{id}")]
        public async Task<IActionResult> DeletePond(int id)
        {
            var pond = await _context.PondsTbls.Where(p => p.PondId == id).FirstOrDefaultAsync();
            if (pond == null)
            {
                return NotFound(new {message = "This pond didn't exist"});
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