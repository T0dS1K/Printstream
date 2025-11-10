using Microsoft.AspNetCore.Mvc;
using Printstream.Attributes;
using Printstream.Infrastructure.Configurations;
using Printstream.Models;
using Printstream.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Printstream.Contollers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private IDBService _dBService { get; }
        private IQueueService _queueService { get; }

        public MainController(IDBService dBService, IQueueService queueService)
        { 
            _dBService = dBService;
            _queueService = queueService;
        }

        [HttpPost("addData")]
        public async Task<ActionResult<ApiResponse<string>>> AddUser([FromForm] UserDTO Data)
        {
            var sessionID = Guid.NewGuid().ToString("N");

            if (await _queueService.AddTaskToQueue(new UserSession(sessionID, Data)))
            {
                return ApiResponse<string>.Success(sessionID, "Entry added to queue");
            }

            return ApiResponse<string>.Error("Entry cannot be added to the queue");
        }

        [HttpGet("searchName")]
        public async Task<ActionResult<ApiResponse<List<PersonAggregatedDTO>>>> SearchFIO([Required][DefaultValue("")][NameFormat] string LastName, [NameFormat] string? FirstName, [NameFormat] string? MiddleName)
        {
            var data = await _dBService.FindPersonsDataFIO(LastName!, FirstName, MiddleName);

            if (data == null || data.Count == 0)
            {
                return ApiResponse<List<PersonAggregatedDTO>>.Error("Not Found");
            }
            
            return ApiResponse<List<PersonAggregatedDTO>>.Success(data, "Data received successfully");   
        }

        [HttpGet("searchValue")]
        public async Task<ActionResult<ApiResponse<List<PersonAggregatedDTO>>>> SearchValue([Required][DefaultValue("")][Description("Адрес | Номер | Почта")] string Value)
        {
            ValueFormatAttribute VFA = new("");
            var result = VFA.IsValid(Value);

            if (result)
            {
                var data = await _dBService.FindPersonsDataValue(VFA.GetType(), Value);

                if (data == null || data.Count == 0)
                {
                    return ApiResponse<List<PersonAggregatedDTO>>.Error("Not Found");
                }

                return ApiResponse<List<PersonAggregatedDTO>>.Success(data, "Data received successfully");
            }
                
            return ApiResponse<List<PersonAggregatedDTO>>.Error("Invalid value format");
        }

        [HttpGet("searchHistory")]
        public async Task<ActionResult<ApiResponse<List<Bunch_History>>>> SearchValue()
        {
            var data = await _dBService.FindPersonsDataHistory();

            if (data == null || data.Count == 0)
            {
                return ApiResponse<List<Bunch_History>>.Error("Not Found");
            }

            return ApiResponse<List<Bunch_History>>.Success(data, "Data received successfully");
        }
    }
}