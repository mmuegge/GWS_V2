using AutoMapper;
using GWS_Api.Dtos.Electric;
using GWS_Api.Models.Electric;
using GWS_Api.Repositories.Electric;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GWS_Api.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/strom")]    // uri mit anderer Route und nicht mit Controllername
    public class ElectricController(ILogger<ElectricController> logger, IElectricRepository repo, IMapper mapper) : ControllerBase
    {
        #region Variablendeklaration
        private readonly IElectricRepository _repo = repo;
        private readonly ILogger<ElectricController> _logger = logger;
        private readonly IMapper _mapper = mapper;   // für Dto's
        public ElectricModel electricModel = new();
        #endregion

        #region GET
        /// <summary>
        /// Get: Abfrage alle Anbieter
        /// Route: /api/strom/anbieter
        /// </summary>
        /// <returns></returns>
        [HttpGet("anbieter")]
        public async Task<ActionResult<IEnumerable<ElectricTarifReadDto>>> GetSuppliers()
        {
            try
            {
                var result = await _repo.GetSuppliersAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Anbieter vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<ElectricTarifReadDto>>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage eines bestimmten Anbieters
        /// Route: /api/strom/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("anbieter/{id}", Name = "GetElectricSupplierById")]
        public async Task<ActionResult<ElectricTarifReadDto>> GetElectricSupplierById(int id)
        {
            var result = await _repo.GetSupplierByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Kein Anbieter mit Id={id} vorhanden!");
            }
            return Ok(_mapper.Map<ElectricTarifReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage aller Zählerstände
        /// Route: /api/strom/zaehlerstaende
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerstaende")]
        public async Task<ActionResult<IEnumerable<ElectricCounterReadDto>>> GetElectricCounters()
        {
            try
            {
                var result = await _repo.GetCountersAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zählerstände vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<ElectricCounterReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage der Zählerstände eines Anbieters
        /// Route: /api/strom/zaehlerstaende/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstaende/anbieter/{id}", Name = "GetElectricCounterBySupplier")]
        public async Task<ActionResult<IEnumerable<ElectricCounterReadDto>>> GetElectricCounterBySupplier(int id)
        {
            try
            {
                var result = await _repo.GetCountersBySupplierAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zaehlerstände vom Anbieter mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<ElectricCounterReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage eines Zählerstandes eines bestimmten Datums
        /// Route: /api/strom/zaehlerstand/day --> (2021.10.23)
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/{day}", Name = "GetElectricCounterByDay")]
        public async Task<ActionResult<ElectricCounterReadDto>> GetElectricCounterByDay(DateTime day)
        {
            var result = await _repo.GetCounterByDateAsync(day);
            if (result == null)
            {
                return NotFound($"Kein Zaehlerstand ({day}) vorhanden!");
            }
            return Ok(_mapper.Map<ElectricCounterReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage eines Zählerstandes mit einer bestimmten Id
        /// Route: /api/strom/zaehlerstand/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/id/{id}", Name = "GetElectricCounterById")]
        public async Task<ActionResult<ElectricCounterReadDto>> GetElectricCounterById(int id)
        {
            var result = await _repo.GetCounterByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Kein Zaehlerstand mit Id={id} vorhanden!");
            }
            return Ok(_mapper.Map<ElectricCounterReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage aller Zahlungen
        /// Route: /api/strom/zahlungen
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungen")]
        public async Task<ActionResult<IEnumerable<ElectricPaymentReadDto>>> GetElectricPayments()
        {
            try
            {
                var result = await _repo.GetPaymentsAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zahlungen vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<ElectricPaymentReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Zahlungen eines bestimmten Anbieters
        /// Route: /api/strom/zahlungen/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zahlungen/anbieter/{id}", Name = "GetElectricPaymentsBySupplier")]
        public async Task<ActionResult<IEnumerable<ElectricPaymentReadDto>>> GetElectricPaymentsBySupplier(int id)
        {
            try
            {
                var result = await _repo.GetPaymentsBySupplierAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound($"Kein Zahlungen vom Anbieter mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<ElectricPaymentReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage einer Zahlungen nach Id
        /// Route: /api/strom/zahlungen/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zahlungen/{id}", Name = "GetElectricPaymentById")]
        public async Task<ActionResult<ElectricPaymentReadDto>> GetElectricPaymentById(int id)
        {
            var result = await _repo.GetPaymentByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Kein Zahlung mit Id={id} vorhanden!");
            }
            return Ok(_mapper.Map<ElectricPaymentReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage aller Zahlungsarten
        /// Route: /api/strom/zahlungsart
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungsart")]
        public async Task<ActionResult<IEnumerable<ElectricPaymentMethodReadDto>>> GetElectricPaymentMethods()
        {
            try
            {
                var result = await _repo.GetPaymentMethodsAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zahlungsart vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<ElectricPaymentMethodReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Kosten eines Anbieters nach Anbieter-Id (nur vohanden wenn während der Vertragslaufzeit geänderte monatliche Kosten eines Anbieters vorhanden ist)
        /// es können mehr als eine Kostenänderung vorhanden sein
        /// Route: /api/strom/kosten/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("kosten/anbieter/{id}", Name = "GetElectricCostsBySupplier")]
        public async Task<ActionResult<IEnumerable<ElectricCostReadDto>>> GetElectricCostsBySupplier(int id)
        {
            try
            {
                var result = await _repo.GetCostsBySupplierIdAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Kosten vom Anbieter mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<ElectricCostReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage alle Zählerwechsel
        /// Route: /api/strom/zaehlerwechsel
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel")]
        public async Task<ActionResult<IEnumerable<ElectricCounterChangeReadDto>>> GetElectricCounterChanges()
        {
            try
            {
                var result = await _repo.GetCounterChangesAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zählerwechsel vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<ElectricCounterChangeReadDto>>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Zählerwechsel nach Id
        /// Route: /api/strom/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/{id}", Name = "GetElectricCounterChangeById")]
        public async Task<ActionResult<ElectricCounterChangeReadDto>> GetElectricCounterChangeById(int id)
        {
            try
            {
                var result = await _repo.GetCounterChangeByIdAsync(id);
                if (result == null)
                {
                    return NotFound($"Kein Zählerwechsel mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<ElectricCounterChangeReadDto>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Zaehlerwechsel nach Anbieter-Id
        /// Route: /api/strom/zaehlerwechsel/anbieter/1 (ein Anbieter kann mehrere Zählerwechsel haben)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/anbieter/{id}", Name = "GetElectricCounterChangesBySupplier")]
        public async Task<ActionResult<IEnumerable<ElectricCounterChangeReadDto>>> GetElectricCounterChangesBySupplier(int id)
        {
            try
            {
                var result = await _repo.GetCounterChangesBySupplierAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zaehlerwechsel vom Anbieter mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<ElectricCounterChangeReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage aller Kosten (alle Anbieter)
        /// Route: /api/strom/kosten
        /// </summary>
        /// <returns></returns>
        [HttpGet("kosten")]
        public async Task<ActionResult<IEnumerable<ElectricCostReadDto>>> GetElectricCosts()
        {
            try
            {
                var result = await _repo.GetCostsAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Kosten vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<ElectricCostReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }
        #endregion

        #region POST
        /// <summary>
        /// Post: neuen Zählerstand schreiben
        /// Route: /api/strom/zaehlerstand
        /// </summary>
        /// <param name="electricCounterCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerstand")]
        public async Task<ActionResult<ElectricCounterReadDto>> AddCounter(ElectricCounterCreateDto electricCounterCreateDto)
        {
            if (electricCounterCreateDto.Ablesetag == null)
            {
                return BadRequest($"Kein gültiges Datum vorhanden! (Ablesetag={electricCounterCreateDto.Ablesetag})");
            }

            DateTime day = (DateTime)electricCounterCreateDto.Ablesetag;
            var result = await _repo.GetCounterByDateAsync(day);
            if (result != null)
            {
                return BadRequest($"Zählerstand schon vorhanden! (Ablesetag={result!.Ablesetag.GetValueOrDefault():dd.MM.yyyy})");
            }

            var counterModel = _mapper.Map<ElectricCounter>(electricCounterCreateDto);
            if (counterModel == null)
            {
                //var day = (DateTime)counterModel.Ablesetag;
                return BadRequest($"Kein gültiger Zählerstand oder Zählerstand schon vorhanden! (Ablesetag={result!.Ablesetag.GetValueOrDefault():dd.MM.yyyy})");
            }
            await _repo.AddCounterAsync(counterModel);
            await _repo.SaveChangesAsync();
            var counterReadDto = _mapper.Map<ElectricCounterReadDto>(counterModel);
            return CreatedAtRoute(nameof(GetElectricCounterByDay), new { day = counterReadDto.ID_Tag }, counterReadDto);
        }

        /// <summary>
        /// Post: neuen Anbieter schreiben
        /// Route: /api/strom/anbieter
        /// </summary>
        /// <param name="electricTarifCreateDto"></param>
        /// <returns></returns>
        [HttpPost("anbieter")]
        public async Task<ActionResult<ElectricTarifReadDto>> AddElectricSupplier(ElectricTarifCreateDto electricTarifCreateDto)
        {
            var tarifModel = _mapper.Map<ElectricTarif>(electricTarifCreateDto);
            if (tarifModel == null)
            {
                return BadRequest("Kein gültiger Anbieter!");
            }
            await _repo.AddSupplierAsync(tarifModel);
            await _repo.SaveChangesAsync();
            var tarifReadDto = _mapper.Map<ElectricTarifReadDto>(tarifModel);
            return CreatedAtRoute(nameof(GetElectricSupplierById), new { Id = tarifReadDto.Id }, tarifReadDto);
        }

        /// <summary>
        /// Post: neue Zahlung schreiben
        /// Route: /api/strom/zahlung
        /// </summary>
        /// <param name="electricPaymentCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zahlung")]
        public async Task<ActionResult<ElectricPaymentReadDto>> AddElectricPayment(ElectricPaymentCreateDto electricPaymentCreateDto)
        {
            var paymentModel = _mapper.Map<ElectricPayment>(electricPaymentCreateDto);
            if (paymentModel == null)
            {
                return BadRequest("Keine gültige Zahlung!");
            }
            await _repo.AddPaymentAsync(paymentModel);
            await _repo.SaveChangesAsync();
            var paymentReadDto = _mapper.Map<ElectricPaymentReadDto>(paymentModel);
            return CreatedAtRoute(nameof(GetElectricPaymentsBySupplier), new { Id = paymentReadDto.ID_Zahlung }, paymentReadDto);
        }

        /// <summary>
        /// Post: neue Kosten schreiben
        /// Route: /api/strom/kosten
        /// </summary>
        /// <param name="electricCostsCreateDto"></param>
        /// <returns></returns>
        [HttpPost("kosten")]
        public async Task<ActionResult<ElectricCostReadDto>> AddElectricCosts(ElectricCostCreateDto electricCostsCreateDto)
        {
            var costsModel = _mapper.Map<ElectricCost>(electricCostsCreateDto);
            if (costsModel == null)
            {
                return BadRequest("Keinen gültigen Kosten!");
            }
            await _repo.AddCostAsync(costsModel);
            await _repo.SaveChangesAsync();
            var costsReadDto = _mapper.Map<ElectricCostReadDto>(costsModel);
            return CreatedAtRoute(nameof(GetElectricCostsBySupplier), new { costsReadDto.Id }, costsReadDto);
        }

        /// <summary>
        /// Post: neuen Zaehlerwechsel schreiben
        /// Route: /api/strom/zaehlerwechsel
        /// </summary>
        /// <param name="electricCounterChangeCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerwechsel")]
        public async Task<ActionResult<ElectricCounterChangeReadDto>> AddElectricCounterChange(ElectricCounterChangeCreateDto electricCounterChangeCreateDto)
        {
            var counterChangeModel = _mapper.Map<ElectricCounterChange>(electricCounterChangeCreateDto);
            if (counterChangeModel == null)
            {
                return BadRequest("Keinen gültigen Zaehlerwechsel!");
            }
            await _repo.AddCounterChangeAsync(counterChangeModel);
            await _repo.SaveChangesAsync();
            var counterChangeReadDto = _mapper.Map<ElectricCounterChangeReadDto>(counterChangeModel);
            return CreatedAtRoute(nameof(GetElectricCounterChangesBySupplier), new { counterChangeReadDto.Id }, counterChangeReadDto);
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete: Löschen eines Anbieters
        /// Route: /api/strom/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("anbieter/{id:int}")]
        public async Task<ActionResult> DeleteElectricSupplier(int id)
        {
            var tarifModelFromRepo = await _repo.GetSupplierByIdAsync(id);

            if (tarifModelFromRepo == null)
            {
                return NotFound($"Anbieter mit Id= {id} nicht gefunden");
            }
            await _repo.DeleteSupplierAsync(tarifModelFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(tarifModelFromRepo);
        }

        /// <summary>
        /// Delete: Löschen eines Zählerstandes
        /// Route: /api/strom/zaehlerstand/day
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpDelete("zaehlerstand/{day}")]
        public async Task<ActionResult> DeleteCounter(DateTime day)
        {
            var counterModelFromRepo = await _repo.GetCounterByDateAsync(day);
            if (counterModelFromRepo == null)
            {
                return NotFound($"Zähler-Ablesetag mit Datum= {day} nicht gefunden");
            }
            await _repo.DeleteCounterAsync(counterModelFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(counterModelFromRepo);
        }

        /// <summary>
        /// Delete: Löschen einer Zahlung
        /// Route: /api/strom/zahlung/id
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpDelete("zahlung/{id:int}")]
        public async Task<ActionResult> DeletePayment(int id)
        {
            var paymentModelFromRepo = await _repo.GetPaymentByIdAsync(id);
            if (paymentModelFromRepo == null)
            {
                return NotFound($"Zahlung mit Id= {id} nicht gefunden");
            }
            await _repo.DeletePaymentAsync(paymentModelFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(paymentModelFromRepo);
        }

        /// <summary>
        /// Delete: Löschen eines Kostensatzes
        /// Route: /api/strom/kosten/id
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpDelete("kosten/{id:int}")]
        public async Task<ActionResult> DeleteCost(int id)
        {
            var costsModelFromRepo = await _repo.GetCostByIdAsync(id);
            if (costsModelFromRepo == null)
            {
                return NotFound($"Kosten mit Id= {id} nicht gefunden");
            }
            await _repo.DeleteCostAsync(costsModelFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(costsModelFromRepo);
        }

        /// <summary>
        /// Delete: Löschen eines Zaehlerwechsels
        /// Route: /api/strom/zaehlerwechsel/id
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpDelete("zaehlerwechsel/{id:int}")]
        public async Task<ActionResult> DeleteCounterChange(int id)
        {
            var counterChangeModelFromRepo = await _repo.GetCounterChangeByIdAsync(id);
            if (counterChangeModelFromRepo == null)
            {
                return NotFound($"Zaehlerwechsel mit Id= {id} nicht gefunden");
            }
            await _repo.DeleteCounterChangeAsync(counterChangeModelFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(counterChangeModelFromRepo);
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Put: Update eines Anbieters
        /// Route: /api/strom/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="electricTarifUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("anbieter/{id:int}")]
        public async Task<ActionResult> UpdateSupplier(int id, ElectricTarifUpdateDto electricTarifUpdateDto)
        {
            var tarifModelFromRepo = await _repo.GetSupplierByIdAsync(id);
            if (tarifModelFromRepo == null)
            {
                return NotFound($"Anbieter mit Id= {id} nicht gefunden");
            }
            _mapper.Map(electricTarifUpdateDto, tarifModelFromRepo);
            await _repo.UpdateSupplierAsync(tarifModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Zählerstandes nach Datum
        /// Route: /api/strom/zaehlerstand/day
        /// </summary>
        /// <param name="day"></param>
        /// <param name="electricCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/{day}")]
        public async Task<ActionResult> UpdateElectricCounter(DateTime day, ElectricCounterUpdateDto electricCounterUpdateDto)
        {
            var counterModelFromRepo = await _repo.GetCounterByDateAsync(day);
            if (counterModelFromRepo == null)
            {
                return NotFound($"Zaehlerstand mit Datum= {day:dd.MM.yyyy} nicht gefunden");
            }
            _mapper.Map(electricCounterUpdateDto, counterModelFromRepo);
            await _repo.UpdateCounterAsync(counterModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Zählerstandes nach Id
        /// Route: /api/strom/zaehlerstand/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="electricCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/id/{id:int}")]
        public async Task<ActionResult> UpdateElectricCounter(int id, ElectricCounterUpdateDto electricCounterUpdateDto)
        {
            var counterModelFromRepo = await _repo.GetCounterByIdAsync(id);
            if (counterModelFromRepo == null)
            {
                return NotFound($"Zaehlerstand mit Id= {id} nicht gefunden");
            }
            _mapper.Map(electricCounterUpdateDto, counterModelFromRepo);
            await _repo.UpdateCounterAsync(counterModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update einer Zahlung
        /// Route: /api/strom/zahlung/id
        /// </summary>
        /// <param></param>
        /// <param name="electricPaymentUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zahlung/{id}")]
        public async Task<ActionResult> UpdateElectricPayment(int id, ElectricPaymentUpdateDto electricPaymentUpdateDto)
        {
            var paymentModelFromRepo = await _repo.GetPaymentByIdAsync(id);
            if (paymentModelFromRepo == null)
            {
                return NotFound($"Zahlung mit Id= {id} nicht gefunden");
            }
            _mapper.Map(electricPaymentUpdateDto, paymentModelFromRepo);
            await _repo.UpdatePaymentAsync(paymentModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Kostensatzes
        /// Route: /api/strom/kosten/id
        /// </summary>
        /// <param></param>
        /// <param name="electricCostsUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("kosten/{id}")]
        public async Task<ActionResult> UpdateElectricCosts(int id, ElectricCostUpdateDto electricCostsUpdateDto)
        {
            var costsModelFromRepo = await _repo.GetCostByIdAsync(id);
            if (costsModelFromRepo == null)
            {
                return NotFound($"Kosten mit Id= {id} nicht gefunden");
            }
            _mapper.Map(electricCostsUpdateDto, costsModelFromRepo);
            await _repo.UpdateCostAsync(costsModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Zaehlerwechsels
        /// Route: /api/wasser/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="electricCounterChangeUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerwechsel/{id:int}")]
        public async Task<ActionResult> UpdateCounterChange(int id, ElectricCounterChangeUpdateDto electricCounterChangeUpdateDto)
        {
            var counterChangeModelFromRepo = await _repo.GetCounterChangeByIdAsync(id);
            if (counterChangeModelFromRepo == null)
            {
                return NotFound($"Zaehlerwechsel mit Id= {id} nicht gefunden");
            }
            _mapper.Map(electricCounterChangeUpdateDto, counterChangeModelFromRepo);
            await _repo.UpdateCounterChangeAsync(counterChangeModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region PATCH

        /// <summary>
        /// Patch: Patch eines Anbieters
        /// Route: /api/strom/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        /* 
        [
          {
            "op": "replace",
            "path": "/anbieter",
            "value": "muegge"
          }

        ]
        */
        [HttpPatch("anbieter/{id:int}")]
        public async Task<ActionResult> PartialElectricSupplierUpdate(int id, JsonPatchDocument<ElectricTarifUpdateDto> patchDoc)
        {
            var tarifModelFromRepo = await _repo.GetSupplierByIdAsync(id);
            if (tarifModelFromRepo == null)
            {
                return NotFound($"Anbieter mit Id= {id} nicht gefunden");
            }
            var tarifToPatch = _mapper.Map<ElectricTarifUpdateDto>(tarifModelFromRepo);  //CreateMap<Strom_tarif, ElectricTarifUpdateDto>() wird hier benutzt
            patchDoc.ApplyTo(tarifToPatch, ModelState);

            if (!TryValidateModel(tarifToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(tarifToPatch, tarifModelFromRepo);

            await _repo.UpdateSupplierAsync(tarifModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Patch: Patch eines Zählerstandes
        /// Route: /api/strom/zaehlerstand/day
        /// </summary>
        /// <param name="day"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        /* 
       [
         {
           "op": "replace",
           "path": "/bemerkungen",
           "value": "22.07.2021"
         }
       ]
       */
        [HttpPatch("zaehlerstand/{day}")]
        public async Task<ActionResult> PartialElectricCounterUpdate(DateTime day, JsonPatchDocument<ElectricCounterUpdateDto> patchDoc)
        {
            var counterModelFromRepo = await _repo.GetCounterByDateAsync(day);
            if (counterModelFromRepo == null)
            {
                return NotFound($"Zahlerstand mit Datum= {day::dd.MM.yyyy} nicht gefunden");
            }
            var counterToPatch = _mapper.Map<ElectricCounterUpdateDto>(counterModelFromRepo);  //CreateMap<Strom_counter, ElectricCounterUpdateDto>() wird hier benutzt
            patchDoc.ApplyTo(counterToPatch, ModelState);

            if (!TryValidateModel(counterToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(counterToPatch, counterModelFromRepo);

            await _repo.UpdateCounterAsync(counterModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Patch: Patch einer Zahlung
        /// Route: /api/strom/zahlung/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zahlung/{id}")]
        public async Task<ActionResult> PartialElectricPaymentsUpdate(int id, JsonPatchDocument<ElectricPaymentUpdateDto> patchDoc)
        {
            var paymentModelFromRepo = await _repo.GetPaymentByIdAsync(id);
            if (paymentModelFromRepo == null)
            {
                return NotFound($"Zahlung mit Id= {id} nicht gefunden");
            }
            var paymentToPatch = _mapper.Map<ElectricPaymentUpdateDto>(paymentModelFromRepo); //CreateMap<Electric_payment, ElectricPaymentUpdateDto>() wird hier benutzt
            patchDoc.ApplyTo(paymentToPatch, ModelState);

            if (!TryValidateModel(paymentToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(paymentToPatch, paymentModelFromRepo);

            await _repo.UpdatePaymentAsync(paymentModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Patch: Patch eines Kostensatzes
        /// Route: /api/wasser/kosten/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("kosten/{id}")]
        public async Task<ActionResult> PartialElectricCostsUpdate(int id, JsonPatchDocument<ElectricCostUpdateDto> patchDoc)
        {
            var costsModelFromRepo = await _repo.GetCostByIdAsync(id);
            if (costsModelFromRepo == null)
            {
                return NotFound($"Kosten mit Id= {id} nicht gefunden");
            }
            var costsToPatch = _mapper.Map<ElectricCostUpdateDto>(costsModelFromRepo); //CreateMap<Electric_costs, ElectricCostsUpdateDto>() wird hier benutzt
            patchDoc.ApplyTo(costsToPatch, ModelState);

            if (!TryValidateModel(costsToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(costsToPatch, costsModelFromRepo);

            await _repo.UpdateCostAsync(costsModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Patch eines Zaehlerwechsels
        /// Route: /api/strom/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zaehlerwechsel/{id:int}")]
        public async Task<ActionResult> PartialElectricCounterChangeUpdate(int id, JsonPatchDocument<ElectricCounterChangeUpdateDto> patchDoc)
        {
            var counterChangeModelFromRepo = await _repo.GetCounterChangeByIdAsync(id);
            if (counterChangeModelFromRepo == null)
            {
                return NotFound($"Zaehlerwechsel mit Id= {id} nicht gefunden");
            }
            var counterChangeToPatch = _mapper.Map<ElectricCounterChangeUpdateDto>(counterChangeModelFromRepo);
            patchDoc.ApplyTo(counterChangeToPatch, ModelState);

            if (!TryValidateModel(counterChangeToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(counterChangeToPatch, counterChangeModelFromRepo);

            await _repo.UpdateCounterChangeAsync(counterChangeModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}