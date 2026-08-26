using GWS_Api.Models;

namespace GWS_Api.Repositories
{
    public class Mock_ParameterRepository : IParameterRepository
    {
        #region Variablendeklaration
        int id_Parameter = 0;

        private static readonly List<Parameter> parameterList =
           [
           new ()
            {
                Id=1,
                Baujahr=new DateTime(2001,1,1),
                Wohnflaeche=149.18d,
                Bemerkung="Kommentar"
            }
           ];

        #endregion
        public Mock_ParameterRepository()
        {
            id_Parameter = parameterList.Count;
        }
        #region Add
        public async Task<Parameter> AddParameterAsync(Parameter para)
        {
            para.Id = ++id_Parameter;
            parameterList.Add(para);
            return await Task.FromResult(para);
        }
        #endregion
        #region DELETE
        public Task DeleteParameterAsync(Parameter para)
        {
            var result = parameterList.Find(m => m.Id == para.Id);
            if (result != null)
            {
                parameterList.Remove(result);
            }
            return Task.FromResult(result);
        }
        #endregion

        #region GET
        public async Task<IEnumerable<Parameter>> GetParameterAsync()
        {
            return await Task.FromResult(parameterList);
        }

        public async Task<Parameter?> GetParameterByIdAsync(int id)
        {
            var para = parameterList.Find(m => m.Id == id);

            return await Task.FromResult(para);
        }
        #endregion

        #region UPDATE
        public async Task<Parameter?> UpdateParameterAsync(Parameter para)
        {
            var result = parameterList.Find(m => m.Id == para.Id);
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
        public Task<bool> SaveChangesAsync()
        {
            var result = true;
            return Task.FromResult(result);
        }
        #endregion
    }
}
