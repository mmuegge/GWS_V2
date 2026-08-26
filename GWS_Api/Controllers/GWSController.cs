using AutoMapper;
using GWS_Api.Dtos;
using GWS_Api.Models;
using GWS_Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GWS_Api.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/gws")]    // uri mit anderer Route und nicht mit Controllername
    public class GWSController(ILogger<GWSController> logger, IGWSRepository repo, IMapper mapper) : ControllerBase
    {
        #region Variablendeklaration
        private readonly IGWSRepository _repo = repo;
        private readonly ILogger<GWSController> _logger = logger;
        private readonly IMapper _mapper = mapper;   // für Dto's
        #endregion

        #region GET
        /// <summary>
        /// Get: Abfrage Effizienz-Daten
        /// </summary>
        /// <returns></returns>
        [HttpGet("effizienz")]
        public async Task<ActionResult<IEnumerable<EfficiencyReadDto>>> GetEnergieEfficiency()
        {
            try
            {
                var result = await _repo.GetEnergieEfficiencyAsync();
                if (result == null)
                {
                    return NotFound($"Keine Effizienz-Daten vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<EfficiencyReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Effizienz-Daten nach Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("effizienz/{id}", Name = "GetEnergieEfficiencyById")]
        public async Task<ActionResult<EfficiencyReadDto>> GetEnergieEfficiencyById(int id)
        {
            try
            {
                var result = await _repo.GetEnergieEfficiencyByIdAsync(id);
                if (result == null)
                {
                    return NotFound($"Kein Effizienz-Daten mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<EfficiencyReadDto>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }
        #endregion

        #region POST
        /// <summary>
        /// Post: neuen Effizienz-Daten schreiben
        /// </summary>
        /// <param name="EnergieEfficiencyCreateDto"></param>
        /// <returns></returns>
        [HttpPost("effizienz")]
        public async Task<ActionResult<EfficiencyReadDto>> AddEnergieEfficiency(EfficiencyCreateDto gasEnergieEfficiencyCreateDto)
        {
            var effModel = _mapper.Map<Efficiency>(gasEnergieEfficiencyCreateDto);
            if (effModel == null)
            {
                return BadRequest("Keine gültigen Effizienz-Daten!");
            }
            await _repo.AddEnergieEfficiencyAsync(effModel);
            await _repo.SaveChangesAsync();
            var energieEfficiencyReadDto = _mapper.Map<EfficiencyReadDto>(effModel);
            return CreatedAtRoute(nameof(GetEnergieEfficiencyById), new { energieEfficiencyReadDto.Id }, energieEfficiencyReadDto);
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete: Löschen EnergieEffizienz Datensatz
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpDelete("effizienz/{id:int}")]
        public async Task<ActionResult> DeleteEnergieEfficiency(int id)
        {
            var energieEfficiencyModelFromRepo = await _repo.GetEnergieEfficiencyByIdAsync(id);
            if (energieEfficiencyModelFromRepo == null)
            {
                return NotFound($"Effizienz Datensatz mit Id= {id} nicht gefunden");
            }
            await _repo.DeleteEnergieEfficiencyAsync(energieEfficiencyModelFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(energieEfficiencyModelFromRepo);
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Put: Update eines Effizienzdatensatzes
        /// </summary>
        /// <param></param>
        /// <param name="gasEnergieEfficiencyUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("effizienz/{id}")]
        public async Task<ActionResult> UpdateEnergieEfficiency(int id, EfficiencyUpdateDto gasEnergieEfficiencyUpdateDto)
        {
            var effModelFromRepo = await _repo.GetEnergieEfficiencyByIdAsync(id);
            if (effModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(gasEnergieEfficiencyUpdateDto, effModelFromRepo);
            await _repo.UpdateEnergieEfficiencyAsync(effModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region PATCH
        /// <summary>
        /// Patch: Patch eines EnergieEffiziezdatensatz
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("effizienz/{id}")]
        public async Task<ActionResult> PartialGasEnergieEfficiencyUpdate(int id, JsonPatchDocument<EfficiencyUpdateDto> patchDoc)
        {
            var effModelFromRepo = await _repo.GetEnergieEfficiencyByIdAsync(id);
            if (effModelFromRepo == null)
            {
                return NotFound();
            }
            var effToPatch = _mapper.Map<EfficiencyUpdateDto>(effModelFromRepo);
            patchDoc.ApplyTo(effToPatch, ModelState);

            if (!TryValidateModel(effToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(effToPatch, effModelFromRepo);

            await _repo.UpdateEnergieEfficiencyAsync(effModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}