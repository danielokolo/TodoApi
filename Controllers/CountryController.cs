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
public class CountryController : ControllerBase
{
    private readonly WorldDbContext _context;

    public CountryController(WorldDbContext context)
    {
        _context = context;
    }

    // GET: api/Country
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CountryDTO>>> GetCountries()
    {
        var countries = await _context.Countries.ToListAsync();
        return countries.Select(c => new CountryDTO
        {
            Code = c.Code,
            Name = c.Name,
            Continent = c.Continent,
            Region = c.Region,
            SurfaceArea = c.SurfaceArea,
            Population = c.Population
        }).ToList();
    }

    // GET: api/Country/5
    [HttpGet("{code}")]
    public async Task<ActionResult<CountryDTO>> GetCountry(string code)
    {
        var country = await _context.Countries.FindAsync(code);

        if (country == null)
        {
            return NotFound();
        }

        return new CountryDTO
        {
            Code = country.Code,
            Name = country.Name,
            Continent = country.Continent,
            Region = country.Region,
            SurfaceArea = country.SurfaceArea,
            Population = country.Population
        };
    }

    // PUT: api/Country/5
    [HttpPut("{code}")]
    public async Task<IActionResult> PutCountry(string code, CountryDTO countryDTO)
    {
        if (code != countryDTO.Code)
        {
            return BadRequest();
        }

        var country = await _context.Countries.FindAsync(code);
        if (country == null)
        {
            return NotFound();
        }

        country.Name = countryDTO.Name;
        country.Continent = countryDTO.Continent;
        country.Region = countryDTO.Region;
        country.SurfaceArea = countryDTO.SurfaceArea;
        country.Population = countryDTO.Population;

        _context.Entry(country).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/Country
    [HttpPost]
    public async Task<ActionResult<CountryDTO>> PostCountry(CountryDTO countryDTO)
    {
        var country = new Country
        {
            Code = countryDTO.Code,
            Name = countryDTO.Name,
            Continent = countryDTO.Continent,
            Region = countryDTO.Region,
            SurfaceArea = countryDTO.SurfaceArea,
            Population = countryDTO.Population
        };

        _context.Countries.Add(country);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCountry), new { code = country.Code }, countryDTO);
    }

    // DELETE: api/Country/5
    [HttpDelete("{code}")]
    public async Task<IActionResult> DeleteCountry(string code)
    {
        var country = await _context.Countries.FindAsync(code);
        if (country == null)
        {
            return NotFound();
        }

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
