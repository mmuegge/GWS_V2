using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWS_Api.Models.Water
{
    public class WaterCost
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Id_Anbieter { get; set; }
        [NotMapped]
        public string? Anbieter { get; set; }
        [Required]
        public DateTime? Gueltig_Ab { get; set; }
        public double? Grundpreis { get; set; }
        public double? Trinkwasserpreis { get; set; }
        public double? Abwasserpreis { get; set; }
        public double? Zaehlermiete { get; set; }
        [MaxLength(250)]
        public string? Bemerkung { get; set; }
    }
}
