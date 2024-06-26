using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Management.Rest.API.Features.Authentication.Commands;
using School.Management.Rest.API.Helpers;

namespace School.Management.Rest.API.Features.Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthenticationController(IMediator mediator, IConfiguration configuration) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AuthenticationRequest request)
        {
            try
            {
                AuthenticationResponse? response = await mediator.Send(request);

                if (response is null)
                    return Unauthorized();
                else
                    return Ok(new { Token = response.GenerateToken(configuration) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("validate/{token}")]
        public async Task<IActionResult> GetAsync([FromRoute] string token)
        {
            try
            {
                return await JwtHelper.ValidateTokenAsync(token, configuration) ? Ok() : Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}