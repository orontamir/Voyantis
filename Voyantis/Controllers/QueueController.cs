using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Voyantis.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Voyantis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IQueueService _queueService;
        public QueueController(IConfiguration configuration, IQueueService queueService)
        {
            _configuration = configuration;
            _queueService = queueService;
        }


        // GET: api/<QueueController>
        [HttpGet("{queue_name}")]
        public async Task<IActionResult> Get(string queue_name, int? timeout)
        {
            try
            {
                var msg =  await _queueService.ConsumAsync(queue_name, timeout);
                return Ok(new { message = msg });
            }
            catch (OperationCanceledException ex)
            {
                return NoContent();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

       

        // POST api/<QueueController>
        [HttpPost("{queue_name}")]
        public async Task<IActionResult> Post(string queue_name, [FromBody] JsonElement value)
        {
            try
            {
                var msg = await _queueService.ProduceAsync(queue_name, value);
                return Ok(new { status = "Message queued", queue = queue_name, message = msg });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
