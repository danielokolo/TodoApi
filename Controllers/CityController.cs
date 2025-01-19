using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models.DTOs;
using TodoApi.Models.Entity;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly WorldDbContext _context;

    public CityController(WorldDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityDTO>>> GetCities()
    {
        var cities = await _context.Cities.ToListAsync();
        return cities.Select(c => new CityDTO
        {
            Id = c.Id,
            Name = c.Name,
            CountryCode = c.CountryCode,
            Population = c.Population
        }).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CityDTO>> GetCity(int id)
    {
        var city = await _context.Cities.FindAsync(id);

        if (city == null)
        {
            return NotFound();
        }

        return new CityDTO
        {
            Id = city.Id,
            Name = city.Name,
            CountryCode = city.CountryCode,
            Population = city.Population
        };
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCity(int id, CityDTO cityDTO)
    {
        if (id != cityDTO.Id)
        {
            return BadRequest();
        }

        var city = await _context.Cities.FindAsync(id);
        if (city == null)
        {
            return NotFound();
        }

        city.Name = cityDTO.Name;
        city.CountryCode = cityDTO.CountryCode;
        city.Population = cityDTO.Population;

        _context.Entry(city).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<CityDTO>> PostCity(CityDTO cityDTO)
    {
        var city = new City
        {
            Name = cityDTO.Name,
            CountryCode = cityDTO.CountryCode,
            Population = cityDTO.Population
        };

        _context.Cities.Add(city);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCity), new { id = city.Id }, cityDTO);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city == null)
        {
            return NotFound();
        }

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
