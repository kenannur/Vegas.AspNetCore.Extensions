using Microsoft.AspNetCore.Mvc;
using Vegas.AspNetCore.Common.Models;

namespace Vegas.AspNetCore.Common.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected ApiController() { }

        protected IActionResult OkResponse<TResponse>(TResponse response) => CreateOkResponse(response);
        protected IActionResult OkResponse() => CreateOkResponse(new VoidResponse());
        private IActionResult CreateOkResponse<TResponse>(TResponse response) => Ok(new ApiResponse
        {
            IsSuccess = true,
            MainResponse = response
        });

        protected IActionResult AcceptedResponse<TResponse>(TResponse response) => CreateAcceptedResponse(response);
        protected IActionResult AcceptedResponse() => CreateAcceptedResponse(new VoidResponse());
        private IActionResult CreateAcceptedResponse<TResponse>(TResponse response) => Accepted(new ApiResponse
        {
            IsSuccess = true,
            MainResponse = response
        });

        protected IActionResult CreatedResponse<TResponse>(TResponse response) => CreateCreatedResponse(response);
        protected IActionResult CreatedResponse() => CreateCreatedResponse(new VoidResponse());
        private IActionResult CreateCreatedResponse<TResponse>(TResponse response) => Created("", new ApiResponse
        {
            IsSuccess = true,
            MainResponse = response
        });
    }


    [Route("v{version:apiVersion}/[controller]/[action]")]
    public abstract class ApiControllerBase : ApiController
    { }

    [Route("v{version:apiVersion}/[controller]")]
    public abstract class HttpControllerBase : ApiController
    { }
}
