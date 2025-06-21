using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPalCMS.Data;
using PetPalCMS.DTOs;
using PetPalCMS.Models;

namespace PetPalCMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetController : ControllerBase
    {
        private readonly PetPalContext _context;

        public PetController(PetPalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetDto>>>> GetPets()
        {
            try
            {
                var pets = await _context.Pets
                    .Select(p => new PetDto
                    {
                        PetId = p.PetId,
                        Name = p.Name,
                        Species = p.Species,
                        Breed = p.Breed,
                        Birthdate = p.Birthdate
                    })
                    .ToListAsync();

                return Ok(ApiResponse<IEnumerable<PetDto>>.SuccessResult(pets));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<PetDto>>.ErrorResult(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PetDto>>> GetPet(int id)
        {
            try
            {
                var pet = await _context.Pets
                    .Include(p => p.VaccinationRecords)
                        .ThenInclude(vr => vr.Vet)
                    .FirstOrDefaultAsync(p => p.PetId == id);

                if (pet == null)
                    return NotFound(ApiResponse<PetDto>.ErrorResult($"Pet {id} not found"));

                var petDto = new PetDto
                {
                    PetId = pet.PetId,
                    Name = pet.Name,
                    Species = pet.Species,
                    Breed = pet.Breed,
                    Birthdate = pet.Birthdate
                };

                return Ok(ApiResponse<PetDto>.SuccessResult(petDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PetDto>.ErrorResult(ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PetDto>>> CreatePet([FromBody] CreatePetDto createPetDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<PetDto>.ErrorResult("Validation failed", errors));
                }

                var pet = new Pet
                {
                    Name = createPetDto.Name,
                    Species = createPetDto.Species,
                    Breed = createPetDto.Breed,
                    Birthdate = createPetDto.Birthdate
                };

                _context.Pets.Add(pet);
                await _context.SaveChangesAsync();

                var result = new PetDto
                {
                    PetId = pet.PetId,
                    Name = pet.Name,
                    Species = pet.Species,
                    Breed = pet.Breed,
                    Birthdate = pet.Birthdate
                };

                return CreatedAtAction(nameof(GetPet), new { id = pet.PetId }, 
                    ApiResponse<PetDto>.SuccessResult(result, "Pet created"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PetDto>.ErrorResult(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PetDto>>> UpdatePet(int id, [FromBody] PetDto petDto)
        {
            try
            {
                if (id != petDto.PetId)
                    return BadRequest(ApiResponse<PetDto>.ErrorResult("ID mismatch"));

                var pet = await _context.Pets.FindAsync(id);
                if (pet == null)
                    return NotFound(ApiResponse<PetDto>.ErrorResult($"Pet {id} not found"));

                pet.Name = petDto.Name;
                pet.Species = petDto.Species;
                pet.Breed = petDto.Breed;
                pet.Birthdate = petDto.Birthdate;

                await _context.SaveChangesAsync();
                return Ok(ApiResponse<PetDto>.SuccessResult(petDto, "Pet updated"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PetDto>.ErrorResult(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeletePet(int id)
        {
            try
            {
                var pet = await _context.Pets.FindAsync(id);
                if (pet == null)
                    return NotFound(ApiResponse<object>.ErrorResult($"Pet {id} not found"));

                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync();

                return Ok(ApiResponse<object>.SuccessResult(null, "Pet deleted"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult(ex.Message));
            }
        }
    }
}