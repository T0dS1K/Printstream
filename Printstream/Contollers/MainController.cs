using Printstream.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Printstream.Contollers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromForm] UserData UserData)
        {
            return Ok();
        }
    }
}