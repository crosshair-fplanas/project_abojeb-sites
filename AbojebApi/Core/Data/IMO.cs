using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbojebApi.Core.Data
{
    public class IMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IMO { get; set; }
    }
}
