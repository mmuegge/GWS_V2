using GWS_Api.Models;
using GWS_Api.Models.Water;

namespace GWS_Api.Repositories.Water
{
    public interface IWaterRepository
    {
        #region GET
        Task<IEnumerable<WaterTarif>> GetSuppliersAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<WaterCounter>> GetCountersAsync(CancellationToken cancellationToken = default);
        Task<WaterTarif?> GetSupplierByIdAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WaterCounter>> GetCountersBySupplierAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<WaterCounter?> GetCounterByDateAsync(DateTime date, CancellationToken cancellationToken = default);
        Task<WaterCounter?> GetCounterByIdAsync(int counterId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WaterPayment>> GetPaymentsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<WaterPayment>> GetPaymentsBySupplierAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<WaterPayment?> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WaterCost>> GetCostsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<WaterCost>> GetCostsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<WaterCost?> GetCostByIdAsync(int costId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentMethod>> GetPaymentMethodsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<WaterCounterChange>> GetCounterChangesAsync(CancellationToken cancellationToken = default);
        Task<WaterCounterChange?> GetCounterChangeByIdAsync(int counterChangeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WaterCounterChange>> GetCounterChangesBySupplierAsync(int supplierId, CancellationToken cancellationToken = default);
        #endregion

        #region ADD
        Task<WaterTarif> AddSupplierAsync(WaterTarif supplier, CancellationToken cancellationToken = default);
        Task<WaterCounter?> AddCounterAsync(WaterCounter zaehlerstand, CancellationToken cancellationToken = default);
        Task<WaterPayment> AddPaymentAsync(WaterPayment payment, CancellationToken cancellationToken = default);
        Task<WaterCost> AddCostAsync(WaterCost cost, CancellationToken cancellationToken = default);
        Task<WaterCounterChange> AddCounterChangeAsync(WaterCounterChange counterChange, CancellationToken cancellationToken = default);
        #endregion

        #region DELETE
        Task DeleteSupplierAsync(WaterTarif supplier, CancellationToken cancellationToken = default);
        Task DeleteCounterAsync(WaterCounter zaehlerstand, CancellationToken cancellationToken = default);
        Task DeletePaymentAsync(WaterPayment payment, CancellationToken cancellationToken = default);
        Task DeleteCostAsync(WaterCost cost, CancellationToken cancellationToken = default);
        Task DeleteCounterChangeAsync(WaterCounterChange counterChange, CancellationToken cancellationToken = default);
        #endregion

        #region UPDATE
        Task<WaterTarif?> UpdateSupplierAsync(WaterTarif supplier, CancellationToken cancellationToken = default);
        Task<WaterCounter?> UpdateCounterAsync(WaterCounter zaehlerstand, CancellationToken cancellationToken = default);
        Task<WaterPayment?> UpdatePaymentAsync(WaterPayment payment, CancellationToken cancellationToken = default);
        Task<WaterCost?> UpdateCostAsync(WaterCost cost, CancellationToken cancellationToken = default);
        Task<WaterCounterChange?> UpdateCounterChangeAsync(WaterCounterChange counterChange, CancellationToken cancellationToken = default);
        #endregion

        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}