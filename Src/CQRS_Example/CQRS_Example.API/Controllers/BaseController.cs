using CQRS_Example.API.Model.Base;
using CQRS_Example.Common.CQRS;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Example.API.Controllers
{
    [ApiController]
    public class BaseController: ControllerBase
    {
        protected IActionResult Error(string errorMessage, string errorIdentifier)
        {
            return BadRequest(new ErrorEnvelope(errorMessage, errorIdentifier));
        }

        protected new IActionResult Ok()
        {
            return base.Ok(new Envelope<string>(string.Empty));
        }

        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(base.Ok(new Envelope<T>(result)));
        }

        protected IActionResult Ok<T>(PagedResult<T> pagedResult)
        {
            return base.Ok(new PagedEnvelope<T>(pagedResult));
        }

        protected IActionResult Nothing()
        {
            return base.NotFound();
        }
    }
}
