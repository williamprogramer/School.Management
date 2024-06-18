using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Management.Rest.API.Features.Administrator.Commands;
using School.Management.Rest.API.Features.Administrator.Queries;

namespace School.Management.Rest.API.Features.Administrator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class AdministratorController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddAdministratorRequest request)
        {
            try
            {
                await mediator.Send(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page, [FromQuery] int size)
        {
            try
            {
                return Ok(await mediator.Send(new GetAdministratorsRequest()
                {
                    Page = page,
                    Size = size
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}