using Maurice.Data.Context;
using Maurice.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maurice.Data
{
    public class DatabaseService : IDatabaseService
    {
        private readonly MauriceDbContext _context;

        public DatabaseService(MauriceDbContext context)
        {
            _context = context;
        }

        public async Task InitializeDatabaseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync(); // Creates database if it doesn't exist
            }
            catch (Exception ex)
            {
                // Handle migration errors
                throw new ApplicationException("Error initializing database", ex);
            }
        }

        public async Task<bool> SaveComprobanteAsync(Comprobante comprobante)
        {
            try
            {
                // Check if comprobante already exists
                var existing = await GetComprobanteByUUIDAsync(comprobante.UUID);
                if (existing != null)
                {
                    return false; // Already exists
                }

                var user = await GetUserAsync();

                if (user == null)
                {
                    throw new ApplicationException("Ningun usuario registrado aun.");
                }

                if (comprobante.RfcReceptor != user.Rfc && comprobante.RfcEmisor != user.Rfc)
                {
                    throw new ApplicationException("El RFC no coincide con el usuario registrado.");
                }

                if (comprobante is Factura factura)
                {
                    await _context.Facturas.AddAsync(factura);
                }
                else if (comprobante is Nomina nomina)
                {
                    await _context.Nominas.AddAsync(nomina);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Factura>> GetFacturasAsync()
        {
            return await _context.Facturas.ToListAsync();
        }

        public async Task<List<Nomina>> GetNominasAsync()
        {
            return await _context.Nominas.ToListAsync();
        }

        public async Task<Comprobante> GetComprobanteByUUIDAsync(string uuid)
        {
            var factura = await _context.Facturas.FirstOrDefaultAsync(f => f.UUID == uuid);
            if (factura != null) return factura;

            var nomina = await _context.Nominas.FirstOrDefaultAsync(n => n.UUID == uuid);
            return nomina;
        }

        public enum SearchPeriod
        {
            Monthly,
            Annual
        }

        public async Task<List<Comprobante>> SearchComprobantesAsync(string? rfc = null, DateTime? date = null, SearchPeriod period = SearchPeriod.Monthly)
        {
            var facturasQuery = _context.Facturas.AsQueryable();
            var nominasQuery = _context.Nominas.AsQueryable();

            if (!string.IsNullOrEmpty(rfc))
            {
                facturasQuery = facturasQuery.Where(f => f.RfcEmisor == rfc);
                nominasQuery = nominasQuery.Where(n => n.RfcEmisor == rfc);
            }

            if (date.HasValue)
            {
                if (period == SearchPeriod.Monthly)
                {
                    facturasQuery = facturasQuery.Where(f => f.Fecha.Value.Year == date.Value.Year && f.Fecha.Value.Month == date.Value.Month);
                    nominasQuery = nominasQuery.Where(n => n.Fecha.Value.Year == date.Value.Year && n.Fecha.Value.Month == date.Value.Month);
                }
                else // Annual
                {
                    facturasQuery = facturasQuery.Where(f => f.Fecha.Value.Year == date.Value.Year);
                    nominasQuery = nominasQuery.Where(n => n.Fecha.Value.Year == date.Value.Year);
                }
            }

            // Execute both queries
            var facturas = await facturasQuery.ToListAsync();
            var nominas = await nominasQuery.ToListAsync();

            // Combine and order results
            var allResults = new List<Comprobante>();
            allResults.AddRange(facturas);
            allResults.AddRange(nominas);

            return allResults
                .OrderByDescending(c => c.Fecha)
                .ToList();
        }

        public async Task<User> GetUserAsync()
        {
            return await _context.Users.FirstOrDefaultAsync();
        }

        public async Task SaveUserAsync(User user)
        {
            try
            {
                var existingUser = await _context.Set<User>().FirstOrDefaultAsync();
                if (existingUser != null)
                {
                    // Update existing user
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Rfc = user.Rfc;
                    existingUser.RegimenesFiscales = user.RegimenesFiscales;
                    existingUser.UpdatedAt = DateTime.UtcNow;
                    _context.Set<User>().Update(existingUser);
                }
                else
                {
                    // Add new user
                    await _context.Set<User>().AddAsync(user);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al guardar usuario", ex);
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}