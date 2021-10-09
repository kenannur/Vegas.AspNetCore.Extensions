using Microsoft.AspNetCore.Mvc;
using Vegas.AspNetCore.Common.Models;

namespace Vegas.AspNetCore.Common.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]/[action]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ApiControllerBase() { }

        protected IActionResult OkResponse<TMainResponse>(TMainResponse mainResponse) => CreateOkResponse(mainResponse);

        protected IActionResult OkResponse() => CreateOkResponse(new VoidResponse());

        private IActionResult CreateOkResponse<TMainResponse>(TMainResponse mainResponse)
        {
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                MainResponse = mainResponse
            });
        }
    }
}
