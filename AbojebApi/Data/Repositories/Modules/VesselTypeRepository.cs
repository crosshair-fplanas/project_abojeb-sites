using AbojebApi.Core.Data;
using AbojebApi.Data.Interfaces.Modules;

namespace AbojebApi.Data.Repositories.Modules
{
    public class VesselTypeRepository : Repository<VesselType>, IVesselTypeRepository
    {
        public VesselTypeRepository(VesselDbContext context)
            : base(context)
        {

        }
    }
}
