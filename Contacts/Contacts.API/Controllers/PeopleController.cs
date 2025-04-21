using Contacts.Application.Features.People.Commands;
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
        /// <summary>
        /// Get all people
        /// </summary>
        /// <param name="PageNumber">Page number</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="SortingParameter">Sorting parameter name</param>
        /// <param name="Ascending">Sorted by ascending or descending</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> GetPeople(
            [FromQuery] int? PageNumber = 1,
            [FromQuery] int? PageSize = 10,
            [FromQuery] string SortingParameter = "",
            [FromQuery] bool Ascending = true)
        {
            var query = new GetAllPeopleQuery(PageNumber, PageSize, SortingParameter, Ascending);
            var result = await mediator.Send(query);
            var response = new EndpointResponse(result, EndpointMessage.successMessage, isSuccess: true, Convert.ToInt32(HttpStatusCode.OK));
            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// Get single person
        /// </summary>
        /// <param name="id">Person id</param>
        /// <returns>IActionResult</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson([FromRoute] string id)
        {
            var query = new GetPersonByIdQuery(id);
            var result = await mediator.Send(query);
            var response = new EndpointResponse(result, EndpointMessage.successMessage, isSuccess: true, Convert.ToInt32(HttpStatusCode.OK));
            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// Add new person
        /// </summary>
        /// <param name="model">Person model</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePersonCommand model)
        {
            var result = await mediator.Send(model);
            var response = new EndpointResponse(result, EndpointMessage.successMessage, isSuccess: true, Convert.ToInt32(HttpStatusCode.Created));
            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// Delete person
        /// </summary>
        /// <param name="id">Person id</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson([FromRoute] string id)
        {
            var command = new DeletePersonCommand(id);
            var result = await mediator.Send(command);
            var response = new EndpointResponse(result, EndpointMessage.successMessage, isSuccess: true, Convert.ToInt32(HttpStatusCode.NoContent));
            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="model">Person model</param>
        /// <returns>IActionResult</returns>
        [HttpPut]
        public async Task<IActionResult> UpdatePerson([FromBody] UpdatePersonCommand model)
        {
            var result = await mediator.Send(model);
            var response = new EndpointResponse(result, EndpointMessage.successMessage, isSuccess: true, Convert.ToInt32(HttpStatusCode.OK));
            return StatusCode(response.HttpStatusCode, response);
        }

    }
}
