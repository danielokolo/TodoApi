using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TodoApi.Models.DTOs;
using TodoApi.Models.Entity;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly MySqlService _context;

    public CityController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WorldDbContext");
        _context = new MySqlService(connectionString);
        
    }

    [HttpGet]   
    [Route("listar")]
    public IActionResult GetAllCitys()
    {
    var citys = _context.getAllCitys();
    return Ok(citys);
    }

    



   
}
