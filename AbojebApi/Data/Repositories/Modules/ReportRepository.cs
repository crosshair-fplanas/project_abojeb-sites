using AbojebApi.Core.Data;
using AbojebApi.Data.Interfaces.Modules;

namespace AbojebApi.Data.Repositories.Modules
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}