using GWS_Api.Models;
using GWS_Api.Models.Gas;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Repositories.Gas
{
    public class MySQL_GasRepository : IGasRepository
    {
        #region Variablendeklaration
        private readonly GWS_DbContext _context;
        public MySQL_GasRepository(GWS_DbContext context)
        {
            _context = context;
        }
        #endregion

        #region ADD
        public async Task<GasTarif?> AddSupplierAsync(GasTarif supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);
            var result = await _context.Gas_tarif.AddAsync(supplier);
            return result.Entity;
        }
        public async Task<GasCounter?> AddCounterAsync(GasCounter zaehlerstand)
        {
            // Sicherheitsnetz: Null-Prüfung wie bei den anderen Methoden
            ArgumentNullException.ThrowIfNull(zaehlerstand);

            // prüfen ob Ablesedatum in der Zukunft liegt
            if (zaehlerstand.Ablesetag > DateTime.Now)
            {
                return null;
            }

            // BEHOBEN: Prüfen, ob der Ablesetag FÜR DIESEN SPEZIFISCHEN LIEFERANTEN/ZÄHLER schon vorhanden ist
            int count = await _context.Gas_zaehlerstand
                .Where(m => m.Ablesetag == zaehlerstand.Ablesetag && m.ID_Anbieter == zaehlerstand.ID_Anbieter)
                .CountAsync();

            if (count > 0)
            {
                return null;
            }

            var result = await _context.Gas_zaehlerstand.AddAsync(zaehlerstand);
            return result.Entity;
        }
        public async Task<GasPayment?> AddPaymentAsync(GasPayment payment)
        {
            ArgumentNullException.ThrowIfNull(payment);
            var result = await _context.Gas_zahlungen.AddAsync(payment);
            return result.Entity;
        }
        public async Task<GasCost?> AddCostAsync(GasCost cost)
        {
            ArgumentNullException.ThrowIfNull(cost);
            var result = await _context.Gas_kosten.AddAsync(cost);
            return result.Entity;
        }
        public async Task<GasBoiler?> AddBoilerDataAsync(GasBoiler boilerData)
        {
            ArgumentNullException.ThrowIfNull(boilerData);
            var result = await _context.Gas_therme.AddAsync(boilerData);
            return result.Entity;
        }
        public async Task<Efficiency?> AddEnergieEfficiencyAsync(Efficiency eff)
        {
            ArgumentNullException.ThrowIfNull(eff);
            var result = await _context.Energie_effizienz.AddAsync(eff);
            return result.Entity;
        }
        public async Task<GasCounterChange?> AddCounterChangeAsync(GasCounterChange counterChange)
        {
            ArgumentNullException.ThrowIfNull(counterChange);
            var result = await _context.Gas_zaehlerwechsel.AddAsync(counterChange);
            return result.Entity;
        }
        #endregion

        #region DELETE
        public async Task DeleteSupplierAsync(GasTarif supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);

            var result = await _context.Gas_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id)
                            ?? throw new KeyNotFoundException($"Tarif mit ID {supplier.Id} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeleteCounterAsync(GasCounter counter)
        {
            ArgumentNullException.ThrowIfNull(counter);

            var result = await _context.Gas_zaehlerstand
                .FirstOrDefaultAsync(m => m.ID_Tag == counter.ID_Tag)
                ?? throw new KeyNotFoundException($"Zählerstand mit ID {counter.ID_Tag} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeletePaymentAsync(GasPayment payment)
        {
            ArgumentNullException.ThrowIfNull(payment);

            var result = await _context.Gas_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == payment.ID_Zahlung)
                 ?? throw new KeyNotFoundException($"Zahlung mit ID {payment.ID_Zahlung} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeleteCostAsync(GasCost cost)
        {
            ArgumentNullException.ThrowIfNull(cost);

            var result = await _context.Gas_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id)
                            ?? throw new KeyNotFoundException($"Kosten-Eintrag mit ID {cost.Id} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeleteBoilerDataAsync(GasBoiler boilerData)
        {
            ArgumentNullException.ThrowIfNull(boilerData);

            var result = await _context.Gas_therme.FirstOrDefaultAsync(m => m.Id == boilerData.Id)
                            ?? throw new KeyNotFoundException($"Kosten-Eintrag mit ID {boilerData.Id} nicht gefunden.");

            _context.Remove(result);
        }
        public async Task DeleteEnergieEfficiencyAsync(Efficiency eff)
        {
            ArgumentNullException.ThrowIfNull(eff);

            var result = await _context.Gas_therme.FirstOrDefaultAsync(m => m.Id == eff.Id)
                            ?? throw new KeyNotFoundException($"Effizienz-Eintrag mit ID {eff.Id} nicht gefunden.");

            _context.Remove(result);

        }
        public async Task DeleteCounterChangeAsync(GasCounterChange counterChange)
        {
            ArgumentNullException.ThrowIfNull(counterChange);

            var result = await _context.Gas_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id)
                            ?? throw new KeyNotFoundException($"Zählerwechsel mit ID {counterChange.Id} nicht gefunden.");
            _context.Remove(result);
        }
        #endregion

        #region GET
        public async Task<GasTarif?> GetSupplierByIdAsync(int supplierId)
        {
            return await _context.Gas_tarif.FirstOrDefaultAsync(m => m.Id == supplierId);
        }
        public async Task<IEnumerable<GasTarif?>> GetSuppliersAsync()
        {
            return await _context.Gas_tarif.ToListAsync();
        }
        public async Task<IEnumerable<GasCounter?>> GetCountersAsync()
        {
            var counters = await _context.Gas_zaehlerstand.ToListAsync();

            // Wir erstellen ein schnelles Dictionary für die Namenssuche (Key: Id, Value: Anbietername)
            var tarifDict = await _context.Gas_tarif
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
        public async Task<IEnumerable<GasCounter?>> GetCountersBySupplierAsync(int supplierId)
        {
            var result = await _context.Gas_zaehlerstand.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Gas_tarif
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
        public async Task<GasCounter?> GetCounterByIdAsync(int id)
        {
            var counter = await _context.Gas_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == id);
            if (counter != null)
            {
                var tarif = await _context.Gas_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter ?? string.Empty;
            }
            return counter;
        }
        public async Task<GasCounter?> GetCounterByDateAsync(DateTime date)
        {
            var counter = await _context.Gas_zaehlerstand.FirstOrDefaultAsync(m => m.Ablesetag == date);
            if (counter != null)
            {
                var tarif = await _context.Gas_tarif.FirstOrDefaultAsync(m => m.Id == counter.ID_Anbieter);
                counter.Anbieter = tarif!.Anbieter ?? string.Empty;
            }
            return counter;
        }
        public async Task<IEnumerable<GasPayment?>> GetPaymentsAsync()
        {
            var payments = await _context.Gas_zahlungen.ToListAsync();

            // Auch hier: Dictionary-Optimierung statt unzuverlässiger Sortierungsschleife
            var tarifDict = await _context.Gas_tarif
                .ToDictionaryAsync(x => x.Id, x => x.Anbieter);

            foreach (var payment in payments)
            {
                if (tarifDict.TryGetValue(payment.ID_Anbieter, out var name))
                {
                    payment.Anbieter = name ?? string.Empty; // Null-safe abgesichert;
                }
            }
            return payments;
        }
        public async Task<IEnumerable<GasPayment?>> GetPaymentsBySupplierAsync(int supplierId)
        {
            var result = await _context.Gas_zahlungen.Where(m => m.ID_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Gas_tarif
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync();

            if (supplierName != null)
            {
                foreach (var payment in result)
                {
                    payment.Anbieter = supplierName ?? string.Empty; // Null-safe abgesichert;
                }
            }
            return result;
        }
        public async Task<GasPayment?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _context.Gas_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == paymentId);
            if (payment != null)
            {
                var tarif = await _context.Gas_tarif.FirstOrDefaultAsync(m => m.Id == payment.ID_Anbieter);
                payment.Anbieter = tarif?.Anbieter ?? string.Empty;
            }
            return payment;
        }
        public async Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync()
        {
            return await _context.Zahlungsarten.ToListAsync();
        }
        public async Task<IEnumerable<GasCost?>> GetCostsBySupplierIdAsync(int supplierId)
        {
            var result = await _context.Gas_kosten.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Gas_tarif
                .Where(m => m.Id == supplierId)
                .Select(x => x.Anbieter)
                .FirstOrDefaultAsync();

            if (supplierName != null)
            {
                foreach (var costs in result)
                {
                    costs.Anbieter = supplierName ?? string.Empty; // Null-safe abgesichert;
                }
            }
            return result;
        }
        public async Task<IEnumerable<GasCost?>> GetCostsAsync()
        {
            return await _context.Gas_kosten.ToListAsync();
        }
        public async Task<GasCost?> GetCostByIdAsync(int costId)
        {
            return await _context.Gas_kosten.FirstOrDefaultAsync(m => m.Id == costId);
        }
        public async Task<IEnumerable<GasCounterChange?>> GetCounterChangesAsync()
        {
            return await _context.Gas_zaehlerwechsel.ToListAsync();
        }
        public async Task<GasCounterChange?> GetCounterChangeByIdAsync(int counterChangeId)
        {
            return await _context.Gas_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChangeId);
        }
        public async Task<IEnumerable<GasCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId)
        {
            var result = await _context.Gas_zaehlerwechsel.Where(m => m.Id_Anbieter == supplierId).ToListAsync();

            var supplierName = await _context.Gas_tarif
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
        public async Task<IEnumerable<GasBoiler?>> GetBoilerDataAsync()
        {
            return await _context.Gas_therme.ToListAsync();
        }
        public async Task<GasBoiler?> GetBoilerDataByIdAsync(int id)
        {
            return await _context.Gas_therme.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<IEnumerable<Efficiency>> GetEnergieEfficiencyAsync()
        {
            return await _context.Energie_effizienz.ToListAsync();
        }
        public async Task<Efficiency?> GetEnergieEfficiencyByIdAsync(int id)
        {
            return await _context.Energie_effizienz.FirstOrDefaultAsync(m => m.Id == id);
        }
        #endregion

        #region UPDATE
        public async Task<GasTarif?> UpdateSupplierAsync(GasTarif supplier)
        {
            var result = await _context.Gas_tarif.FirstOrDefaultAsync(m => m.Id == supplier.Id);
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
                result.Arbeitspreis = supplier.Arbeitspreis;
                result.Grundpreis = supplier.Grundpreis;
                result.Zaehlermiete = supplier.Zaehlermiete;
                result.Brennwert = supplier.Brennwert;
                result.Heizleistung = supplier.Heizleistung;
                result.Zustandszahl = supplier.Zustandszahl;
                result.Bemerkung = supplier.Bemerkung;
                return result;
            }
            return null;
        }
        public async Task<GasCounter?> UpdateCounterAsync(GasCounter zaehlerstand)
        {
            var result = await _context.Gas_zaehlerstand.FirstOrDefaultAsync(m => m.ID_Tag == zaehlerstand.ID_Tag);
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
        public async Task<GasPayment?> UpdatePaymentAsync(GasPayment payment)
        {
            var result = await _context.Gas_zahlungen.FirstOrDefaultAsync(m => m.ID_Zahlung == payment.ID_Zahlung);
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
        public async Task<GasCost?> UpdateCostAsync(GasCost cost)
        {
            var result = await _context.Gas_kosten.FirstOrDefaultAsync(m => m.Id == cost.Id);
            if (result != null)
            {
                result.Id = cost.Id;
                result.Id_Anbieter = cost.Id_Anbieter;
                result.Anbieter = cost.Anbieter;
                result.Grundpreis = cost.Grundpreis;
                result.Arbeitspreis = cost.Arbeitspreis;
                result.Zaehlermiete = cost.Zaehlermiete;
                result.Bemerkung = cost.Bemerkung;
                return result;
            }
            return null;
        }
        public async Task<GasBoiler?> UpdateBoilerDataAsync(GasBoiler boilerData)
        {
            var result = await _context.Gas_therme.FirstOrDefaultAsync(m => m.Id == boilerData.Id);
            if (result != null)
            {
                result.Id = boilerData.Id;
                result.Verbrauchsjahr = boilerData.Verbrauchsjahr;
                result.Gesamt_Verbrauch = boilerData.Gesamt_Verbrauch;
                result.Heizung_Verbrauch = boilerData.Heizung_Verbrauch;
                result.Warmwasser_Verbrauch = boilerData.Warmwasser_Verbrauch;
                result.Strom_Verbrauch = boilerData.Strom_Verbrauch;
                result.Bemerkung = boilerData.Bemerkung;
                return result;
            }
            return null;
        }
        public async Task<Efficiency?> UpdateEnergieEfficiencyAsync(Efficiency eff)
        {
            var result = await _context.Energie_effizienz.FirstOrDefaultAsync(m => m.Id == eff.Id);
            if (result != null)
            {
                result.Id = eff.Id;
                result.Energieklasse = eff.Energieklasse;
                result.Energiebedarf = eff.Energiebedarf;
                result.Farbcode = eff.Farbcode;
                result.Bemerkung = eff.Bemerkung;
                return result;
            }
            return null;
        }
        public async Task<GasCounterChange?> UpdateCounterChangeAsync(GasCounterChange counterChange)
        {
            var result = await _context.Gas_zaehlerwechsel.FirstOrDefaultAsync(m => m.Id == counterChange.Id);
            if (result != null)
            {
                result.Id = counterChange.Id;
                result.Id_Anbieter = counterChange.Id_Anbieter;
                result.Anbieter = counterChange.Anbieter;
                result.Wechsel_Datum = counterChange.Wechsel_Datum;
                result.Zaehlerstand_alt = counterChange.Zaehlerstand_alt;
                result.Zaehlerstand_neu = counterChange.Zaehlerstand_neu;
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
        public async Task<IEnumerable<GasTarif>> Search(string tarif, double? zaehlermiete)
        {
            IQueryable<GasTarif> query = _context.Gas_tarif;

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