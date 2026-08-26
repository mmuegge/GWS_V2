using AutoMapper;
using GWS_Api.Dtos.Gas;
using GWS_Api.Dtos.Water;
using GWS_Api.Models.Water;
using GWS_Api.Repositories.Water;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GWS_Api.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/wasser")]    // uri mit anderer Route und nicht mit Controllername
    public class WaterController(ILogger<WaterController> logger, IWaterRepository repo, IMapper mapper) : ControllerBase
    {
        #region Variablendeklaration
        private readonly IWaterRepository _repo = repo;
        private readonly ILogger<WaterController> _logger = logger;
        private readonly IMapper _mapper = mapper;   // für Dto's
        public WaterModel wasserModel = new();
        #endregion

        #region GET
        /// <summary>
        /// Get: Abfrage alle Anbieter
        /// Route: /api/wasser/anbieter
        /// </summary>
        /// <returns></returns>
        [HttpGet("anbieter")]
        public async Task<ActionResult<IEnumerable<WaterTarifReadDto>>> GetSuppliers()
        {
            try
            {
                var result = await _repo.GetSuppliersAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Anbieter vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<WaterTarifReadDto>>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage eines bestimmten Anbieters
        /// Route: /api/wasser/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("anbieter/{id}", Name = "GetSupplierById")]
        public async Task<ActionResult<WaterTarifReadDto>> GetSupplierById(int id)
        {
            var result = await _repo.GetSupplierByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Kein Anbieter mit Id={id} vorhanden!");
            }
            return Ok(_mapper.Map<WaterTarifReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage aller Zählerstände
        /// Route: /api/wasser/zaehlerstaende
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerstaende")]
        public async Task<ActionResult<IEnumerable<WaterCounterReadDto>>> GetCounters()
        {
            try
            {
                var result = await _repo.GetCountersAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zählerstände vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<WaterCounterReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage der Zählerstände eines Anbieters
        /// Route: /api/wasser/zaehlerstaende/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstaende/anbieter/{id}", Name = "GetCounterBySupplier")]
        public async Task<ActionResult<IEnumerable<WaterCounterReadDto>>> GetCounterBySupplier(int id)
        {
            var result = await _repo.GetCountersBySupplierAsync(id);
            if (result == null || !result.Any())
            {
                return NotFound($"Keine Zaehlerstände vom Anbieter mit Id={id} vorhanden!");
            }
            return Ok(_mapper.Map<IEnumerable<WaterCounterReadDto>>(result));
        }

        /// <summary>
        /// Get: Abfrage eines Zählerstandes eines bestimmten Datums
        /// Route: /api/wasser/zaehlerstand/day --> (2021.10.23)
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/{day}", Name = "GetWaterCounterByDay")]
        public async Task<ActionResult<WaterCounterReadDto>> GetWaterCounterByDay(DateTime day)
        {
            var result = await _repo.GetCounterByDateAsync(day);
            if (result == null)
            {
                return NotFound($"Kein Zaehlerstand ({day}) vorhanden!");
            }
            return Ok(_mapper.Map<WaterCounterReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage eines Zählerstandes mit einer bestimmten Id
        /// Route: /api/wasser/zaehlerstand/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/id/{id}", Name = "GetWaterCounterById")]
        public async Task<ActionResult<WaterCounterReadDto>> GetWaterCounterById(int id)
        {
            var result = await _repo.GetCounterByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Kein Zaehlerstand mit Id={id} vorhanden!");
            }
            return Ok(_mapper.Map<WaterCounterReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage aller Zahlungen
        /// Route: /api/wasser/zahlungen
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungen")]
        public async Task<ActionResult<IEnumerable<WaterPaymentReadDto>>> GetWaterPayments()
        {
            try
            {
                var result = await _repo.GetPaymentsAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zahlungen vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<WaterPaymentReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Zahlungen eines bestimmten Anbieters
        /// Route: /api/wasser/zahlungen/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zahlungen/anbieter/{id}", Name = "GetWaterPaymentsBySupplier")]
        public async Task<ActionResult<IEnumerable<WaterPaymentReadDto>>> GetWaterPaymentsBySupplier(int id)
        {
            try
            {
                var result = await _repo.GetPaymentsBySupplierAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound($"Kein Zahlungen vom Anbieter mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<WaterPaymentReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage einer Zahlungen nach Id
        /// Route: /api/wasser/zahlungen/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zahlungen/{id}", Name = "GetWaterPaymentById")]
        public async Task<ActionResult<WaterPaymentReadDto>> GetWaterPaymentById(int id)
        {
            var result = await _repo.GetPaymentByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Kein Zahlung mit Id={id} vorhanden!");
            }
            return Ok(_mapper.Map<WaterPaymentReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage aller Zahlungsarten
        /// Route: /api/wasser/zahlungsart
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungsart")]
        public async Task<ActionResult<IEnumerable<WaterPaymentMethodReadDto>>> GetWaterPaymentMethods()
        {
            try
            {
                var result = await _repo.GetPaymentMethodsAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zahlungsart vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<WaterPaymentMethodReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Kosten eines Anbieters nach Anbieter-Id (nur vohanden wenn während der Vertragslaufzeit geänderte monatliche Kosten eines Anbieters vorhanden ist)
        /// es können mehr als eine Kostenänderung vorhanden sein
        /// Route: /api/wasser/kosten/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("kosten/anbieter/{id}", Name = "GetWaterCostsBySupplier")]
        public async Task<ActionResult<IEnumerable<WaterCostReadDto>>> GetWaterCostsBySupplier(int id)
        {
            try
            {
                var result = await _repo.GetCostsBySupplierIdAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound($"Kein Kosten vom Anbieter mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<WaterCostReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage alle Zählerwechsel
        /// Route: /api/wasser/zaehlerwechsel
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel")]
        public async Task<ActionResult<IEnumerable<WaterCounterChangeReadDto>>> GetWaterCounterChanges()
        {
            try
            {
                var result = await _repo.GetCounterChangesAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zählerwechsel vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<GasCounterChangeReadDto>>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Zählerwechsel nach Id
        /// Route: /api/wasser/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/{id}", Name = "GetWaterCounterChangeById")]
        public async Task<ActionResult<WaterCounterChangeReadDto>> GetWaterCounterChangeById(int id)
        {
            try
            {
                var result = await _repo.GetCounterChangeByIdAsync(id);
                if (result == null)
                {
                    return NotFound($"Kein Zählerwechsel mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<WaterCounterChangeReadDto>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Zaehlerwechsel nach Anbieter-Id
        /// Route: /api/wasser/zaehlerwechsel/anbieter/1 (ein Anbieter kann mehrere Zählerwechsel haben)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/anbieter/{id}", Name = "GetWaterCounterChangesBySupplier")]
        public async Task<ActionResult<IEnumerable<WaterCounterChangeReadDto>>> GetWaterCounterChangesBySupplier(int id)
        {
            try
            {
                var result = await _repo.GetCounterChangesBySupplierAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zaehlerwechsel vom Anbieter mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<WaterCounterChangeReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage aller Kosten (alle Anbieter)
        /// Route: /api/wasser/kosten
        /// </summary>
        /// <returns></returns>
        [HttpGet("kosten")]
        public async Task<ActionResult<IEnumerable<WaterCostReadDto>>> GetWaterCosts()
        {
            try
            {
                var result = await _repo.GetCostsAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Kosten vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<WaterCostReadDto>>(result));
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
        /// Route: /api/wasser/zaehlerstand
        /// </summary>
        /// <param name="waterCounterCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerstand")]
        public async Task<ActionResult<WaterCounterReadDto>> AddCounter(WaterCounterCreateDto waterCounterCreateDto)
        {
            if (waterCounterCreateDto.Ablesetag == null)
            {
                return BadRequest($"Kein gültiges Datum vorhanden! (Ablesetag={waterCounterCreateDto.Ablesetag})");
            }

            DateTime day = (DateTime)waterCounterCreateDto.Ablesetag;
            var result = await _repo.GetCounterByDateAsync(day);
            if (result != null)
            {
                return BadRequest($"Zählerstand schon vorhanden! (Ablesetag={result!.Ablesetag:dd.MM.yyyy})");
            }

            var counterModel = _mapper.Map<WaterCounter>(waterCounterCreateDto);
            if (counterModel == null)
            {
                return BadRequest($"Kein gültiger Zählerstand oder Zählerstand schon vorhanden! (Ablesetag={result!.Ablesetag:dd.MM.yyyy})");
            }
            await _repo.AddCounterAsync(counterModel);
            await _repo.SaveChangesAsync();
            var counterReadDto = _mapper.Map<WaterCounterReadDto>(counterModel);
            return CreatedAtRoute(nameof(GetWaterCounterByDay), new { day = counterReadDto.ID_Tag }, counterReadDto);
        }

        /// <summary>
        /// Post: neuen Anbieter schreiben
        /// Route: /api/wasser/anbieter
        /// </summary>
        /// <param name="waterTarifCreateDto"></param>
        /// <returns></returns>
        [HttpPost("anbieter")]
        public async Task<ActionResult<WaterTarifReadDto>> AddSupplier(WaterTarifCreateDto waterTarifCreateDto)
        {
            var tarifModel = _mapper.Map<WaterTarif>(waterTarifCreateDto);
            if (tarifModel == null)
            {
                return BadRequest("Kein gültiger Anbieter!");
            }
            await _repo.AddSupplierAsync(tarifModel);
            await _repo.SaveChangesAsync();
            var tarifReadDto = _mapper.Map<WaterTarifReadDto>(tarifModel);
            return CreatedAtRoute(nameof(GetSupplierById), new { Id = tarifReadDto.Id }, tarifReadDto);
        }

        /// <summary>
        /// Post: neue Zahlung schreiben
        /// Route: /api/wasser/zahlung
        /// </summary>
        /// <param name="waterPaymentCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zahlung")]
        public async Task<ActionResult<WaterPaymentReadDto>> AddGasPayment(WaterPaymentCreateDto waterPaymentCreateDto)
        {
            var paymentModel = _mapper.Map<WaterPayment>(waterPaymentCreateDto);
            if (paymentModel == null)
            {
                return BadRequest("Keine gültige Zahlung!");
            }
            await _repo.AddPaymentAsync(paymentModel);
            await _repo.SaveChangesAsync();
            var paymentReadDto = _mapper.Map<WaterPaymentReadDto>(paymentModel);
            return CreatedAtRoute(nameof(GetWaterPaymentsBySupplier), new { Id = paymentReadDto.ID_Zahlung }, paymentReadDto);
        }

        /// <summary>
        /// Post: neue Kosten schreiben
        /// Route: /api/wasser/kosten
        /// </summary>
        /// <param name="waterCostsCreateDto"></param>
        /// <returns></returns>
        [HttpPost("kosten")]
        public async Task<ActionResult<WaterCostReadDto>> AddWaterCosts(WaterCostCreateDto waterCostsCreateDto)
        {
            var costsModel = _mapper.Map<WaterCost>(waterCostsCreateDto);
            if (costsModel == null)
            {
                return BadRequest("Keinen gültigen Kosten vorhanden!");
            }
            await _repo.AddCostAsync(costsModel);
            await _repo.SaveChangesAsync();
            var costsReadDto = _mapper.Map<WaterCostReadDto>(costsModel);
            return CreatedAtRoute(nameof(GetWaterCostsBySupplier), new { costsReadDto.Id }, costsReadDto);
        }

        /// <summary>
        /// Post: neuen Zaehlerwechsel schreiben
        /// Route: /api/wasser/zaehlerwechsel
        /// </summary>
        /// <param name="waterCounterChangeCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerwechsel")]
        public async Task<ActionResult<WaterCounterChangeReadDto>> AddWaterCounterChange(WaterCounterChangeCreateDto waterCounterChangeCreateDto)
        {
            var counterChangeModel = _mapper.Map<WaterCounterChange>(waterCounterChangeCreateDto);
            if (counterChangeModel == null)
            {
                return BadRequest("Keinen gültigen Zaehlerwechsel!");
            }
            await _repo.AddCounterChangeAsync(counterChangeModel);
            await _repo.SaveChangesAsync();
            var counterChangeReadDto = _mapper.Map<WaterCounterChangeReadDto>(counterChangeModel);
            return CreatedAtRoute(nameof(GetWaterCounterChangesBySupplier), new { counterChangeReadDto.Id }, counterChangeReadDto);
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete: Löschen eines Anbieter
        /// Route: /api/wasser/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("anbieter/{id:int}")]
        public async Task<ActionResult> DeleteSupplier(int id)
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
        /// Route: /api/wasser/zaehlerstand/day
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
        /// Route: /api/wasser/zahlung/id
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
        /// Route: /api/wasser/kosten/id
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
        /// Route: /api/wasser/zaehlerwechsel/id
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
        /// Route: /api/wasser/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="waterTarifUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("anbieter/{id:int}")]
        public async Task<ActionResult> UpdateSupplier(int id, WaterTarifUpdateDto waterTarifUpdateDto)
        {
            var tarifModelFromRepo = await _repo.GetSupplierByIdAsync(id);
            if (tarifModelFromRepo == null)
            {
                return NotFound($"Anbieter mit Id= {id} nicht gefunden");
            }
            _mapper.Map(waterTarifUpdateDto, tarifModelFromRepo);
            await _repo.UpdateSupplierAsync(tarifModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Zählerstandes nach Datum
        /// </summary>
        /// <param name="day"></param>
        /// <param name="waterCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/{day}")]
        public async Task<ActionResult> UpdateWaterCounter(DateTime day, WaterCounterUpdateDto waterCounterUpdateDto)
        {
            var counterModelFromRepo = await _repo.GetCounterByDateAsync(day);
            if (counterModelFromRepo == null)
            {
                return NotFound($"Zaehlerstand mit Datum= {day:dd.MM.yyyy} nicht gefunden");
            }
            _mapper.Map(waterCounterUpdateDto, counterModelFromRepo);
            await _repo.UpdateCounterAsync(counterModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Zählerstandes nach Id
        /// </summary>
        /// <param name="day"></param>
        /// <param name="waterCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/id/{id:int}")]
        public async Task<ActionResult> UpdateWaterCounter(int id, WaterCounterUpdateDto waterCounterUpdateDto)
        {
            var counterModelFromRepo = await _repo.GetCounterByIdAsync(id);
            if (counterModelFromRepo == null)
            {
                return NotFound($"Zaehlerstand mit Id= {id} nicht gefunden");
            }
            _mapper.Map(waterCounterUpdateDto, counterModelFromRepo);
            await _repo.UpdateCounterAsync(counterModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update einer Zahlung
        /// </summary>
        /// <param></param>
        /// <param name="waterPaymentUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zahlung/{id}")]
        public async Task<ActionResult> UpdateWaterPayment(int id, WaterPaymentUpdateDto waterPaymentUpdateDto)
        {
            var paymentModelFromRepo = await _repo.GetPaymentByIdAsync(id);
            if (paymentModelFromRepo == null)
            {
                return NotFound($"Zahlung mit Id= {id} nicht gefunden");
            }
            _mapper.Map(waterPaymentUpdateDto, paymentModelFromRepo);
            await _repo.UpdatePaymentAsync(paymentModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Kostensatzes
        /// Route: /api/wasser/kosten/id
        /// </summary>
        /// <param></param>
        /// <param name="waterCostsUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("kosten/{id}")]
        public async Task<ActionResult> UpdateWaterCosts(int id, WaterCostUpdateDto waterCostsUpdateDto)
        {
            var costsModelFromRepo = await _repo.GetCostByIdAsync(id);
            if (costsModelFromRepo == null)
            {
                return NotFound($"Kosten mit Id= {id} nicht gefunden");
            }
            _mapper.Map(waterCostsUpdateDto, costsModelFromRepo);
            await _repo.UpdateCostAsync(costsModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Zaehlerwechsels
        /// Route: /api/wasser/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="waterCounterChangeUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerwechsel/{id:int}")]
        public async Task<ActionResult> UpdateCounterChange(int id, WaterCounterChangeUpdateDto waterCounterChangeUpdateDto)
        {
            var counterChangeModelFromRepo = await _repo.GetCounterChangeByIdAsync(id);
            if (counterChangeModelFromRepo == null)
            {
                return NotFound($"Zaehlerwechsel mit Id= {id} nicht gefunden");
            }
            _mapper.Map(waterCounterChangeUpdateDto, counterChangeModelFromRepo);
            await _repo.UpdateCounterChangeAsync(counterChangeModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region PATCH
        /// <summary>
        /// Patch: Patch eines Anbieters
        /// Route: /api/water/anbieter/id
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
        public async Task<ActionResult> PartialSupplierUpdate(int id, JsonPatchDocument<WaterTarifUpdateDto> patchDoc)
        {
            var tarifModelFromRepo = await _repo.GetSupplierByIdAsync(id);
            if (tarifModelFromRepo == null)
            {
                return NotFound($"Anbieter mit Id= {id} nicht gefunden");
            }
            var tarifToPatch = _mapper.Map<WaterTarifUpdateDto>(tarifModelFromRepo);  //CreateMap<Wasser_tarif, WaterTarifUpdateDto>() wird hier benutzt
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
        /// Route: /api/wasser/zaehlerstand/day
        /// </summary>
        /// <param name="day"></param>
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
        [HttpPatch("zaehlerstand/{day}")]
        public async Task<ActionResult> PartialCounterUpdate(DateTime day, JsonPatchDocument<WaterCounterUpdateDto> patchDoc)
        {
            var counterModelFromRepo = await _repo.GetCounterByDateAsync(day);
            if (counterModelFromRepo == null)
            {
                return NotFound($"Zahlerstand mit Datum= {day::dd.MM.yyyy} nicht gefunden");
            }
            var counterToPatch = _mapper.Map<WaterCounterUpdateDto>(counterModelFromRepo);  //CreateMap<Water_counter, WaterCounterUpdateDto>() wird hier benutzt
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
        /// Route: /api/wasser/zahlung/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zahlung/{id}")]
        public async Task<ActionResult> PartialWaterPaymentsUpdate(int id, JsonPatchDocument<WaterPaymentUpdateDto> patchDoc)
        {
            var paymentModelFromRepo = await _repo.GetPaymentByIdAsync(id);
            if (paymentModelFromRepo == null)
            {
                return NotFound($"Zahlung mit Id= {id} nicht gefunden");
            }
            var paymentToPatch = _mapper.Map<WaterPaymentUpdateDto>(paymentModelFromRepo); //CreateMap<Water_payment, WaterPaymentUpdateDto>() wird hier benutzt
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
        public async Task<ActionResult> PartialWaterCostsUpdate(int id, JsonPatchDocument<WaterCostUpdateDto> patchDoc)
        {
            var costsModelFromRepo = await _repo.GetCostByIdAsync(id);
            if (costsModelFromRepo == null)
            {
                return NotFound($"Kosten mit Id= {id} nicht gefunden");
            }
            var costsToPatch = _mapper.Map<WaterCostUpdateDto>(costsModelFromRepo); //CreateMap<Water_costs, WaterCostsUpdateDto>() wird hier benutzt
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
        /// Route: /api/wasser/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zaehlerwechsel/{id:int}")]
        public async Task<ActionResult> PartialWaterCounterChangeUpdate(int id, JsonPatchDocument<WaterCounterChangeUpdateDto> patchDoc)
        {
            var counterChangeModelFromRepo = await _repo.GetCounterChangeByIdAsync(id);
            if (counterChangeModelFromRepo == null)
            {
                return NotFound($"Zaehlerwechsel mit Id= {id} nicht gefunden");
            }
            var counterChangeToPatch = _mapper.Map<WaterCounterChangeUpdateDto>(counterChangeModelFromRepo);
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