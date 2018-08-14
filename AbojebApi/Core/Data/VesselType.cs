using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbojebApi.Core.Data
{
    public class VesselType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VesselTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual List<Vessel> Vessels { get; set; }
    }
}
