using AbojebApi.Core.DataTransferObjects;
using AbojebApi.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbojebApi.Models
{
    public class ReportModel
    {
        public List<ReportDto> GetVesselReports(SearchReportViewModel vm, bool fetchNewData)
        {
            try
            {
                var selectedIMOs = new List<int>();
                if (vm.SelectedIMOs != null && vm.SelectedIMOs.Any())
                {
                    selectedIMOs = vm.SelectedIMOs.ToList();
                }

                var dateRange = vm.DateRange.Split('-');
                DateTime dateFrom = Convert.ToDateTime(dateRange[0].Trim());
                DateTime dateTo = Convert.ToDateTime(dateRange[1].Trim());

                VesselService vesselSvc = new VesselService();
                var results = vesselSvc.GetVesselsReports(selectedIMOs, dateFrom, dateTo, fetchNewData);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}