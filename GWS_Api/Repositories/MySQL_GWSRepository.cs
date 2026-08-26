using GWS_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Repositories
{
    public class MySQL_GWSRepository : IGWSRepository
    {
        #region Variablendeklaration
        private readonly GWS_DbContext _context;
        public MySQL_GWSRepository(GWS_DbContext context)
        {
            _context = context;
        }
        #endregion

        #region ADD
        public async Task<Efficiency> AddEnergieEfficiencyAsync(Efficiency eff)
        {
            ArgumentNullException.ThrowIfNull(eff);
            var result = await _context.Energie_effizienz.AddAsync(eff);
            return result.Entity;
        }
        #endregion

        #region DELETE
        public async Task DeleteEnergieEfficiencyAsync(Efficiency eff)
        {
            var result = await _context.Energie_effizienz.FirstOrDefaultAsync(m => m.Id == eff.Id);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(eff));
            }
            _context.Remove(result);
        }
        #endregion

        #region GET
        public async Task<IEnumerable<Efficiency>> GetEnergieEfficiencyAsync()
        {
            List<Efficiency> eff = await _context.Energie_effizienz.ToListAsync();

            return await Task.FromResult(eff);
        }
        public async Task<Efficiency?> GetEnergieEfficiencyByIdAsync(int id)
        {
            var eff = await _context.Energie_effizienz.FirstOrDefaultAsync(m => m.Id == id);

            return await Task.FromResult(eff);
        }
        #endregion

        #region UPDATE
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
            }

            return await Task.FromResult(result);
        }
        #endregion

        #region SaveChangesAsync
        /// <summary>
        /// SaveChanges
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);    // return count of state entries written to DB
        }
        #endregion

    }
}