using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TodoApi.Models.DTOs;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly MySqlService<CityDTO> _cityService;

    // Usamos el servicio genérico con CityDTO
    public CityController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WorldDbContext");
        _cityService = new MySqlService<CityDTO>(connectionString);
    }

    // GET: api/City/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult GetAllCities()
    {
        try
        {
            // Obtener todas las ciudades
            var cities = _cityService.GetAll("city");
            return Ok(cities);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET: api/City/{id}
    [HttpGet("{id}")]
    public IActionResult GetCity(int id)
    {
        try
        {
            // Obtener una ciudad por su ID
            var city = _cityService.GetByPrimaryKey("city", id); // Aquí usamos GetByPrimaryKey para admitir tanto "Id" como "Code"
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST: api/City
    [HttpPost]
    public IActionResult CreateCity([FromBody] CityDTO cityDto)
    {
        if (cityDto == null)
        {
            return BadRequest("City data is null.");
        }

        try
        {
            // Crear una nueva ciudad usando el servicio genérico
            var createdCity = _cityService.Add("city", cityDto);
            return CreatedAtAction(nameof(GetCity), new { id = createdCity.Id }, createdCity);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // PUT: api/City/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateCity(int id, [FromBody] CityDTO cityDto)
    {
        if (cityDto == null)
        {
            return BadRequest("City data is null.");
        }

        try
        {
            // Obtener la ciudad existente
            var existingCity = _cityService.GetByPrimaryKey("city", id); // Usamos GetByPrimaryKey aquí también
            if (existingCity == null)
            {
                return NotFound();
            }

            // Actualizar las propiedades del DTO existente
            existingCity.Name = cityDto.Name;
            existingCity.CountryCode = cityDto.CountryCode;
            existingCity.Population = cityDto.Population;

            // Actualizar la ciudad usando el servicio genérico
            var updatedCity = _cityService.Update("city", existingCity);
            return Ok(updatedCity);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/City/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteCity(int id)
    {
        try
        {
            // Obtener la ciudad a eliminar
            var city = _cityService.GetByPrimaryKey("city", id); // Usamos GetByPrimaryKey aquí también
            if (city == null)
            {
                return NotFound();
            }

            // Eliminar la ciudad usando el servicio genérico
            _cityService.Delete("city", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
