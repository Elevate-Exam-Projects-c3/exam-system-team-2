using exam_system.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Controllers
{
    [Route("[controller]/[api]")]
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();

            var response = new EndpointResponse<T>(
                success: result.IsSuccess,
                statusCode: result.IsSuccess ? 200 : 400, 
                message: result.IsSuccess ? "Success" : result.ErrorMessage,
                data: result.IsSuccess ? result.Data : default!
            );

            return StatusCode(response.StatusCode, response);
        }
    }
}