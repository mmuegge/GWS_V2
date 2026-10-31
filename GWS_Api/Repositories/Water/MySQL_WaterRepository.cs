using GWS_Api.Models;
using GWS_Api.Models.Water;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Repositories.Water
{
    public class MySQL_WaterRepository : IWaterRepository
    {
        #region Variablendeklaration
        private readonly GWS_DbContext _context;
        public MySQL_WaterRepository(GWS_DbContext context)
        {
            _context = context;
        }
        #endregion

        #region ADD
        public async Task<WaterTarif> AddSupplierAsync(WaterTarif supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);
            var result = await _context.Wasser_tarif.AddAsync(supplier);
            return result.Entity;
        }
        public async Task<WaterCounter?> AddCounterAsync(WaterCounter zaehlerstand)
        {
            // Sicherheitsnetz: Null-Prüfung wie bei den anderen Methoden
            ArgumentNullException.ThrowIfNull(zaehlerstand);

            // prüfen ob Ablesedatum in der Zukunft liegt
            if (zaehlerstand.Ablesetag > DateTime.Now)
            {
                return null;
            }

            // BEHOBEN: Prüfen, ob der Ablesetag FÜR DIESEN SPEZIFISCHEN LIEFERANTEN/ZÄHLER schon vorhanden ist
            int count = await _context.Wasser_zaehlerstand
                .Where(m => m.Ablesetag == zaehlerstand.Ablesetag && m.ID_Anbieter == zaehlerstand.ID_Anbieter)
                .CountAsync();

            if (count > 0)
            {
                return null;
            }

            var result = await _context.Wasser_zaehlerstand.AddAsync(zaehlerstand);
            return result.Entity;
        }
        public async Task<WaterPayment> AddPaymentAsync(WaterPayment payment)
        {
            ArgumentNullException.ThrowIfNull(payment);
            var result = await _context.Wasser_zahlungen.AddAsync(payment);
            return result.Entity;
        }
        public async Task<WaterCost> AddCostAsync(WaterCost cost)
        {
            ArgumentNullException.ThrowIfNull(cost);
            var result = await _context.Wasser_kosten.AddAsync(cost);
            return result.Entity;
        }
        public async Task<WaterCounterChange> AddCounterChangeAsync(WaterCounterChange counterChange)
        {
            ArgumentNullException.ThrowIfNull(counterChange);
            var result = await _context.Wasser_zaehlerwechsel.AddAsync(counterChange);
            return result.Entity;
        }
        #endregion

        #region DELETE
        public async Task DeleteSupplierAsync(WaterTarif supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            var result = await _context.Wasser_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id)
                            ?? throw new KeyNotFoundException($"Tarif mit ID {supplier.Id} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeleteCounterAsync(WaterCounter counter)
        {
            ArgumentNullException.ThrowIfNull(counter);

            var result = await _context.Wasser_zaehlerstand
                .FirstOrDefaultAsync(m => m.ID_Tag == counter.ID_Tag)
                ?? throw new KeyNotFoundException($"Zählerstand mit ID {counter.ID_Tag} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeletePaymentAsync(WaterPayment payment)
        {
            ArgumentNullException.ThrowIfNull(payment);

            var result = await _context.Wasser_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == payment.ID_Zahlung)
                 ?? throw new KeyNotFoundException($"Zahlung mit ID {payment.ID_Zahlung} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeleteCostAsync(WaterCost cost)
        {
            ArgumentNullException.ThrowIfNull(cost);

            var result = await _context.Wasser_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id)
                            ?? throw new KeyNotFoundException($"Kosten-Eintrag mit ID {cost.Id} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeleteCounterChangeAsync(WaterCounterChange counterChange)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            var result = await _context.Wasser_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id)
                            ?? throw new KeyNotFoundException($"Zählerwechsel mit ID {counterChange.Id} nicht gefunden.");
            _context.Remove(result);
        }
        #endregion

        #region GET
        public async Task<WaterTarif?> GetSupplierByIdAsync(int supplierId)
        {
            return await _context.Wasser_tarif.FirstOrDefaultAsync(m => m.Id == supplierId);
        }
        public async Task<IEnumerable<WaterTarif>> GetSuppliersAsync()
        {
            return await _context.Wasser_tarif.ToListAsync();
        }
        public async Task<IEnumerable<WaterCounter?>> GetCountersAsync()
        {
            var counters = await _context.Wasser_zaehlerstand.ToListAsync();

            // Wir erstellen ein schnelles Dictionary für die Namenssuche (Key: Id, Value: Anbietername)
            var tarifDict = await _context.Wasser_tarif
                .ToDictionaryAsync(x => x.Id, x => x.Anbieter);

            foreach (var counter in counters)
            {
                // Sicherer Zugriff: Falls die ID im Dictionary existiert, Namen zuweisen
                if (tarifDict.TryGetValue(counter.ID_Anbieter, out var name))
                {
                    counter.Anbieter = name ?? string.Empty; // Null-safe abgesichert;
                }
            }
            return counters;
        }
        public async Task<IEnumerable<WaterCounter?>> GetCountersBySupplierAsync(int supplierId)
        {
            var result = await _context.Wasser_zaehlerstand.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Wasser_tarif
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync();

            if (supplierName != null)
            {
                foreach (var counter in result)
                {
                    counter.Anbieter = supplierName ?? string.Empty; // Null-safe abgesichert;
                }
            }
            return result;
        }
        public async Task<WaterCounter?> GetCounterByIdAsync(int id)
        {
            var counter = await _context.Wasser_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == id);
            if (counter != null)
            {
                var tarif = await _context.Wasser_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif?.Anbieter ?? string.Empty; // Null-safe abgesichert
            }
            return counter;
        }
        public async Task<WaterCounter?> GetCounterByDateAsync(DateTime date)
        {
            var counter = await _context.Wasser_zaehlerstand.FirstOrDefaultAsync(m => m.Ablesetag == date);
            if (counter != null)
            {
                var tarif = await _context.Wasser_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif?.Anbieter ?? string.Empty;
            }
            return counter;
        }
        public async Task<IEnumerable<WaterPayment?>> GetPaymentsAsync()
        {
            var payments = await _context.Wasser_zahlungen.ToListAsync();

            // Auch hier: Dictionary-Optimierung statt unzuverlässiger Sortierungsschleife
            var tarifDict = await _context.Wasser_tarif
                .ToDictionaryAsync(x => x.Id, x => x.Anbieter);

            foreach (var payment in payments)
            {
                if (tarifDict.TryGetValue(payment.ID_Anbieter, out var name))
                {
                    payment.Anbieter = name;
                }
            }
            return payments;
        }
        public async Task<IEnumerable<WaterPayment?>> GetPaymentsBySupplierAsync(int supplierId)
        {
            var result = await _context.Wasser_zahlungen.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Wasser_tarif
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync();

            if (supplierName != null)
            {
                foreach (var payment in result)
                {
                    payment.Anbieter = supplierName;
                }
            }
            return result;
        }
        public async Task<WaterPayment?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _context.Wasser_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == paymentId);
            if (payment != null)
            {
                var tarif = await _context.Wasser_tarif.FirstOrDefaultAsync(m => m.Id == payment.ID_Anbieter);
                payment.Anbieter = tarif?.Anbieter ?? string.Empty;
            }
            return payment;
        }
        public async Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync()
        {
            return await _context.Zahlungsarten.ToListAsync();
        }
        public async Task<IEnumerable<WaterCost?>> GetCostsAsync()
        {
            return await _context.Wasser_kosten.ToListAsync();
        }
        public async Task<IEnumerable<WaterCounterChange?>> GetCounterChangesAsync()
        {
            return await _context.Wasser_zaehlerwechsel.ToListAsync();
        }
        public async Task<WaterCounterChange?> GetCounterChangeByIdAsync(int counterChangeId)
        {
            return await _context.Wasser_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChangeId);
        }
        public async Task<IEnumerable<WaterCost?>> GetCostsBySupplierIdAsync(int supplierId)
        {
            var result = await _context.Wasser_kosten.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Wasser_tarif
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync();

            if (supplierName != null)
            {
                foreach (var costs in result)
                {
                    costs.Anbieter = supplierName ?? string.Empty; ;
                }
            }
            return result;
        }
        public async Task<WaterCost?> GetCostByIdAsync(int costId)
        {
            return await _context.Wasser_kosten.FirstOrDefaultAsync(m => m.Id == costId);
        }
        public async Task<IEnumerable<WaterCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId)
        {
            var result = await _context.Wasser_zaehlerwechsel.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Wasser_tarif
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync();

            if (supplierName != null)
            {
                foreach (var counterChange in result)
                {
                    counterChange.Anbieter = supplierName;
                }
            }
            return result;
        }
        #endregion

        #region UPDATE
        public async Task<WaterTarif?> UpdateSupplierAsync(WaterTarif supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            var result = await _context.Wasser_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id);
            if (result != null)
            {
                result.Anbieter = supplier.Anbieter;
                result.Tarif = supplier.Tarif;
                result.Zaehlernummer = supplier.Zaehlernummer;
                result.Kuendigungsfrist = supplier.Kuendigungsfrist;
                result.Zeitraum_Start = supplier.Zeitraum_Start;
                result.Zeitraum_Ende = supplier.Zeitraum_Ende;
                result.Start_Zaehlerstand = supplier.Start_Zaehlerstand;
                result.Ende_Zaehlerstand = supplier.Ende_Zaehlerstand;
                result.Start_Zaehlerstand_aussen = supplier.Start_Zaehlerstand_aussen;
                result.Ende_Zaehlerstand_aussen = supplier.Ende_Zaehlerstand_aussen;
                result.Trinkwasserpreis = supplier.Trinkwasserpreis;
                result.Verbrauch_Trinkwasser = supplier.Verbrauch_Trinkwasser;
                result.Abwasserpreis = supplier.Abwasserpreis;
                result.Verbrauch_Abwasser = supplier.Verbrauch_Abwasser;
                result.Grundpreis = supplier.Grundpreis;
                result.Zaehlermiete = supplier.Zaehlermiete;
                result.Bemerkung = supplier.Bemerkung;
                return result;
            }
            return null;
        }

        public async Task<WaterCounter?> UpdateCounterAsync(WaterCounter zaehlerstand)
        {
            ArgumentNullException.ThrowIfNull(zaehlerstand);

            var result = await _context.Wasser_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == zaehlerstand.ID_Tag);
            if (result != null)
            {
                result.ID_Anbieter = zaehlerstand.ID_Anbieter;
                result.Anbieter = zaehlerstand.Anbieter;
                result.Ablesetag = zaehlerstand.Ablesetag;
                result.Zaehlerstand = zaehlerstand.Zaehlerstand;
                result.Uhrzeit = zaehlerstand.Uhrzeit;
                result.Temperatur_aussen = zaehlerstand.Temperatur_aussen;
                result.Temperatur_innen = zaehlerstand.Temperatur_innen;
                result.Bemerkungen = zaehlerstand.Bemerkungen;
                result.Zaehlerstand_aussen = zaehlerstand.Zaehlerstand_aussen;
                return result;
            }
            return null;
        }

        public async Task<WaterPayment?> UpdatePaymentAsync(WaterPayment payment)
        {
            ArgumentNullException.ThrowIfNull(payment);

            var result = await _context.Wasser_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == payment.ID_Zahlung);
            if (result != null)
            {
                result.ID_Anbieter = payment.ID_Anbieter;
                result.Anbieter = payment.Anbieter;
                result.Datum = payment.Datum;
                result.Zahlungsart = payment.Zahlungsart;
                result.Zahlungen = payment.Zahlungen;
                result.Bemerkungen = payment.Bemerkungen;
                return result;
            }
            return null;
        }

        public async Task<WaterCost?> UpdateCostAsync(WaterCost cost)
        {
            ArgumentNullException.ThrowIfNull(cost);

            var result = await _context.Wasser_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id);
            if (result != null)
            {
                result.Id_Anbieter = cost.Id_Anbieter;
                result.Anbieter = cost.Anbieter;
                result.Grundpreis = cost.Grundpreis;
                result.Trinkwasserpreis = cost.Trinkwasserpreis;
                result.Abwasserpreis = cost.Abwasserpreis;
                result.Zaehlermiete = cost.Zaehlermiete;
                result.Bemerkung = cost.Bemerkung;
                return result;
            }
            return null;
        }

        public async Task<WaterCounterChange?> UpdateCounterChangeAsync(WaterCounterChange counterChange)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            var result = await _context.Wasser_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id);
            if (result != null)
            {
                result.Id_Anbieter = counterChange.Id_Anbieter;
                result.Anbieter = counterChange.Anbieter;
                result.Wechsel_Datum = counterChange.Wechsel_Datum;
                result.Zaehlerstand_alt = counterChange.Zaehlerstand_alt;
                result.Zaehlerstand_neu = counterChange.Zaehlerstand_neu;
                result.Zaehlerstand_aussen_alt = counterChange.Zaehlerstand_aussen_alt;
                result.Zaehlerstand_aussen_neu = counterChange.Zaehlerstand_aussen_neu;
                result.Bemerkung = counterChange.Bemerkung;
                return result;
            }
            return null;
        }
        #endregion

        #region SaveChanges
        // Zwingend erforderlich, damit EF Core die obigen Updates an die MySQL-DB schickt
        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                // Wir führen das Speichern aus. 
                // Ob 0 oder mehr Zeilen geändert wurden, ist für den reinen Erfolg egal.
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Nur wenn die Datenbank wirklich einen Fehler wirft, geben wir false zurück
                return false;
            }
        }

        // nur zum Testen
        public async Task<IEnumerable<WaterTarif>> Search(string tarif, double? zaehlermiete)
        {
            IQueryable<WaterTarif> query = _context.Wasser_tarif;

            if (!string.IsNullOrEmpty(tarif))
            {
                query = query.Where(m => m.Tarif!.Contains(tarif));
            }

            if (zaehlermiete != null)
            {
                query = query.Where(m => m.Zaehlermiete == zaehlermiete);
            }

            return await query.ToListAsync();
        }
        #endregion
    }
}