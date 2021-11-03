using Microsoft.AspNetCore.Mvc;
using Vegas.AspNetCore.Common.Models;

namespace Vegas.AspNetCore.Common.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected ApiController() { }

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


    [Route("v{version:apiVersion}/[controller]/[action]")]
    public abstract class ApiControllerBase : ApiController
    { }

    [Route("v{version:apiVersion}/[controller]")]
    public abstract class HttpControllerBase : ApiController
    { }
}
