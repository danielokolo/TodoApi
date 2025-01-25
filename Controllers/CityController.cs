using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

    // GET: api/City/listar
    [HttpGet]   
    [Route("listar")]
    public IActionResult GetAllCities()
    {
        var cities = _context.getAllCities();
        return Ok(cities);
    }

    // GET: api/City/{id}
    [HttpGet("{id}")]
    public IActionResult GetCity(int id)
    {
        var city = _context.GetCityById(id);
        if (city == null)
        {
            return NotFound();
        }
        return Ok(city);
    }

    // POST: api/City
    [HttpPost]
    public IActionResult CreateCity([FromBody] CityDTO cityDto)
    {
        if (cityDto == null)
        {
            return BadRequest("City data is null.");
        }

        var city = new City
        {
            Name = cityDto.Name,
            CountryCode = cityDto.CountryCode,
            Population = cityDto.Population
        };

        var createdCity = _context.AddCity(city);
        return CreatedAtAction(nameof(GetCity), new { id = createdCity.Id }, createdCity);
    }
// PUT: api/City/{id}
[HttpPut("{id}")]
public IActionResult UpdateCity(int id, [FromBody] CityDTO cityDto)
{
    if (cityDto == null)
    {
        return BadRequest("City data is null.");
    }

    // Obtener la ciudad existente de la base de datos (de tipo City)
    var existingCity = _context.GetCityById(id);
    if (existingCity == null)
    {
        return NotFound();
    }

    // Convertir el CityDTO a un objeto City
    City cityToUpdate = new City
    {
        Id = existingCity.Id,  // Asignamos el ID de la ciudad existente
        Name = cityDto.Name,
        CountryCode = cityDto.CountryCode,
        Population = cityDto.Population
    };

    // Actualizar la ciudad en la base de datos
    var updatedCity = _context.UpdateCity(cityToUpdate);

    // Devolver la ciudad actualizada como respuesta
    return Ok(updatedCity);
}



    // DELETE: api/City/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteCity(int id)
    {
        var city = _context.GetCityById(id);
        if (city == null)
        {
            return NotFound();
        }

        _context.DeleteCity(id);
        return NoContent();
    }
}
