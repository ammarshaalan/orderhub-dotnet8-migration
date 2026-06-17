using OrderHub.Core.Models;

namespace OrderHub.Core.Interfaces;

public interface ISchoolRepository
{
    Task<School?> GetByIdAsync(
        int schoolId,
        CancellationToken cancellationToken = default);
}