using GWS_Api.Models;
using GWS_Api.Models.Gas;

namespace GWS_Api.Repositories.Gas
{
    public interface IGasRepository
    {
        #region GET
        Task<IEnumerable<GasTarif>> GetSuppliersAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<GasCounter>> GetCountersAsync(CancellationToken cancellationToken = default);
        Task<GasTarif?> GetSupplierByIdAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<IEnumerable<GasCounter>> GetCountersBySupplierAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<GasCounter?> GetCounterByDateAsync(DateTime date, CancellationToken cancellationToken = default);
        Task<GasCounter?> GetCounterByIdAsync(int counterId, CancellationToken cancellationToken = default);
        Task<IEnumerable<GasPayment>> GetPaymentsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<GasPayment>> GetPaymentsBySupplierAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<GasPayment?> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<GasCost>> GetCostsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<GasCost>> GetCostsBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<GasCost?> GetCostByIdAsync(int costId, CancellationToken cancellationToken = default);
        Task<IEnumerable<GasBoiler>> GetBoilerDataAsync(CancellationToken cancellationToken = default);
        Task<GasBoiler?> GetBoilerDataByIdAsync(int boilerDataId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentMethod>> GetPaymentMethodsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<GasCounterChange>> GetCounterChangesAsync(CancellationToken cancellationToken = default);
        Task<GasCounterChange?> GetCounterChangeByIdAsync(int counterChangeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<GasCounterChange>> GetCounterChangesBySupplierAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Efficiency>> GetEnergieEfficiencyAsync(CancellationToken cancellationToken = default);
        Task<Efficiency?> GetEnergieEfficiencyByIdAsync(int id, CancellationToken cancellationToken = default);
        #endregion

        #region ADD
        Task<GasTarif> AddSupplierAsync(GasTarif supplier, CancellationToken cancellationToken = default);
        Task<GasCounter?> AddCounterAsync(GasCounter zaehlerstand, CancellationToken cancellationToken = default);
        Task<GasPayment> AddPaymentAsync(GasPayment payment, CancellationToken cancellationToken = default);
        Task<GasCost> AddCostAsync(GasCost costs, CancellationToken cancellationToken = default);
        Task<GasCounterChange> AddCounterChangeAsync(GasCounterChange counterChange, CancellationToken cancellationToken = default);
        Task<GasBoiler> AddBoilerDataAsync(GasBoiler boilerData, CancellationToken cancellationToken = default);
        #endregion

        #region DELETE
        Task DeleteSupplierAsync(GasTarif supplier, CancellationToken cancellationToken = default);
        Task DeleteCounterAsync(GasCounter zaehlerstand, CancellationToken cancellationToken = default);
        Task DeletePaymentAsync(GasPayment payment, CancellationToken cancellationToken = default);
        Task DeleteCostAsync(GasCost costs, CancellationToken cancellationToken = default);
        Task DeleteBoilerDataAsync(GasBoiler boilerData, CancellationToken cancellationToken = default);
        Task DeleteCounterChangeAsync(GasCounterChange counterChange, CancellationToken cancellationToken = default);
        #endregion

        #region UPDATE
        Task<GasTarif?> UpdateSupplierAsync(GasTarif supplier, CancellationToken cancellationToken = default);
        Task<GasCounter?> UpdateCounterAsync(GasCounter zaehlerstand, CancellationToken cancellationToken = default);
        Task<GasPayment?> UpdatePaymentAsync(GasPayment payment, CancellationToken cancellationToken = default);
        Task<GasCost?> UpdateCostAsync(GasCost costs, CancellationToken cancellationToken = default);
        Task<GasCounterChange?> UpdateCounterChangeAsync(GasCounterChange counterChange, CancellationToken cancellationToken = default);
        Task<GasBoiler?> UpdateBoilerDataAsync(GasBoiler boilerData, CancellationToken cancellationToken = default);
        #endregion

        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}