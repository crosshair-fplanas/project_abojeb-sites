using AbojebApi.Data.Interfaces.Members;
using AbojebApi.Data.Interfaces.Modules;
using AbojebApi.Data.Repositories.Modules;
using System;

namespace AbojebApi.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext context;
        private bool disposed;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            this.Report = new ReportRepository(context);
            this.Vessel = new VesselRepository(context);
        }

        public IReportRepository Report { get; private set; }
        public IVesselRepository Vessel { get; private set; }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }
    }
}