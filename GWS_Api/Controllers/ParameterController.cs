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
    [Route("api/")]    // uri mit anderer Route und nicht mit Controllername
    public class ParameterController(ILogger<ParameterController> logger, IParameterRepository repo, IMapper mapper) : ControllerBase
    {
        #region Variablendeklaration
        private readonly IParameterRepository _repo = repo;
        private readonly ILogger<ParameterController> _logger = logger;
        private readonly IMapper _mapper = mapper;   // für Dto's
        #endregion

        #region GET
        /// <summary>
        /// Get: Abfrage Parameter
        /// </summary>
        /// <returns></returns>
        [HttpGet("parameter")]
        public async Task<ActionResult<IEnumerable<ParameterReadDto>>> GetParameter()
        {
            try
            {
                var result = await _repo.GetParameterAsync();
                if (result == null)
                {
                    return NotFound($"Keine Parameter vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<ParameterReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Parameter nach Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("parameter/{id}", Name = "GetParameterById")]
        public async Task<ActionResult<ParameterReadDto>> GetParameterById(int id)
        {
            try
            {
                var result = await _repo.GetParameterByIdAsync(id);
                if (result == null)
                {
                    return NotFound($"Kein Parameter mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<ParameterReadDto>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }
        #endregion

        #region POST
        /// <summary>
        /// Post: neuen Parameter schreiben
        /// </summary>
        /// <param name="ParameterCreateDto"></param>
        /// <returns></returns>
        [HttpPost("parameter")]
        public async Task<ActionResult<ParameterReadDto>> AddParameter(ParameterCreateDto parameterCreateDto)
        {
            var parameterModel = _mapper.Map<Parameter>(parameterCreateDto);
            if (parameterModel == null)
            {
                return BadRequest("Keine gültigen Parameter!");
            }
            await _repo.AddParameterAsync(parameterModel);
            await _repo.SaveChangesAsync();
            var parameterReadDto = _mapper.Map<ParameterReadDto>(parameterModel);
            return CreatedAtRoute(nameof(GetParameterById), new { parameterReadDto.Id }, parameterReadDto);
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete: Löschen Parameter
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpDelete("parameter/{id:int}")]
        public async Task<ActionResult> DeleteParameter(int id)
        {
            var parameterModelFromRepo = await _repo.GetParameterByIdAsync(id);
            if (parameterModelFromRepo == null)
            {
                return NotFound($"Parameter mit Id= {id} nicht gefunden");
            }
            await _repo.DeleteParameterAsync(parameterModelFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(parameterModelFromRepo);
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Put: Update eines Parametersatzes
        /// </summary>
        /// <param></param>
        /// <param name="parameterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("parameter/{id}")]
        public async Task<ActionResult> UpdateParameter(int id, ParameterUpdateDto parameterUpdateDto)
        {
            var parameterModelFromRepo = await _repo.GetParameterByIdAsync(id);
            if (parameterModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(parameterUpdateDto, parameterModelFromRepo);
            await _repo.UpdateParameterAsync(parameterModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region PATCH
        /// <summary>
        /// Patch: Patch eines Parametersatzes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("parameter/{id}")]
        public async Task<ActionResult> PartialParameterUpdate(int id, JsonPatchDocument<ParameterUpdateDto> patchDoc)
        {
            var parameterModelFromRepo = await _repo.GetParameterByIdAsync(id);
            if (parameterModelFromRepo == null)
            {
                return NotFound();
            }
            var parameterToPatch = _mapper.Map<ParameterUpdateDto>(parameterModelFromRepo);
            patchDoc.ApplyTo(parameterToPatch, ModelState);

            if (!TryValidateModel(parameterToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(parameterToPatch, parameterModelFromRepo);

            await _repo.UpdateParameterAsync(parameterModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}