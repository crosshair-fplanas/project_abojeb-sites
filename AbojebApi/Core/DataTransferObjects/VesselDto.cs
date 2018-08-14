using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace AbojebApi.Core.DataTransferObjects
{
    [DataContract]
    public class VesselDto
    {
        [DataMember(Name = "imo")]
        public int IMO { get; set; }

        [DataMember(Name = "boatName")]
        public string BoatName { get; set; }

        public string IMOVessel
        {
            get
            {
                return IMO + " - " + BoatName;
            }
        }

        [DataMember(Name = "vesselType")]
        public string VesselType { get; set; }

        [DataMember(Name = "callSign")]
        public string CallSign { get; set; }

        [DataMember(Name = "mmsi")]
        public int MMSI { get; set; }

        //[DataMember(Name = "gpsTimeStamp")]
        //public DateTime ReportGPSTimeStamp { get; set; }

        //[DataMember(Name = "lat")]
        //public string ReportLat { get; set; }

        //[DataMember(Name = "lon")]
        //public string ReportLon { get; set; }

        //[DataMember(Name = "cog")]
        //public string ReportCog { get; set; }

        //[DataMember(Name = "sog")]
        //public string ReportSog { get; set; }

        //[DataMember(Name = "pollCategory")]
        //public string ReportCollCategory { get; set; }

        //[DataMember(Name = "pollMessage")]
        //public string ReportCollMessage { get; set; }
    }
}
