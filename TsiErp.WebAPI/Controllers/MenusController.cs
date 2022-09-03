using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TsiErp.Business.Entities.Authentication.Menus;

namespace TsiErp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenusAppService _appService;

        public MenusController(IMenusAppService appService)
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
    }
}
