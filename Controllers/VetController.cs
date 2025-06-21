using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPalCMS.Data;
using PetPalCMS.DTOs;
using PetPalCMS.Models;

namespace PetPalCMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VetController : ControllerBase
    {
        private readonly PetPalContext _context;

        public VetController(PetPalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<VetDto>>>> GetVets()
        {
            try
            {
                var vets = await _context.Vets
                    .Select(v => new VetDto
                    {
                        VetId = v.VetId,
                        Name = v.Name,
                        ClinicName = v.ClinicName,
                        ContactInfo = v.ContactInfo
                    })
                    .ToListAsync();

                return Ok(ApiResponse<IEnumerable<VetDto>>.SuccessResult(vets));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<VetDto>>.ErrorResult(ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<VetDto>>> CreateVet([FromBody] CreateVetDto createVetDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<VetDto>.ErrorResult("Validation failed", errors));
                }

                var vet = new Vet
                {
                    Name = createVetDto.Name,
                    ClinicName = createVetDto.ClinicName,
                    ContactInfo = createVetDto.ContactInfo
                };

                _context.Vets.Add(vet);
                await _context.SaveChangesAsync();

                var result = new VetDto
                {
                    VetId = vet.VetId,
                    Name = vet.Name,
                    ClinicName = vet.ClinicName,
                    ContactInfo = vet.ContactInfo
                };

                return Ok(ApiResponse<VetDto>.SuccessResult(result, "Vet created"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<VetDto>.ErrorResult(ex.Message));
            }
        }
    }
}