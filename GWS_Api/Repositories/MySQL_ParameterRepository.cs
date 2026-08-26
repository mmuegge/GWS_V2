using GWS_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GWS_Api.Repositories
{
    public class MySQL_ParameterRepository : IParameterRepository
    {
        #region Variablendeklaration
        private readonly GWS_DbContext _context;
        public MySQL_ParameterRepository(GWS_DbContext context)
        {
            _context = context;
        }
        #endregion
        #region ADD
        public async Task<Parameter> AddParameterAsync(Parameter para)
        {
            ArgumentNullException.ThrowIfNull(para);
            var result = await _context.Haus_parameter.AddAsync(para);
            return result.Entity;
        }
        #endregion
        #region DELETE
        public async Task DeleteParameterAsync(Parameter para)
        {
            var result = await _context.Haus_parameter.FirstOrDefaultAsync(m => m.Id == para.Id);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(para));
            }
            _context.Remove(result);
        }
        #endregion

        #region GET
        public async Task<IEnumerable<Parameter>> GetParameterAsync()
        {
            List<Parameter> para = await _context.Haus_parameter.ToListAsync();

            return await Task.FromResult(para);
        }
        #endregion

        #region
        public async Task<Parameter?> GetParameterByIdAsync(int id)
        {
            var para = await _context.Haus_parameter.FirstOrDefaultAsync(m => m.Id == id);

            return await Task.FromResult(para);
        }
        #endregion

        #region UPDATE
        public async Task<Parameter?> UpdateParameterAsync(Parameter para)
        {
            var result = await _context.Haus_parameter.FirstOrDefaultAsync(m => m.Id == para.Id);

            if (result != null)
            {
                result.Id = para.Id;
                result.Baujahr = para.Baujahr;
                result.Wohnflaeche = para.Wohnflaeche;
                result.Bemerkung = para.Bemerkung;
            }

            return await Task.FromResult(result);
        }
        #endregion

        #region SaveChangesAsync
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);    // return count of state entries written to DB
        }
        #endregion

    }
}
