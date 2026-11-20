using GWS_Api.Models;
using GWS_Api.Models.Water;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GWS_Api.Repositories.Water
{
    public class MySQL_WaterRepository(GWS_DbContext context, ILogger<MySQL_WaterRepository> logger) : IWaterRepository
    {
        #region Variablendeklaration
        private readonly GWS_DbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly ILogger<MySQL_WaterRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        #endregion

        #region ADD
        #region AddSupplierAsync
        public Task<WaterTarif> AddSupplierAsync(WaterTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);
            cancellationToken.ThrowIfCancellationRequested();

            var result = _context.Wasser_tarif.Add(supplier);
            _logger.LogDebug("Wassertarif zur Nachverfolgung (ChangeTracker) hinzugefügt.");
            return Task.FromResult(result.Entity);
        }
        #endregion

        #region AddCounterAsync
        public async Task<WaterCounter?> AddCounterAsync(WaterCounter zaehlerstand, CancellationToken cancellationToken = default)
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
            bool exists = await _context.Wasser_zaehlerstand
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

            var entry = _context.Wasser_zaehlerstand.Add(zaehlerstand);
            _logger.LogDebug("WaterCounter für {Ablesetag:yyyy-MM-dd} zum ChangeTracker hinzugefügt.", ablesetag);

            return entry.Entity;
        }
        #endregion

        #region AddPaymentAsync
        public Task<WaterPayment> AddPaymentAsync(WaterPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);
            cancellationToken.ThrowIfCancellationRequested();

            var entry = _context.Wasser_zahlungen.Add(payment);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(
                    "WaterPayment für Anbieter {AnbieterId} zum ChangeTracker hinzugefügt.",
                    payment.ID_Anbieter);
            }

            return Task.FromResult(entry.Entity);
        }
        #endregion

        #region AddCostAsync
        public Task<WaterCost> AddCostAsync(WaterCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);
            cancellationToken.ThrowIfCancellationRequested();

            var entry = _context.Wasser_kosten.Add(cost);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(
                    "WaterCost für Anbieter {AnbieterId} zum ChangeTracker hinzugefügt.",
                    cost.Id_Anbieter);
            }

            return Task.FromResult(entry.Entity);
        }
        #endregion

        #region AddCounterChangeAsync
        public Task<WaterCounterChange> AddCounterChangeAsync(WaterCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);
            cancellationToken.ThrowIfCancellationRequested();

            var entry = _context.Wasser_zaehlerwechsel.Add(counterChange);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(
                    "WaterCounterChange für Anbieter {AnbieterId} zum ChangeTracker hinzugefügt.",
                    counterChange.Id_Anbieter);
            }

            return Task.FromResult(entry.Entity);
        }
        #endregion
        #endregion

        #region DELETE
        #region DeleteSupplierAsync
        public async Task DeleteSupplierAsync(WaterTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            var result = await _context.Wasser_tarif.FindAsync([supplier.Id], cancellationToken)
                ?? throw new KeyNotFoundException($"Tarif mit ID {supplier.Id} nicht gefunden.");

            _context.Wasser_tarif.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("WaterTarif mit ID {TarifId} im ChangeTracker zum Löschen markiert.", supplier.Id);
            }
        }
        #endregion

        #region DeleteCounterAsync
        public async Task DeleteCounterAsync(WaterCounter counter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counter);

            var result = await _context.Wasser_zaehlerstand.FindAsync([counter.ID_Tag], cancellationToken)
                ?? throw new KeyNotFoundException($"Zählerstand mit ID {counter.ID_Tag} nicht gefunden.");

            _context.Wasser_zaehlerstand.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("WaterCounter mit ID {CounterId} im ChangeTracker zum Löschen markiert.", counter.ID_Tag);
            }
        }
        #endregion

        #region DeletePaymentAsync
        public async Task DeletePaymentAsync(WaterPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);

            var result = await _context.Wasser_zahlungen.FindAsync([payment.ID_Zahlung], cancellationToken)
                 ?? throw new KeyNotFoundException($"Zahlung mit ID {payment.ID_Zahlung} nicht gefunden.");

            _context.Wasser_zahlungen.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("WaterPayment mit ID {PaymentId} im ChangeTracker zum Löschen markiert.", payment.ID_Zahlung);
            }
        }
        #endregion

        #region DeleteCostAsync
        public async Task DeleteCostAsync(WaterCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            var result = await _context.Wasser_kosten.FindAsync([cost.Id], cancellationToken)
                 ?? throw new KeyNotFoundException($"Kosten-Eintrag mit ID {cost.Id} nicht gefunden.");

            _context.Wasser_kosten.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("WaterCost mit ID {CostId} im ChangeTracker zum Löschen markiert.", cost.Id);
            }
        }
        #endregion

        #region DeleteCounterChangeAsync
        public async Task DeleteCounterChangeAsync(WaterCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            var result = await _context.Wasser_zaehlerwechsel.FindAsync([counterChange.Id], cancellationToken   )
                            ?? throw new KeyNotFoundException($"Zählerwechsel mit ID {counterChange.Id} nicht gefunden.");
            _context.Remove(result);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("WaterCounterChange mit ID {CounterChangeId} im ChangeTracker zum Löschen markiert.", counterChange.Id);
            }
        }
        #endregion
        #endregion

        #region GET
        #region GetSupplierByIdAsync
        public Task<WaterTarif?> GetSupplierByIdAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            return _context.Wasser_tarif
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == supplierId, cancellationToken);
        }
        #endregion

        #region GetSuppliersAsync
        public async Task<IEnumerable<WaterTarif>> GetSuppliersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Wasser_tarif
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetCountersAsync
        public async Task<IEnumerable<WaterCounter>> GetCountersAsync(CancellationToken cancellationToken = default)
        {
            var counters = await _context.Wasser_zaehlerstand
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var tarifDict = await _context.Wasser_tarif
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
        public async Task<IEnumerable<WaterCounter>> GetCountersBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Wasser_zaehlerstand
                .AsNoTracking()
                .Where(m => m.ID_Anbieter == supplierId)
                .ToListAsync(cancellationToken);

            if (result.Count == 0)
            {
                return result;
            }

            var supplierName = await _context.Wasser_tarif
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
        public async Task<WaterCounter?> GetCounterByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var counter = await _context.Wasser_zaehlerstand
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID_Tag == id, cancellationToken);

            if (counter is not null)
            {
                var anbieterName = await _context.Wasser_tarif
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
        public async Task<WaterCounter?> GetCounterByDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            var targetDate = date.Date;

            var counter = await _context.Wasser_zaehlerstand
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Ablesetag == targetDate, cancellationToken);

            if (counter is not null)
            {
                var anbieterName = await _context.Wasser_tarif
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
        public async Task<IEnumerable<WaterPayment>> GetPaymentsAsync(CancellationToken cancellationToken = default)
        {
            var payments = await _context.Wasser_zahlungen
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var tarifDict = await _context.Wasser_tarif
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

        #region GetPaymentsBySupplierAsync
        public async Task<IEnumerable<WaterPayment>> GetPaymentsBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Wasser_zahlungen
                .AsNoTracking()
                .Where(m => m.ID_Anbieter == supplierId)
                .ToListAsync(cancellationToken);

            if (result.Count == 0)
            {
                return result;
            }

            var supplierName = await _context.Wasser_tarif
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

        #region GetPaymentByIdAsync
        public async Task<WaterPayment?> GetPaymentByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var counter = await _context.Wasser_zahlungen
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID_Zahlung == id, cancellationToken);

            if (counter is not null)
            {
                var anbieterName = await _context.Wasser_tarif
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
        public async Task<IEnumerable<WaterCost>> GetCostsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Wasser_kosten
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetCostsBySupplierIdAsync
        public async Task<IEnumerable<WaterCost>> GetCostsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Wasser_kosten
                .AsNoTracking()
                .Where(m => m.Id_Anbieter == supplierId)
                .ToListAsync(cancellationToken);

            if (result.Count == 0)
            {
                return result;
            }

            var supplierName = await _context.Wasser_tarif
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
        public async Task<WaterCost?> GetCostByIdAsync(int costId, CancellationToken cancellationToken = default)
        {
            return await _context.Wasser_kosten
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == costId, cancellationToken);
        }
        #endregion

        #region GetCounterChangesAsync
        public async Task<IEnumerable<WaterCounterChange>> GetCounterChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Wasser_zaehlerwechsel
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetCounterChangeByIdAsync
        public async Task<WaterCounterChange?> GetCounterChangeByIdAsync(int counterChangeId, CancellationToken cancellationToken = default)
        {
            return await _context.Wasser_zaehlerwechsel
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == counterChangeId, cancellationToken);
        }
        #endregion

        #region GetCounterChangesBySupplierAsync
        public async Task<IEnumerable<WaterCounterChange>> GetCounterChangesBySupplierAsync(int supplierId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Wasser_zaehlerwechsel
                .AsNoTracking()
                .Where(m => m.Id_Anbieter == supplierId)
                .ToListAsync(cancellationToken);

            if (result.Count == 0)
            {
                return result;
            }

            var supplierName = await _context.Wasser_tarif
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
        #endregion

        #region UPDATE
        #region UpdateSupplierAsync
        public async Task<WaterTarif?> UpdateSupplierAsync(WaterTarif supplier, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            var result = await _context.Wasser_tarif.FindAsync([supplier.Id], cancellationToken);
            if (result is null)
            {
                return null;
            }

            _context.Entry(result).CurrentValues.SetValues(supplier);
            return result;
        }
        #endregion

        #region UpdateCounterAsync
        public async Task<WaterCounter?> UpdateCounterAsync(WaterCounter zaehlerstand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(zaehlerstand);

            var result = await _context.Wasser_zaehlerstand.FindAsync([zaehlerstand.ID_Tag], cancellationToken);
            if (result is null)
            {
                return null;
            }

            _context.Entry(result).CurrentValues.SetValues(zaehlerstand);
            return result;
        }
        #endregion

        #region UpdatePaymentAsync
        public async Task<WaterPayment?> UpdatePaymentAsync(WaterPayment payment, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payment);

            var result = await _context.Wasser_zahlungen.FindAsync([payment.ID_Zahlung], cancellationToken);
            if (result is null)
            {
                return null;
            }

            _context.Entry(result).CurrentValues.SetValues(payment);
            return result;
        }
        #endregion

        #region UpdateCostAsync
        public async Task<WaterCost?> UpdateCostAsync(WaterCost cost, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(cost);

            var result = await _context.Wasser_kosten.FindAsync([cost.Id], cancellationToken);
            if (result is null)
            {
                return null;
            }

            _context.Entry(result).CurrentValues.SetValues(cost);
            return result;
        }
        #endregion

        #region UpdateCounterChangeAsync
        public async Task<WaterCounterChange?> UpdateCounterChangeAsync(WaterCounterChange counterChange, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            var result = await _context.Wasser_zaehlerwechsel.FindAsync([counterChange.Id], cancellationToken);
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
        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
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

        public async Task<IEnumerable<WaterTarif>> SearchAsync(string tarif, double? zaehlermiete, CancellationToken cancellationToken = default)
        {
            IQueryable<WaterTarif> query = _context.Wasser_tarif.AsNoTracking();

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