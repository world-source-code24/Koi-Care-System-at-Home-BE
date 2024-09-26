using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterParameterController : ControllerBase
    {
        private readonly IWaterParameterRepository _waterParamRepository;
        private readonly IGenericRepository<PondsTbl> _pondRepository;
        public WaterParameterController(IWaterParameterRepository waterParamRepository, IGenericRepository<PondsTbl> pondRepository)
        {
            _waterParamRepository = waterParamRepository;
            _pondRepository = pondRepository;
        }
        [HttpGet("get-all{pondId}")]
        public async Task<IActionResult> GetAllAsync(int pondId)
        {
            var pond = await _pondRepository.GetByIdAsync(pondId);
            if (pond == null) return NotFound();
            var param = await _waterParamRepository.GetAllByPondIdAsync(pondId);
            return Ok(param);
        }
        [HttpGet("get-param{pondId}")]
        public async Task<IActionResult> GetParam(int pondId)
        {
            var pond = await _pondRepository.GetByIdAsync(pondId);
            if (pond == null) return NotFound();
            var param = await _waterParamRepository.GetByPondIdAsync(pondId);
            return Ok(param);
        }

        [HttpPost("save-param{pondId}")]
        public async Task<IActionResult> CreateParam(int pondId, WaterParameterDTO param)
        {
            var newParam = new WaterParametersTbl
            { 

                Temperature = param.Temperature,

                Salt = param.Salt,

                PhLevel = param.PhLevel,

                O2Level = param.O2Level,

                No2Level = param.No2Level,

                No3Level = param.No3Level,

                Po4Level = param.Po4Level,

                TotalChlorines = param.TotalChlorines,

                Date = DateTime.Now,

                Note = param.Note,

                PondId = pondId,
            };
            await _waterParamRepository.AddAsync(newParam);
            return CreatedAtAction(nameof(GetParam), new { pondId = newParam.PondId }, newParam);
        }

        [HttpPut("update-param{pamamId}")]
        public async Task<IActionResult> UpdateParam(int pamamId, WaterParameterDTO param)
        {
            var updateParam = await _waterParamRepository.GetByIdAsync(pamamId);
            if (updateParam == null) return BadRequest();
            updateParam.Temperature = param.Temperature;


            updateParam.Salt = param.Salt;

            updateParam.PhLevel = param.PhLevel;

            updateParam.O2Level = param.O2Level;

            updateParam.No2Level = param.No2Level;

            updateParam.No3Level = param.No3Level;

            updateParam.Po4Level = param.Po4Level;

            updateParam.TotalChlorines = param.TotalChlorines;

            updateParam.Date = DateTime.Now;

            updateParam.Note = param.Note;
            return Ok(updateParam);
        }

        [HttpDelete("delete-param{paramId}")]
        public async Task<IActionResult> DeleteParam(int paramId)
        {
            var updateParam = await _waterParamRepository.GetByIdAsync(paramId);
            if (updateParam == null) return BadRequest();
            await _waterParamRepository.DeleteAsync(paramId);
            return Ok("Delete successfully!");
        }
    }
}
