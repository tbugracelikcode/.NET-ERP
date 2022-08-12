using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tsi.Results;
using Tsi.Validation.Validations.FluentValidation.CrossCuttingConcerns;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BranchesController : ControllerBase
    {
        private IBranchesAppService _appService;

        public BranchesController(IBranchesAppService appService)
        {
            _appService = appService;
        }

        [HttpGet("getall")]
        public async Task<IDataResult<List<ListBranchesDto>>> GetList()
        {
            var result = new SuccessDataResult<List<ListBranchesDto>>((await _appService.GetListAsync()).ToList());

            if (!result.Success)
            {
                return new DataResult<List<ListBranchesDto>>(new List<ListBranchesDto>(), result.Success, "Hata Var");
            }

            return new DataResult<List<ListBranchesDto>>(result.Data, true, "Başarılı");
        }

        [HttpGet("getbyId")]
        public async Task<IDataResult<SelectBranchesDto>> GetById(Guid id)
        {
            var result = new SuccessDataResult<SelectBranchesDto>(await _appService.GetAsync(id));

            if (!result.Success)
            {
                return new DataResult<SelectBranchesDto>(new SelectBranchesDto(), result.Success, "Hata Var");
            }

            return new DataResult<SelectBranchesDto>(result.Data, true, "Başarılı");
        }

        [HttpPost("insert")]
        public async Task<IDataResult<SelectBranchesDto>> Insert(CreateBranchesDto branch)
        {
            var result = new SuccessDataResult<SelectBranchesDto>(await _appService.CreateAsync(branch));

            if (!result.Success)
            {
                return new DataResult<SelectBranchesDto>(new SelectBranchesDto(), result.Success, "Hata Var");
            }

            return new DataResult<SelectBranchesDto>(result.Data, true, "Başarılı");
        }

        [HttpPut("update")]
        public async Task<IDataResult<SelectBranchesDto>> Update(UpdateBranchesDto branch)
        {
            var result = new SuccessDataResult<SelectBranchesDto>(await _appService.UpdateAsync(branch));

            if (!result.Success)
            {
                return new DataResult<SelectBranchesDto>(new SelectBranchesDto(), result.Success, "Hata Var");
            }

            return new DataResult<SelectBranchesDto>(result.Data, true, "Başarılı");
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _appService.DeleteAsync(id);

            return Ok();
        }
    }
}
