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
