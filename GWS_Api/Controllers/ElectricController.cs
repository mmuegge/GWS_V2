using AutoMapper;
using GWS_Api.Dtos.Electric;
using GWS_Api.Models.Electric;
using GWS_Api.Repositories.Electric;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/strom")]    // uri mit anderer Route und nicht mit Controllername
    public class ElectricController(IElectricRepository repo, IMapper mapper) : ControllerBase
    {
        #region GET
        #region [HttpGet("anbieter")]
        /// <summary>
        /// Get: Abfrage alle Anbieter
        /// Route: /api/strom/anbieter
        /// </summary>
        /// <returns></returns>
        [HttpGet("anbieter")]
        [ProducesResponseType(typeof(IEnumerable<ElectricTarifReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectricTarifReadDto>>> GetSuppliers(CancellationToken cancellationToken)
        {
            var suppliers = await repo.GetSuppliersAsync(cancellationToken);

            if (suppliers is null || !suppliers.Any())
            {
                return Ok(Enumerable.Empty<ElectricTarifReadDto>());
            }

            var dtos = mapper.Map<IEnumerable<ElectricTarifReadDto>>(suppliers);
            return Ok(dtos);
        }
        #endregion

        #region [HttpGet("anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage eines bestimmten Anbieters
        /// Route: /api/strom/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(ElectricTarifReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ElectricTarifReadDto>> GetSupplierById(int id, CancellationToken cancellationToken)
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

            var dto = mapper.Map<ElectricTarifReadDto>(supplier);
            return Ok(dto);
        }
        #endregion

        #region [HttpGet("zaehlerstaende")]
        /// <summary>
        /// Get: Abfrage aller Zählerstände
        /// Route: /api/strom/zaehlerstaende
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerstaende")]
        [ProducesResponseType(typeof(IEnumerable<ElectricCounterReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectricCounterReadDto>>> GetCounters(CancellationToken cancellationToken)
        {
            var counters = await repo.GetCountersAsync(cancellationToken);

            if (counters is null || !counters.Any())
            {
                return Ok(Enumerable.Empty<ElectricCounterReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<ElectricCounterReadDto>>(counters));
        }
        #endregion

        #region [HttpGet("zaehlerstaende/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage der Zählerstände eines Anbieters
        /// Route: /api/strom/zaehlerstaende/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstaende/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<ElectricCounterReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectricCounterReadDto>>> GetCounterBySupplier(int id, CancellationToken cancellationToken)
        {
            var counters = await repo.GetCountersBySupplierAsync(id, cancellationToken);

            if (counters is null || !counters.Any())
            {
                return Ok(Enumerable.Empty<ElectricCounterReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<ElectricCounterReadDto>>(counters));
        }
        #endregion

        #region [HttpGet("zaehlerstand/{day:datetime}")]
        /// <summary>
        /// Get: Abfrage eines Zählerstandes eines bestimmten Datums
        /// Route: /api/strom/zaehlerstand/day --> (2021.10.23)
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/{day:datetime}")]
        [ProducesResponseType(typeof(ElectricCounterReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ElectricCounterReadDto>> GetElectricCounterByDay(DateTime day, CancellationToken cancellationToken)
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

            return Ok(mapper.Map<ElectricCounterReadDto>(counter));
        }
        #endregion

        #region [HttpGet("zaehlerstand/id/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage eines Zählerstandes mit einer bestimmten Id
        /// Route: /api/strom/zaehlerstand/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/id/{id:int:min(1)}")]
        [ProducesResponseType(typeof(ElectricCounterReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ElectricCounterReadDto>> GetElectricCounterById(int id, CancellationToken cancellationToken)
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

            return Ok(mapper.Map<ElectricCounterReadDto>(counter));
        }
        #endregion

        #region  [HttpGet("zahlungen")]
        /// <summary>
        /// Get: Abfrage aller Zahlungen
        /// Route: /api/strom/zahlungen
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungen")]
        [ProducesResponseType(typeof(IEnumerable<ElectricPaymentReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectricPaymentReadDto>>> GetElectricPayments(CancellationToken cancellationToken)
        {
            var payments = await repo.GetPaymentsAsync(cancellationToken);

            if (payments is null || !payments.Any())
            {
                return Ok(Enumerable.Empty<ElectricPaymentReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<ElectricPaymentReadDto>>(payments));
        }
        #endregion

        #region [HttpGet("zahlungen/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Zahlungen eines bestimmten Anbieters
        /// Route: /api/strom/zahlungen/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zahlungen/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<ElectricPaymentReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectricPaymentReadDto>>> GetElectricPaymentsBySupplier(int id, CancellationToken cancellationToken)
        {
            var payments = await repo.GetPaymentsBySupplierAsync(id, cancellationToken);

            if (payments is null || !payments.Any())
            {
                return Ok(Enumerable.Empty<ElectricPaymentReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<ElectricPaymentReadDto>>(payments));
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
        [ProducesResponseType(typeof(ElectricPaymentReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ElectricPaymentReadDto>> GetElectricPaymentById(int id, CancellationToken cancellationToken)
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

            return Ok(mapper.Map<ElectricPaymentReadDto>(payment));
        }
        #endregion

        #region [HttpGet("zahlungsart")]
        /// <summary>
        /// Get: Abfrage aller Zahlungsarten
        /// Route: /api/strom/zahlungsart
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungsart")]
        [ProducesResponseType(typeof(IEnumerable<ElectricPaymentMethodReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectricPaymentMethodReadDto>>> GetElectricPaymentMethods(CancellationToken cancellationToken)
        {
            var methods = await repo.GetPaymentMethodsAsync(cancellationToken);

            if (methods is null || !methods.Any())
            {
                return Ok(Enumerable.Empty<ElectricPaymentMethodReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<ElectricPaymentMethodReadDto>>(methods));
        }
        #endregion

        #region [HttpGet("kosten/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Kosten eines Anbieters nach Anbieter-Id (nur vohanden wenn während der Vertragslaufzeit geänderte monatliche Kosten eines Anbieters vorhanden ist)
        /// es können mehr als eine Kostenänderung vorhanden sein
        /// Route: /api/strom/kosten/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("kosten/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<ElectricCostReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectricCostReadDto>>> GetElectricCostsBySupplier(int id, CancellationToken cancellationToken)
        {
            var costs = await repo.GetCostsBySupplierIdAsync(id, cancellationToken);

            if (costs is null || !costs.Any())
            {
                return Ok(Enumerable.Empty<ElectricCostReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<ElectricCostReadDto>>(costs));
        }
        #endregion

        #region [HttpGet("zaehlerwechsel")]
        /// <summary>
        /// Get: Abfrage alle Zählerwechsel
        /// Route: /api/strom/zaehlerwechsel
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel")]
        [ProducesResponseType(typeof(IEnumerable<ElectricCounterChangeReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectricCounterChangeReadDto>>> GetElectricCounterChanges(CancellationToken cancellationToken)
        {
            var changes = await repo.GetCounterChangesAsync(cancellationToken);

            if (changes is null || !changes.Any())
            {
                return Ok(Enumerable.Empty<ElectricCounterChangeReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<ElectricCounterChangeReadDto>>(changes));
        }
        #endregion

        #region [HttpGet("zaehlerwechsel/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Zählerwechsel nach Id
        /// Route: /api/strom/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(typeof(ElectricCounterChangeReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ElectricCounterChangeReadDto>> GetElectricCounterChangeById(int id, CancellationToken cancellationToken)
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

            return Ok(mapper.Map<ElectricCounterChangeReadDto>(change));
        }
        #endregion

        #region [HttpGet("zaehlerwechsel/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Zaehlerwechsel nach Anbieter-Id
        /// Route: /api/strom/zaehlerwechsel/anbieter/1 (ein Anbieter kann mehrere Zählerwechsel haben)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<ElectricCounterChangeReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectricCounterChangeReadDto>>> GetElectricCounterChangesBySupplier(int id, CancellationToken cancellationToken)
        {
            var changes = await repo.GetCounterChangesBySupplierAsync(id, cancellationToken);

            if (changes is null || !changes.Any())
            {
                return Ok(Enumerable.Empty<ElectricCounterChangeReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<ElectricCounterChangeReadDto>>(changes));
        }
        #endregion

        #region [HttpGet("kosten")]
        /// <summary>
        /// Get: Abfrage aller Kosten (alle Anbieter)
        /// Route: /api/strom/kosten
        /// </summary>
        /// <returns></returns>
        [HttpGet("kosten")]
        [ProducesResponseType(typeof(IEnumerable<ElectricCostReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectricCostReadDto>>> GetElectricCosts(CancellationToken cancellationToken)
        {
            var costs = await repo.GetCostsAsync(cancellationToken);

            if (costs is null || !costs.Any())
            {
                return Ok(Enumerable.Empty<ElectricCostReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<ElectricCostReadDto>>(costs));
        }
        #endregion
        #endregion

        #region POST
        #region [HttpPost("zaehlerstand")]
        /// <summary>
        /// Post: neuen Zählerstand schreiben
        /// Route: /api/strom/zaehlerstand
        /// </summary>
        /// <param name="electricCounterCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerstand")]
        [ProducesResponseType(typeof(ElectricCounterReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ElectricCounterReadDto>> AddCounter(ElectricCounterCreateDto electricCounterCreateDto, CancellationToken cancellationToken)
        {
            if (electricCounterCreateDto.Ablesetag is not DateTime day)
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

            var counterModel = mapper.Map<ElectricCounter>(electricCounterCreateDto);

            await repo.AddCounterAsync(counterModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var counterReadDto = mapper.Map<ElectricCounterReadDto>(counterModel);

            return CreatedAtAction(
                nameof(GetElectricCounterById),
                new { id = counterReadDto.ID_Tag },
                counterReadDto);
        }
        #endregion

        #region [HttpPost("anbieter")]
        /// <summary>
        /// Post: neuen Anbieter schreiben
        /// Route: /api/strom/anbieter
        /// </summary>
        /// <param name="electricTarifCreateDto"></param>
        /// <returns></returns>
        [HttpPost("anbieter")]
        [ProducesResponseType(typeof(ElectricTarifReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ElectricTarifReadDto>> AddSupplier(ElectricTarifCreateDto electricTarifCreateDto, CancellationToken cancellationToken)
        {
            var tarifModel = mapper.Map<ElectricTarif>(electricTarifCreateDto);

            await repo.AddSupplierAsync(tarifModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var tarifReadDto = mapper.Map<ElectricTarifReadDto>(tarifModel);

            return CreatedAtAction(
                nameof(GetSupplierById),
                new { id = tarifReadDto.Id },
                tarifReadDto);
        }
        #endregion

        #region [HttpPost("zahlung")]
        /// <summary>
        /// Post: neue Zahlung schreiben
        /// Route: /api/strom/zahlung
        /// </summary>
        /// <param name="electricPaymentCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zahlung")]
        [ProducesResponseType(typeof(ElectricPaymentReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ElectricPaymentReadDto>> AddElectricPayment(ElectricPaymentCreateDto electricPaymentCreateDto, CancellationToken cancellationToken)
        {
            var paymentModel = mapper.Map<ElectricPayment>(electricPaymentCreateDto);

            await repo.AddPaymentAsync(paymentModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var paymentReadDto = mapper.Map<ElectricPaymentReadDto>(paymentModel);

            return CreatedAtAction(
                nameof(GetElectricPaymentById),
                new { id = paymentReadDto.ID_Zahlung },
                paymentReadDto);
        }
        #endregion

        #region [HttpPost("kosten")]
        /// <summary>
        /// Post: neue Kosten schreiben
        /// Route: /api/strom/kosten
        /// </summary>
        /// <param name="electricCostsCreateDto"></param>
        /// <returns></returns>
        [HttpPost("kosten")]
        [ProducesResponseType(typeof(ElectricCostReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ElectricCostReadDto>> AddElectricCosts(ElectricCostCreateDto electricCostCreateDto, CancellationToken cancellationToken)
        {
            var costModel = mapper.Map<ElectricCost>(electricCostCreateDto);

            await repo.AddCostAsync(costModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var costReadDto = mapper.Map<ElectricCostReadDto>(costModel);

            return CreatedAtAction(
                nameof(GetElectricCostsBySupplier),
                new { id = costReadDto.Id_Anbieter }, // Wichtig: Hier muss die Anbieter-ID übergeben werden!
                costReadDto);
        }
        #endregion

        #region [HttpPost("zaehlerwechsel")]
        /// <summary>
        /// Post: neuen Zaehlerwechsel schreiben
        /// Route: /api/strom/zaehlerwechsel
        /// </summary>
        /// <param name="electricCounterChangeCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerwechsel")]
        [ProducesResponseType(typeof(ElectricCounterChangeReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ElectricCounterChangeReadDto>> AddElectricCounterChange(ElectricCounterChangeCreateDto electricCounterChangeCreateDto, CancellationToken cancellationToken)
        {
            var counterChangeModel = mapper.Map<ElectricCounterChange>(electricCounterChangeCreateDto);

            await repo.AddCounterChangeAsync(counterChangeModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var counterChangeReadDto = mapper.Map<ElectricCounterChangeReadDto>(counterChangeModel);

            return CreatedAtAction(
                nameof(GetElectricCounterChangeById),
                new { id = counterChangeReadDto.Id },
                counterChangeReadDto);
        }
        #endregion
        #endregion

        #region DELETE
        #region  [HttpDelete("anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Löscht einen Anbieter anhand seiner ID
        /// Route: /api/strom/anbieter/id
        /// </summary>
        /// <param name="id">Die eindeutige ID des zu löschenden Anbieters.</param>
        /// <returns>Ein HTTP-Statuscode und optional der gelöschte Anbieter.</returns>
        /// <response code="200">Anbieter wurde erfolgreich gelöscht und als Objekt zurückgegeben.</response>
        /// <response code="404">Anbieter mit der angegebenen ID wurde nicht gefunden.</response>
        /// <response code="409">Anbieter kann nicht gelöscht werden, da er noch mit anderen Daten verknüpft ist (z. B. Rechnungen/Zahlungen).</response>
        /// <response code="500">Ein interner Serverfehler ist beim Löschen in der Datenbank aufgetreten.</response>
        [HttpDelete("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(ElectricTarifReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ElectricTarifReadDto>> DeleteSupplier(
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
            var deletedDto = mapper.Map<ElectricTarifReadDto>(supplier);

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
        /// Löscht einen Zählerstand anhand seiner ID
        /// </summary>
        /// <param name="id">Die eindeutige ID des Zählerstandes.</param>
        /// <returns>Der gelöschte Zählerstand oder ein NotFound-Ergebnis.</returns>
        /// <response code="200">Erfolgreich gelöscht. Gibt das gelöschte Objekt zurück.</response>
        /// <response code="404">Wenn kein Zählerstand mit der ID existiert.</response>
        [HttpDelete("zaehlerstand/{id:int:min(1)}")]
        [ProducesResponseType(typeof(ElectricCounterReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ElectricCounterReadDto>> DeleteCounter(int id, CancellationToken cancellationToken)
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

            var deletedDto = mapper.Map<ElectricCounterReadDto>(counter);

            await repo.DeleteCounterAsync(counter, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion

        #region [HttpDelete("zahlung/{id:int:min(1)}")]
        ///// <summary>
        ///// Delete: Löschen einer Zahlung
        ///// Route: /api/strom/zahlung/id
        ///// </summary>
        ///// <param></param>
        ///// <returns></returns>
        [HttpDelete("zahlung/{id:int:min(1)}")]
        [ProducesResponseType(typeof(ElectricPaymentReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ElectricPaymentReadDto>> DeletePayment(int id, CancellationToken cancellationToken)
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

            var deletedDto = mapper.Map<ElectricPaymentReadDto>(payment);

            await repo.DeletePaymentAsync(payment, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion

        #region  [HttpDelete("kosten/{id:int:min(1)}")]
        ///// <summary>
        ///// Delete: Löschen eines Kostensatzes
        ///// Route: /api/strom/kosten/id
        ///// </summary>
        ///// <param></param>
        ///// <returns></returns>
        [HttpDelete("kosten/{id:int:min(1)}")]
        [ProducesResponseType(typeof(ElectricCostReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ElectricCostReadDto>> DeleteCost(int id, CancellationToken cancellationToken)
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

            var deletedDto = mapper.Map<ElectricCostReadDto>(cost);

            await repo.DeleteCostAsync(cost, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion

        #region  [HttpDelete("zaehlerwechsel/{id:int:min(1)}")]
        ///// <summary>
        ///// Delete: Löschen eines Zaehlerwechsels
        ///// Route: /api/strom/zaehlerwechsel/id
        ///// </summary>
        ///// <param></param>
        ///// <returns></returns>
        [HttpDelete("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(typeof(ElectricCounterChangeReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ElectricCounterChangeReadDto>> DeleteCounterChange(int id, CancellationToken cancellationToken)
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

            var deletedDto = mapper.Map<ElectricCounterChangeReadDto>(counterChange);

            await repo.DeleteCounterChangeAsync(counterChange, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion
        #endregion

        #region UPDATE
        #region [HttpPut("anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update eines Anbieters
        /// Route: /api/strom/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="electricTarifUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSupplier(int id, ElectricTarifUpdateDto electricTarifUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(electricTarifUpdateDto, supplier);

            await repo.UpdateSupplierAsync(supplier, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("zaehlerstand/{day:datetime}")]
        /// <summary>
        /// Put: Update eines Zählerstandes nach Datum
        /// Route: /api/strom/zaehlerstand/day
        /// </summary>
        /// <param name="day"></param>
        /// <param name="electricCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/{day:datetime}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateElectricCounter(DateTime day, ElectricCounterUpdateDto electricCounterUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(electricCounterUpdateDto, counter);

            await repo.UpdateCounterAsync(counter, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("zaehlerstand/id/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update eines Zählerstandes nach Id
        /// Route: /api/strom/zaehlerstand/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="electricCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/id/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateElectricCounterById(int id, ElectricCounterUpdateDto electricCounterUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(electricCounterUpdateDto, counter);

            await repo.UpdateCounterAsync(counter, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("zahlung/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update einer Zahlung
        /// Route: /api/strom/zahlung/id
        /// </summary>
        /// <param></param>
        /// <param name="electricPaymentUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zahlung/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateElectricPayment(int id, ElectricPaymentUpdateDto electricPaymentUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(electricPaymentUpdateDto, payment);

            await repo.UpdatePaymentAsync(payment, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("kosten/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update eines Kostensatzes
        /// Route: /api/strom/kosten/id
        /// </summary>
        /// <param></param>
        /// <param name="electricCostsUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("kosten/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateElectricCosts(int id, ElectricCostUpdateDto electricCostUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(electricCostUpdateDto, cost);

            await repo.UpdateCostAsync(cost, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("zaehlerwechsel/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update eines Zaehlerwechsels
        /// Route: /api/wasser/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="electricCounterChangeUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCounterChange(int id, ElectricCounterChangeUpdateDto electricCounterChangeUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(electricCounterChangeUpdateDto, counterChange);

            await repo.UpdateCounterChangeAsync(counterChange, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion
        #endregion

        #region PATCH

        #region [HttpPatch("anbieter/{id:int:min(1)}")]
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

        [HttpPatch("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialSupplierUpdate(int id, JsonPatchDocument<ElectricTarifUpdateDto> patchDoc, CancellationToken cancellationToken)
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

            var tarifToPatch = mapper.Map<ElectricTarifUpdateDto>(supplier);

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
        [HttpPatch("zaehlerstand/{day:datetime}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialCounterUpdate(DateTime day, JsonPatchDocument<ElectricCounterUpdateDto> patchDoc, CancellationToken cancellationToken)
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

            var counterToPatch = mapper.Map<ElectricCounterUpdateDto>(counter);

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
        /// Route: /api/strom/zahlung/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zahlung/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialElectricPaymentsUpdate(int id, JsonPatchDocument<ElectricPaymentUpdateDto> patchDoc, CancellationToken cancellationToken)
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

            var paymentToPatch = mapper.Map<ElectricPaymentUpdateDto>(payment);

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
        /// Route: /api/wasser/kosten/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("kosten/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialElectricCostsUpdate(int id, JsonPatchDocument<ElectricCostUpdateDto> patchDoc, CancellationToken cancellationToken)
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

            var costToPatch = mapper.Map<ElectricCostUpdateDto>(cost);

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
        /// Route: /api/strom/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialElectricCounterChangeUpdate(int id, JsonPatchDocument<ElectricCounterChangeUpdateDto> patchDoc, CancellationToken cancellationToken)
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

            var counterChangeToPatch = mapper.Map<ElectricCounterChangeUpdateDto>(counterChange);

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
        #endregion
    }
}