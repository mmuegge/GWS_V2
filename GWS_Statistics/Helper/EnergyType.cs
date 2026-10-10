using System.ComponentModel.DataAnnotations;

namespace GWS_Statistics.Helper
{
    //public enum EnergyType
    //{
    //    Electric,
    //    Gas,
    //    Water
    //}

    public enum Interval
    {
        [Display(Name = "täglich")]
        täglich = 1,
        [Display(Name = "monatlich")]
        monatlich = 2,
        [Display(Name = "jährlich")]
        jährlich = 3
    }
}
