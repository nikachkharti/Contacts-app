using Contacts.Application.Features.People.Queries;
using Contacts.Application.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Contacts.API.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PeopleController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllPeople([FromQuery] GetAllPeopleQuery query)
        {
            var result = await mediator.Send(query);
            var response = new EndpointResponse(result, EndpointMessage.successMessage, isSuccess: true, Convert.ToInt32(HttpStatusCode.OK));
            return StatusCode(response.HttpStatusCode, response);
        }
    }
}
