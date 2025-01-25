using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TodoApi.Models.DTOs;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly MySqlService<LanguageDTO> _languageService;

        public LanguageController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("WorldDbContext");
            _languageService = new MySqlService<LanguageDTO>(connectionString);
        }

        // GET: api/Language/listar
        [HttpGet]
        [Route("listar")]
        public IActionResult GetAllLanguages()
        {
            try
            {
                var languages = _languageService.GetAll("countryLanguage");  // Cambié 'language' a 'countryLanguage'
                return Ok(languages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Language/{countryCode}/{languageCountry}
        [HttpGet("{countryCode}/{languageCountry}")]
        public IActionResult GetLanguage(string countryCode, string languageCountry)
        {
            try
            {
                var language = _languageService.GetByPrimaryKey("countryLanguage", countryCode);  // Cambié 'language' a 'countryLanguage'
                if (language == null)
                {
                    return NotFound();
                }
                return Ok(language);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Language
        [HttpPost]
        public IActionResult CreateLanguage([FromBody] LanguageDTO languageDto)
        {
            if (languageDto == null)
            {
                return BadRequest("Language data is null.");
            }

            try
            {
                var createdLanguage = _languageService.Add("countryLanguage", languageDto);  // Cambié 'language' a 'countryLanguage'
                return CreatedAtAction(nameof(GetLanguage), new { countryCode = createdLanguage.CountryCode, languageCountry = createdLanguage.LanguageCountry }, createdLanguage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Language/{countryCode}/{languageCountry}
        [HttpPut("{countryCode}/{languageCountry}")]
        public IActionResult UpdateLanguage(string countryCode, string languageCountry, [FromBody] LanguageDTO languageDto)
        {
            if (languageDto == null)
            {
                return BadRequest("Language data is null.");
            }

            try
            {
                var existingLanguage = _languageService.GetByPrimaryKey("countryLanguage", countryCode);  // Cambié 'language' a 'countryLanguage'
                if (existingLanguage == null)
                {
                    return NotFound();
                }

                // Actualizar las propiedades del DTO existente
                existingLanguage.LanguageCountry = languageDto.LanguageCountry;
                existingLanguage.IsOfficial = languageDto.IsOfficial;
                existingLanguage.Percentage = languageDto.Percentage;

                var updatedLanguage = _languageService.Update("countryLanguage", existingLanguage);  // Cambié 'language' a 'countryLanguage'
                return Ok(updatedLanguage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Language/{countryCode}/{languageCountry}
        [HttpDelete("{countryCode}/{languageCountry}")]
        public IActionResult DeleteLanguage(string countryCode, string languageCountry)
        {
            try
            {
                var language = _languageService.GetByPrimaryKey("countryLanguage", countryCode);  // Cambié 'language' a 'countryLanguage'
                if (language == null)
                {
                    return NotFound();
                }

                _languageService.Delete("countryLanguage", countryCode);  // Cambié 'language' a 'countryLanguage'
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
