using System.Collections.Generic;

namespace AbojebApi.Core.DataTransferObjects
{
    public class VesselReportDto
    {
        public int IMO { get; set; }

        public string BoatName { get; set; }

        public string VesselTypeName { get; set; }

        public string CallSign { get; set; }

        public int MMSI { get; set; }

        public IEnumerable<ReportDto> Reports { get; set; }
    }
}
