using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tsi.Authentication.Dtos.RolePermissions;
using TsiErp.Business.Entities.Authentication.RolePermissions;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IRolePermissionsAppService _appService;

        public RolePermissionsController(IRolePermissionsAppService appService)
        {
            _appService = appService;
        }

        [HttpGet("GetListAsync")]
        public async Task<IActionResult> GetList()
        {
            var result = await _appService.GetListAsync();

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
        public async Task<IActionResult> Insert(CreateRolePermissionsDto rolePermissions)
        {
            var result = await _appService.CreateAsync(rolePermissions);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}
