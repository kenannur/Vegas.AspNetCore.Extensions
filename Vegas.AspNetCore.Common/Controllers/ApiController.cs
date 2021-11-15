using Microsoft.AspNetCore.Mvc;
using Vegas.AspNetCore.Common.Models;

namespace Vegas.AspNetCore.Common.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected ApiController() { }

        protected IActionResult OkResponse<TResponse>(TResponse response) => Ok(ApiResponse.Success(response));
        protected IActionResult OkResponse() => OkResponse(VoidResponse.New);

        protected IActionResult AcceptedResponse<TResponse>(TResponse response) => Accepted(ApiResponse.Success(response));
        protected IActionResult AcceptedResponse() => AcceptedResponse(VoidResponse.New);

        protected IActionResult CreatedResponse<TResponse>(TResponse response) => Created("", ApiResponse.Success(response));
        protected IActionResult CreatedResponse() => CreatedResponse(VoidResponse.New);
    }

    [Route("v{version:apiVersion}/[controller]/[action]")]
    public abstract class ApiControllerBase : ApiController
    { }

    [Route("v{version:apiVersion}/[controller]")]
    public abstract class HttpControllerBase : ApiController
    { }
}
