using GWS_Api.Models;
using GWS_Api.Models.Electric;

namespace GWS_Api.Repositories.Electric
{
    public interface IElectricRepository
    {
        #region GET
        Task<IEnumerable<ElectricTarif>> GetSuppliersAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<ElectricCounter>> GetCountersAsync(CancellationToken cancellationToken = default);
        Task<ElectricTarif?> GetSupplierByIdAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ElectricCounter>> GetCountersBySupplierAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<ElectricCounter?> GetCounterByDateAsync(DateTime date, CancellationToken cancellationToken = default);
        Task<ElectricCounter?> GetCounterByIdAsync(int counterId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ElectricPayment>> GetPaymentsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<ElectricPayment>> GetPaymentsBySupplierAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<ElectricPayment?> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ElectricCost>> GetCostsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<ElectricCost>> GetCostsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<ElectricCost?> GetCostByIdAsync(int costId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentMethod>> GetPaymentMethodsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<ElectricCounterChange>> GetCounterChangesAsync(CancellationToken cancellationToken = default);
        Task<ElectricCounterChange?> GetCounterChangeByIdAsync(int counterChangeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ElectricCounterChange>> GetCounterChangesBySupplierAsync(int supplierId, CancellationToken cancellationToken = default);
        #endregion

        #region ADD
        Task<ElectricTarif> AddSupplierAsync(ElectricTarif supplier, CancellationToken cancellationToken = default);
        Task<ElectricCounter?> AddCounterAsync(ElectricCounter zaehlerstand, CancellationToken cancellationToken = default);
        Task<ElectricPayment> AddPaymentAsync(ElectricPayment payment, CancellationToken cancellationToken = default);
        Task<ElectricCost> AddCostAsync(ElectricCost cost, CancellationToken cancellationToken = default);
        Task<ElectricCounterChange> AddCounterChangeAsync(ElectricCounterChange counterChange, CancellationToken cancellationToken = default);
        #endregion

        #region DELETE
        Task DeleteSupplierAsync(ElectricTarif supplier, CancellationToken cancellationToken = default);
        Task DeleteCounterAsync(ElectricCounter zaehlerstand, CancellationToken cancellationToken = default);
        Task DeletePaymentAsync(ElectricPayment payment, CancellationToken cancellationToken = default);
        Task DeleteCostAsync(ElectricCost cost, CancellationToken cancellationToken = default);
        Task DeleteCounterChangeAsync(ElectricCounterChange counterChange, CancellationToken cancellationToken = default);
        #endregion

        #region UPDATE
        Task<ElectricTarif?> UpdateSupplierAsync(ElectricTarif supplier, CancellationToken cancellationToken = default);
        Task<ElectricCounter?> UpdateCounterAsync(ElectricCounter zaehlerstand, CancellationToken cancellationToken = default);
        Task<ElectricCost?> UpdateCostAsync(ElectricCost costs, CancellationToken cancellationToken = default);
        Task<ElectricPayment?> UpdatePaymentAsync(ElectricPayment payment, CancellationToken cancellationToken = default);
        Task<ElectricCounterChange?> UpdateCounterChangeAsync(ElectricCounterChange counterChange, CancellationToken cancellationToken = default);
        #endregion

        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}