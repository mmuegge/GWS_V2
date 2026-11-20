using AutoMapper;
using GWS_Api.Dtos.Water;
using GWS_Api.Models.Water;
using GWS_Api.Repositories.Water;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/wasser")]    // uri mit anderer Route und nicht mit Controllername
    public class WaterController(IWaterRepository repo, IMapper mapper) : ControllerBase
    {
        #region GET
        #region [HttpGet("anbieter")]
        ///// <summary>
        ///// Get: Abfrage alle Anbieter
        ///// Route: /api/wasser/anbieter
        ///// </summary>
        ///// <returns></returns>
        [HttpGet("anbieter")]
        [ProducesResponseType(typeof(IEnumerable<WaterTarifReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WaterTarifReadDto>>> GetSuppliers(CancellationToken cancellationToken)
        {
            var suppliers = await repo.GetSuppliersAsync(cancellationToken);

            if (suppliers is null || !suppliers.Any())
            {
                return Ok(Enumerable.Empty<WaterTarifReadDto>());
            }

            var dtos = mapper.Map<IEnumerable<WaterTarifReadDto>>(suppliers);
            return Ok(dtos);
        }
        #endregion

        #region [HttpGet("anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage eines bestimmten Anbieters
        /// Route: /api/wasser/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(WaterTarifReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WaterTarifReadDto>> GetSupplierById(int id, CancellationToken cancellationToken)
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

            var dto = mapper.Map<WaterTarifReadDto>(supplier);
            return Ok(dto);
        }
        #endregion

        #region [HttpGet("zaehlerstaende")]
        /// <summary>
        /// Get: Abfrage aller Zählerstände
        /// Route: /api/wasser/zaehlerstaende
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerstaende")]
        [ProducesResponseType(typeof(IEnumerable<WaterCounterReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WaterCounterReadDto>>> GetCounters(CancellationToken cancellationToken)
        {
            var counters = await repo.GetCountersAsync(cancellationToken);

            if (counters is null || !counters.Any())
            {
                return Ok(Enumerable.Empty<WaterCounterReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<WaterCounterReadDto>>(counters));
        }
        #endregion

        #region [HttpGet("zaehlerstaende/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage der Zählerstände eines Anbieters
        /// Route: /api/wasser/zaehlerstaende/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstaende/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<WaterCounterReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WaterCounterReadDto>>> GetCounterBySupplier(int id, CancellationToken cancellationToken)
        {
            var counters = await repo.GetCountersBySupplierAsync(id, cancellationToken);

            if (counters is null || !counters.Any())
            {
                return Ok(Enumerable.Empty<WaterCounterReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<WaterCounterReadDto>>(counters));
        }
        #endregion

        #region [HttpGet("zaehlerstand/{day:datetime}")]
        /// <summary>
        /// Get: Abfrage eines Zählerstandes eines bestimmten Datums
        /// Route: /api/wasser/zaehlerstand/day --> (2021.10.23)
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/{day:datetime}")]
        [ProducesResponseType(typeof(WaterCounterReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WaterCounterReadDto>> GetWaterCounterByDay(DateTime day, CancellationToken cancellationToken)
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

            return Ok(mapper.Map<WaterCounterReadDto>(counter));
        }
        #endregion

        #region [HttpGet("zaehlerstand/id/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage eines Zählerstandes mit einer bestimmten Id
        /// Route: /api/wasser/zaehlerstand/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerstand/id/{id:int:min(1)}")]
        [ProducesResponseType(typeof(WaterCounterReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WaterCounterReadDto>> GetWaterCounterById(int id, CancellationToken cancellationToken)
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

            return Ok(mapper.Map<WaterCounterReadDto>(counter));
        }
        #endregion

        #region [HttpGet("zahlungen")]
        /// <summary>
        /// Get: Abfrage aller Zahlungen
        /// Route: /api/wasser/zahlungen
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungen")]
        [ProducesResponseType(typeof(IEnumerable<WaterPaymentReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WaterPaymentReadDto>>> GetWaterPayments(CancellationToken cancellationToken)
        {
            var payments = await repo.GetPaymentsAsync(cancellationToken);

            if (payments is null || !payments.Any())
            {
                return Ok(Enumerable.Empty<WaterPaymentReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<WaterPaymentReadDto>>(payments));
        }
        #endregion

        #region [HttpGet("zahlungen/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Zahlungen eines bestimmten Anbieters
        /// Route: /api/wasser/zahlungen/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zahlungen/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<WaterPaymentReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WaterPaymentReadDto>>> GetWaterPaymentsBySupplier(int id, CancellationToken cancellationToken)
        {
            var payments = await repo.GetPaymentsBySupplierAsync(id, cancellationToken);

            if (payments is null || !payments.Any())
            {
                return Ok(Enumerable.Empty<WaterPaymentReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<WaterPaymentReadDto>>(payments));
        }
        #endregion

        #region [HttpGet("zahlungen/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage einer Zahlungen nach Id
        /// Route: /api/wasser/zahlungen/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zahlungen/{id:int:min(1)}")]
        [ProducesResponseType(typeof(WaterPaymentReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WaterPaymentReadDto>> GetWaterPaymentById(int id, CancellationToken cancellationToken)
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

            return Ok(mapper.Map<WaterPaymentReadDto>(payment));
        }
        #endregion

        #region [HttpGet("zahlungsart")]
        /// <summary>
        /// Get: Abfrage aller Zahlungsarten
        /// Route: /api/wasser/zahlungsart
        /// </summary>
        /// <returns></returns>
        [HttpGet("zahlungsart")]
        [ProducesResponseType(typeof(IEnumerable<WaterPaymentMethodReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WaterPaymentMethodReadDto>>> GetWaterPaymentMethods(CancellationToken cancellationToken)
        {
            var methods = await repo.GetPaymentMethodsAsync(cancellationToken);

            if (methods is null || !methods.Any())
            {
                return Ok(Enumerable.Empty<WaterPaymentMethodReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<WaterPaymentMethodReadDto>>(methods));
        }
        #endregion

        #region [HttpGet("kosten/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Kosten eines Anbieters nach Anbieter-Id (nur vohanden wenn während der Vertragslaufzeit geänderte monatliche Kosten eines Anbieters vorhanden ist)
        /// es können mehr als eine Kostenänderung vorhanden sein
        /// Route: /api/wasser/kosten/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("kosten/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<WaterCostReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WaterCostReadDto>>> GetWaterCostsBySupplier(int id, CancellationToken cancellationToken)
        {
            var costs = await repo.GetCostsBySupplierIdAsync(id, cancellationToken);

            if (costs is null || !costs.Any())
            {
                return Ok(Enumerable.Empty<WaterCostReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<WaterCostReadDto>>(costs));
        }
        #endregion

        #region [HttpGet("zaehlerwechsel")]
        /// <summary>
        /// Get: Abfrage alle Zählerwechsel
        /// Route: /api/wasser/zaehlerwechsel
        /// </summary>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel")]
        [ProducesResponseType(typeof(IEnumerable<WaterCounterChangeReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WaterCounterChangeReadDto>>> GetWaterCounterChanges(CancellationToken cancellationToken)
        {
            var changes = await repo.GetCounterChangesAsync(cancellationToken);

            if (changes is null || !changes.Any())
            {
                return Ok(Enumerable.Empty<WaterCounterChangeReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<WaterCounterChangeReadDto>>(changes));
        }
        #endregion

        #region [HttpGet("zaehlerwechsel/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Zählerwechsel nach Id
        /// Route: /api/wasser/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(typeof(WaterCounterChangeReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WaterCounterChangeReadDto>> GetWaterCounterChangeById(int id, CancellationToken cancellationToken)
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

            return Ok(mapper.Map<WaterCounterChangeReadDto>(change));
        }
        #endregion

        #region [HttpGet("zaehlerwechsel/anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Get: Abfrage Zaehlerwechsel nach Anbieter-Id
        /// Route: /api/wasser/zaehlerwechsel/anbieter/1 (ein Anbieter kann mehrere Zählerwechsel haben)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("zaehlerwechsel/anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(IEnumerable<WaterCounterChangeReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WaterCounterChangeReadDto>>> GetWaterCounterChangesBySupplier(int id, CancellationToken cancellationToken)
        {
            var changes = await repo.GetCounterChangesBySupplierAsync(id, cancellationToken);

            if (changes is null || !changes.Any())
            {
                return Ok(Enumerable.Empty<WaterCounterChangeReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<WaterCounterChangeReadDto>>(changes));
        }
        #endregion

        #region [HttpGet("kosten")]
        /// <summary>
        /// Get: Abfrage aller Kosten (alle Anbieter)
        /// Route: /api/wasser/kosten
        /// </summary>
        /// <returns></returns>
        [HttpGet("kosten")]
        [ProducesResponseType(typeof(IEnumerable<WaterCostReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WaterCostReadDto>>> GetWaterCosts(CancellationToken cancellationToken)
        {
            var costs = await repo.GetCostsAsync(cancellationToken);

            if (costs is null || !costs.Any())
            {
                return Ok(Enumerable.Empty<WaterCostReadDto>());
            }

            return Ok(mapper.Map<IEnumerable<WaterCostReadDto>>(costs));
        }
        #endregion
        #endregion

        #region POST
        #region [HttpPost("zaehlerstand")]
        /// <summary>
        /// Post: neuen Zählerstand schreiben
        /// Route: /api/wasser/zaehlerstand
        /// </summary>
        /// <param name="waterCounterCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerstand")]
        [ProducesResponseType(typeof(WaterCounterReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<WaterCounterReadDto>> AddCounter(WaterCounterCreateDto waterCounterCreateDto, CancellationToken cancellationToken)
        {
            if (waterCounterCreateDto.Ablesetag is not DateTime day)
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

            var counterModel = mapper.Map<WaterCounter>(waterCounterCreateDto);

            await repo.AddCounterAsync(counterModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var counterReadDto = mapper.Map<WaterCounterReadDto>(counterModel);

            return CreatedAtAction(
                nameof(GetWaterCounterById),
                new { id = counterReadDto.ID_Tag },
                counterReadDto);
        }
        #endregion

        #region [HttpPost("anbieter")]
        /// <summary>
        /// Post: neuen Anbieter schreiben
        /// Route: /api/wasser/anbieter
        /// </summary>
        /// <param name="waterTarifCreateDto"></param>
        /// <returns></returns>
        [HttpPost("anbieter")]
        [ProducesResponseType(typeof(WaterTarifReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WaterTarifReadDto>> AddSupplier(WaterTarifCreateDto waterTarifCreateDto, CancellationToken cancellationToken)
        {
            var tarifModel = mapper.Map<WaterTarif>(waterTarifCreateDto);

            await repo.AddSupplierAsync(tarifModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var tarifReadDto = mapper.Map<WaterTarifReadDto>(tarifModel);

            return CreatedAtAction(
                nameof(GetSupplierById),
                new { id = tarifReadDto.Id },
                tarifReadDto);
        }
        #endregion

        #region [HttpPost("zahlung")]
        /// <summary>
        /// Post: neue Zahlung schreiben
        /// Route: /api/wasser/zahlung
        /// </summary>
        /// <param name="waterPaymentCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zahlung")]
        [ProducesResponseType(typeof(WaterPaymentReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WaterPaymentReadDto>> AddWaterPayment(WaterPaymentCreateDto waterPaymentCreateDto, CancellationToken cancellationToken)
        {
            var paymentModel = mapper.Map<WaterPayment>(waterPaymentCreateDto);

            await repo.AddPaymentAsync(paymentModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var paymentReadDto = mapper.Map<WaterPaymentReadDto>(paymentModel);

            return CreatedAtAction(
                nameof(GetWaterPaymentById),
                new { id = paymentReadDto.ID_Zahlung },
                paymentReadDto);
        }
        #endregion

        #region [HttpPost("kosten")]
        /// <summary>
        /// Post: neue Kosten schreiben
        /// Route: /api/wasser/kosten
        /// </summary>
        /// <param name="waterCostsCreateDto"></param>
        /// <returns></returns>
        [HttpPost("kosten")]
        [ProducesResponseType(typeof(WaterCostReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WaterCostReadDto>> AddWaterCosts(WaterCostCreateDto waterCostCreateDto, CancellationToken cancellationToken)
        {
            var costModel = mapper.Map<WaterCost>(waterCostCreateDto);

            await repo.AddCostAsync(costModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var costReadDto = mapper.Map<WaterCostReadDto>(costModel);

            return CreatedAtAction(
                nameof(GetWaterCostsBySupplier),
                new { id = costReadDto.Id_Anbieter }, // Wichtig: Hier muss die Anbieter-ID übergeben werden!
                costReadDto);
        }
        #endregion

        #region [HttpPost("zaehlerwechsel")]
        /// <summary>
        /// Post: neuen Zaehlerwechsel schreiben
        /// Route: /api/wasser/zaehlerwechsel
        /// </summary>
        /// <param name="waterCounterChangeCreateDto"></param>
        /// <returns></returns>
        [HttpPost("zaehlerwechsel")]
        [ProducesResponseType(typeof(WaterCounterChangeReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WaterCounterChangeReadDto>> AddWaterCounterChange(WaterCounterChangeCreateDto waterCounterChangeCreateDto, CancellationToken cancellationToken)
        {
            var counterChangeModel = mapper.Map<WaterCounterChange>(waterCounterChangeCreateDto);

            await repo.AddCounterChangeAsync(counterChangeModel, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            var counterChangeReadDto = mapper.Map<WaterCounterChangeReadDto>(counterChangeModel);

            return CreatedAtAction(
                nameof(GetWaterCounterChangeById),
                new { id = counterChangeReadDto.Id },
                counterChangeReadDto);
        }
        #endregion
        #endregion

        #region DELETE
        #region [HttpDelete("anbieter/{id:int:min(1)}")]
        /// <summary>
        /// Löscht einen Wasseranbieter anhand seiner ID.
        /// Route: DELETE /api/wasser/anbieter/{id}
        /// </summary>
        /// <param name="id">Die eindeutige ID des zu löschenden Anbieters.</param>
        /// <param name="cancellationToken">Token zum Abbrechen der Anfrage.</param>
        /// <returns>Der gelöschte Anbieter als DTO.</returns>
        /// <response code="200">Anbieter wurde erfolgreich gelöscht und als DTO zurückgegeben.</response>
        /// <response code="404">Anbieter mit der angegebenen ID wurde nicht gefunden.</response>
        /// <response code="409">Anbieter kann nicht gelöscht werden, da er noch mit anderen Daten verknüpft ist.</response>
        [HttpDelete("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(typeof(WaterTarifReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<WaterTarifReadDto>> DeleteSupplier(
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
            var deletedDto = mapper.Map<WaterTarifReadDto>(supplier);

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
        [ProducesResponseType(typeof(WaterCounterReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WaterCounterReadDto>> DeleteCounter(int id, CancellationToken cancellationToken)
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

            var deletedDto = mapper.Map<WaterCounterReadDto>(counter);

            await repo.DeleteCounterAsync(counter, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion

        #region [HttpDelete("zahlung/{id:int:min(1)}")]
        ///// <summary>
        ///// Delete: Löschen einer Zahlung anhand der Id
        ///// Route: /api/wasser/zahlung/id
        ///// </summary>
        ///// <param></param>
        ///// <returns></returns>
        [HttpDelete("zahlung/{id:int:min(1)}")]
        [ProducesResponseType(typeof(WaterPaymentReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WaterPaymentReadDto>> DeletePayment(int id, CancellationToken cancellationToken)
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

            var deletedDto = mapper.Map<WaterPaymentReadDto>(payment);

            await repo.DeletePaymentAsync(payment, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion

        #region [HttpDelete("kosten/{id:int:min(1)}")]
        ///// <summary>
        ///// Delete: Löschen eines Kostensatzes
        ///// Route: /api/wasser/kosten/id
        ///// </summary>
        ///// <param></param>
        ///// <returns></returns>
        [HttpDelete("kosten/{id:int:min(1)}")]
        [ProducesResponseType(typeof(WaterCostReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WaterCostReadDto>> DeleteCost(int id, CancellationToken cancellationToken)
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

            var deletedDto = mapper.Map<WaterCostReadDto>(cost);

            await repo.DeleteCostAsync(cost, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return Ok(deletedDto);
        }
        #endregion

        #region [HttpDelete("zaehlerwechsel/{id:int:min(1)}")]
        ///// <summary>
        ///// Delete: Löschen eines Zaehlerwechsels
        ///// Route: /api/wasser/zaehlerwechsel/id
        ///// </summary>
        ///// <param></param>
        ///// <returns></returns>
        [HttpDelete("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(typeof(WaterCounterChangeReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WaterCounterChangeReadDto>> DeleteCounterChange(int id, CancellationToken cancellationToken)
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

            var deletedDto = mapper.Map<WaterCounterChangeReadDto>(counterChange);

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
        /// Route: /api/wasser/anbieter/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="waterTarifUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSupplier(int id, WaterTarifUpdateDto waterTarifUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(waterTarifUpdateDto, supplier);

            await repo.UpdateSupplierAsync(supplier, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("zaehlerstand/{day:datetime}")]
        /// <summary>
        /// Put: Update eines Zählerstandes nach Datum
        /// </summary>
        /// <param name="day"></param>
        /// <param name="waterCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/{day:datetime}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWaterCounter(DateTime day, WaterCounterUpdateDto waterCounterUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(waterCounterUpdateDto, counter);

            await repo.UpdateCounterAsync(counter, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("zaehlerstand/id/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update eines Zählerstandes nach Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="waterCounterUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerstand/id/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWaterCounterById(int id, WaterCounterUpdateDto waterCounterUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(waterCounterUpdateDto, counter);

            await repo.UpdateCounterAsync(counter, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("zahlung/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update einer Zahlung
        /// </summary>
        /// <param></param>
        /// <param name="waterPaymentUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zahlung/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWaterPayment(int id, WaterPaymentUpdateDto waterPaymentUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(waterPaymentUpdateDto, payment);

            await repo.UpdatePaymentAsync(payment, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        #endregion

        #region [HttpPut("kosten/{id:int:min(1)}")]
        /// <summary>
        /// Put: Update eines Kostensatzes
        /// Route: /api/wasser/kosten/id
        /// </summary>
        /// <param></param>
        /// <param name="waterCostsUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("kosten/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWaterCosts(int id, WaterCostUpdateDto waterCostUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(waterCostUpdateDto, cost);

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
        /// <param name="waterCounterChangeUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCounterChange(int id, WaterCounterChangeUpdateDto waterCounterChangeUpdateDto, CancellationToken cancellationToken)
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

            mapper.Map(waterCounterChangeUpdateDto, counterChange);

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
        [HttpPatch("anbieter/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialSupplierUpdate(int id, JsonPatchDocument<WaterTarifUpdateDto> patchDoc, CancellationToken cancellationToken)
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

            var tarifToPatch = mapper.Map<WaterTarifUpdateDto>(supplier);

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
        [HttpPatch("zaehlerstand/{day:datetime}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialCounterUpdate(DateTime day, JsonPatchDocument<WaterCounterUpdateDto> patchDoc, CancellationToken cancellationToken)
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

            var counterToPatch = mapper.Map<WaterCounterUpdateDto>(counter);

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
        /// Route: /api/wasser/zahlung/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zahlung/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialWaterPaymentsUpdate(int id, JsonPatchDocument<WaterPaymentUpdateDto> patchDoc, CancellationToken cancellationToken)
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

            var paymentToPatch = mapper.Map<WaterPaymentUpdateDto>(payment);

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
        public async Task<IActionResult> PartialWaterCostsUpdate(int id, JsonPatchDocument<WaterCostUpdateDto> patchDoc, CancellationToken cancellationToken)
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

            var costToPatch = mapper.Map<WaterCostUpdateDto>(cost);

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
        /// Route: /api/wasser/zaehlerwechsel/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("zaehlerwechsel/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialWaterCounterChangeUpdate(int id, JsonPatchDocument<WaterCounterChangeUpdateDto> patchDoc, CancellationToken cancellationToken)
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

            var counterChangeToPatch = mapper.Map<WaterCounterChangeUpdateDto>(counterChange);

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