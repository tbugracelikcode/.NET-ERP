using Microsoft.AspNetCore.Mvc;
using TsiErp.Business.Entities.Shift.Services;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;

namespace TsiErp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShiftsController : ControllerBase
    {
        private IShiftsAppService _appService;

        public ShiftsController(IShiftsAppService appService)
        {
            _appService = appService;
        }

        [HttpPost("GetListAsync")]
        public async Task<IActionResult> GetList(ListShiftsParameterDto input)
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
        public async Task<IActionResult> Insert(CreateShiftsDto branch)
        {
            var result = await _appService.CreateAsync(branch);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPut("UpdateAsync")]
        public async Task<IActionResult> Update(UpdateShiftsDto branch)
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
