using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models.Entity;
using TodoApi.Models.DTOs;

[Route("api/[controller]")]
[ApiController]
public class LanguageController : ControllerBase
{
    private readonly WorldDbContext _context;

    public LanguageController(WorldDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LanguageDTO>>> GetLanguages()
    {
        var languages = await _context.Languages.ToListAsync();
        return languages.Select(l => new LanguageDTO
        {
            CountryCode = l.CountryCode,
            LanguageCountry = l.LanguageCountry,  // Actualizado: LanguageCountry en lugar de Language
            IsOfficial = l.IsOfficial,
            Percentage = l.Percentage
        }).ToList();
    }

    [HttpGet("{countryCode}")]
    public async Task<ActionResult<LanguageDTO>> GetLanguage(string countryCode)
    {
        var language = await _context.Languages.FindAsync(countryCode);

        if (language == null)
        {
            return NotFound();
        }

        return new LanguageDTO
        {
            CountryCode = language.CountryCode,
            LanguageCountry = language.LanguageCountry,  // Actualizado: LanguageCountry en lugar de Language
            IsOfficial = language.IsOfficial,
            Percentage = language.Percentage
        };
    }

    [HttpPut("{countryCode}")]
    public async Task<IActionResult> PutLanguage(string countryCode, LanguageDTO languageDTO)
    {
        if (countryCode != languageDTO.CountryCode)
        {
            return BadRequest();
        }

        var language = await _context.Languages.FindAsync(countryCode);
        if (language == null)
        {
            return NotFound();
        }

        language.LanguageCountry = languageDTO.LanguageCountry;  // Actualizado: LanguageCountry en lugar de Language
        language.IsOfficial = languageDTO.IsOfficial;
        language.Percentage = languageDTO.Percentage;

        _context.Entry(language).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<LanguageDTO>> PostLanguage(LanguageDTO languageDTO)
    {
        var language = new LanguageEntity
        {
            CountryCode = languageDTO.CountryCode,
            LanguageCountry = languageDTO.LanguageCountry,  // Actualizado: LanguageCountry en lugar de Language
            IsOfficial = languageDTO.IsOfficial,
            Percentage = languageDTO.Percentage
        };

        _context.Languages.Add(language);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLanguage), new { countryCode = language.CountryCode }, languageDTO);
    }

    [HttpDelete("{countryCode}")]
    public async Task<IActionResult> DeleteLanguage(string countryCode)
    {
        var language = await _context.Languages.FindAsync(countryCode);
        if (language == null)
        {
            return NotFound();
        }

        _context.Languages.Remove(language);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
