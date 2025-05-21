using Domain.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using WebTransactionAPI.Commands;
using WebTransactionAPI.DTOs;

namespace WebTransactionAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewJournalController : ControllerBase
    {
        private readonly ILogger<NewJournalController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NewJournalController(ILogger<NewJournalController> logger, ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> NewJournalAsync(NewJournalCommand command)
        {
            var id = Guid.NewGuid();
            try
            {
                command.Id = id;    
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewPostResponse
                {
                    Id = id,
                    Message = "New Journal creation request completed successfully!",
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new Journal!";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Id = id,
                    Message = SAFE_ERROR_MESSAGE,
                });
            }
        }
    }
}
