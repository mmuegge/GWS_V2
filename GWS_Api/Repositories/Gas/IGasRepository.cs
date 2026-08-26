using GWS_Api.Models;
using GWS_Api.Models.Gas;

namespace GWS_Api.Repositories.Gas
{
    public interface IGasRepository
    {
        #region GET
        Task<IEnumerable<GasTarif?>> GetSuppliersAsync();
        Task<IEnumerable<GasCounter?>> GetCountersAsync();
        Task<GasTarif?> GetSupplierByIdAsync(int supplierId);
        Task<IEnumerable<GasCounter?>> GetCountersBySupplierAsync(int supplierId);
        Task<GasCounter?> GetCounterByDateAsync(DateTime date);
        Task<GasCounter?> GetCounterByIdAsync(int counterId);
        Task<IEnumerable<GasPayment?>> GetPaymentsAsync();
        Task<IEnumerable<GasPayment?>> GetPaymentsBySupplierAsync(int supplierId);
        Task<GasPayment?> GetPaymentByIdAsync(int paymentId);
        Task<IEnumerable<GasCost?>> GetCostsAsync();
        Task<IEnumerable<GasCost?>> GetCostsBySupplierIdAsync(int supplierId);
        Task<GasCost?> GetCostByIdAsync(int costId);
        Task<IEnumerable<GasBoiler?>> GetBoilerDataAsync();
        Task<GasBoiler?> GetBoilerDataByIdAsync(int boilerDataId);
        Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync();
        Task<IEnumerable<GasCounterChange?>> GetCounterChangesAsync();
        Task<GasCounterChange?> GetCounterChangeByIdAsync(int counterChangeId);
        Task<IEnumerable<GasCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId);
        #endregion

        #region ADD
        Task<GasTarif?> AddSupplierAsync(GasTarif supplier);
        Task<GasCounter?> AddCounterAsync(GasCounter zaehlerstand);
        Task<GasPayment?> AddPaymentAsync(GasPayment payment);
        Task<GasCost?> AddCostAsync(GasCost costs);
        Task<GasBoiler?> AddBoilerDataAsync(GasBoiler boilerData);
        Task<GasCounterChange?> AddCounterChangeAsync(GasCounterChange counterChange);
        #endregion

        #region DELETE
        Task DeleteSupplierAsync(GasTarif supplier);
        Task DeleteCounterAsync(GasCounter zaehlerstand);
        Task DeletePaymentAsync(GasPayment payment);
        Task DeleteCostAsync(GasCost costs);
        Task DeleteBoilerDataAsync(GasBoiler boilerData);
        Task DeleteCounterChangeAsync(GasCounterChange counterChange);
        #endregion

        #region UPDATE
        Task<GasTarif?> UpdateSupplierAsync(GasTarif supplier);
        Task<GasCounter?> UpdateCounterAsync(GasCounter zaehlerstand);
        Task<GasPayment?> UpdatePaymentAsync(GasPayment payment);
        Task<GasCost?> UpdateCostAsync(GasCost costs);
        Task<GasBoiler?> UpdateBoilerDataAsync(GasBoiler boilerData);
        Task<GasCounterChange?> UpdateCounterChangeAsync(GasCounterChange counterChange);
        #endregion

        Task<bool> SaveChangesAsync();
    }
}