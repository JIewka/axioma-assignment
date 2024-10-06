namespace AxiomaAssignment
{
    public interface IAuditRepository
    {
        Task AddAuditAsync(IAudit audit);
        Task<List<IAudit>> GetAllAuditsAsync();
    }
}
