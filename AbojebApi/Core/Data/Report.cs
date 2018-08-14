using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AbojebApi.Core.Data
{
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ReportId { get; set; }

        public int IMO { get; set; }

        public DateTime GPSTimeStamp { get; set; }

        [StringLength(20)]
        public string Lat { get; set; }

        [StringLength(20)]
        public string Lon { get; set; }

        [StringLength(20)]
        public string Cog { get; set; }

        [StringLength(20)]
        public string Sog { get; set; }

        [StringLength(20)]
        public string PollCategory { get; set; }

        [StringLength(50)]
        public string PollMessage { get; set; }

        [ForeignKey("IMO")]
        public virtual Vessel Vessel { get; set; }
    }
}