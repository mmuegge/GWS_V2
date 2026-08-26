using GWS_Api.Models;

namespace GWS_Api.Repositories
{
    public interface IGWSRepository
    {
        #region GET
        Task<Efficiency?> GetEnergieEfficiencyByIdAsync(int effId);
        Task<IEnumerable<Efficiency>> GetEnergieEfficiencyAsync();
        #endregion

        #region ADD
        Task<Efficiency> AddEnergieEfficiencyAsync(Efficiency eff);
        #endregion

        #region DELETE
        Task DeleteEnergieEfficiencyAsync(Efficiency eff);
        #endregion

        #region UPDATE
        Task<Efficiency?> UpdateEnergieEfficiencyAsync(Efficiency eff);
        #endregion

        Task<bool> SaveChangesAsync();
    }
}