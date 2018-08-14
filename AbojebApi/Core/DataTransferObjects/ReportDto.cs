using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbojebApi.Core.DataTransferObjects
{
    [DataContract]
    public class ReportDto
    {
        [DataMember(Name = "reportId")]
        public int ReportId { get; set; }

        [DataMember(Name = "imo")]
        public int IMO { get; set; }

        [DataMember(Name = "gpsTimeStamp", EmitDefaultValue = false)]
        private string timestampForSerialization;

        public DateTime GPSTimeStamp { get; set; }

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            this.timestampForSerialization = this.GPSTimeStamp.ToString();
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            this.GPSTimeStamp = Convert.ToDateTime(this.timestampForSerialization);
        }

        [DataMember(Name = "lat")]
        public string Lat { get; set; }

        [DataMember(Name = "lon")]
        public string Lon { get; set; }

        [DataMember(Name = "cog")]
        public string Cog { get; set; }

        [DataMember(Name = "sog")]
        public string Sog { get; set; }

        [DataMember(Name = "pollCategory")]
        public string PollCategory { get; set; }

        [DataMember(Name = "pollMessage")]
        public string PollMessage { get; set; }
    }
}
