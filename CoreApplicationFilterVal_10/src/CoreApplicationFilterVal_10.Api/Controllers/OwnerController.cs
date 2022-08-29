using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApplicationFilterVal_10.Api.Common.Responses;
using CoreApplicationFilterVal_10.Domain.Common.Filters;
using CoreApplicationFilterVal_10.Domain.Common.Paging;
using CoreApplicationFilterVal_10.Domain.Contracts.Services;
using CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("TD.WebApi.REST.Controllers", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Api.Controllers;

[IntentManaged(Mode.Fully, Body = Mode.Merge)]
[ApiController]
[Route("api/v1/[controller]/[action]")]
[SwaggerTag("q")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status502BadGateway)]
public class OwnerController : ControllerBase
{

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [IntentManaged(Mode.Fully)]
    public async Task<ActionResult<OwnerVM>> Get(
        [FromServices] IOwnerService service,
        [FromRoute] int id
        )
    {
        var viewModel = await service.Get(id);
        return Ok(viewModel);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [IntentManaged(Mode.Fully)]
    public async Task<ActionResult<OwnerVM>> Create(
        [FromServices] IOwnerService service,
        [FromBody] OwnerDTO createModel
        )
    {
        var viewModel = await service.Create(createModel);
        return Ok(viewModel);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [IntentManaged(Mode.Fully)]
    public async Task<ActionResult<OwnerVM>> Update(
        [FromServices] IOwnerService service,
        [FromRoute] int id,
        [FromBody] OwnerDTO updateModel
        )
    {
        var viewModel = await service.Update(id, updateModel);
        return Ok(viewModel);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [IntentManaged(Mode.Fully)]
    public async Task<IActionResult> Delete(
        [FromServices] IOwnerService service,
        [FromRoute] int id
        )
    {
        await service.Delete(id);
        return Ok();
    }

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(description: "When not explicitly specified, a default page size of <strong>50</strong> will enforced.")]
    [IntentManaged(Mode.Fully)]
    public async Task<ActionResult<PagedList<OwnerVM>>> PagedList(
        [FromServices] AbstractValidator<OwnerFilter> filterValidator,
        [FromServices] IOwnerService service,
        [FromQuery] PagedFilter paging,
        [FromQuery] OwnerFilter filter
        )
    {
        var list = await service.PagedList(paging, filter, filterValidator);
        return Ok(list);
    }
}
