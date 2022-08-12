using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tsi.Results;
using TsiErp.Business.Entities.Period;
using TsiErp.Entities.Entities.Period.Dtos;

namespace TsiErp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodsController : ControllerBase
    {
        private IPeriodsAppService _appService;

        public PeriodsController(IPeriodsAppService appService)
        {
            _appService = appService;
        }

        [HttpGet("getall")]
        public async Task<IDataResult<List<ListPeriodsDto>>> GetList()
        {
            var result = new SuccessDataResult<List<ListPeriodsDto>>((await _appService.GetListAsync()).ToList());

            if (!result.Success)
            {
                return new DataResult<List<ListPeriodsDto>>(new List<ListPeriodsDto>(), result.Success, "Hata Var");
            }

            return new DataResult<List<ListPeriodsDto>>(result.Data, true, "Başarılı");
        }

        [HttpGet("getbyId")]
        public async Task<IDataResult<SelectPeriodsDto>> GetById(Guid id)
        {
            var result = new SuccessDataResult<SelectPeriodsDto>(await _appService.GetAsync(id));

            if (!result.Success)
            {
                return new DataResult<SelectPeriodsDto>(new SelectPeriodsDto(), result.Success, "Hata Var");
            }

            return new DataResult<SelectPeriodsDto>(result.Data, true, "Başarılı");
        }

        [HttpPost("insert")]
        public async Task<IDataResult<SelectPeriodsDto>> Insert(CreatePeriodsDto branch)
        {
            var result = new SuccessDataResult<SelectPeriodsDto>(await _appService.CreateAsync(branch));

            if (!result.Success)
            {
                return new DataResult<SelectPeriodsDto>(new SelectPeriodsDto(), result.Success, "Hata Var");
            }

            return new DataResult<SelectPeriodsDto>(result.Data, true, "Başarılı");
        }

        [HttpPut("update")]
        public async Task<IDataResult<SelectPeriodsDto>> Update(UpdatePeriodsDto branch)
        {
            var result = new SuccessDataResult<SelectPeriodsDto>(await _appService.UpdateAsync(branch));

            if (!result.Success)
            {
                return new DataResult<SelectPeriodsDto>(new SelectPeriodsDto(), result.Success, "Hata Var");
            }

            return new DataResult<SelectPeriodsDto>(result.Data, true, "Başarılı");
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _appService.DeleteAsync(id);

            return Ok();
        }
    }
}
