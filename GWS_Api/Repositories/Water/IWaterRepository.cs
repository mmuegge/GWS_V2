using GWS_Api.Models;
using GWS_Api.Models.Water;

namespace GWS_Api.Repositories.Water
{
    public interface IWaterRepository
    {
        #region GET
        Task<IEnumerable<WaterTarif>> GetSuppliersAsync();
        Task<IEnumerable<WaterCounter?>> GetCountersAsync();
        Task<WaterTarif?> GetSupplierByIdAsync(int supplierId);
        Task<IEnumerable<WaterCounter?>> GetCountersBySupplierAsync(int supplierId);
        Task<WaterCounter?> GetCounterByDateAsync(DateTime date);
        Task<WaterCounter?> GetCounterByIdAsync(int counterId);
        Task<IEnumerable<WaterPayment?>> GetPaymentsAsync();
        Task<IEnumerable<WaterPayment?>> GetPaymentsBySupplierAsync(int supplierId);
        Task<WaterPayment?> GetPaymentByIdAsync(int paymentId);
        Task<IEnumerable<WaterCost?>> GetCostsAsync();
        Task<IEnumerable<WaterCost?>> GetCostsBySupplierIdAsync(int supplierId);
        Task<WaterCost?> GetCostByIdAsync(int costId);
        Task<IEnumerable<PaymentMethod?>> GetPaymentMethodsAsync();
        Task<IEnumerable<WaterCounterChange?>> GetCounterChangesAsync();
        Task<WaterCounterChange?> GetCounterChangeByIdAsync(int counterChangeId);
        Task<IEnumerable<WaterCounterChange?>> GetCounterChangesBySupplierAsync(int supplierId);
        #endregion

        #region ADD
        Task<WaterTarif> AddSupplierAsync(WaterTarif supplier);
        Task<WaterCounter?> AddCounterAsync(WaterCounter zaehlerstand);
        Task<WaterPayment> AddPaymentAsync(WaterPayment payment);
        Task<WaterCost> AddCostAsync(WaterCost cost);
        Task<WaterCounterChange> AddCounterChangeAsync(WaterCounterChange counterChange);
        #endregion

        #region DELETE
        Task DeleteSupplierAsync(WaterTarif supplier);
        Task DeleteCounterAsync(WaterCounter zaehlerstand);
        Task DeletePaymentAsync(WaterPayment payment);
        Task DeleteCostAsync(WaterCost cost);
        Task DeleteCounterChangeAsync(WaterCounterChange counterChange);
        #endregion

        #region UPDATE
        Task<WaterTarif?> UpdateSupplierAsync(WaterTarif supplier);
        Task<WaterCounter?> UpdateCounterAsync(WaterCounter zaehlerstand);
        Task<WaterPayment?> UpdatePaymentAsync(WaterPayment payment);
        Task<WaterCost?> UpdateCostAsync(WaterCost cost);
        Task<WaterCounterChange?> UpdateCounterChangeAsync(WaterCounterChange counterChange);
        #endregion

        Task<bool> SaveChangesAsync();
    }
}