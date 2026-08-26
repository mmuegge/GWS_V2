using AutoMapper;
using GWS_Api.Dtos.Gas;
using GWS_Api.Models.Gas;
using GWS_Api.Repositories.Gas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GWS_Api.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/gas")]    // uri mit anderer Route und nicht mit Controllername
    public class GasController(ILogger<GasController> logger, IGasRepository repo, IMapper mapper) : ControllerBase
    {
        #region Variablendeklaration
        private readonly IGasRepository _repo = repo;
        private readonly ILogger<GasController> _logger = logger;
        private readonly IMapper _mapper = mapper;   // für Dto's
        #endregion

        #region GET
        /// <summary>
        /// Get: Abfrage alle Anbieter
        ///  Route: /api/gas/anbieter
        /// </summary>
        /// <returns></returns>
        [HttpGet("anbieter")]
        public async Task<ActionResult<IEnumerable<GasTarifReadDto>>> GetSuppliers()
        {
            try
            {
                var result = await _repo.GetSuppliersAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Anbieter vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<GasTarifReadDto>>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage eines bestimmten Anbieters
        /// Route: /api/gas/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("anbieter/{id}", Name = "GetGasSupplierById")]
        public async Task<ActionResult<GasTarifReadDto>> GetGasSupplierById(int id)
        {
            var result = await _repo.GetSupplierByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Kein Anbieter mit Id={id} vorhanden!");
            }
            return Ok(_mapper.Map<GasTarifReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage aller Zählerstände
        /// Route: /api/gas/zaehlerstaende
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerstaende")]
        public async Task<ActionResult<IEnumerable<GasCounterReadDto>>> GetGasCounters()
        {
            try
            {
                var result = await _repo.GetCountersAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zählerstände vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<GasCounterReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage der Zählerstände eines Anbieters
        /// Route: /api/gas/zaehlerstaende/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstaende/anbieter/{id}", Name = "GetGasCounterBySupplier")]
        public async Task<ActionResult<IEnumerable<GasCounterReadDto>>> GetGasCounterBySupplier(int id)
        {
            var result = await _repo.GetCountersBySupplierAsync(id);
            if (result == null || !result.Any())
            {
                return NotFound($"Keine Zaehlerstände vom Anbieter mit Id={id} vorhanden!");
            }
            return Ok(_mapper.Map<IEnumerable<GasCounterReadDto>>(result));
        }

        /// <summary>
        /// Get: Abfrage eines Zählerstandes eines bestimmten Datums
        /// Route: /api/gas/zaehlerstand/day --> (2021.10.23)
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/{day}", Name = "GetGasCounterByDay")]
        public async Task<ActionResult<GasCounterReadDto>> GetGasCounterByDay(DateTime day)
        {
            var result = await _repo.GetCounterByDateAsync(day);
            if (result == null)
            {
                return NotFound($"Kein Zaehlerstand ({day}) vorhanden!");
            }
            return Ok(_mapper.Map<GasCounterReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage eines Zählerstandes mit einer bestimmten Id
        /// Route: /api/gas/zaehlerstand/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/id/{id}", Name = "GetGasCounterById")]
        public async Task<ActionResult<GasCounterReadDto>> GetGasCounterById(int id)
        {
            var result = await _repo.GetCounterByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Kein Zaehlerstand mit Id={id} vorhanden!");
            }
            return Ok(_mapper.Map<GasCounterReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage aller Zahlungen
        /// Route: /api/gas/zahlungen
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungen")]
        public async Task<ActionResult<IEnumerable<GasPaymentReadDto>>> GetGasPayments()
        {
            try
            {
                var result = await _repo.GetPaymentsAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zahlungen vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<GasPaymentReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Zahlungen eines bestimmten Anbieters
        /// Route: /api/gas/zahlungen/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zahlungen/anbieter/{id}", Name = "GetGasPaymentsBySupplier")]
        public async Task<ActionResult<IEnumerable<GasPaymentReadDto>>> GetGasPaymentsBySupplier(int id)
        {
            try
            {
                var result = await _repo.GetPaymentsBySupplierAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound($"Kein Zahlungen vom Anbieter mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<GasPaymentReadDto>>(result));
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
        [HttpGet("zahlungen/{id}", Name = "GetGasPaymentById")]
        public async Task<ActionResult<GasPaymentReadDto>> GetGasPaymentById(int id)
        {
            var result = await _repo.GetPaymentByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Kein Zahlung mit Id={id} vorhanden!");
            }
            return Ok(_mapper.Map<GasPaymentReadDto>(result));
        }

        /// <summary>
        /// Get: Abfrage aller Zahlungsarten
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungsart")]
        public async Task<ActionResult<IEnumerable<GasPaymentMethodReadDto>>> GetGasPaymentMethods()
        {
            try
            {
                var result = await _repo.GetPaymentMethodsAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zahlungsart vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<GasPaymentMethodReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Kosten eines Anbieters nach Anbieter-Id (nur vohanden wenn während der Vertragslaufzeit geänderte monatliche Kosten eines Anbieters vorhanden ist)
        /// es können mehr als eine Kostenänderung vorhanden sein
        /// Route: /api/gas/kosten/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("kosten/anbieter/{id}", Name = "GetGasCostsBySupplier")]
        public async Task<ActionResult<IEnumerable<GasCostReadDto>>> GetGasCostsBySupplier(int id)
        {
            try
            {
                var result = await _repo.GetCostsBySupplierIdAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Kosten vom Anbieter mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<GasCostReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage alle Zählerwechsel
        /// Route: /api/gas/zaehlerwechsel
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel")]
        public async Task<ActionResult<IEnumerable<GasCounterChangeReadDto>>> GetGasCounterChanges()
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
        /// Route: /api/gas/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/{id}", Name = "GetGasCounterChangeById")]
        public async Task<ActionResult<GasCounterChangeReadDto>> GetGasCounterChangeById(int id)
        {
            try
            {
                var result = await _repo.GetCounterChangeByIdAsync(id);
                if (result == null)
                {
                    return NotFound($"Kein Zählerwechsel mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<GasCounterChangeReadDto>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Zaehlerwechsel nach Anbieter-Id
        /// Route: /api/gas/zaehlerwechsel/anbieter/1 (ein Anbieter kann mehrere Zählerwechsel haben)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/anbieter/{id}", Name = "GetGasCounterChangesBySupplier")]
        public async Task<ActionResult<IEnumerable<GasCounterChangeReadDto>>> GetGasCounterChangesBySupplier(int id)
        {
            try
            {
                var result = await _repo.GetCounterChangesBySupplierAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Zaehlerwechsel vom Anbieter mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<GasCounterChangeReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage aller Kosten (alle Anbieter)
        /// Route: /api/gas/kosten
        /// </summary>
        /// <returns></returns>
        [HttpGet("kosten")]
        public async Task<ActionResult<IEnumerable<GasCostReadDto>>> GetGasCosts()
        {
            try
            {
                var result = await _repo.GetCostsAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Kosten vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<GasCostReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Therme-Daten
        /// Route: /api/gas/therme
        /// </summary>
        /// <returns></returns>
        [HttpGet("therme")]
        public async Task<ActionResult<IEnumerable<GasBoilerReadDto>>> GetBoilerData()
        {
            try
            {
                var result = await _repo.GetBoilerDataAsync();
                if (result == null || !result.Any())
                {
                    return NotFound($"Keine Therme-Daten vorhanden!");
                }
                return Ok(_mapper.Map<IEnumerable<GasBoilerReadDto>>(result));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Fehler beim Lesen der Daten aus der Datenbank");
            }
        }

        /// <summary>
        /// Get: Abfrage Therme-Daten nach Id
        /// Route: /api/gas/therme/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("therme/{id}", Name = "GetBoilerDataById")]
        public async Task<ActionResult<GasCostReadDto>> GetBoilerDataById(int id)
        {
            try
            {
                var result = await _repo.GetBoilerDataByIdAsync(id);
                if (result == null)
                {
                    return NotFound($"Kein Therme-Daten mit Id={id} vorhanden!");
                }
                return Ok(_mapper.Map<GasBoilerReadDto>(result));
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
        /// Route: /api/gas/zaehlerstand
        /// </summary>
        /// <param name="gasCounterCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerstand")]
        public async Task<ActionResult<GasCounterReadDto>> AddGasCounter(GasCounterCreateDto gasCounterCreateDto)
        {
            if (gasCounterCreateDto.Ablesetag == null)
            {
                return BadRequest($"Kein gültiges Datum vorhanden! (Ablesetag={gasCounterCreateDto.Ablesetag})");
            }

            DateTime day = (DateTime)gasCounterCreateDto.Ablesetag;
            var result = await _repo.GetCounterByDateAsync(day);
            if (result != null)
            {
                return BadRequest($"Zählerstand schon vorhanden! (Ablesetag={result!.Ablesetag.GetValueOrDefault():dd.MM.yyyy})");
            }

            var counterModel = _mapper.Map<GasCounter>(gasCounterCreateDto);
            if (counterModel == null)
            {
                return BadRequest($"Kein gültiger Zählerstand oder Zählerstand schon vorhanden! (Ablesetag={result!.Ablesetag.GetValueOrDefault():dd.MM.yyyy})");
            }
            await _repo.AddCounterAsync(counterModel);
            await _repo.SaveChangesAsync();
            var counterReadDto = _mapper.Map<GasCounterReadDto>(counterModel);
            return CreatedAtRoute(nameof(GetGasCounterByDay), new { day = counterReadDto.ID_Tag }, counterReadDto);
        }

        /// <summary>
        /// Post: neuen Anbieter schreiben
        /// Route: /api/strom/anbieter
        /// </summary>
        /// <param name="gasTarifCreateDto"></param>
        /// <returns></returns>
        [HttpPost("anbieter")]
        public async Task<ActionResult<GasTarifReadDto>> AddGasSupplier(GasTarifCreateDto gasTarifCreateDto)
        {
            var tarifModel = _mapper.Map<GasTarif>(gasTarifCreateDto);
            if (tarifModel == null)
            {
                return BadRequest("Kein gültiger Anbieter!");
            }
            await _repo.AddSupplierAsync(tarifModel);
            await _repo.SaveChangesAsync();
            var tarifReadDto = _mapper.Map<GasTarifReadDto>(tarifModel);
            return CreatedAtRoute(nameof(GetGasSupplierById), new { Id = tarifReadDto.Id }, tarifReadDto);
        }

        /// <summary>
        /// Post: neue Zahlung schreiben
        /// Route: /api/gas/zahlung
        /// </summary>
        /// <param name="gasPaymentCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zahlung")]
        public async Task<ActionResult<GasPaymentReadDto>> AddGasPayment(GasPaymentCreateDto gasPaymentCreateDto)
        {
            var paymentModel = _mapper.Map<GasPayment>(gasPaymentCreateDto);
            if (paymentModel == null)
            {
                return BadRequest("Keine gültige Zahlung!");
            }
            await _repo.AddPaymentAsync(paymentModel);
            await _repo.SaveChangesAsync();
            var paymentReadDto = _mapper.Map<GasPaymentReadDto>(paymentModel);
            return CreatedAtRoute(nameof(GetGasPaymentsBySupplier), new { Id = paymentReadDto.ID_Zahlung }, paymentReadDto);
        }

        /// <summary>
        /// Post: neuen Kosten schreiben
        ///  Route: /api/gas/kosten
        /// </summary>
        /// <param name="gasCostsCreateDto"></param>
        /// <returns></returns>
        [HttpPost("kosten")]
        public async Task<ActionResult<GasCostReadDto>> AddGasCost(GasCostCreateDto gasCostsCreateDto)
        {
            var costsModel = _mapper.Map<GasCost>(gasCostsCreateDto);
            if (costsModel == null)
            {
                return BadRequest("Keine gültigen Kosten!");
            }
            await _repo.AddCostAsync(costsModel);
            await _repo.SaveChangesAsync();
            var costsReadDto = _mapper.Map<GasCostReadDto>(costsModel);
            return CreatedAtRoute(nameof(GetGasCostsBySupplier), new { costsReadDto.Id }, costsReadDto);
        }

        /// <summary>
        /// Post: neuen Therme-Daten schreiben
        ///  Route: /api/gas/therme
        /// </summary>
        /// <param name="gasBoilerCreateDto"></param>
        /// <returns></returns>
        [HttpPost("therme")]
        public async Task<ActionResult<GasBoilerReadDto>> AddBoilerData(GasBoilerCreateDto gasBoilerDataCreateDto)
        {
            var boilerModel = _mapper.Map<GasBoiler>(gasBoilerDataCreateDto);
            if (boilerModel == null)
            {
                return BadRequest("Keine gültigen Thermen-Daten!");
            }
            await _repo.AddBoilerDataAsync(boilerModel);
            await _repo.SaveChangesAsync();
            var boilerDataReadDto = _mapper.Map<GasBoilerReadDto>(boilerModel);
            return CreatedAtRoute(nameof(GetBoilerDataById), new { boilerDataReadDto.Id }, boilerDataReadDto);
        }

        /// <summary>
        /// Post: neuen Zaehlerwechsel schreiben
        /// Route: /api/gas/zaehlerwechsel
        /// </summary>
        /// <param name="gasCounterChangeCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerwechsel")]
        public async Task<ActionResult<GasCounterChangeReadDto>> AddGasCounterChange(GasCounterChangeCreateDto gasCounterChangeCreateDto)
        {
            var counterChangeModel = _mapper.Map<GasCounterChange>(gasCounterChangeCreateDto);
            if (counterChangeModel == null)
            {
                return BadRequest("Keinen gültigen Zaehlerwechsel!");
            }
            await _repo.AddCounterChangeAsync(counterChangeModel);
            await _repo.SaveChangesAsync();
            var counterChangeReadDto = _mapper.Map<GasCounterChangeReadDto>(counterChangeModel);
            return CreatedAtRoute(nameof(GetGasCounterChangesBySupplier), new { counterChangeReadDto.Id }, counterChangeReadDto);
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete: Löschen eines Anbieters
        /// Route: /api/gas/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("anbieter/{id:int}")]
        public async Task<ActionResult> DeleteGasSupplier(int id)
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
        /// Route: /api/gas/zaehlerstand/day
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
        /// Route: /api/gas/zahlung/id
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
        /// Route: /api/gas/kosten/id
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
        /// Route: /api/gas/zaehlerwechsel/id
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

        /// <summary>
        /// Delete: Löschen Therme Datensatz
        /// Route: /api/strom/therme/id
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpDelete("therme/{id:int}")]
        public async Task<ActionResult> DeleteBoilerData(int id)
        {
            var boilerDataModelFromRepo = await _repo.GetBoilerDataByIdAsync(id);
            if (boilerDataModelFromRepo == null)
            {
                return NotFound($"Therme Datensatz mit Id= {id} nicht gefunden");
            }
            await _repo.DeleteBoilerDataAsync(boilerDataModelFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(boilerDataModelFromRepo);
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Put: Update eines Anbieters
        /// Route: /api/gas/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gasTarifUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("anbieter/{id:int}")]
        public async Task<ActionResult> UpdateSupplier(int id, GasTarifUpdateDto gasTarifUpdateDto)
        {
            var tarifModelFromRepo = await _repo.GetSupplierByIdAsync(id);
            if (tarifModelFromRepo == null)
            {
                return NotFound($"Anbieter mit Id= {id} nicht gefunden");
            }
            _mapper.Map(gasTarifUpdateDto, tarifModelFromRepo);
            await _repo.UpdateSupplierAsync(tarifModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Zählerstandes nach Datum
        /// Route: /api/gas/zaehlerstand/day
        /// </summary>
        /// <param name="day"></param>
        /// <param name="gasCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/{day}")]
        public async Task<ActionResult> UpdateGasCounter(DateTime day, GasCounterUpdateDto gasCounterUpdateDto)
        {
            var counterModelFromRepo = await _repo.GetCounterByDateAsync(day);
            if (counterModelFromRepo == null)
            {
                return NotFound($"Zaehlerstand mit Datum= {day:dd.MM.yyyy} nicht gefunden");
            }
            _mapper.Map(gasCounterUpdateDto, counterModelFromRepo);
            await _repo.UpdateCounterAsync(counterModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Zählerstandes nach Id
        /// Route: /api/gas/zaehlerstand/id
        /// </summary>
        /// <param name="day"></param>
        /// <param name="gasCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/id/{id:int}")]
        public async Task<ActionResult> UpdateGasCounter(int id, GasCounterUpdateDto gasCounterUpdateDto)
        {
            var counterModelFromRepo = await _repo.GetCounterByIdAsync(id);
            if (counterModelFromRepo == null)
            {
                return NotFound($"Zaehlerstand mit Id= {id} nicht gefunden");
            }
            _mapper.Map(gasCounterUpdateDto, counterModelFromRepo);
            await _repo.UpdateCounterAsync(counterModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update einer Zahlung
        /// Route: /api/gas/zahlung/id
        /// </summary>
        /// <param></param>
        /// <param name="gasPaymentUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zahlung/{id}")]
        public async Task<ActionResult> UpdateGasPayment(int id, GasPaymentUpdateDto gasPaymentUpdateDto)
        {
            var paymentModelFromRepo = await _repo.GetPaymentByIdAsync(id);
            if (paymentModelFromRepo == null)
            {
                return NotFound($"Zahlung mit Id= {id} nicht gefunden");
            }
            _mapper.Map(gasPaymentUpdateDto, paymentModelFromRepo);
            await _repo.UpdatePaymentAsync(paymentModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Kostensatzes
        /// Route: /api/gas/kosten/id
        /// </summary>
        /// <param></param>
        /// <param name="gasCostsUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("kosten/{id}")]
        public async Task<ActionResult> UpdateGasCosts(int id, GasCostUpdateDto gasCostsUpdateDto)
        {
            var costsModelFromRepo = await _repo.GetCostByIdAsync(id);
            if (costsModelFromRepo == null)
            {
                return NotFound($"Kosten mit Id= {id} nicht gefunden");
            }
            _mapper.Map(gasCostsUpdateDto, costsModelFromRepo);
            await _repo.UpdateCostAsync(costsModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Thermendatensatzes
        /// Route: /api/gas/therme/id
        /// </summary>
        /// <param></param>
        /// <param name="gasBoilerUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("therme/{id}")]
        public async Task<ActionResult> UpdateGasBoilers(int id, GasBoilerUpdateDto gasBoilerUpdateDto)
        {
            var boilerModelFromRepo = await _repo.GetBoilerDataByIdAsync(id);
            if (boilerModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(gasBoilerUpdateDto, boilerModelFromRepo);
            await _repo.UpdateBoilerDataAsync(boilerModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Put: Update eines Zaehlerwechsels
        /// Route: /api/gas/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gasCounterChangeUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerwechsel/{id:int}")]
        public async Task<ActionResult> UpdateCounterChange(int id, GasCounterChangeUpdateDto gasCounterChangeUpdateDto)
        {
            var counterChangeModelFromRepo = await _repo.GetCounterChangeByIdAsync(id);
            if (counterChangeModelFromRepo == null)
            {
                return NotFound($"Zaehlerwechsel mit Id= {id} nicht gefunden");
            }
            _mapper.Map(gasCounterChangeUpdateDto, counterChangeModelFromRepo);
            await _repo.UpdateCounterChangeAsync(counterChangeModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region PATCH
        /// <summary>
        /// Patch: Patch eines Anbieters
        /// Route: /api/gas/zaehlerwechsel/id
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
        public async Task<ActionResult> PartialGasSupplierUpdate(int id, JsonPatchDocument<GasTarifUpdateDto> patchDoc)
        {
            var tarifModelFromRepo = await _repo.GetSupplierByIdAsync(id);
            if (tarifModelFromRepo == null)
            {
                return NotFound($"Anbieter mit Id= {id} nicht gefunden");
            }
            var tarifToPatch = _mapper.Map<GasTarifUpdateDto>(tarifModelFromRepo);  //CreateMap<Gas_tarif, GasTarifUpdateDto>() wird hier benutzt
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
        /// Route: /api/gas/zaehlerstand/day
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
        public async Task<ActionResult> PartialGasCounterUpdate(DateTime day, JsonPatchDocument<GasCounterUpdateDto> patchDoc)
        {
            var counterModelFromRepo = await _repo.GetCounterByDateAsync(day);
            if (counterModelFromRepo == null)
            {
                return NotFound($"Zahlerstand mit Datum= {day::dd.MM.yyyy} nicht gefunden");
            }
            var counterToPatch = _mapper.Map<GasCounterUpdateDto>(counterModelFromRepo);  //CreateMap<Gas_counter, GasCounterUpdateDto>() wird hier benutzt
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
        /// Route: /api/gas/zahlung/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zahlung/{id}")]
        public async Task<ActionResult> PartialGasPaymentsUpdate(int id, JsonPatchDocument<GasPaymentUpdateDto> patchDoc)
        {
            var paymentModelFromRepo = await _repo.GetPaymentByIdAsync(id);
            if (paymentModelFromRepo == null)
            {
                return NotFound($"Zahlung mit Id= {id} nicht gefunden");
            }
            var paymentToPatch = _mapper.Map<GasPaymentUpdateDto>(paymentModelFromRepo); //CreateMap<Gas_payment, GasPaymentUpdateDto>() wird hier benutzt
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
        /// Route: /api/gas/kosten/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("kosten/{id}")]
        public async Task<ActionResult> PartialGasCostsUpdate(int id, JsonPatchDocument<GasCostUpdateDto> patchDoc)
        {
            var costsModelFromRepo = await _repo.GetCostByIdAsync(id);
            if (costsModelFromRepo == null)
            {
                return NotFound($"Kosten mit Id= {id} nicht gefunden");
            }
            var costsToPatch = _mapper.Map<GasCostUpdateDto>(costsModelFromRepo); //CreateMap<Gas_costs, GasCostsUpdateDto>() wird hier benutzt
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
        /// Route: /api/gas/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zaehlerwechsel/{id:int}")]
        public async Task<ActionResult> PartialGasCounterChangeUpdate(int id, JsonPatchDocument<GasCounterChangeUpdateDto> patchDoc)
        {
            var counterChangeModelFromRepo = await _repo.GetCounterChangeByIdAsync(id);
            if (counterChangeModelFromRepo == null)
            {
                return NotFound($"Zaehlerwechsel mit Id= {id} nicht gefunden");
            }
            var counterChangeToPatch = _mapper.Map<GasCounterChangeUpdateDto>(counterChangeModelFromRepo);
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

        /// <summary>
        /// Patch: Patch eines Thermendatensatz
        /// Route: /api/gas/therme/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("therme/{id}")]
        public async Task<ActionResult> PartialGasBoilerUpdate(int id, JsonPatchDocument<GasBoilerUpdateDto> patchDoc)
        {
            var boilerModelFromRepo = await _repo.GetBoilerDataByIdAsync(id);
            if (boilerModelFromRepo == null)
            {
                return NotFound($"Thermedaten mit Id= {id} nicht gefunden");
            }
            var boilerToPatch = _mapper.Map<GasBoilerUpdateDto>(boilerModelFromRepo);
            patchDoc.ApplyTo(boilerToPatch, ModelState);

            if (!TryValidateModel(boilerToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(boilerToPatch, boilerModelFromRepo);

            await _repo.UpdateBoilerDataAsync(boilerModelFromRepo);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}