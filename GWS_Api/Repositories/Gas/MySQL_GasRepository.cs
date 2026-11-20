using GWS_Api.Models;
using GWS_Api.Models.Gas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GWS_Api.Repositories.Gas
{
    public class MySQL_GasRepository(GWS_DbContext context, ILogger<MySQL_GasRepository> logger) : IGasRepository
    {
        #region Variablendeklaration
        private readonly GWS_DbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly ILogger<MySQL_GasRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        #endregion

        #region ADD
        #region AddSupplierAsync
        public Task<GasTarif> AddSupplierAsync(GasTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);
            cancellationToken.ThrowIfCancellationRequested();

            var result = _context.Gas_tarif.Add(supplier);
            _logger.LogDebug("Gastarif zur Nachverfolgung (ChangeTracker) hinzugefügt.");
            return Task.FromResult(result.Entity);
        }
        #endregion

        #region AddCounterAsync
        public async Task<GasCounter?> AddCounterAsync(GasCounter zaehlerstand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(zaehlerstand);

            // 1. Prüfen, ob Ablesetag gesetzt ist (löst den CS1061-Fehler)
            if (zaehlerstand.Ablesetag is not DateTime rawDate)
            {
                _logger.LogWarning("Ablesetag ist null und kann nicht gespeichert werden.");
                return null;
            }

            // 2. Datum auf 00:00:00 normalisieren
            var ablesetag = rawDate.Date;
            var heute = DateTime.UtcNow.Date;

            if (ablesetag > heute)
            {
                _logger.LogWarning("Ablesetag {Ablesetag:yyyy-MM-dd} liegt in der Zukunft.", ablesetag);
                return null;
            }

            // 3. Schneller exakter Index-Seek, da in der DB immer 00:00:00 steht
            bool exists = await _context.Gas_zaehlerstand
                .AnyAsync(m => m.ID_Anbieter == zaehlerstand.ID_Anbieter
                            && m.Ablesetag == ablesetag,
                          cancellationToken);

            if (exists)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Eintrag für Anbieter {AnbieterId} am {Ablesetag:yyyy-MM-dd} existiert bereits.",
                        zaehlerstand.ID_Anbieter,
                        ablesetag);
                }
                return null;
            }

            // 4. Sicherstellen, dass auch der neue Eintrag sauber mit 00:00:00 gespeichert wird
            zaehlerstand.Ablesetag = ablesetag;

            var entry = _context.Gas_zaehlerstand.Add(zaehlerstand);
            _logger.LogDebug("GasCounter für {Ablesetag:yyyy-MM-dd} zum ChangeTracker hinzugefügt.", ablesetag);

            return entry.Entity;
        }
        #endregion

        #region AddPaymentAsync
        public Task<GasPayment> AddPaymentAsync(GasPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);
            cancellationToken.ThrowIfCancellationRequested();

            var entry = _context.Gas_zahlungen.Add(payment);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(
                    "GasPayment für Anbieter {AnbieterId} zum ChangeTracker hinzugefügt.",
                    payment.ID_Anbieter);
            }

            return Task.FromResult(entry.Entity);
        }
        #endregion

        #region AddCostAsync
        public Task<GasCost> AddCostAsync(GasCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);
            cancellationToken.ThrowIfCancellationRequested();

            var entry = _context.Gas_kosten.Add(cost);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(
                    "GasCost für Anbieter {AnbieterId} zum ChangeTracker hinzugefügt.",
                    cost.Id_Anbieter);
            }

            return Task.FromResult(entry.Entity);
        }
        #endregion

        #region AddCounterChangeAsync
        public Task<GasCounterChange> AddCounterChangeAsync(GasCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);
            cancellationToken.ThrowIfCancellationRequested();

            var entry = _context.Gas_zaehlerwechsel.Add(counterChange);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(
                    "GasCounterChange für Anbieter {AnbieterId} zum ChangeTracker hinzugefügt.",
                    counterChange.Id_Anbieter);
            }

            return Task.FromResult(entry.Entity);
        }
        #endregion

        #region AddBoilerDataAsync
        public Task<GasBoiler> AddBoilerDataAsync(GasBoiler boilerData, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(boilerData);
            cancellationToken.ThrowIfCancellationRequested();

            var entry = _context.Gas_therme.Add(boilerData);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(
                    "GasBoilerData {Id} zum ChangeTracker hinzugefügt.",
                    boilerData.Id);
            }

            return Task.FromResult(entry.Entity);
        }
        #endregion

        #region AddEnergieEfficiencyAsync
        public Task<Efficiency> AddEnergieEfficiencyAsync(Efficiency eff, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(eff);
            cancellationToken.ThrowIfCancellationRequested();

            var entry = _context.Energie_effizienz.Add(eff);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(
                    "EnergieEfficiency {Id} zum ChangeTracker hinzugefügt.",
                    eff.Id);
            }

            return Task.FromResult(entry.Entity);
        }
        #endregion
        #endregion

        #region DELETE
        #region DeleteSupplierAsync
        public async Task DeleteSupplierAsync(GasTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            var result = await _context.Gas_tarif.FindAsync([supplier.Id], cancellationToken)
                ?? throw new KeyNotFoundException($"Tarif mit ID {supplier.Id} nicht gefunden.");

            _context.Gas_tarif.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("GasTarif mit ID {TarifId} im ChangeTracker zum Löschen markiert.", supplier.Id);
            }
        }
        #endregion

        #region DeleteCounterAsync
        public async Task DeleteCounterAsync(GasCounter counter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counter);

            var result = await _context.Gas_zaehlerstand.FindAsync([counter.ID_Tag], cancellationToken)
                ?? throw new KeyNotFoundException($"Zählerstand mit ID {counter.ID_Tag} nicht gefunden.");

            _context.Gas_zaehlerstand.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("GasCounter mit ID {CounterId} im ChangeTracker zum Löschen markiert.", counter.ID_Tag);
            }
        }
        #endregion

        #region DeletePaymentAsync
        public async Task DeletePaymentAsync(GasPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);

            var result = await _context.Gas_zahlungen.FindAsync([payment.ID_Zahlung], cancellationToken)
                 ?? throw new KeyNotFoundException($"Zahlung mit ID {payment.ID_Zahlung} nicht gefunden.");

            _context.Gas_zahlungen.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("GasPayment mit ID {PaymentId} im ChangeTracker zum Löschen markiert.", payment.ID_Zahlung);
            }
        }
        #endregion

        #region DeleteCostAsync
        public async Task DeleteCostAsync(GasCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            var result = await _context.Gas_kosten.FindAsync([cost.Id], cancellationToken)
                 ?? throw new KeyNotFoundException($"Kosten-Eintrag mit ID {cost.Id} nicht gefunden.");

            _context.Gas_kosten.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("GasCost mit ID {CostId} im ChangeTracker zum Löschen markiert.", cost.Id);
            }
        }
        #endregion

        #region DeleteCounterChangeAsync
        public async Task DeleteCounterChangeAsync(GasCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            var result = await _context.Gas_zaehlerwechsel.FindAsync([counterChange.Id], cancellationToken)
                            ?? throw new KeyNotFoundException($"Zählerwechsel mit ID {counterChange.Id} nicht gefunden.");
            _context.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("GasCounterChange mit ID {CounterChangeId} im ChangeTracker zum Löschen markiert.", counterChange.Id);
            }
        }
        #endregion

        #region DeleteBoilerDataAsync
        public async Task DeleteBoilerDataAsync(GasBoiler boilerData, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(boilerData);

            var result = await _context.Gas_therme.FindAsync([boilerData.Id], cancellationToken)
                 ?? throw new KeyNotFoundException($"GasBoiler-Eintrag mit ID {boilerData.Id} nicht gefunden.");

            _context.Gas_therme.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("GasBoiler mit ID {BoilerId} im ChangeTracker zum Löschen markiert.", boilerData.Id);
            }
        }
        #endregion

        #region DeleteEnergieEfficiencyAsync
        public async Task DeleteEnergieEfficiencyAsync(Efficiency eff, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(eff);

            var result = await _context.Gas_therme.FindAsync([eff.Id], cancellationToken)
                 ?? throw new KeyNotFoundException($"EnergieEfficiency-Eintrag mit ID {eff.Id} nicht gefunden.");

            _context.Gas_therme.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("EnergieEfficiency mit ID {ThermeId} im ChangeTracker zum Löschen markiert.", eff.Id);
            }

        }
        #endregion
        #endregion

        #region GET
        #region GetSupplierByIdAsync
        public Task<GasTarif?> GetSupplierByIdAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            return _context.Gas_tarif
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == supplierId, cancellationToken);
        }
        #endregion

        #region GetSuppliersAsync
        public async Task<IEnumerable<GasTarif>> GetSuppliersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Gas_tarif
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetCountersAsync
        public async Task<IEnumerable<GasCounter>> GetCountersAsync(CancellationToken cancellationToken = default)
        {
            var counters = await _context.Gas_zaehlerstand
               .AsNoTracking()
               .ToListAsync(cancellationToken);

            var tarifDict = await _context.Gas_tarif
                .AsNoTracking()
                .ToDictionaryAsync(x => x.Id, x => x.Anbieter, cancellationToken);

            foreach (var counter in counters)
            {
                if (tarifDict.TryGetValue(counter.ID_Anbieter, out var name))
                {
                    counter.Anbieter = name ?? string.Empty;
                }
            }

            return counters;
        }
        #endregion

        #region GetCountersBySupplierAsync
        public async Task<IEnumerable<GasCounter>> GetCountersBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Gas_zaehlerstand
                .AsNoTracking()
                .Where(m => m.ID_Anbieter == supplierId)
                .ToListAsync(cancellationToken);

            if (result.Count == 0)
            {
                return result;
            }

            var supplierName = await _context.Gas_tarif
                .AsNoTracking()
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync(cancellationToken);

            if (supplierName is not null)
            {
                foreach (var counter in result)
                {
                    counter.Anbieter = supplierName;
                }
            }

            return result;
        }
        #endregion

        #region GetCounterByIdAsync
        public async Task<GasCounter?> GetCounterByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var counter = await _context.Gas_zaehlerstand
               .AsNoTracking()
               .FirstOrDefaultAsync(m => m.ID_Tag == id, cancellationToken);

            if (counter is not null)
            {
                var anbieterName = await _context.Gas_tarif
                    .AsNoTracking()
                    .Where(m => m.Id == counter.ID_Anbieter)
                    .Select(m => m.Anbieter)
                    .FirstOrDefaultAsync(cancellationToken);

                counter.Anbieter = anbieterName ?? string.Empty;
            }

            return counter;
        }
        #endregion

        #region GetCounterByDateAsync
        public async Task<GasCounter?> GetCounterByDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            var targetDate = date.Date;

            var counter = await _context.Gas_zaehlerstand
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Ablesetag == targetDate, cancellationToken);

            if (counter is not null)
            {
                var anbieterName = await _context.Gas_tarif
                    .AsNoTracking()
                    .Where(m => m.Id == counter.ID_Anbieter)
                    .Select(m => m.Anbieter)
                    .FirstOrDefaultAsync(cancellationToken);

                counter.Anbieter = anbieterName ?? string.Empty;
            }

            return counter;
        }
        #endregion

        #region GetPaymentsAsync
        public async Task<IEnumerable<GasPayment>> GetPaymentsAsync(CancellationToken cancellationToken = default)
        {
            var payments = await _context.Gas_zahlungen
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var tarifDict = await _context.Gas_tarif
                .AsNoTracking()
                .ToDictionaryAsync(x => x.Id, x => x.Anbieter, cancellationToken);

            foreach (var payment in payments)
            {
                if (tarifDict.TryGetValue(payment.ID_Anbieter, out var name))
                {
                    payment.Anbieter = name ?? string.Empty;
                }
            }

            return payments;
        }
        #endregion

        #region 
        public async Task<IEnumerable<GasPayment>> GetPaymentsBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Gas_zahlungen
                .AsNoTracking()
                .Where(m => m.ID_Anbieter == supplierId)
                .ToListAsync(cancellationToken);

            if (result.Count == 0)
            {
                return result;
            }

            var supplierName = await _context.Gas_tarif
                .AsNoTracking()
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync(cancellationToken);

            if (supplierName is not null)
            {
                foreach (var payment in result)
                {
                    payment.Anbieter = supplierName;
                }
            }

            return result;
        }
        #endregion

        #region GetPaymentsBySupplierAsync
        public async Task<GasPayment?> GetPaymentByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var counter = await _context.Gas_zahlungen
               .AsNoTracking()
               .FirstOrDefaultAsync(m => m.ID_Zahlung == id, cancellationToken);

            if (counter is not null)
            {
                var anbieterName = await _context.Gas_tarif
                    .AsNoTracking()
                    .Where(m => m.Id == counter.ID_Anbieter)
                    .Select(m => m.Anbieter)
                    .FirstOrDefaultAsync(cancellationToken);

                counter.Anbieter = anbieterName ?? string.Empty;
            }

            return counter;
        }
        #endregion

        #region GetPaymentMethodsAsync
        public async Task<IEnumerable<PaymentMethod>> GetPaymentMethodsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Zahlungsarten
               .AsNoTracking()
               .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetCostsAsync
        public async Task<IEnumerable<GasCost>> GetCostsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Gas_kosten
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetCostsBySupplierIdAsync
        public async Task<IEnumerable<GasCost>> GetCostsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Gas_kosten
                .AsNoTracking()
                .Where(m => m.Id_Anbieter == supplierId)
                .ToListAsync(cancellationToken);

            if (result.Count == 0)
            {
                return result;
            }

            var supplierName = await _context.Gas_tarif
                .AsNoTracking()
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync(cancellationToken);

            if (supplierName is not null)
            {
                foreach (var cost in result)
                {
                    cost.Anbieter = supplierName;
                }
            }

            return result;
        }
        #endregion

        #region GetCostByIdAsync
        public async Task<GasCost?> GetCostByIdAsync(int costId, CancellationToken cancellationToken = default)
        {
            return await _context.Gas_kosten
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == costId, cancellationToken);
        }
        #endregion

        #region GetCounterChangesAsync
        public async Task<IEnumerable<GasCounterChange>> GetCounterChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Gas_zaehlerwechsel
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetCounterChangeByIdAsync
        public async Task<GasCounterChange?> GetCounterChangeByIdAsync(int counterChangeId, CancellationToken cancellationToken = default)
        {
            return await _context.Gas_zaehlerwechsel
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == counterChangeId, cancellationToken);
        }
        #endregion

        #region GetCounterChangesBySupplierAsync
        public async Task<IEnumerable<GasCounterChange>> GetCounterChangesBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Gas_zaehlerwechsel
                .AsNoTracking()
                .Where(m => m.Id_Anbieter == supplierId)
                .ToListAsync(cancellationToken);

            if (result.Count == 0)
            {
                return result;
            }

            var supplierName = await _context.Gas_tarif
                .AsNoTracking()
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync(cancellationToken);

            if (supplierName is not null)
            {
                foreach (var counterChange in result)
                {
                    counterChange.Anbieter = supplierName;
                }
            }

            return result;
        }
        #endregion

        #region GetBoilerDataAsync
        public async Task<IEnumerable<GasBoiler>> GetBoilerDataAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Gas_therme
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetBoilerDataByIdAsync
        public async Task<GasBoiler?> GetBoilerDataByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Gas_therme
               .AsNoTracking()
               .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }
        #endregion

        #region GetEnergieEfficiencyAsync
        public async Task<IEnumerable<Efficiency>> GetEnergieEfficiencyAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Energie_effizienz
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetEnergieEfficiencyByIdAsync
        public async Task<Efficiency?> GetEnergieEfficiencyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Energie_effizienz
              .AsNoTracking()
              .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }
        #endregion
        #endregion

        #region UPDATE
        #region UpdateSupplierAsync
        public async Task<GasTarif?> UpdateSupplierAsync(GasTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            var result = await _context.Gas_tarif.FindAsync([supplier.Id], cancellationToken);
            if (result is null)
            {
                return null;
            }

            _context.Entry(result).CurrentValues.SetValues(supplier);
            return result;
        }
        #endregion

        #region UpdateCounterAsync
        public async Task<GasCounter?> UpdateCounterAsync(GasCounter zaehlerstand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(zaehlerstand);

            var result = await _context.Gas_zaehlerstand.FindAsync([zaehlerstand.ID_Tag], cancellationToken);
            if (result is null)
            {
                return null;
            }

            _context.Entry(result).CurrentValues.SetValues(zaehlerstand);
            return result;
        }
        #endregion

        #region UpdatePaymentAsync
        public async Task<GasPayment?> UpdatePaymentAsync(GasPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);

            var result = await _context.Gas_zahlungen.FindAsync([payment.ID_Zahlung], cancellationToken);
            if (result is null)
            {
                return null;
            }

            _context.Entry(result).CurrentValues.SetValues(payment);
            return result;
        }
        #endregion

        #region UpdateCostAsync
        public async Task<GasCost?> UpdateCostAsync(GasCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            var result = await _context.Gas_kosten.FindAsync([cost.Id], cancellationToken);
            if (result is null)
            {
                return null;
            }

            _context.Entry(result).CurrentValues.SetValues(cost);
            return result;
        }
        #endregion

        #region UpdateBoilerDataAsync
        public async Task<GasBoiler?> UpdateBoilerDataAsync(GasBoiler boilerData, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(boilerData);

            var result = await _context.Gas_therme.FindAsync([boilerData.Id], cancellationToken);
            if (result is null)
            {
                return null;
            }

            _context.Entry(result).CurrentValues.SetValues(boilerData);
            return result;
        }
        #endregion

        #region UpdateEnergieEfficiencyAsync
        public async Task<Efficiency?> UpdateEnergieEfficiencyAsync(Efficiency eff, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(eff);

            var result = await _context.Energie_effizienz.FindAsync([eff.Id], cancellationToken);
            if (result is null)
            {
                return null;
            }

            _context.Entry(result).CurrentValues.SetValues(eff);
            return result;
        }
        #endregion

        #region UpdateCounterChangeAsync
        public async Task<GasCounterChange?> UpdateCounterChangeAsync(GasCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            var result = await _context.Gas_zaehlerwechsel.FindAsync([counterChange.Id], cancellationToken);
            if (result is null)
            {
                return null;
            }

            _context.Entry(result).CurrentValues.SetValues(counterChange);
            return result;
        }
        #endregion
        #endregion

        #region SaveChanges
        // Zwingend erforderlich, damit EF Core die obigen Updates an die MySQL-DB schickt
        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //try
            //{
            //    // Wir führen das Speichern aus. 
            //    // Ob 0 oder mehr Zeilen geändert wurden, ist für den reinen Erfolg egal.
            //    await _context.SaveChangesAsync();
            //    return true;
            //}
            //catch (Exception)
            //{
            //    // Nur wenn die Datenbank wirklich einen Fehler wirft, geben wir false zurück
            //    return false;
            //}
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Speichern in die Datenbank.");
                return false;
            }
        }

        // nur zum Testen
        public async Task<IEnumerable<GasTarif>> SearchAsync(string tarif, double? zaehlermiete, CancellationToken cancellationToken = default)
        {
            IQueryable<GasTarif> query = _context.Gas_tarif.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(tarif))
            {
                var searchTerm = tarif.Trim();
                query = query.Where(m => m.Tarif != null && m.Tarif.Contains(searchTerm));
            }

            if (zaehlermiete.HasValue)
            {
                query = query.Where(m => m.Zaehlermiete == zaehlermiete.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }
        #endregion
    }
}