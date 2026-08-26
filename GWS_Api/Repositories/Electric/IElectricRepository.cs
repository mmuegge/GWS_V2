using GWS_Api.Models;
using GWS_Api.Models.Electric;

namespace GWS_Api.Repositories.Electric
{
    public interface IElectricRepository
    {
        #region GET
        Task<IEnumerable<ElectricTarif?>> GetSuppliersAsync();
        Task<IEnumerable<ElectricCounter?>> GetCountersAsync();
        Task<ElectricTarif?> GetSupplierByIdAsync(int supplierId);
        Task<IEnumerable<ElectricCounter?>> GetCountersBySupplierAsync(int supplierId);
        Task<ElectricCounter?> GetCounterByDateAsync(DateTime date);
        Task<ElectricCounter?> GetCounterByIdAsync(int counterId);
        Task<IEnumerable<ElectricPayment?>> GetPaymentsAsync();
        Task<IEnumerable<ElectricPayment?>> GetPaymentsBySupplierAsync(int supplierId);
        Task<ElectricPayment?> GetPaymentByIdAsync(int paymentId);
        Task<IEnumerable<ElectricCost?>> GetCostsAsync();
        Task<IEnumerable<ElectricCost?>> GetCostsBySupplierIdAsync(int supplierId);
        Task<ElectricCost?> GetCostByIdAsync(int costId);
        Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync();
        Task<IEnumerable<ElectricCounterChange?>> GetCounterChangesAsync();
        Task<ElectricCounterChange?> GetCounterChangeByIdAsync(int counterChangeId);
        Task<IEnumerable<ElectricCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId);
        #endregion

        #region ADD
        Task<ElectricTarif?> AddSupplierAsync(ElectricTarif supplier);
        Task<ElectricCounter?> AddCounterAsync(ElectricCounter zaehlerstand);
        Task<ElectricPayment?> AddPaymentAsync(ElectricPayment payment);
        Task<ElectricCost?> AddCostAsync(ElectricCost costs);
        Task<ElectricCounterChange?> AddCounterChangeAsync(ElectricCounterChange counterChange);
        #endregion

        #region DELETE
        Task DeleteSupplierAsync(ElectricTarif supplier);
        Task DeleteCounterAsync(ElectricCounter zaehlerstand);
        Task DeletePaymentAsync(ElectricPayment payment);
        Task DeleteCostAsync(ElectricCost cost);
        Task DeleteCounterChangeAsync(ElectricCounterChange counterChange);
        #endregion

        #region UPDATE
        Task<ElectricTarif?> UpdateSupplierAsync(ElectricTarif supplier);
        Task<ElectricCounter?> UpdateCounterAsync(ElectricCounter zaehlerstand);
        Task<ElectricCost?> UpdateCostAsync(ElectricCost costs);
        Task<ElectricPayment?> UpdatePaymentAsync(ElectricPayment payment);
        Task<ElectricCounterChange?> UpdateCounterChangeAsync(ElectricCounterChange counterChange);
        #endregion

        Task<bool> SaveChangesAsync();
    }
}