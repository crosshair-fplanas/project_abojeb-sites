using AbojebApi.Core.Data;
using AbojebApi.Data.Interfaces.Modules;

namespace AbojebApi.Data.Repositories.Modules
{
    public class VesselRepository : Repository<Vessel>, IVesselRepository
    {
        public VesselRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}
