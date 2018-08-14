using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AbojebApi.Core.Data
{
    [DataContract]
    public class ApiFunction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FunctionId { get; set; }

        public int ApiVersionId { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [StringLength(10)]
        public string Name { get; set; }

        [DataMember]
        [StringLength(20)]
        public string ReturnType { get; set; }

        [DataMember]
        [StringLength(200)]
        public string Description { get; set; }

        [DataMember]
        [StringLength(200)]
        public string Example { get; set; }

        [DataMember]
        public virtual List<ApiFunctionParameter> Parameters { get; set; }

        [ForeignKey("ApiVersionId")]
        public virtual ApiVersion ApiVersion { get; set; }
    }
}
