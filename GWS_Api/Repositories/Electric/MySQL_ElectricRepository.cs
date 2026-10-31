using GWS_Api.Models;
using GWS_Api.Models.Electric;
using GWS_Api.Models.Water;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Repositories.Electric
{
    public class MySQL_ElectricRepository(GWS_DbContext context) : IElectricRepository
    {
        #region Variablendeklaration
        private readonly GWS_DbContext _context = context;
        #endregion

        #region ADD
        public async Task<ElectricTarif?> AddSupplierAsync(ElectricTarif supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);
            var result = await _context.Strom_tarif.AddAsync(supplier);
            return result.Entity;
        }
        public async Task<ElectricCounter?> AddCounterAsync(ElectricCounter zaehlerstand)
        {
            // Sicherheitsnetz: Null-Prüfung wie bei den anderen Methoden
            ArgumentNullException.ThrowIfNull(zaehlerstand);

            // prüfen ob Ablesedatum in der Zukunft liegt
            if (zaehlerstand.Ablesetag > DateTime.Now)
            {
                return null;
            }

            // BEHOBEN: Prüfen, ob der Ablesetag FÜR DIESEN SPEZIFISCHEN LIEFERANTEN/ZÄHLER schon vorhanden ist
            int count = await _context.Strom_zaehlerstand
                .Where(m => m.Ablesetag == zaehlerstand.Ablesetag && m.ID_Anbieter == zaehlerstand.ID_Anbieter)
                .CountAsync();

            if (count > 0)
            {
                return null;
            }

            var result = await _context.Strom_zaehlerstand.AddAsync(zaehlerstand);
            return result.Entity;
        }
        public async Task<ElectricPayment?> AddPaymentAsync(ElectricPayment payment)
        {
            ArgumentNullException.ThrowIfNull(payment);
            var result = await _context.Strom_zahlungen.AddAsync(payment);
            return result.Entity;
        }
        public async Task<ElectricCost?> AddCostAsync(ElectricCost cost)
        {
            ArgumentNullException.ThrowIfNull(cost);
            var result = await _context.Strom_kosten.AddAsync(cost);
            return result.Entity;
        }
        public async Task<ElectricCounterChange?> AddCounterChangeAsync(ElectricCounterChange counterChange)
        {
            ArgumentNullException.ThrowIfNull(counterChange);
            var result = await _context.Strom_zaehlerwechsel.AddAsync(counterChange);
            return result.Entity;
        }
        #endregion

        #region DELETE
        public async Task DeleteSupplierAsync(ElectricTarif supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            var result = await _context.Strom_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id)
                            ?? throw new KeyNotFoundException($"Tarif mit ID {supplier.Id} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeleteCounterAsync(ElectricCounter counter)
        {
            ArgumentNullException.ThrowIfNull(counter);

            var result = await _context.Strom_zaehlerstand
                .FirstOrDefaultAsync(m => m.ID_Tag == counter.ID_Tag)
                ?? throw new KeyNotFoundException($"Zählerstand mit ID {counter.ID_Tag} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeletePaymentAsync(ElectricPayment payment)
        {
            ArgumentNullException.ThrowIfNull(payment);

            var result = await _context.Strom_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == payment.ID_Zahlung)
                 ?? throw new KeyNotFoundException($"Zahlung mit ID {payment.ID_Zahlung} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeleteCostAsync(ElectricCost cost)
        {
            ArgumentNullException.ThrowIfNull(cost);

            var result = await _context.Strom_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id)
                            ?? throw new KeyNotFoundException($"Kosten-Eintrag mit ID {cost.Id} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeleteCounterChangeAsync(ElectricCounterChange counterChange)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            var result = await _context.Strom_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id)
                            ?? throw new KeyNotFoundException($"Zählerwechsel mit ID {counterChange.Id} nicht gefunden.");
            _context.Remove(result);
        }
        #endregion

        #region GET
        public async Task<ElectricTarif?> GetSupplierByIdAsync(int supplierId)
        {
            return await _context.Strom_tarif.FirstOrDefaultAsync(m => m.Id == supplierId);
        }
        public async Task<IEnumerable<ElectricTarif?>> GetSuppliersAsync()
        {
            return await _context.Strom_tarif.ToListAsync();
        }
        public async Task<IEnumerable<ElectricCounter?>> GetCountersAsync()
        {
            var counters = await _context.Strom_zaehlerstand.ToListAsync();

            // Wir erstellen ein schnelles Dictionary für die Namenssuche (Key: Id, Value: Anbietername)
            var tarifDict = await _context.Strom_tarif
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
        public async Task<IEnumerable<ElectricCounter?>> GetCountersBySupplierAsync(int supplierId)
        {
            var result = await _context.Strom_zaehlerstand.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Strom_tarif
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
        public async Task<ElectricCounter?> GetCounterByIdAsync(int id)
        {
            var counter = await _context.Strom_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == id);
            if (counter != null)
            {
                var tarif = await _context.Strom_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif?.Anbieter ?? string.Empty; // Null-safe abgesichert
            }
            return counter;
        }
        public async Task<ElectricCounter?> GetCounterByDateAsync(DateTime date)
        {
            var counter = await _context.Strom_zaehlerstand.FirstOrDefaultAsync(m => m.Ablesetag == date);
            if (counter != null)
            {
                var tarif = await _context.Strom_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif?.Anbieter ?? string.Empty;
            }
            return counter;
        }
        public async Task<IEnumerable<ElectricPayment?>> GetPaymentsAsync()
        {
            var payments = await _context.Strom_zahlungen.ToListAsync();

            // Auch hier: Dictionary-Optimierung statt unzuverlässiger Sortierungsschleife
            var tarifDict = await _context.Strom_tarif
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
        public async Task<IEnumerable<ElectricPayment?>> GetPaymentsBySupplierAsync(int supplierId)
        {
            var result = await _context.Strom_zahlungen.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Strom_tarif
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
        public async Task<ElectricPayment?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _context.Strom_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == paymentId);
            if (payment != null)
            {
                var tarif = await _context.Strom_tarif.FirstOrDefaultAsync(m => m.Id == payment.ID_Anbieter);
                payment.Anbieter = tarif?.Anbieter ?? string.Empty;
            }
            return payment;
        }
        public async Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync()
        {
            return await _context.Zahlungsarten.ToListAsync();
        }
        public async Task<IEnumerable<ElectricCost?>> GetCostsAsync()
        {
            return await _context.Strom_kosten.ToListAsync();
        }
        public async Task<IEnumerable<ElectricCost?>> GetCostsBySupplierIdAsync(int supplierId)
        {
            var result = await _context.Strom_kosten.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Strom_tarif
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync();

            if (supplierName != null)
            {
                foreach (var costs in result)
                {
                    costs.Anbieter = supplierName ?? string.Empty;
                }
            }
            return result;
        }
        public async Task<ElectricCost?> GetCostByIdAsync(int costId)
        {
            return await _context.Strom_kosten.FirstOrDefaultAsync(m => m.Id == costId);
        }
        public async Task<IEnumerable<ElectricCounterChange?>> GetCounterChangesAsync()
        {
            return await _context.Strom_zaehlerwechsel.ToListAsync();
        }
        public async Task<ElectricCounterChange?> GetCounterChangeByIdAsync(int counterChangeId)
        {
            return await _context.Strom_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChangeId);
        }
        public async Task<IEnumerable<ElectricCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId)
        {
            var result = await _context.Strom_zaehlerwechsel.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Strom_tarif
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync();

            if (supplierName != null)
            {
                foreach (var counterChange in result)
                {
                    counterChange.Anbieter = supplierName ?? string.Empty;
                }
            }
            return result;
        }
        #endregion

        #region UPDATE
        public async Task<ElectricTarif?> UpdateSupplierAsync(ElectricTarif supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            var result = await _context.Strom_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id);
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
                result.Start_Zaehlerstand_280 = supplier.Start_Zaehlerstand_280;
                result.Ende_Zaehlerstand_280 = supplier.Ende_Zaehlerstand_280;
                result.Start_Zaehlerstand_Enfluri = supplier.Start_Zaehlerstand_Enfluri;
                result.Ende_Zaehlerstand_Enfluri = supplier.Ende_Zaehlerstand_Enfluri;
                result.Arbeitspreis = supplier.Arbeitspreis;
                result.Grundpreis = supplier.Grundpreis;
                result.Zaehlermiete = supplier.Zaehlermiete;
                result.Anzahl_Personen = supplier.Anzahl_Personen;
                result.Bemerkung = supplier.Bemerkung;
                return result;
            }
            return null;
        }
        public async Task<ElectricCounter?> UpdateCounterAsync(ElectricCounter zaehlerstand)
        {
            ArgumentNullException.ThrowIfNull(zaehlerstand);

            var result = await _context.Strom_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == zaehlerstand.ID_Tag);
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
                return result;
            }
            return null;
        }
        public async Task<ElectricPayment?> UpdatePaymentAsync(ElectricPayment payment)
        {
            ArgumentNullException.ThrowIfNull(payment);

            var result = await _context.Strom_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == payment.ID_Zahlung);
            if (result != null)
            {
                result.ID_Zahlung = payment.ID_Zahlung;
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
        public async Task<ElectricCost?> UpdateCostAsync(ElectricCost cost)
        {
            ArgumentNullException.ThrowIfNull(cost);

            var result = await _context.Strom_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id);

            if (result != null)
            {
                result.Id = cost.Id;
                result.Id_Anbieter = cost.Id_Anbieter;
                result.Anbieter = cost.Anbieter;
                result.Grundpreis = cost.Grundpreis;
                result.Arbeitspreis = cost.Arbeitspreis;
                result.Zaehlermiete = cost.Zaehlermiete;
                result.Bemerkung = cost.Bemerkung;
            }

            return await Task.FromResult(result);
        }
        public async Task<ElectricCounterChange?> UpdateCounterChangeAsync(ElectricCounterChange counterChange)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            var result = await _context.Strom_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id);
            if (result != null)
            {
                result.Id = counterChange.Id;
                result.Id_Anbieter = counterChange.Id_Anbieter;
                result.Anbieter = counterChange.Anbieter;
                result.Wechsel_Datum = counterChange.Wechsel_Datum;
                result.Zaehlerstand_alt = counterChange.Zaehlerstand_alt;
                result.Zaehlerstand_neu = counterChange.Zaehlerstand_neu;
                result.Zaehlerstand_280_alt = counterChange.Zaehlerstand_280_alt;
                result.Zaehlerstand_280_neu = counterChange.Zaehlerstand_280_neu;
                result.Zaehlerstand_Enfluri_alt = counterChange.Zaehlerstand_Enfluri_alt;
                result.Zaehlerstand_Enfluri_neu = counterChange.Zaehlerstand_Enfluri_neu;
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
        public async Task<IEnumerable<ElectricTarif>> Search(string tarif, double? zaehlermiete)
        {
            IQueryable<ElectricTarif> query = _context.Strom_tarif;

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