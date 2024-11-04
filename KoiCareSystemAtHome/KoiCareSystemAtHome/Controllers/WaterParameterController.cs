using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
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
            return Ok(new { success = true, Parameter = param });
        }

        [HttpGet("get-param{pondId}")]
        public async Task<IActionResult> GetParam(int pondId)
        {
            var pond = await _pondRepository.GetByIdAsync(pondId);
            if (pond == null) return NotFound("Pond is not available!!");
            var param = await _waterParamRepository.GetByPondIdAsync(pondId);
            return Ok(new { success = true, Parameter = param });
        }

        [HttpPost("save-param{pondId}")]
        public async Task<IActionResult> CreateParam(int pondId, WaterParameterDTO param)
        {
            var pond = await _pondRepository.GetByIdAsync(pondId);
            if (pond == null) return BadRequest();
            var currentParam = await _waterParamRepository.GetByPondIdAsync(pondId);
           
            WaterParametersTbl newParam = new WaterParametersTbl();
            DateTime currentDate = DateTime.Now.Date;
            if (currentParam == null || currentParam.Date.Date != currentDate)
            {
                newParam = new WaterParametersTbl
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

            }
            else
            {
                newParam = await _waterParamRepository.UpdateParameter(pondId, currentParam, param);
                await _waterParamRepository.UpdateAsync(newParam);
                return Ok(newParam);
            }

            return CreatedAtAction(nameof(GetParam), new { pondId = newParam.PondId }, newParam);
        }

        //[HttpPut("update-param{pamamId}")]
        //public async Task<IActionResult> UpdateParam(int pamamId, WaterParameterDTO param)
        //{
        //    if (param == null)
        //    {
        //        return BadRequest();
        //    }
        //    var updateParam = await _waterParamRepository.GetByIdAsync(pamamId);
        //    if (updateParam == null)
        //    {
        //        return NotFound();
        //    }
        //    updateParam = await _waterParamRepository.UpdateParameter(updateParam, param);
        //    await _waterParamRepository.UpdateAsync(updateParam);
        //    return Ok(new { success = true, Parameter = updateParam });
        //}

        [HttpDelete("delete-param{paramId}")]
        public async Task<IActionResult> DeleteParam(int paramId)
        {
            var updateParam = await _waterParamRepository.GetByIdAsync(paramId);
            if (updateParam == null) return NotFound();
            await _waterParamRepository.DeleteAsync(paramId);
            return Ok(new { success = true, Message = "Delete successfully!" });
        }

        [HttpPost("calculate")]
        public IActionResult EvaluateParameter([FromBody] WaterParameterDTO param)
        {
            if (param == null)
            {
                return BadRequest("Parameters cannot be null.");
            }

            var message = param.CalculateMessage();
            return Ok(new { success = true, Message = message });
        }
    }
}