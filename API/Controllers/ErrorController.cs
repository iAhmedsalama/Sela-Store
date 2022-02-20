using API.Errors;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{

    //Handle empty error end points

    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi =true)] //swagger have an issue if IgnoreApi = false
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }

    }
}
