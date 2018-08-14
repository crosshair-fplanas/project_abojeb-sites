using AbojebApi.Data.Interfaces.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbojebApi.Data.Interfaces.Members
{
    public interface IUnitOfWork
    {
        IReportRepository Report { get; }
        IVesselRepository Vessel { get; }
        void Save();
    }
}
