using AbojebApi.Core.DataTransferObjects;
using AbojebApi.Data.Services;
using AbojebApi.Models;
using AbojebApi.Utilities.Export;
using AbojebApi.Utilities.Models.KML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AbojebApi.Controllers
{
    public class VesselController : Controller
    {
        public ActionResult Index()
        {
            SearchReportViewModel vm = new SearchReportViewModel();
            VesselService vesselSvc = new VesselService();
            vm.Vessels = vesselSvc.GetVesselList();

            return View(vm);
        }

        [HttpPost]
        public PartialViewResult Index(SearchReportViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ReportModel rm = new ReportModel();
                    var results = rm.GetVesselReports(vm, true);
                    return PartialView("Report", results);
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("Report", new Core.DataTransferObjects.ReportDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportToCsv(SearchReportViewModel vm)
        {
            ReportModel rm = new ReportModel();
            var report = rm.GetVesselReports(vm, false);
            string dateTimeNow = DateTime.Now.ToString("yyyyMMddHHmm");
            return new CsvFileResult<ReportDto>(report, string.Format("report_{0}.csv", dateTimeNow));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportToExcel(SearchReportViewModel vm)
        {
            ReportModel rm = new ReportModel();
            var report = rm.GetVesselReports(vm, false);
            string dateTimeNow = DateTime.Now.ToString("yyyyMMddHHmm");
            return new XlsFileResult<ReportDto>(report, string.Format("report_{0}.xls", dateTimeNow));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportToKml(SearchReportViewModel vm)
        {
            ReportModel rm = new ReportModel();
            var report = rm.GetVesselReports(vm, false);

            //convert to Placemark list
            double latitude;
            double longitude;
            var loc = report.Select(s => new Placemark
            {
                Name = s.IMO.ToString(),
                Description = "",
                Point = new Location
                {
                    Latitude = Double.TryParse(s.Lat, out latitude) ? latitude : 0,
                    Longitude = Double.TryParse(s.Lon, out longitude) ? longitude : 0
                }
            }).ToList();

            string dateTimeNow = DateTime.Now.ToString("yyyyMMddHHmm");
            return new KmlFileResult(loc, string.Format("report_{0}.kml", dateTimeNow));
        }
    }
}
