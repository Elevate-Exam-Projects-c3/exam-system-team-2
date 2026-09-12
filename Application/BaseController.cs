using exam_system.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Application
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult CustomResult<T>(ApiResponse<T> response)
        {
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
