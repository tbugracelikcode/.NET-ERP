using Microsoft.AspNetCore.Mvc;
using TsiErp.Business.Entities.ContractUnsuitabilityItem.Services;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityItem.Dtos;

namespace TsiErp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractUnsuitabilityItemsController : ControllerBase
    {
        private IContractUnsuitabilityItemsAppService _appService;

        public ContractUnsuitabilityItemsController(IContractUnsuitabilityItemsAppService appService)
        {
            _appService = appService;
        }

        [HttpGet("GetListAsync")]
        public async Task<IActionResult> GetList(ListContractUnsuitabilityItemsParameterDto input)
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
        public async Task<IActionResult> Insert(CreateContractUnsuitabilityItemsDto branch)
        {
            var result = await _appService.CreateAsync(branch);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPut("UpdateAsync")]
        public async Task<IActionResult> Update(UpdateContractUnsuitabilityItemsDto branch)
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
