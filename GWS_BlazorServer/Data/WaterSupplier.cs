using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_BlazorServer.Data
{
    public class WaterSupplier
    {
        public int Id { get; set; }

        public string? Anbieter { get; set; }
    }
}
