using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AbojebApi.Core.Data
{
    [DataContract]
    public class ApiFunctionParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParameterId { get; set; }

        public int FunctionId { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        [StringLength(50)]
        public string Type { get; set; }

        [DataMember]
        [StringLength(200)]
        public string Description { get; set; }

        [ForeignKey("FunctionId")]
        public virtual ApiFunction ApiFunction { get; set; }
    }
}
