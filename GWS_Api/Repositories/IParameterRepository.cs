using GWS_Api.Models;

namespace GWS_Api.Repositories
{
    public interface IParameterRepository
    {
        #region GET
        Task<Parameter?> GetParameterByIdAsync(int paraId);
        Task<IEnumerable<Parameter>> GetParameterAsync();
        #endregion

        #region ADD
        Task<Parameter> AddParameterAsync(Parameter para);
        #endregion

        #region DELETE
        Task DeleteParameterAsync(Parameter para);
        #endregion

        #region UPDATE
        Task<Parameter?> UpdateParameterAsync(Parameter para);
        #endregion

        Task<bool> SaveChangesAsync();
    }
}
