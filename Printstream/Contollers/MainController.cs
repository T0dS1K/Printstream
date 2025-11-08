using Microsoft.AspNetCore.Mvc;
using Printstream.Models;
using Printstream.Services;

namespace Printstream.Contollers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private IQueueService _queueService { get; }

        public MainController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddUser([FromForm] UserDTO Data)
        {
            var sessionID = Guid.NewGuid().ToString();

            if (await _queueService.AddTaskToQueue(new UserSession(sessionID, Data)))
            {
                return ApiResponse.Success(sessionID, "200");
            }

            return ApiResponse.Error("400"); ;
        }
    }
}