using GWS_Api.Models;

namespace GWS_Api.Repositories
{
    public class Mock_GWSRepository : IGWSRepository
    {
        #region Variablendeklaration
        int id_EnergieEff = 0;

        private static readonly List<Efficiency> energieEffList =
           [
           new ()
            {
                Id=1,
                Energieklasse="A+",
                Energiebedarf=30,
                Farbcode="#006400",
                Bemerkung="kWh/m2/a"
            },
            new ()
            {
                Id=2,
                Energieklasse="A",
                Energiebedarf=50,
                Farbcode="#00e600",
                Bemerkung="kWh/m2/a"
            },
            new ()
            {
               Id=3,
                Energieklasse="B",
                Energiebedarf=75,
                Farbcode="#00e600",
                Bemerkung="kWh/m2/a"
            },
             new ()
            {
               Id=4,
                Energieklasse="C",
                Energiebedarf=100,
                Farbcode="#e6e600",
                Bemerkung="kWh/m2/a"
            },
             new ()
            {
               Id=5,
                Energieklasse="D",
                Energiebedarf=130,
                Farbcode="#e6ac00",
                Bemerkung="kWh/m2/a"
            },
             new ()
            {
               Id=6,
                Energieklasse="E",
                Energiebedarf=160,
                Farbcode="#e67300",
                Bemerkung="kWh/m2/a"
            },
             new ()
            {
               Id=7,
                Energieklasse="F",
                Energiebedarf=200,
                Farbcode="#e63900",
                Bemerkung="kWh/m2/a"
            },
              new ()
            {
               Id=8,
                Energieklasse="G",
                Energiebedarf=250,
                Farbcode="#e60000",
                Bemerkung="kWh/m2/a"
            },
             new ()
            {
               Id=9,
                Energieklasse="H",
                Energiebedarf=250,
                Farbcode="#ff0000",
                Bemerkung="kWh/m2/a"
            },
           ];

        #endregion
        public Mock_GWSRepository()
        {
            id_EnergieEff = energieEffList.Count;
        }

        #region ADD
        public async Task<Efficiency> AddEnergieEfficiencyAsync(Efficiency eff)
        {
            eff.Id = ++id_EnergieEff;
            energieEffList.Add(eff);
            return await Task.FromResult(eff);
        }
        #endregion

        #region DELETE
        public Task DeleteEnergieEfficiencyAsync(Efficiency eff)
        {
            var result = energieEffList.Find(m => m.Id == eff.Id);
            if (result != null)
            {
                energieEffList.Remove(result);
            }
            return Task.FromResult(result);
        }
        #endregion

        #region GET
        public async Task<IEnumerable<Efficiency>> GetEnergieEfficiencyAsync()
        {
            return await Task.FromResult(energieEffList);
        }

        public async Task<Efficiency?> GetEnergieEfficiencyByIdAsync(int id)
        {
            var eff = energieEffList.Find(m => m.Id == id);

            return await Task.FromResult(eff!);
        }
        #endregion

        #region UPDATE
        public async Task<Efficiency?> UpdateEnergieEfficiencyAsync(Efficiency eff) // not used
        {
            var result = energieEffList.Find(m => m.Id == eff.Id);
            if (result != null)
            {
                result.Id = eff.Id;
                result.Energieklasse = eff.Energieklasse;
                result.Energiebedarf = eff.Energiebedarf;
                result.Farbcode = eff.Farbcode;
                result.Bemerkung = eff.Bemerkung;
            }

            return await Task.FromResult(result);
        }
        #endregion

        #region SaveChangesAsync
        /// <summary>
        /// SaveChanges
        /// </summary>
        /// <returns></returns>
        public Task<bool> SaveChangesAsync()
        {
            var result = true;
            return Task.FromResult(result);
        }
        #endregion
    }
}