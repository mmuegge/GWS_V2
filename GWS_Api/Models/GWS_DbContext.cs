using GWS_Api.Models.Electric;
using GWS_Api.Models.Gas;
using GWS_Api.Models.Water;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Models
{
    public class GWS_DbContext : DbContext
    {
        public GWS_DbContext(DbContextOptions<GWS_DbContext> options)
             : base(options)
        {

        }
        public DbSet<WaterTarif> Wasser_tarif { get; set; }             // Wasser_tarif --> Name der Tabelle  
        public DbSet<WaterCounter> Wasser_zaehlerstand { get; set; }    // Wasser_zaehlerstand --> Name der Tabelle
        public DbSet<WaterPayment> Wasser_zahlungen { get; set; }       // Wasser_zahlungen --> Name der Tabelle  
        public DbSet<WaterCost> Wasser_kosten { get; set; }            // Wasser_kosten --> Name der Tabelle
        public DbSet<WaterCounterChange> Wasser_zaehlerwechsel { get; set; }                 // Wasser_zaehlerwechsel --> Name der Tabelle

        public DbSet<ElectricTarif> Strom_tarif { get; set; }           // Strom_tarif --> Name der Tabelle  
        public DbSet<ElectricCounter> Strom_zaehlerstand { get; set; }  // Strom_zaehlerstand --> Name der Tabelle
        public DbSet<ElectricPayment> Strom_zahlungen { get; set; }     // Strom_zahlungen --> Name der Tabelle 
        public DbSet<ElectricCost> Strom_kosten { get; set; }          // Strom_kosten --> Name der Tabelle
        public DbSet<ElectricCounterChange> Strom_zaehlerwechsel { get; set; }                 // Strom_zaehlerwechsel --> Name der Tabelle

        public DbSet<GasTarif> Gas_tarif { get; set; }                  // Gas_tarif --> Name der Tabelle  
        public DbSet<GasCounter> Gas_zaehlerstand { get; set; }         // Gas_zaehlerstand --> Name der Tabelle
        public DbSet<GasPayment> Gas_zahlungen { get; set; }            // Gas_zahlungen --> Name der Tabelle  
        public DbSet<GasCost> Gas_kosten { get; set; }                 // Gas_kosten --> Name der Tabelle
        public DbSet<GasBoiler> Gas_therme {  get; set; }               // Gas_therme --> Name der Tablelle
        public DbSet<GasCounterChange> Gas_zaehlerwechsel { get; set; }                 // Gas_zaehlerwechsel --> Name der Tabelle

        public DbSet<PaymentMethod> Zahlungsarten { get; set; }         // Zahlungsarten --> Name der Tabelle
        public DbSet<Efficiency> Energie_effizienz { get; set; }               // Energie_effizienz --> Name der Tablelle
        public DbSet<Parameter> Haus_parameter { get; set; }             // Haus_parameter --> Name der Tabelle  

    }
}
