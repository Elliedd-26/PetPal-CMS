using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPalCMS.Data;
using PetPalCMS.DTOs;
using PetPalCMS.Models;

namespace PetPalCMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaccinationRecordController : ControllerBase
    {
        private readonly PetPalContext _context;

        public VaccinationRecordController(PetPalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<VaccinationRecordDto>>>> GetVaccinationRecords()
        {
            try
            {
                var records = await _context.VaccinationRecords
                    .Include(vr => vr.Pet)
                    .Include(vr => vr.Vet)
                    .Select(vr => new VaccinationRecordDto
                    {
                        VaccinationRecordId = vr.VaccinationRecordId,
                        PetId = vr.PetId,
                        VetId = vr.VetId,
                        VaccineName = vr.VaccineName,
                        VaccinationDate = vr.VaccinationDate,
                        PetName = vr.Pet.Name,
                        VetName = vr.Vet.Name
                    })
                    .ToListAsync();

                return Ok(ApiResponse<IEnumerable<VaccinationRecordDto>>.SuccessResult(records));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<VaccinationRecordDto>>.ErrorResult(ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<VaccinationRecordDto>>> CreateVaccinationRecord([FromBody] CreateVaccinationRecordDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<VaccinationRecordDto>.ErrorResult("Validation failed", errors));
                }

                // Check if pet and vet exist
                var petExists = await _context.Pets.AnyAsync(p => p.PetId == createDto.PetId);
                var vetExists = await _context.Vets.AnyAsync(v => v.VetId == createDto.VetId);

                if (!petExists)
                    return BadRequest(ApiResponse<VaccinationRecordDto>.ErrorResult($"Pet {createDto.PetId} not found"));
                if (!vetExists)
                    return BadRequest(ApiResponse<VaccinationRecordDto>.ErrorResult($"Vet {createDto.VetId} not found"));

                var record = new VaccinationRecord
                {
                    PetId = createDto.PetId,
                    VetId = createDto.VetId,
                    VaccineName = createDto.VaccineName,
                    VaccinationDate = createDto.VaccinationDate
                };

                _context.VaccinationRecords.Add(record);
                await _context.SaveChangesAsync();

                // Get full record with relationships
                var fullRecord = await _context.VaccinationRecords
                    .Include(vr => vr.Pet)
                    .Include(vr => vr.Vet)
                    .FirstOrDefaultAsync(vr => vr.VaccinationRecordId == record.VaccinationRecordId);

                var result = new VaccinationRecordDto
                {
                    VaccinationRecordId = fullRecord.VaccinationRecordId,
                    PetId = fullRecord.PetId,
                    VetId = fullRecord.VetId,
                    VaccineName = fullRecord.VaccineName,
                    VaccinationDate = fullRecord.VaccinationDate,
                    PetName = fullRecord.Pet.Name,
                    VetName = fullRecord.Vet.Name
                };

                return Ok(ApiResponse<VaccinationRecordDto>.SuccessResult(result, "Vaccination record created"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<VaccinationRecordDto>.ErrorResult(ex.Message));
            }
        }

        // 演示M:M关系 - 获取某个宠物的所有疫苗记录
        [HttpGet("pet/{petId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<VaccinationRecordDto>>>> GetVaccinationsByPet(int petId)
        {
            try
            {
                var records = await _context.VaccinationRecords
                    .Include(vr => vr.Pet)
                    .Include(vr => vr.Vet)
                    .Where(vr => vr.PetId == petId)
                    .Select(vr => new VaccinationRecordDto
                    {
                        VaccinationRecordId = vr.VaccinationRecordId,
                        PetId = vr.PetId,
                        VetId = vr.VetId,
                        VaccineName = vr.VaccineName,
                        VaccinationDate = vr.VaccinationDate,
                        PetName = vr.Pet.Name,
                        VetName = vr.Vet.Name
                    })
                    .OrderByDescending(vr => vr.VaccinationDate)
                    .ToListAsync();

                return Ok(ApiResponse<IEnumerable<VaccinationRecordDto>>.SuccessResult(records, $"Found {records.Count} vaccination records"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<VaccinationRecordDto>>.ErrorResult(ex.Message));
            }
        }
    }
}