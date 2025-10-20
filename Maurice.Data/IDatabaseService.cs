using Maurice.Data.Models;
using static Maurice.Data.DatabaseService;

namespace Maurice.Data
{
    public interface IDatabaseService
    {
        Task InitializeDatabaseAsync();
        Task<bool> SaveComprobanteAsync(Comprobante comprobante);
        Task<List<Factura>> GetFacturasAsync();
        Task<List<Nomina>> GetNominasAsync();
        Task<Comprobante> GetComprobanteByUUIDAsync(string uuid);
        Task SaveUserAsync(User user);
        Task<User> GetUserAsync();
        Task<List<Comprobante>> SearchComprobantesAsync(string? rfc = null, DateTime? date = null, SearchPeriod period = SearchPeriod.Monthly);
    }
}
