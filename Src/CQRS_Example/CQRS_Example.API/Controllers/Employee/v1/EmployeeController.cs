using Asp.Versioning;
using CQRS_Example.API.Controllers.Employee.v1.DTO;
using CQRS_Example.API.Model.Base;
using CQRS_Example.Common.CQRS;
using CQRS_Example.Domain.Commands;
using CQRS_Example.Domain.Queries;
using CQRS_Example.Domain.ReadModels;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Example.API.Controllers.Employee.v1
{
    [ApiController]
    [Route("api/v{version:apiversion}/employee")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorEnvelope))]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorEnvelope))]
    //[Authorize]
    public class EmployeeController : BaseController
    {
        private readonly IDispatcher _dispatcher;

        public EmployeeController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedEnvelope<EmployeeModel>))]
        public async Task<IActionResult> GetEmployees()
        {
            var query = new GetAllEmployeesQuery();
            var result = await _dispatcher.DispatchAsync<GetAllEmployeesQuery, PagedResult<EmployeeModel>>(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployee([FromBody] RegisterEmployeeRequest request)
        {
            var command = new RegisterEmployeeCommand
            {
                correlationId = HttpContext.Items["CorrelationId"].ToString(),
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Level= (Domain.Aggregates.EmployeeLevel)Enum.Parse(typeof(Domain.Aggregates.EmployeeLevel), request.Level.ToString()),
                EmployeeId= request.EmployeeId,
            };

            await _dispatcher.DispatchAsync(command);
            return Ok();
        }
    }
}
