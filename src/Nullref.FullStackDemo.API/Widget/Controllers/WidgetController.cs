using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nullref.FullStackDemo.API.Services;
using Nullref.FullStackDemo.API.Widget.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Nullref.FullStackDemo.API.Widget.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route($"api/v{{version:apiVersion}}/{ControllerMainPath}")]
    [SwaggerTag("Widget API")]
    public sealed class WidgetController : ControllerBase
    {
        internal const string ControllerName = "Widget";
        internal const string ControllerMainPath = "widgets";
        private readonly IWidgetService _service;

        public WidgetController(ILoggerFactory loggerFactory, IWidgetService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a paginated list of widgets")]
        public async Task<ActionResult<PaginatedResponseModel<WidgetModel>>> Get([FromQuery, Required] WidgetQueryModel query)
        {
            var result = _service.Get(query);
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a widget")]
        public async Task<ActionResult<IdResponseModel>> Create([FromBody, Required] WidgetUpdateModel model)
        {
            var result = _service.Create(model);
            return CreatedAtAction(nameof(Get), new { id = result }, new IdResponseModel { Id = result });
        }

        [HttpGet($"{{{nameof(id)}}}")]
        [SwaggerOperation(Summary = "Get a single widget")]
        public async Task<ActionResult<WidgetModel>> Get([FromRoute, Required] Guid id)
        {
            var result = _service.Get(id);
            return Ok(result);
        }

        [HttpPut($"{{{nameof(id)}}}")]
        [SwaggerOperation(Summary = "Update a widget")]
        public async Task<ActionResult> UpdateNoReturn([FromRoute, Required] Guid id, [FromBody, Required] WidgetUpdateModel model)
        {
            _service.Update(id, model);
            return NoContent();
        }

        [HttpDelete($"{{{nameof(id)}}}")]
        [SwaggerOperation(Summary = "Delete a widget")]
        public async Task<ActionResult> Delete([FromRoute, Required] Guid id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
