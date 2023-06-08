using Microsoft.AspNetCore.Mvc;
using TsiErp.Business.Entities.ExchangeRate.Services;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos;

namespace TsiErp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private IExchangeRatesAppService _appService;

        public ExchangeRatesController(IExchangeRatesAppService appService)
        {
            _appService = appService;
        }

        [HttpPost("GetListAsync")]
        public async Task<IActionResult> GetList(ListExchangeRatesParameterDto input)
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
        public async Task<IActionResult> Insert(CreateExchangeRatesDto branch)
        {
            var result = await _appService.CreateAsync(branch);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPut("UpdateAsync")]
        public async Task<IActionResult> Update(UpdateExchangeRatesDto branch)
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
