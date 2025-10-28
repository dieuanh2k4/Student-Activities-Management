using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Events;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ApiControllerBase
    {
        private readonly IEventService _svc;

        public EventsController(IEventService svc ,ILogger<EventsController> logger) : base(logger)
        {
            _svc = svc;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateEventDto dto, IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var uploadResult = await _svc.UploadImage(file);
                    dto.Thumbnail = uploadResult.SecureUrl.ToString();
                }

                var created = await _svc.CreateAsync(dto);

                // return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            try
            {
                var list = await _svc.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _svc.GetByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEventDto dto)
        {
            try
            {
                var ok = await _svc.UpdateAsync(id, dto);
                if (!ok) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _svc.DeleteAsync(id);
                if (!ok) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}