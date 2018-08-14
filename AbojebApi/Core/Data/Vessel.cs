using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbojebApi.Core.Data
{
    public class Vessel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IMO { get; set; }

        public string VesselType { get; set; }

        [StringLength(50)]
        public string BoatName { get; set; }

        [StringLength(20)]
        public string CallSign { get; set; }

        public int MMSI { get; set; }

        //[ForeignKey("VesselTypeId")]
        //public virtual VesselType VesselType { get; set; }

        public virtual List<Report> Report { get; set; }
    }
}