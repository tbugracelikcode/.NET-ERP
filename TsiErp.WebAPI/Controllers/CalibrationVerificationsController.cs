using Microsoft.AspNetCore.Mvc;
using TsiErp.Business.Entities.CalibrationVerification.Services;
using TsiErp.Entities.Entities.QualityControl.CalibrationVerification.Dtos;

namespace TsiErp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalibrationVerificationsController : ControllerBase
    {
        private ICalibrationVerificationsAppService _appService;

        public CalibrationVerificationsController(ICalibrationVerificationsAppService appService)
        {
            _appService = appService;
        }

        [HttpGet("GetListAsync")]
        public async Task<IActionResult> GetList(ListCalibrationVerificationsParameterDto input)
        {
            var result = await _appService.GetListAsync(input);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpGet("GetAsync")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _appService.GetAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("InsertAsync")]
        public async Task<IActionResult> Insert(CreateCalibrationVerificationsDto branch)
        {
            var result = await _appService.CreateAsync(branch);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPut("UpdateAsync")]
        public async Task<IActionResult> Update(UpdateCalibrationVerificationsDto branch)
        {
            var result = await _appService.UpdateAsync(branch);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _appService.DeleteAsync(id);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
