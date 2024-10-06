using Microsoft.EntityFrameworkCore;

namespace AxiomaAssignment
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AuditDbContext _context;

        public AuditRepository(AuditDbContext context)
        {
            _context = context;
        }

        public async Task AddAuditAsync(IAudit audit)
        {
            var auditEntity = new Audit
            {
                Query = audit.Query,
                NumberOfOccurrences = audit.NumberOfOccurrences,
                DateTime = audit.DateTime
            };

            await _context.Audits.AddAsync(auditEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<IAudit>> GetAllAuditsAsync()
        {
            return await _context.Audits
                .Select(a => (IAudit)a)
                .ToListAsync();
        }
    }

}
