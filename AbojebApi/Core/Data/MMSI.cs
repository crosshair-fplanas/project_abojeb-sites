using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbojebApi.Core.Data
{
    public class MMSI
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MMSI { get; set; }
    }
}
