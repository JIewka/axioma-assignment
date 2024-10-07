namespace AxiomaAssignment.Repositories
{
    public interface IAuditRepository
    {
        Task AddAuditAsync(IAudit audit);
        Task<List<IAudit>> GetAllAuditsAsync();
    }
}
