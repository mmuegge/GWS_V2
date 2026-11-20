using AutoMapper;
using GWS_Api.Dtos.Gas;
using GWS_Api.Models.Gas;
using GWS_Api.Repositories.Gas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/gas")]    // uri mit anderer Route und nicht mit Controllername
    public class GasController(IGasRepository repo, IMapper mapper) : ControllerBase
    {
        #region GET
        #region [HttpGet("anbieter")]
        /// <summary>
        /// Get: Abfrage alle Anbieter
        ///  Route: /api/gas/anbieter
        /// </summary>
        /// <returns></returns>
        [HttpGet("anbieter")]
        [ProducesResponseType(typeof(IEnumerable<GasTarifReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GasTarifReadDto>>> GetSuppliers(CancellationToken cancellationToken)
        {
            var suppliers = await repo.GetSuppliersAsync(cancellationToken);

            if (suppliers is null || !suppliers.Any())
            {
                return Ok(Enumerable.Empty<GasTarifReadDto>());
            }

            var dtos = mapper.Map<IEnumerable<GasTarifReadDto>>(suppliers);
            return Ok(dtos);
        }
        #endregion

        #region [HttpGet("anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage eines bestimmten Anbieters
        /// Route: /api/gas/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(GasTarifReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GasTarifReadDto>> GetSupplierById(int id, CancellationToken cancellationToken)
        {
            var supplier = await repo.GetSupplierByIdAsync(id, cancellationToken);

            if (supplier is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Anbieter nicht gefunden",
                    Detail = $"Kein Anbieter mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var dto = mapper.Map<GasTarifReadDto>(supplier);
            return Ok(dto);
        }
        #endregion

        #region [HttpGet("zaehlerstaende")]
        /// <summary>
        /// Get: Abfrage aller Zählerstände
        /// Route: /api/gas/zaehlerstaende
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerstaende")]
        [ProducesResponseType(typeof(IEnumerable<GasCounterReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GasCounterReadDto>>> GetGasCounters(CancellationToken cancellationToken)
        {
            var counters = await repo.GetCountersAsync(cancellationToken);

            if (counters is null || !counters.Any())
            {
                return Ok(Enumerable.Empty<GasCounterReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<GasCounterReadDto>>(counters));
        }
        #endregion

        #region [HttpGet("zaehlerstaende/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage der Zählerstände eines Anbieters
        /// Route: /api/gas/zaehlerstaende/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstaende/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<GasCounterReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GasCounterReadDto>>> GetGasCounterBySupplier(int id, CancellationToken cancellationToken)
        {
            var counters = await repo.GetCountersBySupplierAsync(id, cancellationToken);

            if (counters is null || !counters.Any())
            {
                return Ok(Enumerable.Empty<GasCounterReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<GasCounterReadDto>>(counters));
        }
        #endregion

        #region [HttpGet("zaehlerstand/{day:datetime}")]
        /// <summary>
        /// Get: Abfrage eines Zählerstandes eines bestimmten Datums
        /// Route: /api/gas/zaehlerstand/day --> (2021.10.23)
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/{day:datetime}")]
        [ProducesResponseType(typeof(GasCounterReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GasCounterReadDto>> GetGasCounterByDay(DateTime day, CancellationToken cancellationToken)
        {
            var counter = await repo.GetCounterByDateAsync(day, cancellationToken);

            if (counter is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zählerstand nicht gefunden",
                    Detail = $"Kein Zählerstand für das Datum {day:yyyy-MM-dd} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(mapper.Map<GasCounterReadDto>(counter));
        }
        #endregion

        #region [HttpGet("zaehlerstand/id/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage eines Zählerstandes mit einer bestimmten Id
        /// Route: /api/gas/zaehlerstand/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/id/{id:int:min(1)}")]
        [ProducesResponseType(typeof(GasCounterReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GasCounterReadDto>> GetGasCounterById(int id, CancellationToken cancellationToken)
        {
            var counter = await repo.GetCounterByIdAsync(id, cancellationToken);

            if (counter is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zählerstand nicht gefunden",
                    Detail = $"Kein Zählerstand mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(mapper.Map<GasCounterReadDto>(counter));
        }
        #endregion

        #region [HttpGet("zahlungen")]
        /// <summary>
        /// Get: Abfrage aller Zahlungen
        /// Route: /api/gas/zahlungen
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungen")]
        [ProducesResponseType(typeof(IEnumerable<GasPaymentReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GasPaymentReadDto>>> GetGasPayments(CancellationToken cancellationToken)
        {
            var payments = await repo.GetPaymentsAsync(cancellationToken);

            if (payments is null || !payments.Any())
            {
                return Ok(Enumerable.Empty<GasPaymentReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<GasPaymentReadDto>>(payments));
        }
        #endregion

        #region [HttpGet("zahlungen/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Zahlungen eines bestimmten Anbieters
        /// Route: /api/gas/zahlungen/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zahlungen/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<GasPaymentReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GasPaymentReadDto>>> GetGasPaymentsBySupplier(int id, CancellationToken cancellationToken)
        {
            var payments = await repo.GetPaymentsBySupplierAsync(id, cancellationToken);

            if (payments is null || !payments.Any())
            {
                return Ok(Enumerable.Empty<GasPaymentReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<GasPaymentReadDto>>(payments));
        }
        #endregion

        #region [HttpGet("zahlungen/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage einer Zahlungen nach Id
        /// Route: /api/strom/zahlungen/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zahlungen/{id:int:min(1)}")]
        [ProducesResponseType(typeof(GasPaymentReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GasPaymentReadDto>> GetGasPaymentById(int id, CancellationToken cancellationToken)
        {
            var payment = await repo.GetPaymentByIdAsync(id, cancellationToken);

            if (payment is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zahlung nicht gefunden",
                    Detail = $"Keine Zahlung mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(mapper.Map<GasPaymentReadDto>(payment));
        }
        #endregion

        #region [HttpGet("zahlungsart")]
        /// <summary>
        /// Get: Abfrage aller Zahlungsarten
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungsart")]
        [ProducesResponseType(typeof(IEnumerable<GasPaymentMethodReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GasPaymentMethodReadDto>>> GetGasPaymentMethods(CancellationToken cancellationToken)
        {
            var methods = await repo.GetPaymentMethodsAsync(cancellationToken);

            if (methods is null || !methods.Any())
            {
                return Ok(Enumerable.Empty<GasPaymentMethodReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<GasPaymentMethodReadDto>>(methods));
        }
        #endregion

        #region [HttpGet("kosten/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Kosten eines Anbieters nach Anbieter-Id (nur vohanden wenn während der Vertragslaufzeit geänderte monatliche Kosten eines Anbieters vorhanden ist)
        /// es können mehr als eine Kostenänderung vorhanden sein
        /// Route: /api/gas/kosten/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("kosten/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<GasCostReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GasCostReadDto>>> GetGasCostsBySupplier(int id, CancellationToken cancellationToken)
        {
            var costs = await repo.GetCostsBySupplierIdAsync(id, cancellationToken);

            if (costs is null || !costs.Any())
            {
                return Ok(Enumerable.Empty<GasCostReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<GasCostReadDto>>(costs));
        }
        #endregion

        #region [HttpGet("zaehlerwechsel")]
        /// <summary>
        /// Get: Abfrage alle Zählerwechsel
        /// Route: /api/gas/zaehlerwechsel
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel")]
        [ProducesResponseType(typeof(IEnumerable<GasCounterChangeReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GasCounterChangeReadDto>>> GetGasCounterChanges(CancellationToken cancellationToken)
        {
            var changes = await repo.GetCounterChangesAsync(cancellationToken);

            if (changes is null || !changes.Any())
            {
                return Ok(Enumerable.Empty<GasCounterChangeReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<GasCounterChangeReadDto>>(changes));
        }
        #endregion

        #region [HttpGet("zaehlerwechsel/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Zählerwechsel nach Id
        /// Route: /api/gas/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(typeof(GasCounterChangeReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GasCounterChangeReadDto>> GetGasCounterChangeById(int id, CancellationToken cancellationToken)
        {
            var change = await repo.GetCounterChangeByIdAsync(id, cancellationToken);

            if (change is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zählerwechsel nicht gefunden",
                    Detail = $"Kein Zählerwechsel mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(mapper.Map<GasCounterChangeReadDto>(change));
        }
        #endregion

        #region [HttpGet("zaehlerwechsel/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Zaehlerwechsel nach Anbieter-Id
        /// Route: /api/gas/zaehlerwechsel/anbieter/1 (ein Anbieter kann mehrere Zählerwechsel haben)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<GasCounterChangeReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GasCounterChangeReadDto>>> GetGasCounterChangesBySupplier(int id, CancellationToken cancellationToken)
        {
            var changes = await repo.GetCounterChangesBySupplierAsync(id, cancellationToken);

            if (changes is null || !changes.Any())
            {
                return Ok(Enumerable.Empty<GasCounterChangeReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<GasCounterChangeReadDto>>(changes));
        }
        #endregion

        #region [HttpGet("kosten")]
        /// <summary>
        /// Get: Abfrage aller Kosten (alle Anbieter)
        /// Route: /api/gas/kosten
        /// </summary>
        /// <returns></returns>
        [HttpGet("kosten")]
        [ProducesResponseType(typeof(IEnumerable<GasCostReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GasCostReadDto>>> GetGasCosts(CancellationToken cancellationToken)
        {
            var costs = await repo.GetCostsAsync(cancellationToken);

            if (costs is null || !costs.Any())
            {
                return Ok(Enumerable.Empty<GasCostReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<GasCostReadDto>>(costs));
        }
        #endregion

        #region [HttpGet("therme")]
        /// <summary>
        /// Get: Abfrage Therme-Daten
        /// Route: /api/gas/therme
        /// </summary>
        /// <returns></returns>
        [HttpGet("therme")]
        [ProducesResponseType(typeof(IEnumerable<GasBoilerReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GasBoilerReadDto>>> GetBoilerData(CancellationToken cancellationToken)
        {
            var boilerData = await repo.GetBoilerDataAsync(cancellationToken);

            if (boilerData is null || !boilerData.Any())
            {
                return Ok(Enumerable.Empty<GasBoilerReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<GasBoilerReadDto>>(boilerData));
        }
        #endregion

        #region [HttpGet("therme/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Therme-Daten nach Id
        /// Route: /api/gas/therme/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("therme/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<GasBoilerReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GasBoilerReadDto>> GetBoilerDataById(int id, CancellationToken cancellationToken)
        {
            var boilerData = await repo.GetBoilerDataByIdAsync(id, cancellationToken);

            if (boilerData is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Therme-Daten nicht gefunden",
                    Detail = $"Keine Therme-Daten mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(mapper.Map<IEnumerable<GasBoilerReadDto>>(boilerData));
        }
        #endregion
        #endregion

        #region POST
        #region [HttpPost("zaehlerstand")]
        /// <summary>
        /// Post: neuen Zählerstand schreiben
        /// Route: /api/gas/zaehlerstand
        /// </summary>
        /// <param name="gasCounterCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerstand")]
        [ProducesResponseType(typeof(GasCounterReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GasCounterReadDto>> AddGasCounter(GasCounterCreateDto gasCounterCreateDto, CancellationToken cancellationToken)
        {
            if (gasCounterCreateDto.Ablesetag is not DateTime day)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Ungültiges Datum",
                    Detail = "Es wurde kein gültiges Ablesedatum angegeben.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var existingCounter = await repo.GetCounterByDateAsync(day, cancellationToken);
            if (existingCounter is not null)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Zählerstand bereits vorhanden",
                    Detail = $"Für den Ablesetag {existingCounter.Ablesetag:dd.MM.yyyy} existiert bereits ein Zählerstand.",
                    Status = StatusCodes.Status409Conflict
                });
            }

            var counterModel = mapper.Map<GasCounter>(gasCounterCreateDto);

            await repo.AddCounterAsync(counterModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var counterReadDto = mapper.Map<GasCounterReadDto>(counterModel);

            return CreatedAtAction(
                nameof(GetGasCounterById),
                new { id = counterReadDto.ID_Tag },
                counterReadDto);
        }
        #endregion

        #region [HttpPost("anbieter")]
        /// <summary>
        /// Post: neuen Anbieter schreiben
        /// Route: /api/gas/anbieter
        /// </summary>
        /// <param name="gasTarifCreateDto"></param>
        /// <returns></returns>
        [HttpPost("anbieter")]
        [ProducesResponseType(typeof(GasTarifReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GasTarifReadDto>> AddGasSupplier(GasTarifCreateDto gasTarifCreateDto, CancellationToken cancellationToken)
        {
            var tarifModel = mapper.Map<GasTarif>(gasTarifCreateDto);

            await repo.AddSupplierAsync(tarifModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var tarifReadDto = mapper.Map<GasTarifReadDto>(tarifModel);

            return CreatedAtAction(
                nameof(GetSupplierById),
                new { id = tarifReadDto.Id },
                tarifReadDto);
        }
        #endregion

        #region [HttpPost("zahlung")]
        /// <summary>
        /// Post: neue Zahlung schreiben
        /// Route: /api/gas/zahlung
        /// </summary>
        /// <param name="gasPaymentCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zahlung")]
        [ProducesResponseType(typeof(GasPaymentReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GasPaymentReadDto>> AddGasPayment(GasPaymentCreateDto gasPaymentCreateDto, CancellationToken cancellationToken)
        {
            var paymentModel = mapper.Map<GasPayment>(gasPaymentCreateDto);

            await repo.AddPaymentAsync(paymentModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var paymentReadDto = mapper.Map<GasPaymentReadDto>(paymentModel);

            return CreatedAtAction(
                nameof(GetGasPaymentById),
                new { id = paymentReadDto.ID_Zahlung },
                paymentReadDto);
        }
        #endregion

        #region [HttpPost("kosten")]
        /// <summary>
        /// Post: neuen Kosten schreiben
        ///  Route: /api/gas/kosten
        /// </summary>
        /// <param name="gasCostsCreateDto"></param>
        /// <returns></returns>
        [HttpPost("kosten")]
        [ProducesResponseType(typeof(GasCostReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GasCostReadDto>> AddGasCosts(GasCostCreateDto gasCostCreateDto, CancellationToken cancellationToken)
        {
            var costModel = mapper.Map<GasCost>(gasCostCreateDto);

            await repo.AddCostAsync(costModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var costReadDto = mapper.Map<GasCostReadDto>(costModel);

            return CreatedAtAction(
                nameof(GetGasCostsBySupplier),
                new { id = costReadDto.Id_Anbieter }, // Wichtig: Hier muss die Anbieter-ID übergeben werden!
                costReadDto);
        }
        #endregion

        #region [HttpPost("zaehlerwechsel")]
        /// <summary>
        /// Post: neuen Zaehlerwechsel schreiben
        /// Route: /api/gas/zaehlerwechsel
        /// </summary>
        /// <param name="gasCounterChangeCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerwechsel")]
        [ProducesResponseType(typeof(GasCounterChangeReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GasCounterChangeReadDto>> AddGasCounterChange(GasCounterChangeCreateDto gasCounterChangeCreateDto, CancellationToken cancellationToken)
        {
            var counterChangeModel = mapper.Map<GasCounterChange>(gasCounterChangeCreateDto);

            await repo.AddCounterChangeAsync(counterChangeModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var counterChangeReadDto = mapper.Map<GasCounterChangeReadDto>(counterChangeModel);

            return CreatedAtAction(
                nameof(GetGasCounterChangeById),
                new { id = counterChangeReadDto.Id },
                counterChangeReadDto);
        }
        #endregion

        #region [HttpPost("therme")]
        /// <summary>
        /// Post: neuen Therme-Daten schreiben
        ///  Route: /api/gas/therme
        /// </summary>
        /// <param name="gasBoilerCreateDto"></param>
        /// <returns></returns>
        [HttpPost("therme")]
        [ProducesResponseType(typeof(GasBoilerReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GasBoilerReadDto>> AddBoilerData(GasBoilerCreateDto gasBoilerDataCreateDto, CancellationToken cancellationToken)
        {
            var boilerModel = mapper.Map<GasBoiler>(gasBoilerDataCreateDto);

            await repo.AddBoilerDataAsync(boilerModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var boilerDataReadDto = mapper.Map<GasBoilerReadDto>(boilerModel);

            return CreatedAtAction(
                nameof(GetBoilerDataById),
                new { id = boilerDataReadDto.Id },
                boilerDataReadDto);
        }
        #endregion
        #endregion

        #region DELETE
        #region [HttpDelete("anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Löscht einen Anbieter anhand seiner ID
        /// Route: /api/gas/anbieter/id
        /// </summary>
        /// <param name="id">Die eindeutige ID des zu löschenden Anbieters.</param>
        /// <returns>Ein HTTP-Statuscode und optional der gelöschte Anbieter.</returns>
        /// <response code="200">Anbieter wurde erfolgreich gelöscht und als Objekt zurückgegeben.</response>
        /// <response code="404">Anbieter mit der angegebenen ID wurde nicht gefunden.</response>
        /// <response code="409">Anbieter kann nicht gelöscht werden, da er noch mit anderen Daten verknüpft ist (z. B. Rechnungen/Zahlungen).</response>
        [HttpDelete("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(GasTarifReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GasTarifReadDto>> DeleteGasSupplier(
            int id,
            CancellationToken cancellationToken)
        {
            var supplier = await repo.GetSupplierByIdAsync(id, cancellationToken);
            if (supplier is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Anbieter nicht gefunden",
                    Detail = $"Kein Anbieter mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            // Mappt vor dem Löschen das DTO für die Rückgabe an Blazor
            var deletedDto = mapper.Map<GasTarifReadDto>(supplier);

            try
            {
                await repo.DeleteSupplierAsync(supplier, cancellationToken);
                await repo.SaveChangesAsync(cancellationToken);

                return Ok(deletedDto);
            }
            catch (DbUpdateException)
            {
                // Gezieltes Abfangen von Foreign-Key-Konflikten für HTTP 409
                return Conflict(new ProblemDetails
                {
                    Title = "Löschen nicht möglich",
                    Detail = "Der Anbieter kann nicht gelöscht werden, da er noch mit anderen Datensätzen verknüpft ist.",
                    Status = StatusCodes.Status409Conflict
                });
            }
        }
        #endregion

        #region [HttpDelete("zaehlerstand/{id:int:min(1)}")]
        /// <summary>
        /// Löscht einen Zählerstand anhand seiner ID.
        /// </summary>
        /// <param name="id">Die eindeutige ID des Zählerstandes.</param>
        /// <returns>Der gelöschte Zählerstand oder ein NotFound-Ergebnis.</returns>
        /// <response code="200">Erfolgreich gelöscht. Gibt das gelöschte Objekt zurück.</response>
        /// <response code="404">Wenn kein Zählerstand mit der ID existiert.</response>
        [HttpDelete("zaehlerstand/{id:int:min(1)}")]
        [ProducesResponseType(typeof(GasCounterReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GasCounterReadDto>> DeleteCounter(int id, CancellationToken cancellationToken)
        {
            var counter = await repo.GetCounterByIdAsync(id, cancellationToken);
            if (counter is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zählerstand nicht gefunden",
                    Detail = $"Kein Zählerstand mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var deletedDto = mapper.Map<GasCounterReadDto>(counter);

            await repo.DeleteCounterAsync(counter, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion

        #region [HttpDelete("zahlung/{id:int:min(1)}")]
        ///// <summary>
        ///// Delete: Löschen einer Zahlung
        ///// Route: /api/gas/zahlung/id
        ///// </summary>
        ///// <param></param>
        ///// <returns></returns>
        [HttpDelete("zahlung/{id:int:min(1)}")]
        [ProducesResponseType(typeof(GasPaymentReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GasPaymentReadDto>> DeletePayment(int id, CancellationToken cancellationToken)
        {
            var payment = await repo.GetPaymentByIdAsync(id, cancellationToken);
            if (payment is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zahlung nicht gefunden",
                    Detail = $"Keine Zahlung mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var deletedDto = mapper.Map<GasPaymentReadDto>(payment);

            await repo.DeletePaymentAsync(payment, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion

        #region [HttpDelete("kosten/{id:int:min(1)}")]
        ///// <summary>
        ///// Delete: Löschen eines Kostensatzes
        ///// Route: /api/gas/kosten/id
        ///// </summary>
        ///// <param></param>
        ///// <returns></returns>
        [HttpDelete("kosten/{id:int:min(1)}")]
        [ProducesResponseType(typeof(GasCostReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GasCostReadDto>> DeleteCost(int id, CancellationToken cancellationToken)
        {
            var cost = await repo.GetCostByIdAsync(id, cancellationToken);
            if (cost is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Kosten nicht gefunden",
                    Detail = $"Keine Kosten mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var deletedDto = mapper.Map<GasCostReadDto>(cost);

            await repo.DeleteCostAsync(cost, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion

        #region [HttpDelete("zaehlerwechsel/{id:int:min(1)}")]
        ///// <summary>
        ///// Delete: Löschen eines Zaehlerwechsels
        ///// Route: /api/gas/zaehlerwechsel/id
        ///// </summary>
        ///// <param></param>
        ///// <returns></returns>
        [HttpDelete("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(typeof(GasCounterChangeReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GasCounterChangeReadDto>> DeleteCounterChange(int id, CancellationToken cancellationToken)
        {
            var counterChange = await repo.GetCounterChangeByIdAsync(id, cancellationToken);
            if (counterChange is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zählerwechsel nicht gefunden",
                    Detail = $"Kein Zählerwechsel mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var deletedDto = mapper.Map<GasCounterChangeReadDto>(counterChange);

            await repo.DeleteCounterChangeAsync(counterChange, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion

        #region [HttpDelete("therme/{id:int:min(1)}")]
        /// <summary>
        /// Delete: Löschen Therme Datensatz
        /// Route: /api/strom/therme/id
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpDelete("therme/{id:int:min(1)}")]
        [ProducesResponseType(typeof(GasBoilerReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GasBoilerReadDto>> DeleteBoilerData(int id, CancellationToken cancellationToken)
        {
            var boilerData = await repo.GetBoilerDataByIdAsync(id, cancellationToken);
            if (boilerData is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Therme-Datensatz nicht gefunden",
                    Detail = $"Kein Therme-Datensatz mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var deletedDto = mapper.Map<GasBoilerReadDto>(boilerData);

            await repo.DeleteBoilerDataAsync(boilerData, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion
        #endregion

        #region UPDATE
        #region [HttpPut("anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update eines Anbieters
        /// Route: /api/gas/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gasTarifUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSupplier(int id, GasTarifUpdateDto gasTarifUpdateDto, CancellationToken cancellationToken)
        {
            var supplier = await repo.GetSupplierByIdAsync(id, cancellationToken);
            if (supplier is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Anbieter nicht gefunden",
                    Detail = $"Kein Anbieter mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            mapper.Map(gasTarifUpdateDto, supplier);

            await repo.UpdateSupplierAsync(supplier, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("zaehlerstand/{day:datetime}")]
        /// <summary>
        /// Put: Update eines Zählerstandes nach Datum
        /// Route: /api/gas/zaehlerstand/day
        /// </summary>
        /// <param name="day"></param>
        /// <param name="gasCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/{day:datetime}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGasCounter(DateTime day, GasCounterUpdateDto gasCounterUpdateDto , CancellationToken cancellationToken)
        {
            var counter = await repo.GetCounterByDateAsync(day, cancellationToken);
            if (counter is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zählerstand nicht gefunden",
                    Detail = $"Kein Zählerstand für das Datum {day:yyyy-MM-dd} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            mapper.Map(gasCounterUpdateDto, counter);

            await repo.UpdateCounterAsync(counter, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("zaehlerstand/id/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update eines Zählerstandes nach Id
        /// Route: /api/gas/zaehlerstand/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gasCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/id/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGasCounterById(int id, GasCounterUpdateDto gasCounterUpdateDto, CancellationToken cancellationToken)
        {
            var counter = await repo.GetCounterByIdAsync(id, cancellationToken);
            if (counter is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zählerstand nicht gefunden",
                    Detail = $"Kein Zählerstand mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            mapper.Map(gasCounterUpdateDto, counter);

            await repo.UpdateCounterAsync(counter, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("zahlung/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update einer Zahlung
        /// Route: /api/gas/zahlung/id
        /// </summary>
        /// <param></param>
        /// <param name="gasPaymentUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zahlung/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGasPayment(int id, GasPaymentUpdateDto gasPaymentUpdateDto, CancellationToken cancellationToken)
        {
            var payment = await repo.GetPaymentByIdAsync(id, cancellationToken);
            if (payment is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zahlung nicht gefunden",
                    Detail = $"Keine Zahlung mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            mapper.Map(gasPaymentUpdateDto, payment);

            await repo.UpdatePaymentAsync(payment, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("kosten/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update eines Kostensatzes
        /// Route: /api/gas/kosten/id
        /// </summary>
        /// <param></param>
        /// <param name="gasCostsUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("kosten/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGasCosts(int id, GasCostUpdateDto gasCostUpdateDto, CancellationToken cancellationToken)
        {
            var cost = await repo.GetCostByIdAsync(id, cancellationToken);
            if (cost is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Kosten nicht gefunden",
                    Detail = $"Keine Kosten mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            mapper.Map(gasCostUpdateDto, cost);

            await repo.UpdateCostAsync(cost, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("zaehlerwechsel/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update eines Zaehlerwechsels
        /// Route: /api/gas/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gasCounterChangeUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCounterChange(int id, GasCounterChangeUpdateDto gasCounterChangeUpdateDto, CancellationToken cancellationToken)
        {
            var counterChange = await repo.GetCounterChangeByIdAsync(id, cancellationToken);
            if (counterChange is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zählerwechsel nicht gefunden",
                    Detail = $"Kein Zählerwechsel mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            mapper.Map(gasCounterChangeUpdateDto, counterChange);

            await repo.UpdateCounterChangeAsync(counterChange, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("therme/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update eines Thermendatensatzes
        /// Route: /api/gas/therme/id
        /// </summary>
        /// <param></param>
        /// <param name="gasBoilerUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("therme/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGasBoilers(int id, GasBoilerUpdateDto gasBoilerUpdateDto, CancellationToken cancellationToken)
        {
            var boilerModelFromRepo = await repo.GetBoilerDataByIdAsync(id, cancellationToken);
            if (boilerModelFromRepo is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Thermedaten nicht gefunden",
                    Detail = $"Keine Thermedaten mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            mapper.Map(gasBoilerUpdateDto, boilerModelFromRepo);

            await repo.UpdateBoilerDataAsync(boilerModelFromRepo, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
        #endregion

        #region PATCH
        #region [HttpPatch("anbieter/{id:int:min(1)}")]
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
        [HttpPatch("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialGasSupplierUpdate(int id, JsonPatchDocument<GasTarifUpdateDto> patchDoc, CancellationToken cancellationToken)
        {
            var supplier = await repo.GetSupplierByIdAsync(id, cancellationToken);
            if (supplier is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Anbieter nicht gefunden",
                    Detail = $"Kein Anbieter mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var tarifToPatch = mapper.Map<GasTarifUpdateDto>(supplier);

            patchDoc.ApplyTo(tarifToPatch, ModelState);

            if (!TryValidateModel(tarifToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(tarifToPatch, supplier);

            await repo.UpdateSupplierAsync(supplier, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPatch("zaehlerstand/{day:datetime}")]
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
        [HttpPatch("zaehlerstand/{day:datetime}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialGasCounterUpdate(DateTime day, JsonPatchDocument<GasCounterUpdateDto> patchDoc, CancellationToken cancellationToken)
        {
            var counter = await repo.GetCounterByDateAsync(day, cancellationToken);
            if (counter is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zählerstand nicht gefunden",
                    Detail = $"Kein Zählerstand für das Datum {day:yyyy-MM-dd} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var counterToPatch = mapper.Map<GasCounterUpdateDto>(counter);

            patchDoc.ApplyTo(counterToPatch, ModelState);

            if (!TryValidateModel(counterToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(counterToPatch, counter);

            await repo.UpdateCounterAsync(counter, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPatch("zahlung/{id:int:min(1)}")]
        /// <summary>
        /// Patch: Patch einer Zahlung
        /// Route: /api/gas/zahlung/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zahlung/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialGasPaymentsUpdate(int id, JsonPatchDocument<GasPaymentUpdateDto> patchDoc, CancellationToken cancellationToken)
        {
            var payment = await repo.GetPaymentByIdAsync(id, cancellationToken);
            if (payment is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zahlung nicht gefunden",
                    Detail = $"Keine Zahlung mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var paymentToPatch = mapper.Map<GasPaymentUpdateDto>(payment);

            patchDoc.ApplyTo(paymentToPatch, ModelState);

            if (!TryValidateModel(paymentToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(paymentToPatch, payment);

            await repo.UpdatePaymentAsync(payment, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPatch("kosten/{id:int:min(1)}")]
        /// <summary>
        /// Patch: Patch eines Kostensatzes
        /// Route: /api/gas/kosten/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("kosten/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialGasCostsUpdate(int id, JsonPatchDocument<GasCostUpdateDto> patchDoc, CancellationToken cancellationToken)
        {
            var cost = await repo.GetCostByIdAsync(id, cancellationToken);
            if (cost is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Kosten nicht gefunden",
                    Detail = $"Keine Kosten mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var costToPatch = mapper.Map<GasCostUpdateDto>(cost);

            patchDoc.ApplyTo(costToPatch, ModelState);

            if (!TryValidateModel(costToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(costToPatch, cost);

            await repo.UpdateCostAsync(cost, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPatch("zaehlerwechsel/{id:int:min(1)}")]
        /// <summary>
        /// Patch eines Zaehlerwechsels
        /// Route: /api/gas/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialGasCounterChangeUpdate(int id, JsonPatchDocument<GasCounterChangeUpdateDto> patchDoc, CancellationToken cancellationToken)
        {
            var counterChange = await repo.GetCounterChangeByIdAsync(id, cancellationToken);
            if (counterChange is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Zählerwechsel nicht gefunden",
                    Detail = $"Kein Zählerwechsel mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var counterChangeToPatch = mapper.Map<GasCounterChangeUpdateDto>(counterChange);

            patchDoc.ApplyTo(counterChangeToPatch, ModelState);

            if (!TryValidateModel(counterChangeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(counterChangeToPatch, counterChange);

            await repo.UpdateCounterChangeAsync(counterChange, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPatch("therme/{id:int:min(1)}")]
        /// <summary>
        /// Patch: Patch eines Thermendatensatz
        /// Route: /api/gas/therme/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("therme/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialGasBoilerUpdate(int id, JsonPatchDocument<GasBoilerUpdateDto> patchDoc, CancellationToken cancellationToken)
        {
            var boilerData = await repo.GetBoilerDataByIdAsync(id, cancellationToken);
            if (boilerData is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Thermedaten nicht gefunden",
                    Detail = $"Keine Thermedaten mit der Id {id} vorhanden.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var boilerToPatch = mapper.Map<GasBoilerUpdateDto>(boilerData);

            patchDoc.ApplyTo(boilerToPatch, ModelState);

            if (!TryValidateModel(boilerToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(boilerToPatch, boilerData);

            await repo.UpdateBoilerDataAsync(boilerData, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
        #endregion
    }
}