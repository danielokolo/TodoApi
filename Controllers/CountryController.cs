using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TodoApi.Models.DTOs;

[Route("api/[controller]")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly MySqlService<CountryDTO> _countryService;

    public CountryController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WorldDbContext");
        _countryService = new MySqlService<CountryDTO>(connectionString);
    }

    // GET: api/Country/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult GetAllCountries()
    {
        try
        {
            var countries = _countryService.GetAll("country");
            return Ok(countries);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET: api/Country/{code}
    [HttpGet("{code}")]
    public IActionResult GetCountry(string code)
    {
        try
        {
            var country = _countryService.GetByPrimaryKey("country", code);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST: api/Country
    [HttpPost]
    public IActionResult CreateCountry([FromBody] CountryDTO countryDto)
    {
        if (countryDto == null)
        {
            return BadRequest("Country data is null.");
        }

        try
        {
            var createdCountry = _countryService.Add("country", countryDto);
            return CreatedAtAction(nameof(GetCountry), new { code = createdCountry.Code }, createdCountry);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // PUT: api/Country/{code}
    [HttpPut("{code}")]
    public IActionResult UpdateCountry(string code, [FromBody] CountryDTO countryDto)
    {
        if (countryDto == null)
        {
            return BadRequest("Country data is null.");
        }

        try
        {
            var existingCountry = _countryService.GetByPrimaryKey("country", code);
            if (existingCountry == null)
            {
                return NotFound();
            }

            // Actualizar las propiedades del DTO existente
            existingCountry.Name = countryDto.Name;
            existingCountry.Continent = countryDto.Continent;
            existingCountry.Region = countryDto.Region;
            existingCountry.SurfaceArea = countryDto.SurfaceArea;
            existingCountry.Population = countryDto.Population;

            var updatedCountry = _countryService.Update("country", existingCountry);
            return Ok(updatedCountry);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/Country/{code}
    [HttpDelete("{code}")]
    public IActionResult DeleteCountry(string code)
    {
        try
        {
            var country = _countryService.GetByPrimaryKey("country", code);
            if (country == null)
            {
                return NotFound();
            }

            _countryService.Delete("country", code);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
