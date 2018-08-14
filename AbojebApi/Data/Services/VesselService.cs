using AbojebApi.Core.Data;
using AbojebApi.Core.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbojebApi.Data.Services
{
    public class VesselService
    {
        /// <summary>
        /// Gets list of IMO and Vessel pairs
        /// </summary>
        /// <returns></returns>
        public List<VesselDto> GetVesselList()
        {
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
                return unitOfWork.Vessel.Collections
                    .Select(s => new VesselDto
                    {
                        IMO = s.IMO,
                        BoatName = s.BoatName})
                    .ToList();
            }
        }

        /// <summary>
        /// Gets vessels and corresponding reports
        /// </summary>
        /// <param name="IMOs">List of IMO numbers</param>
        /// <param name="dateFrom">Start for date range filter</param>
        /// <param name="dateTo">End for date range filter</param>
        /// <returns></returns>
        public List<ReportDto> GetVesselsReports(List<int> IMOs, DateTime? dateFrom, DateTime? dateTo, bool fetchNewData)
        {
            //Update records from API
            if (fetchNewData)
            {
                ApiService apiSvc = new ApiService();
                apiSvc.GetVesselsByIMOs(IMOs);
            }

            //Get vessels and corresponding reports
            if (dateTo != null)
            {
                dateTo = dateTo.Value.AddDays(1);
            }
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
                var reports = unitOfWork.Report.Find(f =>
                        (IMOs.Count == 0 || (IMOs.Count > 0 && IMOs.Contains(f.IMO))) &&
                        f.GPSTimeStamp >= dateFrom && f.GPSTimeStamp < dateTo)
                    .AsEnumerable()
                    .Select(s => s.Convert<Report, ReportDto>())
                    .ToList();

                //var results = unitOfWork.Vessel.Collections
                //    .Join(reports,
                //        vessel => vessel.IMO,
                //        report => report.IMO,
                //        (vessel, report) => vessel)
                //    .AsEnumerable()
                //    .Select(s => s.Convert<Vessel, VesselReportDto>())
                //    .ToList();

                return reports;
            }
        }
    }
}
