using BuberDinner.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Controllers;

[Route("[controller]")]
public class DinnersController : ApiController
{
    [HttpGet]
    public IActionResult ListDinners()
    {
        return Ok(Array.Empty<string>());
    }
}